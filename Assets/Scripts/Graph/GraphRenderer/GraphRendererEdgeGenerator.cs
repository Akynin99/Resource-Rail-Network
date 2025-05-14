using System.Collections.Generic;

namespace ResourceRailNetwork.Graph.GraphRenderer
{
    public static class GraphRendererEdgeGenerator
    {
        public static List<GraphEdge> CreateEdges(Node[] nodes)
        {
            List<GraphEdge> edges = new List<GraphEdge>();

            List<Node> nodesPassed = new List<Node>();

            foreach (var node in nodes)
            {
                if (node == null || node.Edges == null || node.Edges.Length < 1) continue;
                
                edges.AddRange(CreateEdgesForNode(node, nodesPassed));
                nodesPassed.Add(node);
            }

            return edges;
        }

        private static List<GraphEdge> CreateEdgesForNode(Node node, List<Node> excludeNodes)
        {
            List<GraphEdge> rendererEdges = new List<GraphEdge>();

            foreach (var edge in node.Edges)
            {
                GraphEdge newGraphEdge = new GraphEdge
                {
                    StartNode = node,
                    EndNode = edge.EndNode
                };

                bool twoWay = edge.EndNode.IsConnectedTo(node);

                bool correctLength = !twoWay || edge.EndNode.LengthTo(node) == edge.Length;

                if (twoWay)
                {
                    newGraphEdge.Type = correctLength
                        ? GraphEdge.GraphEdgeType.Correct
                        : GraphEdge.GraphEdgeType.IncorrectLength;
                }
                else
                {
                    newGraphEdge.Type = GraphEdge.GraphEdgeType.OneWay;
                }

                if (!twoWay || !excludeNodes.Contains(edge.EndNode))
                {
                    rendererEdges.Add(newGraphEdge);
                }
            }

            return rendererEdges;
        }
    }
}