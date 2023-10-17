using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Data; // MySQL 사용을 위한 import
using MySql.Data;
using MySql.Data.MySqlClient;

public class DB_Control : MonoBehaviour
{
    public static MySqlConnection SqlConn;

    static string db_host = "127.0.0.1";
    static string db_id = "root";
    static string db_port = "3306";
    static string db_pw = "1234";
    static string db_name = "db";

    [SerializeField] public TextMeshProUGUI dbConnectLogText;

    string strConn = string.Format("server={0};uid={1};port={2};pwd={3};database={4};charset=utf8 ;", db_host, db_id, db_port, db_pw, db_name);

    private void Awake()
    {
        try
        {
            Debug.Log("DB 접속 테스트");
            dbConnectLogText.text = "DB 접속 테스트";
            SqlConn = new MySqlConnection(strConn);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
            dbConnectLogText.text = e.ToString();
        }
    }


    void Start()
    {
        // SELECT
        string query = "select * from test";

        Debug.Log("OnSelectRequest() 실행");
        dbConnectLogText.text = "OnSelectRequest() 실행";
        DataSet ds = OnSelectRequest(query, "test");

        Debug.Log("OnSelectRequest() 종료 :: 결과 ds = " + ds);
        if (ds != null)
        {
            Debug.Log(ds.GetXml());
        }
    }

    // SELECT -> OnSelectRequest()
    public static DataSet OnSelectRequest(string p_query, string table_name)
    {
        try
        {
            Debug.Log(p_query);
            Debug.Log(table_name);
            
            Debug.Log("SqlConn.Open()");
            
            SqlConn.Open(); // 연결

            Debug.Log("MySqlCommand 에 값 넣기");
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = SqlConn;
            Debug.Log("cmd.Connection = " + cmd.Connection);
            cmd.CommandText = p_query;
            Debug.Log("cmd.CommandText = " + cmd.CommandText);

            MySqlDataAdapter sd = new MySqlDataAdapter();
            DataSet ds = new DataSet();

            
            sd.Fill(ds, table_name);
            Debug.Log("sd.Fill(ds, table_name) 실행" + sd);

            SqlConn.Close(); // 연결 해제

            return ds;
        }
        catch(System.Exception e)
        {
            Debug.Log(e.ToString());
            return null;
        }
    }

}
