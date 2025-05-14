using System.Collections.Generic;
using ResourceRailNetwork.Graph;
using UnityEngine;

namespace ResourceRailNetwork.GraphRenderer
{
    public class GraphRenderer : MonoBehaviour
    {
        [SerializeField] private LineRenderer correctEdgeRendererPrefab;
        [SerializeField] private LineRenderer oneWayEdgeRendererPrefab;
        [SerializeField] private LineRenderer badLengthEdgeRendererPrefab;
        [SerializeField] private float yOffset;

        private LineRenderer[] _edgeRenderers;
        
        public void Init(Node[] nodes)
        {
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
                
                
                
                lineRenderer.SetPosition(0, edge.StartNode.transform.position + Vector3.up * yOffset);
                lineRenderer.SetPosition(1, edge.EndNode.transform.position + Vector3.up * yOffset);
            }
        }
    }
}