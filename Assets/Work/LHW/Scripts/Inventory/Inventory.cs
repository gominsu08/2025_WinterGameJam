using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public int money;
    
    private Dictionary<ItemBase, int> itemCounts = new Dictionary<ItemBase, int>();
    

    private void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddMoney(int amount)
    {
        money += amount;
    }

    public bool SpendMoney(int amount)
    {
        if(money < amount) return false;
        money -= amount;
        return true;
    }

    public void AddItem(ItemBase item)
    {
        itemCounts[item] = itemCounts.ContainsKey(item) ? itemCounts[item] + 1 : 1;
    }

    public void RemoveItem(ItemBase item)
    {
        itemCounts[item] = itemCounts.ContainsKey(item) && itemCounts[item] > 0 ? itemCounts[item] - 1 : 0;
    }

    public int GetItemCount(ItemBase item)
    {
        return itemCounts.ContainsKey(item) ? itemCounts[item] : 0;
    }
}
