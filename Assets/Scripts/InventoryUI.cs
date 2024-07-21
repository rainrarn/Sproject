using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject itemSlotPrefab;

    public void RefreshInventoryUI(Dictionary<Item, int> items)
    {
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (KeyValuePair<Item, int> entry in items)
        {
            GameObject itemSlot = Instantiate(itemSlotPrefab, inventoryPanel.transform);
            // 아이템 정보 설정 (예: 아이템 이미지, 이름 등)
            itemSlot.GetComponentInChildren<Text>().text = entry.Key.itemName + " x" + entry.Value;
            itemSlot.GetComponentInChildren<Image>().sprite = entry.Key.icon;
        }
    }
}
