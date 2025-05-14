using ResourceRailNetwork.Graph;
using UnityEngine;

namespace ResourceRailNetwork.Train
{
    public class Train
    {
        public GameObject TrainView;
        public Node LastNode;
        public Node NextNode;
        public Node TargetNode;
        public float Progress;
        public TrainState State;

        public enum TrainState
        {
            Moving,
            Mining,
            Delivering
        }
    }
}