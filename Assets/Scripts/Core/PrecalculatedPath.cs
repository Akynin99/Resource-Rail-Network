using System.Collections.Generic;

namespace ResourceRailNetwork.Core
{
    /// <summary>
    /// Stores precomputed path data between nodes. Contains target destination, immediate next hop,
    /// full node sequence, and total traversal distance for optimized pathfinding queries.
    /// </summary>
    public struct PrecalculatedPath
    {
        public Node TargetNode;
        public Node NextNodeInPath;
        public List<Node> Path;
        public int Distance;
    }
}