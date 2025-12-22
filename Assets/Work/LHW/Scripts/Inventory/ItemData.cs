using System;
using UnityEngine;

public enum PlacementTarget { Up, Down, Both }

public enum ItemType
{
    Deco,
    Buff,
}

public enum DecorationType
{
    Muffler,
    Arm,
    Hat,
    Button,
    Eye,
    Mouth,
    Nose,
}

[Serializable]
public class ItemBase : ScriptableObject{
    public string itemName;
    [TextArea(3, 10)]public string itemDescription;
    public ItemType itemType;
    public Sprite itemIcon;
}


[Serializable, CreateAssetMenu(fileName = "New Decoration Item", menuName = "Items/Decoration Item")]
public class DecorationItem : ItemBase
{
    public int decorationObjectIndex;
    public DecorationType decorationType;

    // 배치형 옵션
    public bool isPlaceable;
    public GameObject placeablePrefab;
    public PlacementTarget placementTarget = PlacementTarget.Up;
    public float surfaceOffset = 0.02f;
}

