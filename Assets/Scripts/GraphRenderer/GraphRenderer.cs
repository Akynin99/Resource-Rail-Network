using System;
using System.Collections.Generic;
using ResourceRailNetwork.Graph;
using UnityEngine;
using Zenject;

namespace ResourceRailNetwork.GraphRenderer
{
    public class GraphRenderer : MonoBehaviour, IGraphRenderer
    {
        [SerializeField] private LineRenderer correctEdgeRendererPrefab;
        [SerializeField] private LineRenderer oneWayEdgeRendererPrefab;
        [SerializeField] private LineRenderer badLengthEdgeRendererPrefab;
        [SerializeField] private float yOffset;

        [Inject] private IRailNetworkGraph _graph;

        private LineRenderer[] _edgeRenderers;

        private void Start()
        {
            Init();
        }

        private void Init()
        {
            Node[] nodes = _graph.GetAllNodes();
            
            List<GraphEdge> edges = GraphRendererEdgeGenerator.CreateEdges(nodes);
            
            if (edges == null || edges.Count < 1) return;

            GameObject folderGO = new GameObject
            {
                transform =
                {
                    parent = transform
                },
                name = "EdgeRenderers"
            };

            foreach (var edge in edges)
            {
                LineRenderer prefab;

                switch (edge.Type)
                {
                    case GraphEdge.GraphEdgeType.Correct:
                        prefab = correctEdgeRendererPrefab;
                        break;
                    
                    case GraphEdge.GraphEdgeType.OneWay:
                        prefab = oneWayEdgeRendererPrefab;
                        break;
                    
                    case GraphEdge.GraphEdgeType.IncorrectLength:
                        prefab = badLengthEdgeRendererPrefab;
                        break;
                    
                    default:
                        prefab = correctEdgeRendererPrefab;
                        break;
                }

                LineRenderer lineRenderer = Instantiate(prefab, folderGO.transform);
                
                lineRenderer.positionCount = 2;
                
                
                
                lineRenderer.SetPosition(0, edge.StartNode.Position + Vector3.up * yOffset);
                lineRenderer.SetPosition(1, edge.EndNode.Position + Vector3.up * yOffset);
            }
        }
    }
}