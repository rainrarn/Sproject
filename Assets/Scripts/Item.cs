using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName = "New Item";
    public string description = "Item Description";
    public Sprite icon = null;  // 아이템 아이콘
    public bool isStackable = true;  // 중첩 가능한지 여부

    // 아이템 사용 메서드
    public virtual void Use()
    {
        Debug.Log("Using " + itemName);
    }
}
