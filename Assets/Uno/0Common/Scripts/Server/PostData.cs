using BackEnd;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

#region
public class PostData
{
    public string title;
    public string content;
    public string inDate;
    public string expirationDate;   // ���� ���� ��¥
    public bool isCanReceive;     // ���� ���� �� �ִ� �������� �ִ��� ����
    // ������ �̸�(string), ����(int)
    public Dictionary<string, int> postReward = new Dictionary<string, int>();
    // ���� ������ Debug.log�� ����ϱ� ���� �޼ҵ�
    public override string ToString()
    {
        string result = string.Empty;
        result += $"title : {title}\n";
        result += $"content : {content}\n";
        result += $"inDate : {inDate}\n";

        if (isCanReceive)
        {
            result += "���� ������\n";
            foreach (string itemKey in postReward.Keys)
            {
                result += $"|{itemKey} : {postReward[itemKey]}��\n";
            }
        }
        else
        {
            result += "�������� �ʴ� �������Դϴ�.";
        }
        return result;
    }
}
#endregion

public class PostDataDB
{
    UserInfoData data;
    public List<PostData> postList = new List<PostData>();

    public void PostListGet(PostType postType)
    {
        Backend.UPost.GetPostList(postType, callback => {
            
            if (!callback.IsSuccess())
            {
                Debug.LogError($"���� �ҷ����� �� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }
            Debug.Log($"���� ����Ʈ�ҷ����� ��û�� �����߽��ϴ� : {callback}");
            // json ������ �Ľ� ���� / ����
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["postList"];

                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("�������� ������ϴ�");
                    return;
                }

                postList.Clear();

                for (int i = 0; i < jsonData.Count; i++)
                {      // ���� ���� ������ ��� ���� ���� �ҷ�����
                    PostData post = new PostData();

                    post.title = jsonData[i]["title"].ToString();
                    post.content = jsonData[i]["content"].ToString();
                    post.inDate = jsonData[i]["inDate"].ToString();
                    post.expirationDate = jsonData[i]["expirationDate"].ToString();

                    foreach (LitJson.JsonData itemJson in jsonData[i]["items"])
                    { // ���� �Բ� �߼۵� ��� ������ ����
                        if (itemJson["chartName"].ToString() == "��ȭ ��Ʈ")
                        {
                            string itemName = itemJson["item"]["itemName"].ToString();
                            int itemCount = int.Parse(itemJson["itemCount"].ToString());

                            if (post.postReward.ContainsKey(itemName))
                            {          // ���� ���Ե� �������� �������� ��
                                post.postReward[itemName] += itemCount;         // �̹� ������ ���� �߰�
                            }
                            else
                            {
                                post.postReward.Add(itemName, itemCount);       // ������ ��� �߰�
                            }

                            post.isCanReceive = true;
                        }
                        else
                        {
                            Debug.LogWarning($"���� �������� �ʴ� ��Ʈ �����Դϴ�. : {itemJson["chartName"].ToString()}");
                            post.isCanReceive = false;
                        }
                    }
                    postList.Add(post);
                }

                for (int i = 0; i < postList.Count; i++)
                {                         // �ҷ��� ���� ���
                    Debug.Log($"{i}���� ����\n{postList[i].ToString()}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        });
    }

    public void PostReceive(PostType postType, int index)
    {
        if (postList.Count <= 0)
        {
            Debug.LogWarning("���� �� �ִ� ������ �����ϴ�. Ȥ�� ���� ����Ʈ�� ���� ȣ�����ּ���.");
            return;
        }
        if (index >= postList.Count)
        {
            Debug.LogError($"�ش� ������ �������� �ʽ��ϴ�; ��û ��ȣ: {index}, ���� �ִ� ����: {postList.Count}");
            return;
        }

        data = Managers.Data.GetUserInfoData();

        Debug.Log($"{postType.ToString()}�� {postList[index].inDate} ���� ������ ��û;");
        Backend.UPost.ReceivePostItem(postType, postList[index].inDate, callback => {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"���� �� ������ �߻��߽��ϴ�. {callback}");
                return;
            }
            Debug.Log($"�����߽��ϴ� : {callback}");
            postList.RemoveAt(index); // ������ �����ϸ� ������

            if (callback.GetFlattenJSON()["postItems"].Count > 0)
            {  // ������ �������� ���� ��
                // SavePostToLocal(callback.GetFlattenJSON()["postItems"]);

                if (postList[index].postReward.ContainsKey("heart"))
                {
                    data.heart += postList[index].postReward["heart"];
                }
                else
                {
                    data.freeDia += postList[index].postReward["freeDia"];
                }
                Managers.Data.UpdataUserData(Define.UpdateDateSort.PostReward, data);
            }
            else
                Debug.Log("������ �������� �����ϴ�.");
        });
    }

    public void PostReceiveAll(PostType postType)
    {
        if (postList.Count <= 0)
        {
            Debug.LogWarning("���� �� �ִ� ������ �����ϴ�. Ȥ�� ���� ����Ʈ�� ���� ȣ�����ּ���.");
            return;
        }
        Debug.Log($"{postType.ToString()} ���� ��ü������ ��û;");

        Backend.UPost.ReceivePostItemAll(postType, callback => {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"���� ��ü���� �� ���� �߻� {callback}");
                return;
            }
            Debug.Log($"{postType.ToString()} ������ ��ü �����߽��ϴ�.");
            postList.Clear();

            foreach (LitJson.JsonData postItemsJson in callback.GetFlattenJSON()["postItems"])
            {
                foreach(LitJson.JsonData itemJson in postItemsJson)
                {
                    if (itemJson["item"]["itemName"].ToString().Equals(""))
                    {
                        data.heart += int.Parse(itemJson["itemCount"].ToString());
                    }
                    else
                    {
                        data.freeDia += int.Parse(itemJson["itemCount"].ToString());
                    }
                }
            }
            Managers.Data.UpdataUserData(Define.UpdateDateSort.PostReward, data);
        });
    }
}


