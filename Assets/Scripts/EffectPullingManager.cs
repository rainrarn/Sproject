using System.Collections.Generic;
using UnityEngine;

public class EffectPullingManager : MonoBehaviour
{
    // Singleton �ν��Ͻ� ������ ���� ���� ����
    public static EffectPullingManager Instance;

    // Ǯ���� ������Ʈ ������ ���
    [System.Serializable]
    public class Pool
    {
        public string tag; // ������Ʈ�� �±�
        public GameObject prefab; // Ǯ���� ������
        public int size; // �ʱ� Ǯ ũ��
    }

    public List<Pool> pools; // Ǯ ���
    private Dictionary<string, Queue<GameObject>> poolDictionary; // Ǯ ��ųʸ�

    void Awake()
    {
        // Singleton ������ ����Ͽ� �ν��Ͻ��� �����մϴ�.
        Instance = this;
    }

    void Start()
    {
        // ������Ʈ Ǯ�� ������ ��ųʸ��� �ʱ�ȭ�մϴ�.
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // �� Ǯ�� ���� �ʱ�ȭ �۾��� �����մϴ�.
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    // ������Ʈ�� �������� �޼���
    public GameObject EffectPull(string tag)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToReuse = poolDictionary[tag].Dequeue();
        objectToReuse.SetActive(true);
        poolDictionary[tag].Enqueue(objectToReuse);

        return objectToReuse;
    }

    // ������Ʈ�� ��ȯ�ϴ� �޼���
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
