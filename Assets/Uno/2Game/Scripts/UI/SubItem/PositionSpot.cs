using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSpot : UI_SubItem
{
    [SerializeField] Transform myCardLeft;
    [SerializeField] Transform myCardRight;
    [SerializeField] Transform otherCardLeft;
    [SerializeField] Transform otherCardRight;
    [SerializeField] GameObject putCardArea;
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
        myCardLeft.transform.position = new Vector3(-7, -3, 0);
        myCardLeft.transform.rotation = Quaternion.Euler(0, 0, 5);
        myCardRight.transform.position = new Vector3(-1.5f, -3, 0);
        myCardRight.transform.rotation = Quaternion.Euler(0, 0, -5);
        otherCardLeft.transform.position = new Vector3(7.5f, 1.5f, 0);
        otherCardLeft.transform.rotation = Quaternion.Euler(0, 0, -5);
        otherCardRight.transform.position = new Vector3(4, 1.5f, 0);
        otherCardRight.transform.rotation = Quaternion.Euler(0, 0, 5);

        putCardArea = Utill.FindChild(gameObject, "PutCardArea", true);
        //myCardArea.layer = 6;
        //myCardArea.transform.position = new Vector3(0, 0, 10);
        BoxCollider2D coll = putCardArea.gameObject.GetOrAddComponent<BoxCollider2D>();
        coll.size = new Vector2(200, 280);
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
