using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName = "New Item";
    public string description = "Item Description";
    public Sprite icon = null;  // ������ ������
    public bool isStackable = true;  // ��ø �������� ����

    // ������ ��� �޼���
    public virtual void Use()
    {
        Debug.Log("Using " + itemName);
    }
}
