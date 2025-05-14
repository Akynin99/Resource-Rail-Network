using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResourceRailNetwork.Core
{
    /// <summary>
    /// Core network node managing connections and path data. Handles edges, stores precalculated paths,
    /// provides position information, and triggers events on changes. Includes editor visualization for connections.
    /// </summary>
    public class Node : MonoBehaviour
    {
        [SerializeField] protected Edge[] edges;

        private Vector3 _position;

        public Edge[] Edges => edges;
        public List<PrecalculatedPath> CalculatedPaths { get; private set; }

        public Vector3 Position => _position;

        public event Action OnSettingsChanged;

        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;
            
            if (edges == null || edges.Length < 1) return;

            foreach (var edge in edges)
            {
                if (edge == null || edge.EndNode == null) continue;
                
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
        
        
        
        protected virtual void Awake()
        {
            _position = transform.position;
        }

        protected void SettingsChanged()
        {
            OnSettingsChanged?.Invoke();
        }
        
        public void CheckEdges()
        {
            foreach (var edge in edges)
            {
                if (edge == null || edge.EndNode == null) continue;

                bool twoWayConnection = edge.EndNode.IsConnectedTo(this);

                if (!twoWayConnection)
                {
                    Debug.LogError("Incorrect graph topology!");
                }

                if (twoWayConnection && edge.Length != edge.EndNode.LengthTo(this))
                {
                    Debug.LogError("Incorrect edges length!");
                }
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