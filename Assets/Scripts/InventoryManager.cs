using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Inventory inventory;
    public GameObject healthPotionPrefab;
    public GameObject swordPrefab;

    void Start()
    {
        // ����: ���� ���� �� ������ �߰�
        GameObject healthPotion = Instantiate(healthPotionPrefab);
        inventory.AddItem(healthPotion.GetComponent<Item>());

        //GameObject sword = Instantiate(swordPrefab);
        //inventory.AddItem(sword.GetComponent<Item>());
    }
}
