using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtyType : MonoBehaviour
{
    public BTNType currentType;
    public void OnBynClick()
    {
        switch (currentType)
        {
            // hahaha
            // start
            case BTNType.New:
                Debug.Log("new game");
                break;
            case BTNType.Continue:
                Debug.Log("continue game...");
                break;
        }
    }
}
