using Unity.VisualScripting;
using UnityEngine;

// Represents an edge in a graph, storing the index from and to for the vertex
// and stores the weight and edge value, according to type parameter R.
public class GraphEdge <R>
{
    protected float _weight;
    protected int _from;
    protected int _to;
    protected R _value;
    public float Weight
    {
        get => _weight;
        set => _weight = value;
    }

    public int From => _from;

    public int To => _to;

    public R Value
    {
        get => _value;
        set => _value = value;
    }

    public GraphEdge(int from, int to, float weight = 0, R value = default(R))
    {
        _weight = weight;
        _from = from;
        _to = to;
        _value = value;
    }
}
