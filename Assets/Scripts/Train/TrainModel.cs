﻿using System.Collections.Generic;
using ResourceRailNetwork.Core;
using ResourceRailNetwork.Graph;
using UnityEngine;

namespace ResourceRailNetwork.Train
{
    /// <summary>
    /// Represents a train entity in the rail network. Maintains train state,
    /// movement parameters, and visual representation. Handles path following
    /// and resource transportation logic.
    /// </summary>
    public class TrainModel
    {
        public TrainSettings TrainSettings { get; }
        public TrainDebugInfo DebugInfo { get; }
        public Node LastNode { get; private set; }
        public Mine LastMine { get; private set; }
        public Node NextNode { get; private set; }
        public float Progress { get; private set; }
        public TrainState State { get; private set; }
        public Route BestRoute { get; private set; }
        public List<Node> CurrentPath { get; private set; }
        public bool HasCargo { get; private set; }
        public float MiningTimer { get; private set; }
        
        private GameObject View { get; }
        
        public float Speed => TrainSettings.Speed;
        public float MiningDuration => TrainSettings.MiningTime;

        public enum TrainState
        {
            Moving,
            Mining,
            Delivering
        }

        public TrainModel(TrainSettings trainSettings, TrainDebugInfo debugInfo, GameObject trainView, Node startNode)
        {
            TrainSettings = trainSettings;
            DebugInfo = debugInfo;
            View = trainView;
            LastNode = startNode;
            State = TrainState.Moving;
            Progress = 0;
            HasCargo = false;
        }

        public void SetRoute(Route route)
        {
            BestRoute = route;
        }

        public void SetNextNode(Node node)
        {
            NextNode = node;
        }
        
        public void SetLastNode(Node node)
        {
            LastNode = node;
        }
        
        public void ResetProgress()
        {
            Progress = 0;
        }
        
        public void IncrementProgress(float deltaTime)
        {
            Progress += deltaTime;
        }
        
        public void SetProgress(float progress)
        {
            Progress = progress;
        }
        
        public void ResetMiningTimer()
        {
            MiningTimer = 0;
        }
        
        public void IncrementMiningTimer(float deltaTime)
        {
            MiningTimer += deltaTime;
        }
        
        public void SetCargo(bool value)
        {
            HasCargo = value;
        }
        
        public void SetLastMine(Mine mine)
        {
            LastMine = mine;
        }

        public void RefreshView()
        {
            if (NextNode == null)
            {
                View.transform.position = LastNode.Position;
                
                return;
            }
            
            Vector3 pos  = Vector3.Lerp(LastNode.Position, NextNode.Position, Progress);
            View.transform.position = pos;
            View.transform.forward = NextNode.Position - LastNode.Position;
        }

        public void SetState(TrainState state)
        {
            State = state;
        }

        public void SetCurrentPath(List<Node> nodes)
        {
            CurrentPath = nodes;
        }
        
        public void OnDrawGizmos()
        {
            if (State == TrainState.Mining) return;
            
            if (CurrentPath == null) return;
            
            Gizmos.color = Color.blue;

            Vector3 lastPoint = View.transform.position;

            for (int i = 1; i < CurrentPath.Count; i++)
            {
                Vector3 nextPoint = CurrentPath[i].Position;
                Gizmos.DrawLine(lastPoint, nextPoint);
                lastPoint = nextPoint;
            }
        }
    }
}