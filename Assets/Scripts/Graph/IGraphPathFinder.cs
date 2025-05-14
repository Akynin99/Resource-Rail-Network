using System.Collections.Generic;

namespace ResourceRailNetwork.Graph
{
    public interface IGraphPathFinder
    {
        public List<Node> FindShortestPath(Node[] allNodes, Node startNode, Node targetNode, out int totalDistance);
    }
}