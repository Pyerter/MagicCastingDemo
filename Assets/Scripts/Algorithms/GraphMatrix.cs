using System;
using Unity.VisualScripting;
using UnityEngine;

public class GraphMatrix <T, R>
{

    protected int _size;
    public int Size => _size;

    protected T[] _vertices;
    protected GraphEdge<R>[,] _matrix;

    public T this[int i]
    {
        get
        {
            if (ValidIndex(i)) return _vertices[i];
            throw new IndexOutOfRangeException();
        }
        set => _vertices[i] = value;
    }
    
    public GraphEdge<R> this[int i,int j]
    {
        get { 
            if (ValidIndex(i, j)) return _matrix[i, j];
            throw new IndexOutOfRangeException();
        }
    }

    public GraphMatrix(int vertices, Func<int, T> vertexGenerator, GraphEdgeGenerator<T, R> edgeGenerator = null)
    {
        T[] vertexArray = new T[vertices];
        for (int i = 0; i < vertexArray.Length; i++) vertexArray[i] = vertexGenerator(i);
        InitializeMatrix(vertexArray, edgeGenerator);
    }

    public GraphMatrix(int vertices, GraphEdgeGenerator<T, R> edgeGenerator = null)
    {
        InitializeMatrix(new T[vertices], edgeGenerator);
    }

    public GraphMatrix(T[] vertices, GraphEdgeGenerator<T, R> edgeGenerator = null)
    {
        InitializeMatrix(vertices, edgeGenerator);
    }

    protected void InitializeMatrix(T[] vertices, GraphEdgeGenerator<T, R> edgeGenerator = null)
    {
        _size = vertices.Length;
        _vertices = vertices;
        _matrix = new GraphEdge<R>[_size, _size];
        for (int i = 0; i < _size; i++)
        {
            for (int j = 0; j < _size; j++)
            {
                _matrix[i, j] = new GraphEdge<R>(i, j);
                if (edgeGenerator != null)
                {
                    _matrix[i, j].Value = edgeGenerator.GenerateEdgeValue(this, i, j);
                    _matrix[i, j].Weight = edgeGenerator.GenerateEdgeWeight(this, i, j);
                }
            }
        }
    }

    public bool ValidIndex(int i, int j)
    {
        return i >= 0 && i < _matrix.GetLength(0) && j >= 0 && j < _matrix.GetLength(i);
    }

    public bool ValidIndex(int i)
    {
        return i >= 0 && i < _vertices.Length;
    }
    
    public bool SetEdgeValue(int i, int j, R value)
    {
        if (!ValidIndex(i, j)) return false;
        _matrix[i, j].Value = value;
        return true;
    }
    
    public bool SetEdge(int i, int j, R value, float weight)
    {
        if (!ValidIndex(i, j)) return false;
        _matrix[i, j].Value = value;
        _matrix[i, j].Weight = weight;
        return true;
    }
    
    public bool SetEdgeWeight(int i, int j, float weight)
    {
        if (!ValidIndex(i, j)) return false;
        _matrix[i, j].Weight = weight;
        return true;
    }
}
