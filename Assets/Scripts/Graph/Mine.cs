using UnityEngine;

namespace ResourceRailNetwork.Graph
{
    public class Mine : Node
    {
        [SerializeField] private float timeMult;
        public float TimeMult =>timeMult;
    }
}