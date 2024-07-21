using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    public Dictionary<Item, int> items = new Dictionary<Item, int>();

    public void AddItem(Item item)
    {
        if (items.ContainsKey(item))
        {
            items[item]++;
        }
        else
        {
            items[item] = 1;
        }
        // �κ��丮 UI ������Ʈ ȣ��
        FindObjectOfType<InventoryUI>().RefreshInventoryUI(items);
    }

    public void RemoveItem(Item item)
    {
        if (items.ContainsKey(item))
        {
            items[item]--;
            if (items[item] <= 0)
            {
                items.Remove(item);
            }
            // �κ��丮 UI ������Ʈ ȣ��
            FindObjectOfType<InventoryUI>().RefreshInventoryUI(items);
        }
    }
}
