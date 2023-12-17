using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    // ���� ��� Base���� ����
    public GameMode.PVCMode PVCMode { get; protected set; } = GameMode.PVCMode.EASY; // �ʱ� ���� EASY

    // ���� ��� Base���� ����
    UI_LevelSelect levelSelect;

    protected override void init()
    {
        base.init();

        // BGM
        Managers.Sound.Play("GameBGM", Define.Sound.BGM);

        StartCoroutine(CoSelectedGameMode());
    }

    IEnumerator CoSelectedGameMode()
    {
        levelSelect = Managers.UI.ShowPopup<UI_LevelSelect>();

        yield return null;

        if (levelSelect != null)
        {
            switch (levelSelect.gameLevel)
            {
                case 0:
                    PVCMode = GameMode.PVCMode.EASY;
                    break;
                case 1:
                    PVCMode = GameMode.PVCMode.NORMAL;
                    break;
                case 2:
                    PVCMode = GameMode.PVCMode.HARD;
                    break;
            }

            Debug.Log($"selected mode");
        }
        else
            yield break;
    }

    public override void Clear()
    {
        Debug.Log("Game Scene Clear!!!");
    }


}
