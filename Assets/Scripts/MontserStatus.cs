using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MontserStatus : MonoBehaviour
{
    [SerializeField] private float hp = 100.0f;
    [SerializeField] private float def = 10.0f;
    

    public void TakeDamaged(float attack)
    {
        float damage = attack - def;

        hp -= damage;
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sword"))
        {
            Debug.Log("적중당함");
        }
    }
}
