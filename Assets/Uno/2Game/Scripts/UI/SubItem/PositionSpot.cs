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

    public void SetTransformPosition()
    {
        myCardLeft.transform.position = new Vector3(-6, -3, 0);
        myCardLeft.transform.rotation = Quaternion.identity;
        myCardRight.transform.position = new Vector3(0, -3, 0);
        myCardRight.transform.rotation = Quaternion.identity;
        otherCardLeft.transform.position = new Vector3(8, 2, 0);
        otherCardLeft.transform.rotation = Quaternion.identity;
        otherCardRight.transform.position = new Vector3(5, 2, 0);
        otherCardRight.transform.rotation = Quaternion.identity;

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
