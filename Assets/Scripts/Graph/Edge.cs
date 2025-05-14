using System;
using UnityEngine;

namespace ResourceRailNetwork.Graph
{
    [Serializable]
    public class Edge
    {
        [SerializeField] private Node endNode;
        [SerializeField] private int length;
        
        public Node EndNode => endNode;
        public int Length => length;
    }
}