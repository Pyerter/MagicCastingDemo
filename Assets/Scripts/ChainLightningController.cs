using System;
using System.Collections.Generic;
using Algorithms;
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
            TrySpawnTreeLightning();
        }
    }

    // Spawn lightning along the MST for the targets
    public void TrySpawnTreeLightning()
    {
        if (completed) return;
        completed = true;
        
        // Create array of targets
        Transform[] targets = new Transform[targetChain.Count + 1];
        targets[0] = sourceChain;
        for (int i = 0; i < targetChain.Count; i++)
        {
            targets[i + 1] = targetChain[i];
        }
        
        // create the graph
        GraphMatrix<Transform, bool> graph = new GraphMatrix<Transform, bool>(targets, new TransformEdgeGenerator<bool>((t1, t2) => false));
        // get the tree
        bool[,] tree = PrimsAlgorithm.MST(graph, 0);
        
        // for all edges included in the tree, draw lightning along that edge
        for (int i = 0; i < tree.GetLength(0); i++)
        {
            for (int j = 0; j < tree.GetLength(1); j++)
            {
                if (tree[i, j])
                {
                    SpawnLightingFromTo(targets[i].position, targets[j].position);
                }
            }
        }
        
        TriggerLightning();
    }

    public void SpawnLightingFromTo(Vector3 position, Vector3 targetPosition)
    {
        // cap the lightning spawned according to the max vfx value
        for (int i = 0; i < maximumVFX; i++)
        {
            // instantiate the lightning and orient it
            VisualEffect visualEffect = Instantiate(lightningTemplate, targetParent);
            lightningEffects.Add(visualEffect);
            visualEffect.transform.position = position;
            visualEffect.gameObject.SetActive(true);
            visualEffect.transform.LookAt(targetPosition);
            
            // Exit early if close enoygh
            if (Vector3.Distance(position, targetPosition) <= lightningLength)
                break;
            
            // Move position towards
            position = Vector3.MoveTowards(position, targetPosition, lightningLength);
            if (Vector3.Distance(position, targetPosition) <= lightningLength)
            {
                // move just under the distance away from the target
                position = targetPosition + (Vector3.Normalize(position - targetPosition) * (lightningLength - 0.01f));
            }
        }
    }

    // Spawn lightning along the target path
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
        
        //cap lighting at max vfx
        for (int i = 0; i < maximumVFX; i++)
        {
            // instantiate lightning at location
            VisualEffect visualEffect = Instantiate(lightningTemplate, targetParent);
            lightningEffects.Add(visualEffect);
            visualEffect.transform.position = currentPosition;
            visualEffect.gameObject.SetActive(true);
            visualEffect.transform.LookAt(targetChain[nextTarget].position);
            
            //Quaternion rot = visualEffect.transform.rotation;
            //rot *= Quaternion.Euler(0, 0, 90);
            //visualEffect.transform.rotation = rot;
            
            // if lightning is near distance, increment next target
            if (Vector3.Distance(currentPosition, targetChain[nextTarget].position) <= lightningLength)
            {
                currentPosition = targetChain[nextTarget].position;
                nextTarget++;
                // if done with targets, stop
                if (nextTarget >= targetChain.Count) break;
            }
            else
            {
                // update current position towards target
                currentPosition = Vector3.MoveTowards(currentPosition, targetChain[nextTarget].position,
                    lightningLength);
                // if close enough, move to align the lightning perfectly with target location
                if (Vector3.Distance(currentPosition, targetChain[nextTarget].position) <= lightningLength)
                {
                    currentPosition = targetChain[nextTarget].position + (Vector3.Normalize(currentPosition - targetChain[nextTarget].position) * (lightningLength - 0.01f));
                }
            }
        }

        TriggerLightning();
    }

    // play vfx for all lightning
    public void TriggerLightning()
    {
        if (!completed) TrySpawnTreeLightning();
        foreach (VisualEffect visualEffect in lightningEffects)
        {
            visualEffect.Play();
        }
    }

    // destroy lightning objects
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
