using ResourceRailNetwork.Graph;
using UnityEngine;

namespace ResourceRailNetwork.Train
{
    public class TrainSpawner
    {
        public TrainModel[] SpawnTrains(TrainConfig[] trainConfigs, Transform trainsParent, IRailNetworkGraph graph,
            TrainController trainController)
        {
            TrainModel[] trains = new TrainModel[trainConfigs.Length];

            for (int i = 0; i < trains.Length; i++)
            {
                trains[i] = SpawnTrain(trainConfigs[i], trainsParent, graph, trainController);
            }

            return trains;
        }

        private TrainModel SpawnTrain(TrainConfig config, Transform trainsParent, IRailNetworkGraph graph,
            TrainController trainController)
        {
            Node randomNode = graph.GetRandomNode();

            GameObject trainView = Object.Instantiate(config.Prefab, randomNode.transform.position, Quaternion.identity,
                trainsParent);

            var trainSettings = trainView.AddComponent<TrainSettings>();
            var trainDebugInfo = trainView.AddComponent<TrainDebugInfo>();

            TrainModel trainModel = new TrainModel(trainSettings, trainDebugInfo, trainView, randomNode);
            trainSettings.Init(config.Speed, config.MiningTime, trainController, trainModel);
            
            return trainModel;
        }
    }
}