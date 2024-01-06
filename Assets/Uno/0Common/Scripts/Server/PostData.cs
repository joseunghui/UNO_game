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
    public string expirationDate;   // 우편 만료 날짜
    public bool isCanReceive;     // 우편에 받을 수 있는 아이템이 있는지 여부
    // 아이템 이름(string), 개수(int)
    public Dictionary<string, int> postReward = new Dictionary<string, int>();
    // 우편 정보를 Debug.log로 출력하기 위한 메소드
    public override string ToString()
    {
        string result = string.Empty;
        result += $"title : {title}\n";
        result += $"content : {content}\n";
        result += $"inDate : {inDate}\n";

        if (isCanReceive)
        {
            result += "우편 아이템\n";
            foreach (string itemKey in postReward.Keys)
            {
                result += $"|{itemKey} : {postReward[itemKey]}개\n";
            }
        }
        else
        {
            result += "지원하지 않는 아이템입니다.";
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
                Debug.LogError($"우편 불러오기 중 에러가 발생했습니다. : {callback}");
                return;
            }
            Debug.Log($"우편 리스트불러오기 요청에 성공했습니다 : {callback}");
            // json 데이터 파싱 성공 / 실패
            try
            {
                LitJson.JsonData jsonData = callback.GetFlattenJSON()["postList"];

                if (jsonData.Count <= 0)
                {
                    Debug.LogWarning("우편함이 비었습니다");
                    return;
                }

                postList.Clear();

                for (int i = 0; i < jsonData.Count; i++)
                {      // 현재 저장 가능한 모든 우편 정보 불러오기
                    PostData post = new PostData();

                    post.title = jsonData[i]["title"].ToString();
                    post.content = jsonData[i]["content"].ToString();
                    post.inDate = jsonData[i]["inDate"].ToString();
                    post.expirationDate = jsonData[i]["expirationDate"].ToString();

                    foreach (LitJson.JsonData itemJson in jsonData[i]["items"])
                    { // 우편에 함께 발송된 모든 아이템 정보
                        if (itemJson["chartName"].ToString() == "재화 차트")
                        {
                            string itemName = itemJson["item"]["itemName"].ToString();
                            int itemCount = int.Parse(itemJson["itemCount"].ToString());

                            if (post.postReward.ContainsKey(itemName))
                            {          // 우편에 포함된 아이템이 여러개일 때
                                post.postReward[itemName] += itemCount;         // 이미 있으면 개수 추가
                            }
                            else
                            {
                                post.postReward.Add(itemName, itemCount);       // 없으면 요소 추가
                            }

                            post.isCanReceive = true;
                        }
                        else
                        {
                            Debug.LogWarning($"아직 지원하지 않는 차트 정보입니다. : {itemJson["chartName"].ToString()}");
                            post.isCanReceive = false;
                        }
                    }
                    postList.Add(post);
                }

                for (int i = 0; i < postList.Count; i++)
                {                         // 불러온 정보 출력
                    Debug.Log($"{i}번쨰 우편\n{postList[i].ToString()}");
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
            Debug.LogWarning("받을 수 있는 우편이 없습니다. 혹은 우편 리스트를 먼저 호출해주세요.");
            return;
        }
        if (index >= postList.Count)
        {
            Debug.LogError($"해당 우편은 존재하지 않습니다; 요청 번호: {index}, 우편 최대 갯수: {postList.Count}");
            return;
        }

        data = Managers.Data.GetUserInfoData();

        Debug.Log($"{postType.ToString()}의 {postList[index].inDate} 우편 수령을 요청;");
        Backend.UPost.ReceivePostItem(postType, postList[index].inDate, callback => {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"수령 중 에러가 발생했습니다. {callback}");
                return;
            }
            Debug.Log($"성공했습니다 : {callback}");
            postList.RemoveAt(index); // 우편은 수령하면 삭제됨

            if (callback.GetFlattenJSON()["postItems"].Count > 0)
            {  // 수령할 아이템이 있을 때
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
                Debug.Log("수령할 아이템이 없습니다.");
        });
    }

    public void PostReceiveAll(PostType postType)
    {
        if (postList.Count <= 0)
        {
            Debug.LogWarning("받을 수 있는 우편이 없습니다. 혹은 우편 리스트를 먼저 호출해주세요.");
            return;
        }
        Debug.Log($"{postType.ToString()} 우편 전체수령을 요청;");

        Backend.UPost.ReceivePostItemAll(postType, callback => {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"우편 전체수령 중 에러 발생 {callback}");
                return;
            }
            Debug.Log($"{postType.ToString()} 우편을 전체 수령했습니다.");
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


