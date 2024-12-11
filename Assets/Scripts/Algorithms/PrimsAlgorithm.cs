using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
using Utils;

namespace Algorithms
{
    // An implementation of the Prim's MST algorithm.
    public class PrimsAlgorithm
    {
        // Create the algorithm.
        // The bool[,] array contains true wherever a unidirectional edge
        // is included in the MST.
        // This algorithm runs in log(n) * n^2 time, if n is the number of
        // vertices in the graph. Equivalently, it runs in log(n) * |E| time, where
        // E is the set of edges.
        public static bool[,] MST<T, R>(GraphMatrix<T, R> graph, int startNode)
        {
            // initialize the tree
            bool[,] tree = new bool[graph.Size, graph.Size];
            for (int i = 0; i < graph.Size; i++)
            {
                for (int j = 0; j < graph.Size; j++)
                {
                    tree[i, j] = false;
                }
            }

            // create an array of covered vertices
            // create the half of the cut that we're mapping onto
            bool[] covered = new bool[graph.Size];
            List<int> cutOnto = new List<int>();
            covered[startNode] = true;
            for (int i = 0; i < graph.Size; i++)
            {
                if (i != startNode)
                {
                    cutOnto.Add(i);
                    covered[i] = false;
                }
            }
            
            // initialize the queue
            PriorityQueue<GraphEdge<R>, float> queue = new PriorityQueue<GraphEdge<R>, float>();
            // add the edges from the source to the queue
            EnqueueEdgesOnCut(graph, startNode, cutOnto, queue);
            
            //Debug.Log("Running MST on " + graph.Size + " vertices.");
            //Debug.Log("Covered: " + ToPrintCovered(covered));
            
            // while the queue is not empty and the onto cut is not empty
            while (cutOnto.Count > 0 && queue.Count > 0)
            {
                // dequeue
                GraphEdge<R> edge = queue.Dequeue();
                // ensure the target edge is correct and we have covered the source vertex
                if (covered[edge.From] && !covered[edge.To])
                {
                    // add to the tree
                    tree[edge.From, edge.To] = true;
                    // cover it
                    covered[edge.To] = true;
                    // remove the onto cut
                    cutOnto.Remove(edge.To);
                    // add the edges to the queue
                    EnqueueEdgesOnCut(graph, edge.To, cutOnto, queue);
                    //Debug.Log("Adding edge (" + edge.From + ", " + edge.To + ")");
                    //Debug.Log("Covered: " + ToPrintCovered(covered));
                }
            }

            return tree;
        }

        public static string ToPrintCovered(bool[] covered)
        {
            string output = "";
            for (int i = 0; i < covered.Length; i++)
            {
                if (covered[i])
                {
                    if (string.IsNullOrEmpty(output))
                    {
                        output = "" + i;
                    }
                    else
                    {
                        output += ", " + i;
                    }
                }
            }

            return output;
        }

        protected static void EnqueueEdgesOnCut<T, R>(GraphMatrix<T, R> graph, int source, List<int> onto, PriorityQueue<GraphEdge<R>, float> queue)
        {
            for (int i = 0; i < onto.Count; i++)
            {
                queue.Enqueue(graph[source, onto[i]], graph[source, onto[i]].Weight);
                queue.Enqueue(graph[onto[i], source], graph[onto[i], source].Weight);
            }
        }
    }
}