using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSpot : UI_SubItem
{
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] Transform otherCardLeft;
    [SerializeField] Transform otherCardRight;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();
    }

    public Transform GetMyLeft()
    {
        return myCardLeft;
    }

    public Transform GetMyRight()
    {
        return myCardRight;
    }

    public Transform GetOtherLeft()
    {
        return otherCardLeft;
    }

    public Transform GetOtherRight()
    {
        return otherCardRight;
    }
}
