using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ResourceRailNetwork.Graph
{
    public class GraphManager : MonoBehaviour
    {
        [SerializeField] private Node[] allNodes;
        [SerializeField] private GraphRenderer.GraphRenderer graphRenderer;
        
        private IGraphPathFinder _pathFinder = new DijkstraPathFinder();

        private void Awake()
        {
            graphRenderer.Init(allNodes);
            
            PrecalculatePaths();
        }

        private void PrecalculatePaths()
        {
            List<Node> specificNodes = new List<Node>();
            
            foreach (var node in allNodes)
            {
                Mine mine = node as Mine;
                if (mine != null)
                {
                    specificNodes.Add(mine);
                    continue;
                }
                
                BaseStation baseStation = node as BaseStation;
                if (baseStation != null)
                {
                    specificNodes.Add(baseStation);
                }
            }

            foreach (var node in allNodes)
            {
                foreach (var specificNode in specificNodes)
                {
                    if (node == specificNode) continue;

                    int distance;
                    List<Node> path = _pathFinder.FindShortestPath(allNodes, node, specificNode, out distance);

                    PrecalculatedPath precalculatedPath = new PrecalculatedPath()
                    {
                        TargetNode = specificNode,
                        NextNodeInPath = path[1],
                        Distance = distance,
                    };
                    
                    node.AddPrecalculatedPath(precalculatedPath);
                }
            }
        }

        

        public Node[] FindPath()
        {
            return null;
        }
    }
}