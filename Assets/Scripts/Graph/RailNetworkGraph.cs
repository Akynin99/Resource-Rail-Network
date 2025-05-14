using System;
using System.Collections.Generic;
using ResourceRailNetwork.GraphRenderer;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;
using Random = UnityEngine.Random;

namespace ResourceRailNetwork.Graph
{
    public class RailNetworkGraph : MonoBehaviour, IRailNetworkGraph
    {
        [SerializeField] private Node[] allNodes;

        private readonly List<Mine> _allMines = new();
        private readonly List<BaseStation> _allBaseStations = new();
        private readonly List<Node> _specificNodes = new();
        
        private readonly IGraphPathFinder _pathFinder = new DijkstraPathFinder();

        private void Awake()
        {
            foreach (var node in allNodes)
            {
                Mine mine = node as Mine;
                if (mine != null)
                {
                    _allMines.Add(mine);
                    _specificNodes.Add(mine);
                    continue;
                }
                
                BaseStation baseStation = node as BaseStation;
                if (baseStation != null)
                {
                    _allBaseStations.Add(baseStation);
                    _specificNodes.Add(baseStation);
                }
            }
            
            PrecalculatePaths();
        }

        private void PrecalculatePaths()
        {
            foreach (var node in allNodes)
            {
                foreach (var specificNode in _specificNodes)
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

        public Node[] GetAllNodes()
        {
            return allNodes;
        }

        public Node GetRandomNode()
        {
            int random = Random.Range(0, allNodes.Length);

            return allNodes[random];
        }
        
        public List<Node> GetAllSpecificNodes()
        {
            return _specificNodes;
        }

        public List<Mine> GetAllMines()
        {
            return _allMines;
        }

        public List<BaseStation> GetAllBaseStations()
        {
            return _allBaseStations;
        }

        public int GetDistance(Node start, Node end)
        {
            if (_specificNodes.Contains(end))
            {
                // this is already calculated path

                foreach (var calculatedPath in start.CalculatedPaths)
                {
                    if (calculatedPath.TargetNode == end) return calculatedPath.Distance;
                }
            }

            _pathFinder.FindShortestPath(allNodes, start, end, out var distance);

            return distance;
        }

        public Node GetNextNode(Node start, Node end)
        {
            if (_specificNodes.Contains(end))
            {
                // this is already calculated path

                foreach (var calculatedPath in start.CalculatedPaths)
                {
                    if (calculatedPath.TargetNode == end) return calculatedPath.NextNodeInPath;
                }
            }

            List<Node> path = _pathFinder.FindShortestPath(allNodes, start, end, out _);

            return path[1];
        }
    }
}