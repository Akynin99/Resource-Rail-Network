using UnityEngine;

namespace ResourceRailNetwork.Train
{
    /// <summary>
    /// Handles debug visualization and runtime information display
    /// Active only in Unity Editor development builds
    /// </summary>
    public class TrainDebugger
    {
        public void RefreshDebugInfo(TrainModel train)
        {
#if UNITY_EDITOR
            string status = train.State switch
            {
                TrainModel.TrainState.Mining =>
                    $"{train.State}, Timer: {(int)train.MiningTimer} / {train.MiningDuration * train.LastMine.TimeMult}",
                _ => $"{train.State}, Progress: {train.Progress:P0}"
            };

            train.DebugInfo.SetText(status);
#endif
        }

        public void DrawGizmos(TrainModel[] trains)
        {
            if (!Application.isPlaying) return;

            foreach (var train in trains)
            {
                train.OnDrawGizmos();
            }
        }
    }
}