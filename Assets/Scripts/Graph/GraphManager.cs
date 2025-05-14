using System;
using UnityEngine;

namespace ResourceRailNetwork.Graph
{
    public class GraphManager : MonoBehaviour
    {
        [SerializeField] private Node[] nodes;
        [SerializeField] private GraphRenderer.GraphRenderer graphRenderer;

        private void Awake()
        {
            graphRenderer.Init(nodes);
        }

        public Node[] FindPath()
        {
            return null;
        }
    }
}