using System;
using System.Collections.Generic;
using ResourceRailNetwork.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ResourceRailNetwork.Graph
{
    /// <summary>
    /// Central graph manager handling rail network topology. Maintains node relationships,
    /// precomputes critical paths, and provides graph traversal utilities. Implements
    /// observer pattern for real-time graph change notifications.
    /// </summary>
    public class RailNetworkGraph : MonoBehaviour, IRailNetworkGraph
    {
        [SerializeField] private Node[] allNodes;

        private readonly List<Mine> _allMines = new();
        private readonly List<BaseStation> _allBaseStations = new();
        private readonly List<Node> _specificNodes = new();
        
        private readonly IGraphPathFinder _pathFinder = new DijkstraPathFinder();

        private void Awake()
        {
            // Initialize node tracking and event subscriptions
            foreach (var node in allNodes)
            {
                node.OnSettingsChanged += OnSettingsChanged;
                
                // Categorize special node types
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

        /// <summary>
        /// Handle graph changes by notifying subscribers through event
        /// </summary>
        private void OnSettingsChanged() => OnGraphUpdated?.Invoke();

        private void OnDestroy()
        {
            foreach (var node in allNodes)
            {
                node.OnSettingsChanged -= OnSettingsChanged;
            }
        }
        
        /// <summary>
        /// Precompute paths between all nodes and special nodes (Mines/BaseStations)
        /// Optimizes frequent path queries at runtime
        /// </summary>
        private void PrecalculatePaths()
        {
            foreach (var node in allNodes)
            {
                foreach (var specificNode in _specificNodes)
                {
                    if (node == specificNode) continue; // Skip self-connections

                    int distance;
                    List<Node> path = _pathFinder.FindShortestPath(allNodes, node, specificNode, out distance);

                    PrecalculatedPath precalculatedPath = new PrecalculatedPath()
                    {
                        TargetNode = specificNode,
                        NextNodeInPath = path[1], // First hop in path
                        Path = path,
                        Distance = distance,
                    };
                    
                    node.AddPrecalculatedPath(precalculatedPath);
                }
            }
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

        /// <summary>
        /// Get distance between nodes using precomputed paths when available
        /// </summary>
        /// <returns>-1 if no path exists</returns>
        public int GetDistance(Node start, Node end)
        {
            if (start == end) return 0;
            
            if (_specificNodes.Contains(end))
            {
                // this is already calculated path

                foreach (var calculatedPath in start.CalculatedPaths)
                {
                    if (calculatedPath.TargetNode == end) return calculatedPath.Distance;
                }
            }

            // Fallback to real-time calculation
            _pathFinder.FindShortestPath(allNodes, start, end, out var distance);
            return distance;
        }

        /// <summary>
        /// Get immediate next node in path to target
        /// </summary>
        /// <param name="path">Full node sequence output (if needed)</param>
        public Node GetNextNode(Node start, Node end, out List<Node> path)
        {
            if (_specificNodes.Contains(end))
            {
                // this is already calculated path

                foreach (var calculatedPath in start.CalculatedPaths)
                {
                    
                    if (calculatedPath.TargetNode != end) continue;

                    path = calculatedPath.Path;
                    return calculatedPath.NextNodeInPath;
                }
            }

            path = _pathFinder.FindShortestPath(allNodes, start, end, out _);

            return path[1];
        }

        /// <summary>
        /// Event triggered when parameters modified
        /// </summary>
        public event Action OnGraphUpdated;
    }
}