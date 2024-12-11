using UnityEngine;

public interface GraphEdgeGenerator <T, R>
{
    public float GenerateEdgeWeight(T[] v, int i, int j);
    public R GenerateEdgeValue(T[] v, int i, int j);
}
