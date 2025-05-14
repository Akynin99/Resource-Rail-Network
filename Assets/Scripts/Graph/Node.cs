using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceRailNetwork.Graph
{
    public class Node : MonoBehaviour
    {
        [SerializeField] protected Edge[] edges;

        private Vector3 _position;

        public Edge[] Edges => edges;
        public List<PrecalculatedPath> CalculatedPaths { get; private set; }

        public Vector3 Position => _position;

        private void OnDrawGizmos()
        {
            if (edges == null || edges.Length < 1) return;

            foreach (var edge in edges)
            {
                if (edge == null || edge.EndNode == null || edge.Length < 0) continue;
                
                Vector3 startPos = transform.position;
                Vector3 endPos = edge.EndNode.transform.position;

                bool twoWayConnection = edge.EndNode.IsConnectedTo(this);
                
                Gizmos.color = twoWayConnection ? Color.white: Color.yellow;

                if (twoWayConnection)
                {
                    if (edge.Length != edge.EndNode.LengthTo(this)) 
                        Gizmos.color = Color.red; 
                }
                
                Gizmos.DrawLine(startPos, endPos);
            }
        }

        public bool IsConnectedTo(Node node)
        {
            if (node == null || edges == null) return false;

            foreach (var edge in edges)
            {
                if (edge.EndNode == node) return true;
            }

            return false;
        }

        private void Awake()
        {
            _position = transform.position;
        }

        public int LengthTo(Node node)
        {
            if (!IsConnectedTo(node)) return -1;

            foreach (var edge in edges)
            {
                if (edge.EndNode == node) return (int)edge.Length;
            }

            return -1;
        }

        public void AddPrecalculatedPath(PrecalculatedPath path)
        {
            CalculatedPaths ??= new List<PrecalculatedPath>();
            
            CalculatedPaths.Add(path);
        }
    }
}