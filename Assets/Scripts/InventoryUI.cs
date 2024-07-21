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
            // ������ ���� ���� (��: ������ �̹���, �̸� ��)
            itemSlot.GetComponentInChildren<Text>().text = entry.Key.itemName + " x" + entry.Value;
            itemSlot.GetComponentInChildren<Image>().sprite = entry.Key.icon;
        }
    }
}
