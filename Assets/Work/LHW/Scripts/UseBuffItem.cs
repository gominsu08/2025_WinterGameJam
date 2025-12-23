using System.Collections.Generic;
using UnityEngine;

public class UseBuffItem : MonoBehaviour
{
    public GameObject buffItemBaseButton;
    public Transform buttonSpawnParent;

    public ItemBase SprayerItem;
    public ItemBase TeruTeruBozuItem;

    public GameObject SprayerCheckIcon;
    public GameObject TeruTeruBozuCheckIcon;

    public bool isUsedSprayer;
    public bool isUsedTeruTeruBozu;



    public void UseSprayer()
    {
        if(isUsedSprayer == false)
        {
            Inventory.Instance.UseBuffItem(SprayerItem);
            SprayerCheckIcon.SetActive(true);
            isUsedSprayer = true;
        }
        else
        {
            Inventory.Instance.UnUseBuffItem(SprayerItem);
            SprayerCheckIcon.SetActive(false);
            isUsedSprayer = false;

            Inventory.Instance.IsUsedBuffItem("물뿌리개");
        }
        
    }

    public void UseTeruTeruBozu()
    {
        if(isUsedTeruTeruBozu == false)
        {
            Inventory.Instance.UseBuffItem(TeruTeruBozuItem);
            TeruTeruBozuCheckIcon.SetActive(true);
            isUsedTeruTeruBozu = true;
        }
        else
        {
            Inventory.Instance.UnUseBuffItem(TeruTeruBozuItem);
            TeruTeruBozuCheckIcon.SetActive(false);
            isUsedTeruTeruBozu = false;
        }
    }
}
