using UnityEngine;

namespace ResourceRailNetwork.Train
{
    public class TrainSettings : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float miningTime;
        
        public float Speed => speed;
        public float MiningTime => miningTime;

        public void Init(float initialSpeed, float initialMiningTime)
        {
            speed = initialSpeed;
            miningTime = initialMiningTime;
        }
    }
}