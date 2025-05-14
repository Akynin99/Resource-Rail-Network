using System;
using UnityEngine;

namespace ResourceRailNetwork.Train
{
    public class TrainSettings : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float miningTime;

        private float _lastSpeed;
        private float _lastMiningTime;
        private TrainController _trainController;
        private TrainModel _trainModel;
        
        public float Speed
        {
            get
            {
                if (speed < 0) return 0;
                return speed;
            }
        }

        public float MiningTime
        {
            get
            {
                if (miningTime < 0) return 0;
                return miningTime;
            }
        }

        public void Init(float initialSpeed, float initialMiningTime, TrainController trainController, TrainModel trainModel)
        {
            speed = initialSpeed;
            _lastSpeed = speed;
            miningTime = initialMiningTime;
            _lastMiningTime = miningTime;
            _trainController = trainController;
            _trainModel = trainModel;
        }
        
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying || !_trainController) return;

            if (Mathf.Approximately(_lastSpeed, speed) || Mathf.Approximately(_lastMiningTime, miningTime))
            {
                _lastSpeed = speed;
                _lastMiningTime = miningTime;
                
                _trainController.OnTrainSettingsChanged(_trainModel);
            }
        }
#endif
    }
}