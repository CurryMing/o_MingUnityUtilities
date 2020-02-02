using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/New Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public int itemCount;
    [TextArea]
    public string itemInfo;
    public Sprite itemImage;

    public bool equip;
}
