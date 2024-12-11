using UnityEngine;

public interface GraphEdgeGenerator <T, R>
{
    public float GenerateEdgeWeight(GraphMatrix<T, R> graph, int i, int j);
    public R GenerateEdgeValue(GraphMatrix<T, R> graph, int i, int j);
}
