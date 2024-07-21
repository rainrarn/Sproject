using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public Item potion;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Inventory inventory = other.GetComponent<Inventory>();
            inventory.AddItem(potion);
            Destroy(this);
        }
    }
}
