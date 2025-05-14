using UnityEngine;

namespace ResourceRailNetwork.Train
{
    [CreateAssetMenu(fileName = "TrainConfig", menuName = "ResourceRailNetwork/TrainConfig", order = 0)]
    public class TrainConfig : ScriptableObject
    {
        [SerializeField] private GameObject prefab;
        [SerializeField] private float speed;
        [SerializeField] private float miningTime;

        public GameObject Prefab => prefab;
        public float Speed => speed;
        public float MiningTime => miningTime;
    }
}