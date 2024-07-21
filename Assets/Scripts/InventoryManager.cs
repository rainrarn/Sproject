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
        // 예시: 게임 시작 시 아이템 추가
        GameObject healthPotion = Instantiate(healthPotionPrefab);
        inventory.AddItem(healthPotion.GetComponent<Item>());

        //GameObject sword = Instantiate(swordPrefab);
        //inventory.AddItem(sword.GetComponent<Item>());
    }
}
