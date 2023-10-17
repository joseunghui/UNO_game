using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Data; // MySQL 사용을 위한 import
using MySql.Data;
using UnityEngine.Networking;
using MySql.Data.MySqlClient;

public class DB_Control : MonoBehaviour
{
    private void Start()
    {

        StartCoroutine(GetMySQLData());
    }

    private IEnumerator GetMySQLData()
    {
        string serverPath = "http://localhost/serverInfo.php";
        WWWForm form = new WWWForm();

        // 전달할 정보 입력
        form.AddField("way", 0);
        form.AddField("word", 1);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(serverPath, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isDone)
            {
                Debug.Log(webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log(webRequest.error);
            }
        }
    }

}
