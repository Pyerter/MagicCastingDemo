using System;
using UnityEngine;

public class TransformEdgeGenerator<T> : GraphEdgeGenerator<Transform, T>
{
    protected Func<Transform, Transform, T> edgeGenerator;
    public TransformEdgeGenerator() : this((transform, transform1) => default(T))
    {
        
    }

    public TransformEdgeGenerator(Func<Transform, Transform, T> edgeGenerator)
    {
        this.edgeGenerator = edgeGenerator;
    }
    
    public float GenerateEdgeWeight(Transform[] v, int i, int j)
    {
        return Vector3.Distance(v[i].position, v[j].position);
    }

    public T GenerateEdgeValue(Transform[] v, int i, int j)
    {
        return edgeGenerator(v[i], v[j]);
    }
}
