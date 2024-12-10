using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class ChainLightningController : MonoBehaviour
{
    [SerializeField] protected Transform sourceChain;
    [SerializeField] protected List<Transform> targetChain = new List<Transform>();
    [SerializeField] protected Transform targetParent;
    [SerializeField] protected float lightningLength = 5f;
    [SerializeField] protected VisualEffect lightningTemplate;
    [SerializeField] protected List<VisualEffect> lightningEffects = new List<VisualEffect>();
    [SerializeField] protected float delayEffect = 3f;
    [SerializeField] protected float startTime = 0;
    [SerializeField] protected bool completed = false;
    [SerializeField] protected int maximumVFX = 100;
        
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        if (!completed && startTime + delayEffect > Time.time)
        {
            TrySpawnLightning();
        }
    }

    public void TrySpawnLightning()
    {
        if (completed) return;
        completed = true;
        if (targetChain.Count == 0)
        {
            return;
        }
        lightningEffects = new List<VisualEffect>();
        Vector3 currentPosition = sourceChain.position;
        int nextTarget = 0;
        for (int i = 0; i < maximumVFX; i++)
        {
            VisualEffect visualEffect = Instantiate(lightningTemplate, targetParent);
            lightningEffects.Add(visualEffect);
            visualEffect.transform.position = currentPosition;
            visualEffect.gameObject.SetActive(true);
            visualEffect.transform.LookAt(targetChain[nextTarget].position);
            //Quaternion rot = visualEffect.transform.rotation;
            //rot *= Quaternion.Euler(0, 0, 90);
            //visualEffect.transform.rotation = rot;
            if (Vector3.Distance(currentPosition, targetChain[nextTarget].position) <= lightningLength)
            {
                currentPosition = targetChain[nextTarget].position;
                nextTarget++;
                if (nextTarget >= targetChain.Count) break;
            }
            else
            {
                currentPosition = Vector3.MoveTowards(currentPosition, targetChain[nextTarget].position,
                    lightningLength);
                if (Vector3.Distance(currentPosition, targetChain[nextTarget].position) <= lightningLength)
                {
                    currentPosition = targetChain[nextTarget].position + (Vector3.Normalize(currentPosition - targetChain[nextTarget].position) * (lightningLength - 0.01f));
                }
            }
        }

        TriggerLightning();
    }

    public void TriggerLightning()
    {
        if (!completed) TrySpawnLightning();
        foreach (VisualEffect visualEffect in lightningEffects)
        {
            visualEffect.Play();
        }
    }

    public void ResetLighting()
    {
        if (!completed) return;
        completed = false;

        for (int i = lightningEffects.Count - 1; i >= 0; i--)
        {
            VisualEffect visualEffect = lightningEffects[i];
            lightningEffects.RemoveAt(i);
            if (Application.isPlaying)
                Destroy(visualEffect.gameObject);
            else
                DestroyImmediate(visualEffect.gameObject);
        }
    }
}
