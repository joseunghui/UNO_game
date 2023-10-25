using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField] public Button LevelBtn;

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
        Debug.Log(this.gameObject.name);

        string btnName = this.gameObject.name;

        if (btnName == "EasyBtn")
        {
            // 넘겨줄 구분 값?

            // CardScene으로 변경
            LoadingSceneManager.LoadScene("CardScenes");
        } else if (btnName == "NormalBtn")
        {
            // 넘겨줄 구분 값?

            // CardScene으로 변경
            LoadingSceneManager.LoadScene("CardScenes");
        } else if (btnName == "HardBtn")
        {
            // 넘겨줄 구분 값?

            // CardScene으로 변경
            LoadingSceneManager.LoadScene("CardScenes");
        }
        

    }

}
