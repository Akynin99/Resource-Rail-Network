using System.Collections.Generic;
using ResourceRailNetwork.Core;

namespace ResourceRailNetwork.Graph
{
    /// <summary>
    /// Dijkstra's algorithm implementation for finding shortest paths in a node-based graph.
    /// Uses iterative approach with O(n^2) time complexity (suitable for small-medium graphs).
    /// Maintains distance tracking and path reconstruction through predecessor nodes.
    /// </summary>
    public class DijkstraPathFinder : IGraphPathFinder
    {
        public List<Node> FindShortestPath(Node[] allNodes, Node startNode, Node targetNode, out int totalDistance)
        {
            // Initialization phase
            Dictionary<Node, uint> distances = new Dictionary<Node, uint>();
            Dictionary<Node, Node> previousNodes = new Dictionary<Node, Node>();
            List<Node> unvisitedNodes = new List<Node>();
            totalDistance = -1; // Default invalid distance

            // Set initial state: infinite distance, no predecessors
            foreach (Node node in allNodes)
            {
                distances[node] = int.MaxValue;
                previousNodes[node] = null;
                unvisitedNodes.Add(node);
            }

            distances[startNode] = 0; // Zero-distance from start to itself

            // Main processing loop - visits nodes in optimal order
            while (unvisitedNodes.Count > 0)
            {
                // Find unvisited node with smallest tentative distance
                Node currentNode = null;
                uint minDistance = uint.MaxValue;
                foreach (Node node in unvisitedNodes)
                {
                    if (distances[node] < minDistance)
                    {
                        minDistance = distances[node];
                        currentNode = node;
                    }
                }

                if (currentNode == null) break; // No reachable nodes left

                unvisitedNodes.Remove(currentNode);

                // Explore neighbors through edges
                for (int i = 0; i < currentNode.Edges.Length; i++)
                {
                    Node neighbor = currentNode.Edges[i].EndNode;
                    uint edgeWeight = currentNode.Edges[i].Length;

                    // Calculate tentative distance through current node
                    uint tentativeDistance = distances[currentNode] + edgeWeight;

                    // Found better path to neighbor
                    if (tentativeDistance < distances[neighbor])
                    {
                        distances[neighbor] = tentativeDistance;
                        previousNodes[neighbor] = currentNode; // Update optimal predecessor
                    }
                }
            }

            // Path reconstruction - walk back from target using predecessors
            List<Node> path = new List<Node>();
            Node current = targetNode;

            while (current != null)
            {
                path.Insert(0, current); // Build path in reverse order
                current = previousNodes[current];
            }
            
            // Validate path integrity
            if (path[0] == startNode)
            {
                totalDistance = (int)distances[targetNode];
                return path;
            }

            return null; // No valid path exists
        }
    }
}