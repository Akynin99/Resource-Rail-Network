using ResourceRailNetwork.Core;
using ResourceRailNetwork.Graph;

namespace ResourceRailNetwork.GraphRenderer
{
    public struct GraphEdge
    {
        public enum GraphEdgeType
        {
            Correct,
            OneWay,
            IncorrectLength,
        }
        
        public GraphEdgeType Type;
        public Node StartNode;
        public Node EndNode;
    }
}