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
    
    public float GenerateEdgeWeight(GraphMatrix<Transform, T> matrix, int i, int j)
    {
        return Vector3.Distance(matrix[i].position, matrix[j].position);
    }

    public T GenerateEdgeValue(GraphMatrix<Transform, T> matrix, int i, int j)
    {
        return edgeGenerator(matrix[i], matrix[j]);
    }
}
