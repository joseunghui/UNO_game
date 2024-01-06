using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Mail : UI_Popup
{
    List<PostData> _post = new List<PostData>();

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        

        Bind<GameObject>(typeof(Define.Groups));
        Bind<Button>(typeof(Define.Buttons));

        GetButton((int)Define.Buttons.CloseBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Managers.UI.ClosePopup();
        });

        GetData();
    }

    async void GetData()
    {
        await Task.Run(() => { _post = Managers.Data.GetPostDataList(); });

        SetMailData();
    }

    void SetMailData()
    {
        Debug.Log($"_post >> {_post}");

        // mail data 
        GameObject mailList = Get<GameObject>((int)Define.Groups.MailList);

        // 혹시 모르니 기존 데이터 날리기
        foreach (Transform child in mailList.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < _post.Count; i++)
        {
            GameObject Mail_EA = Managers.UI.MakeSubItem<UI_Mail_EA>(parent: mailList.transform).gameObject;

            Mail_EA.transform.SetParent(mailList.transform);
            Mail_EA.transform.localScale = Vector3.one;

            UI_Mail_EA mailItem = Mail_EA.GetOrAddComponent<UI_Mail_EA>();

            if (_post[i].postReward.ContainsKey("heart"))
            {
                mailItem.SetMailEAData(Define.Goods.Heart, _post[i].postReward["heart"], _post[i].content);
            }

        }


        GetButton((int)Define.Buttons.ReceiveAllBtn).gameObject.BindEvent((PointerEventData) =>
        {
            Debug.Log("Receive all mail Button Click!");
        });
    }
}
