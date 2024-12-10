using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.VFX;
using Random = System.Random;

public class FireworksController : MonoBehaviour
{
    [SerializeField] protected Vector3 bounds;
    [SerializeField] protected Transform targetParent;
    [SerializeField] protected VisualEffect explosionTemplate;
    [SerializeField] protected List<VisualEffect> explosionEffects;
    [SerializeField] protected float[] nextTrigger;
    [SerializeField] protected int numberOfExplosions;
    [SerializeField] protected Vector2 minMaxDelay;
    protected bool initialized = false;
    private Random rand;

    private void Start()
    {
        rand = new Random();
    }

    public void TrySpawnExplosions()
    {
        if (initialized) return;
        initialized = true;
        explosionEffects = new List<VisualEffect>();
        for (int i = 0; i < numberOfExplosions; i++)
        {
            VisualEffect effect = Instantiate(explosionTemplate, targetParent);
            explosionEffects.Add(effect);
            effect.gameObject.SetActive(true);
            Vector3 position = new Vector3(GenRandom(bounds.x), GenRandom(bounds.y), GenRandom(bounds.z));
            position += transform.position;
            effect.transform.position = position;
        }
        nextTrigger = new float[numberOfExplosions];
        TriggerExplosions();
    }

    public float GenRandom(float size)
    {
        if (rand == null)
            rand = new Random();
        return ((float)rand.NextDouble() * size * 2) - size;
    }

    public float GenRandom(float min, float max)
    {
        if (rand == null) rand = new Random();
        return (float)rand.NextDouble() * (max - min) + min;
    }

    public void TriggerExplosions()
    {
        TrySpawnExplosions();
        for (int i = 0; i < nextTrigger.Length; i++)
        {
            if (Time.time >= nextTrigger[i])
            {
                nextTrigger[i] = Time.time + GenRandom(minMaxDelay.x, minMaxDelay.y);
                explosionEffects[i].Play();
            }
        }
    }

    private void Update()
    {
        TriggerExplosions();
    }

    public void ResetExplosions()
    {
        for (int i = explosionEffects.Count - 1; i >= 0; i--)
        {
            VisualEffect effect = explosionEffects[i];
            explosionEffects.RemoveAt(i);
            if (Application.isPlaying)
                Destroy(effect);
            else
                DestroyImmediate(effect);
        }
    }

}
