using System;
using System.Collections.Generic;
using ResourceRailNetwork.Core;

namespace ResourceRailNetwork.Graph
{
    public interface IRailNetworkGraph
    {
        public Node[] GetAllNodes();
        public Node GetRandomNode();
        public List<Mine> GetAllMines();
        public List<BaseStation> GetAllBaseStations();
        public int GetDistance(Node start, Node end);
        public Node GetNextNode(Node start, Node end, out List<Node> path);
        public event Action OnGraphUpdated;
    }
}