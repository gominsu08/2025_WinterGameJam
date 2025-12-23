using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UseBuffItem : MonoBehaviour
{
    public GameObject buffItemBaseButton;
    public Transform buttonSpawnParent;

    public ItemBase SprayerItem;
    public ItemBase TeruTeruBozuItem;

    public TMP_Text SprayerItemCountText;
    public TMP_Text TeruTeruBozuItemCountText;

    public GameObject SprayerCheckIcon;
    public GameObject TeruTeruBozuCheckIcon;

    public bool isUsedSprayer;
    public bool isUsedTeruTeruBozu;


    void FixedUpdate()
    {
        SprayerItemCountText.text = Inventory.Instance.GetItemCount(SprayerItem).ToString();
        TeruTeruBozuItemCountText.text = Inventory.Instance.GetItemCount(TeruTeruBozuItem).ToString();
    }


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
