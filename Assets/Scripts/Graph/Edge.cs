using System;
using UnityEngine;

namespace ResourceRailNetwork.Graph
{
    [Serializable]
    public class Edge
    {
        [SerializeField] private Node endNode;
        [SerializeField] private uint length;
        
        public Node EndNode => endNode;
        public uint Length => length;
    }
}