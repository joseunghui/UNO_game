using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

using BackEnd; // 뒤끝 디비

public class UserData
{
    public int uid;
    public string email;
    public string password;
    public string nick;

    // data 디버깅 하기 위한 함수 (Debug.Log(UserData);)
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"uid : {uid}");
        result.AppendLine($"email : {email}");
        result.AppendLine($"password : {password}");
        result.AppendLine($"nick : {nick}");

        // list 형식이면 이런/
        // result.AppendLine($"inventory");
        // foreach (var itemKey in inventory.Keys)
        // {
        //     result.AppendLine($"| {itemKey} : {inventory[itemKey]}개");
        // }

        return result.ToString();
    }
}

// 인스턴스 만들기
public class UserDataIns
{
    private static UserDataIns _instance = null;

    public static UserDataIns Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new UserDataIns();
            }
            return _instance;
        }
    }

    public static UserData userData;

    private string userDataRowInData = string.Empty;

    // user Insert(sign up)
    public void InsertUserData()
    {
        if (userData == null)
        {
            userData = new UserData();
        }

        Debug.Log("데이터를 초기화 합니다.");

        userData.uid = 1;
        userData.email = "user@gmail.com";
        userData.password = "1234";
        userData.nick = "sample";

        // list 형식은 이렇게
        // userData.inventory.Add("cloth", 1);

        // 업데이트 목록에 추가하기 위한 param 생성
        Debug.Log("뒤끝 업데이트 목록에 해당 데이터들을 추가합니다.");
        Param param = new Param();
        param.Add("uid", userData.uid);
        param.Add("email", userData.email);
        param.Add("password", userData.password);
        param.Add("nick", userData.nick);

        // Insert excute
        Debug.Log("유저 DB Insert 실행");
        var bro = Backend.GameData.Insert("user", param);

        // 성공 or 실패
        if (bro.IsSuccess())
        {
            Debug.Log("계정 정보 데이터 삽입에 성공 했습니다." + bro);

            // 삽입한 계정 정보의 고유 값
            // userDataRowInData = bro.GetInData();
        } else {
            Debug.Log("계정 정보 데이터 삽입에 실패 했습니다." + bro);
        }


    }

    // user login(sign in)
    public void LoginUser()
    {

    }

    // user select(get user data)
    public void UserDataGet()
    {

    }

    // user update(change userInfo)
    public void UserDataUpdate()
    {

    }
}

public class user : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
