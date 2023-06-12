using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
     string log;
     Button ClearLog;
     Button Login;
     Button Logout;

    void OnGUI(){
        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one * 3);

        if(GUILayout.Button("ClearLog"))
            log = "";

        if(GUILayout.Button("Login"))
            GPGSBinder.Inst.Login((success, localUser) =>
            log = $"{success}, {localUser.id}, {localUser.userName}, {localUser.state}, {localUser.underage}");

        if(GUILayout.Button("Logout"))
            GPGSBinder.Inst.Logout();

        GUILayout.Label(log);
    }
}
