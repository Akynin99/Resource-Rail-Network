using UnityEngine;

namespace ResourceRailNetwork.Train
{
    public class TrainController : MonoBehaviour
    {
        [SerializeField] private TrainConfig[] trainConfigs;

        private Train[] _trains;

        private void Init()
        {
            SpawnTrains();
        }

        private void SpawnTrains()
        {
            _trains = new Train[trainConfigs.Length];

            for (int i = 0; i < _trains.Length; i++)
            {
                _trains[i] = SpawnTrain(trainConfigs[i]);
            }
        }

        private Train SpawnTrain(TrainConfig config)
        {
            GameObject view = Instantiate(config.Prefab, transform);
            Train train = new Train()
            {
                TrainView = view,
                State = Train.TrainState.Moving
            };
            
            
            
            return train;
        }
    }
}