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
        myCardLeft.transform.rotation = Quaternion.Euler(0, 10, 5);
        myCardRight.transform.position = new Vector3(0, -3, 0);
        myCardRight.transform.rotation = Quaternion.Euler(0, 370, -5);
        otherCardLeft.transform.position = new Vector3(8, 2, 0);
        otherCardLeft.transform.rotation = Quaternion.Euler(0, 10, -5);
        otherCardRight.transform.position = new Vector3(5, 2, 0);
        otherCardRight.transform.rotation = Quaternion.Euler(0, -10, 5);

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
