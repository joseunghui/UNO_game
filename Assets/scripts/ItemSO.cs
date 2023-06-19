using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    [System.Serializable]
public class Item
{
    public int num;
    public string color;
    public Sprite sprite;
}

    [CreateAssetMenu(fileName ="ItemSO", menuName = "Scriptable Object/ItemSO")]
public class ItemSO : ScriptableObject{
    public Item[] items;
}
