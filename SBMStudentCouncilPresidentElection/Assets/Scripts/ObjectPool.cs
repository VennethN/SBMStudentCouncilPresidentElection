using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;
    public Transform objectsContainer;
    private Dictionary<string, Queue<IPoolAble>> objectPool = new Dictionary<string, Queue<IPoolAble>>();
    private void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += assignOnLoad;
        SceneManager.sceneUnloaded += assignOnUnload;

    }
    public void assignOnLoad(Scene scene, LoadSceneMode lsM)
    {
        GameObject[] objectPools = GameObject.FindGameObjectsWithTag("ObjectPool");
        for (int i = 0; i < objectPools.Length; i++)
        {
            if (objectPools[i].scene != scene) { continue; }
            objectsContainer = objectPools[i].transform;
        }
    }
    public void assignOnUnload(Scene scene)
    {
        objectsContainer = GameObject.FindGameObjectWithTag("ObjectPool").transform;
    }

    public static T GetObject<T>(T genericObject) where T : Object, IPoolAble
    {
        GameObject gameObject = genericObject.gameObject;
        Dictionary<string, Queue<IPoolAble>> objectPool = Instance.objectPool;
        if (objectPool.TryGetValue(gameObject.name, out Queue<IPoolAble> objectList))
        {
            if (objectList.Count == 0)
            {
                return CreateNewObject<T>(genericObject);
            }
            else
            {
                IPoolAble _object = objectList.Dequeue();
                while ((_object.Equals(null) || _object.gameObject == null) && objectList.Count > 0)
                {
                    _object = objectList.Dequeue();
                }
                if (objectList.Count == 0)
                {
                    return CreateNewObject<T>(genericObject);
                }
                _object.gameObject.SetActive(true);
                return (T)_object;

            }
        }
        else
            return CreateNewObject<T>(genericObject);
    }
    public static T GetObject<T>(T genericObject, Vector3 pos, Quaternion rot) where T : Object, IPoolAble
    {
        T genOb = GetObject(genericObject);
        genOb.gameObject.transform.position = pos;
        genOb.gameObject.transform.rotation = rot;
        return genOb;
    }
    private static T CreateNewObject<T>(T genericObject) where T : Object, IPoolAble
    {
        T newGO = Instantiate(genericObject, Instance.objectsContainer);
        newGO.name = genericObject.gameObject.name;
        return newGO;
    }

    public static void ReturnGameObject(IPoolAble ipoolAble)
    {
        Dictionary<string, Queue<IPoolAble>> objectPool = Instance.objectPool;
        GameObject gameObject = ipoolAble.gameObject;
        if (objectPool.TryGetValue(gameObject.name, out Queue<IPoolAble> objectList))
        {
            objectList.Enqueue(ipoolAble);
        }
        else
        {
            Queue<IPoolAble> newObjectQueue = new Queue<IPoolAble>();
            newObjectQueue.Enqueue(ipoolAble);
            objectPool.Add(gameObject.name, newObjectQueue);
        }

        gameObject.SetActive(false);

    }
}
public interface IPoolAble
{
    GameObject gameObject { get; }
    void OnEnable();
    void OnDisable();
}