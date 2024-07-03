using System.Collections.Generic;
using UnityEngine;

public class EffectPullingManager : MonoBehaviour
{
    // Singleton 인스턴스 접근을 위한 정적 변수
    public static EffectPullingManager Instance;

    // 풀링할 오브젝트 프리팹 목록
    [System.Serializable]
    public class Pool
    {
        public string tag; // 오브젝트의 태그
        public GameObject prefab; // 풀링할 프리팹
        public int size; // 초기 풀 크기
    }

    public List<Pool> pools; // 풀 목록
    private Dictionary<string, Queue<GameObject>> poolDictionary; // 풀 딕셔너리

    void Awake()
    {
        // Singleton 패턴을 사용하여 인스턴스를 설정합니다.
        Instance = this;
    }

    void Start()
    {
        // 오브젝트 풀을 관리할 딕셔너리를 초기화합니다.
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        // 각 풀에 대해 초기화 작업을 수행합니다.
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

    // 오브젝트를 가져오는 메서드
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

    // 오브젝트를 반환하는 메서드
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }
}
