using System;
using UnityEngine;

public enum ItemType
{
    Deco,
    Buff,
}

public enum DecorationType
{
}

[Serializable]
public class ItemBase : ScriptableObject{
    public string itemName;
    public ItemType itemType;
    public Sprite itemIcon;
}


[Serializable, CreateAssetMenu(fileName = "New Decoration Item", menuName = "Items/Decoration Item")]
public class DecorationItem : ItemBase
{
    public int decorationObjectIndex;
    public DecorationType decorationType;
}

