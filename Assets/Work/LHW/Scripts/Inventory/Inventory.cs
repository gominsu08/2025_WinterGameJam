using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Work.KJY.Code.Event;
using Work.Utils.EventBus;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public int money;

    public List<ItemBase> defaultItems;

    private Dictionary<ItemBase, int> itemCounts = new Dictionary<ItemBase, int>();
    

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        foreach(var item in defaultItems)
        {
            AddItem(item);
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        Bus<ChangeMoneyEvent>.Raise(new ChangeMoneyEvent(money));
    }

    public bool SpendMoney(int amount)
    {
        if(money < amount) return false;
        money -= amount;
        Bus<ChangeMoneyEvent>.Raise(new ChangeMoneyEvent(money));
        return true;
    }

    public void AddItem(ItemBase item)
    {
        itemCounts[item] = itemCounts.ContainsKey(item) ? itemCounts[item] + 1 : 1;
    }

    public bool RemoveItem(ItemBase item)
    {
        if(itemCounts.ContainsKey(item) && itemCounts[item] > 0)
        {
            itemCounts[item]--;
            return true;
        }
        return false;
    }

    public List<ItemBase> GetAllItems(DecorationType type)
    {
        List<ItemBase> keys = new List<ItemBase>();
        foreach(var kvp in itemCounts)
        {
            keys.Add(kvp.Key);
        }

        return keys;
    }

    public int GetItemCount(ItemBase item)
    {
        return itemCounts.ContainsKey(item) ? itemCounts[item] : 0;
    }
}
