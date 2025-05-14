using System.Collections.Generic;

namespace ResourceRailNetwork.Graph
{
    public class DijkstraPathFinder : IGraphPathFinder
    {
        public List<Node> FindShortestPath(Node[] allNodes, Node startNode, Node targetNode, out int totalDistance)
        {
            Dictionary<Node, uint> distances = new Dictionary<Node, uint>();
            Dictionary<Node, Node> previousNodes = new Dictionary<Node, Node>();
            List<Node> unvisitedNodes = new List<Node>();
            totalDistance = -1;

            foreach (Node node in allNodes)
            {
                distances[node] = int.MaxValue;
                previousNodes[node] = null;
                unvisitedNodes.Add(node);
            }

            distances[startNode] = 0;

            while (unvisitedNodes.Count > 0)
            {
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

                if (currentNode == null) break;

                unvisitedNodes.Remove(currentNode);

                for (int i = 0; i < currentNode.Edges.Length; i++)
                {
                    Node neighbor = currentNode.Edges[i].EndNode;
                    uint edgeWeight = currentNode.Edges[i].Length;

                    uint tentativeDistance = distances[currentNode] + edgeWeight;

                    if (tentativeDistance < distances[neighbor])
                    {
                        distances[neighbor] = tentativeDistance;
                        previousNodes[neighbor] = currentNode;
                    }
                }
            }

            List<Node> path = new List<Node>();
            Node current = targetNode;

            while (current != null)
            {
                path.Insert(0, current);
                current = previousNodes[current];
            }
            
            if (path[0] == startNode)
            {
                totalDistance = (int)distances[targetNode];
                return path;
            }

            return null;
        }
    }
}