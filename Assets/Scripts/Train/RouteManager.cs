using System.Collections.Generic;
using ResourceRailNetwork.Core;
using ResourceRailNetwork.Graph;

namespace ResourceRailNetwork.Train
{
    /// <summary>
    /// Manages route calculations and optimization between stations and mines
    /// Maintains cached routes and handles profit calculations
    /// </summary>
    public class RouteManager
    {
        private Route[] _routes;
        private readonly IRailNetworkGraph _graph;

        public RouteManager(IRailNetworkGraph graph)
        {
            _graph = graph;
            CalculateAllRoutes();
        }

        /// <summary>
        /// Builds all possible station-mine route combinations
        /// Should be called when network topology changes
        /// </summary>
        public void CalculateAllRoutes()
        {
            List<BaseStation> baseStations = _graph.GetAllBaseStations();
            List<Mine> mines = _graph.GetAllMines();

            _routes = new Route[baseStations.Count * mines.Count];
            int idx = 0;

            foreach (var baseStation in baseStations)
            {
                foreach (var mine in mines)
                {
                    _routes[idx] = new Route
                    {
                        BaseStation = baseStation,
                        Mine = mine,
                        Distance = _graph.GetDistance(baseStation, mine)
                    };
                    idx++;
                }
            }
        }

        /// <summary>
        /// Finds most profitable route using economic model
        /// </summary>
        public Route FindBestRoute(TrainModel train)
        {
            Route bestRoute = _routes[0];
            float bestProfit = float.MinValue;

            foreach (var route in _routes)
            {
                float profit = CalculateProfitPerSecond(route, train);
                if (profit > bestProfit)
                {
                    bestProfit = profit;
                    bestRoute = route;
                }
            }

            return bestRoute;
        }

        private float CalculateProfitPerSecond(Route route, TrainModel train)
        {
            float duration = route.Distance / train.Speed * 2 + train.MiningDuration * route.Mine.TimeMult;
            return route.BaseStation.ResourceMult / duration;
        }
    }
}