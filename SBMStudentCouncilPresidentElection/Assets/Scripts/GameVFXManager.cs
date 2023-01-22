using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVFXManager : MonoBehaviour
{
    public static GameVFXManager Instance;
    public List<ParticleDataInfo> particleInfos;
    public Dictionary<string, ParticleComponent> particleDictionary;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

    }
    public static void SpawnParticle(string particleName)
    {
        Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mPos.z = Camera.main.nearClipPlane;
        ObjectPool.GetObject<ParticleComponent>(Instance.particleDictionary[particleName], mPos, Quaternion.identity);
    }
    public static void SpawnParticle(string particleName, Vector3 location)
    {

        ObjectPool.GetObject<ParticleComponent>(Instance.particleDictionary[particleName], location, Quaternion.identity);
    }
    public static void SpawnParticle(ParticleComponent particle, Vector3 location)
    {
        ObjectPool.GetObject<ParticleComponent>(particle, location, Quaternion.identity);
    }
    void Start()
    {
        particleDictionary = new Dictionary<string, ParticleComponent>();
        for (int i = 0; i < particleInfos.Count; i++)
        {
            particleDictionary[particleInfos[i].particleName] = particleInfos[i].particleC;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
[System.Serializable]
public class ParticleDataInfo
{
    public string particleName;
    public ParticleComponent particleC;
}
