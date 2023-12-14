using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    protected override void init()
    {
        base.init();

        // BGM
        Managers.Sound.Play("GameBGM", Define.Sound.BGM);

        TurnManager.instance.StartGame();
    }

    public override void Clear()
    {
        Debug.Log("Game Scene Clear!!!");
    }
}
