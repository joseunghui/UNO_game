using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField] public Button LevelBtn;
    public static int TurnlimitTime;

    private void Start()
    {
        LevelBtn = this.transform.GetComponent<Button>();

        if (LevelBtn != null)
        {
            LevelBtn.onClick.AddListener(LevelSelectBtnClick);
        }
    }

    private void LevelSelectBtnClick()
    {
        string btnName = this.gameObject.name;

        if (btnName == "EasyBtn")
        {
            TurnlimitTime = 20;
        } else if (btnName == "NormalBtn")
        {
            TurnlimitTime = 10;
        } else if (btnName == "HardBtn")
        {
            TurnlimitTime = 5;
        }
        LoadingSceneManager.LoadScene("CardScenes");

    }

}
