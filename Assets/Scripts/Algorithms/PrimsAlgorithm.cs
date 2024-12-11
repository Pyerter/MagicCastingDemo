using System.Collections.Generic;
using Utils;

namespace Algorithms
{
    public class PrimsAlgorithm
    {
        public static bool[,] MST<T, R>(GraphMatrix<T, R> graph, int startNode)
        {
            bool[,] tree = new bool[graph.Size, graph.Size];
            for (int i = 0; i < graph.Size; i++)
            {
                for (int j = 0; j < graph.Size; j++)
                {
                    tree[i, j] = false;
                }
            }

            bool[] covered = new bool[graph.Size];
            List<int> cutOnto = new List<int>();
            covered[startNode] = true;
            for (int i = 0; i < graph.Size; i++)
            {
                if (i != startNode) cutOnto.Add(i);
            }
            
            PriorityQueue<GraphEdge<R>, float> queue = new PriorityQueue<GraphEdge<R>, float>();
            
            EnqueueEdgesFrom(graph, startNode, cutOnto, queue);
            while (cutOnto.Count > 0 && queue.Count > 0)
            {
                GraphEdge<R> edge = queue.Dequeue();
                if (!covered[edge.To])
                {
                    tree[edge.From, edge.To] = true;
                    covered[edge.To] = true;
                    cutOnto.Remove(edge.To);
                    EnqueueEdgesFrom(graph, edge.To, cutOnto, queue);
                }
            }

            return tree;
        }

        protected static void EnqueueEdgesFrom<T, R>(GraphMatrix<T, R> graph, int source, List<int> onto, PriorityQueue<GraphEdge<R>, float> queue)
        {
            for (int i = 0; i < onto.Count; i++)
            {
                queue.Enqueue(graph[source, i], graph[source, i].Weight);
            }
        }
    }
}