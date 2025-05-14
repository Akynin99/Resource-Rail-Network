using System;
using UnityEngine;

namespace ResourceRailNetwork.Core
{
    /// <summary>
    /// Represents a directional connection between nodes in a rail network. Defines traversal cost/distance
    /// for pathfinding calculations. Serialized for editor configuration.
    /// </summary>
    [Serializable]
    public class Edge
    {
        [SerializeField] private Node endNode;
        [SerializeField] private uint length;
        
        public Node EndNode => endNode;
        public uint Length => length;
    }
}