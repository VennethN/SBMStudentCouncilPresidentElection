using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ParticleComponent : MonoBehaviour, IPoolAble
{
    ParticleSystem particleSystem;
    private void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
    }
    public void OnEnable()
    {
    }
    public void OnDisable()
    {
        ObjectPool.ReturnGameObject(this);
    }
}
