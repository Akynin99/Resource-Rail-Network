using System.Collections.Generic;

namespace ResourceRailNetwork.Graph
{
    public struct PrecalculatedPath
    {
        public Node TargetNode;
        public Node NextNodeInPath;
        public List<Node> Path;
        public int Distance;
    }
}