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
            case BTNType.New:
                Debug.Log("new game");
            case BTNType.Continue:
                Debug.Log("continue game...");
                break;
        }
    }
}
