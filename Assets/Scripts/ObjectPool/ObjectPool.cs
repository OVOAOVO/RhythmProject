using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ObjectPool
{
    private static ObjectPool instance;
    private Dictionary<string, Queue<GameObject>> objectPool = new Dictionary<string, Queue<GameObject>>();
    private GameObject pool;
    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ObjectPool();
                SceneManager.sceneLoaded += instance.OnSceneLoaded;
            }
            return instance;
        }
    }
        // 场景加载时自动清理对象池
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ClearPool();
    }

    // 清理所有池内对象
    private void ClearPool()
    {
        foreach (var queue in objectPool.Values)
        {
            while (queue.Count > 0)
            {
                GameObject obj = queue.Dequeue();
                if (obj != null)
                {
                    GameObject.Destroy(obj);
                }
            }
        }
        objectPool.Clear();

        if (pool != null)
        {
            GameObject.Destroy(pool);
            pool = null;
        }
    }

    public GameObject GetGameObject(GameObject prefab)
    {
        GameObject _object;
        if(!objectPool.ContainsKey(prefab.name) || objectPool[prefab.name].Count == 0)
        {
            _object = GameObject.Instantiate(prefab);
            PushObject(_object);
            if(pool == null)
                pool = new GameObject("ObjectPool");
            GameObject child = GameObject.Find(prefab.name);
            if(!child)
            {
                child = new GameObject(prefab.name);
                child.transform.SetParent(pool.transform);
            }
            _object.transform.SetParent(child.transform);
        }
        _object = objectPool[prefab.name].Dequeue();
        _object.SetActive(true);
        return _object;
    }

    public void PushObject(GameObject prefab)
    {
        string _name = prefab.name.Replace("(Clone)", string.Empty);
        if (!objectPool.ContainsKey(_name))
            objectPool.Add(_name, new Queue<GameObject>());
        objectPool[_name].Enqueue(prefab);
        prefab.SetActive(false);
    }
}
