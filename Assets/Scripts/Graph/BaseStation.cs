using UnityEngine;

namespace ResourceRailNetwork.Graph
{
    public class BaseStation : Node
    {
        [SerializeField] private float resourceMult;
        
        public float ResourceMult => resourceMult;
    }
}