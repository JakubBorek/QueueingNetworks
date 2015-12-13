using QueueingNetworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gui
{
    class GraphNode
    {
        public bool ColoredRed { get; set; }
        public bool ColoredBlue { get; set; }
        public string Text { get; private set; }
        public IReadOnlyCollection<GraphNode> Children { get { return _children; } }
        private List<GraphNode> _children;

        public GraphNode(Node node, int id)
        {
            this.Text = text;
            this._children = new List<TreeGraphNode>();
        }



        public void AddChild(TreeGraphNode child)
        {
            _children.Add(child);
        }

        public IEnumerable<IEdge<TreeGraphNode>> GetSharpGraphEdges()
        {
            var edges = new List<IEdge<TreeGraphNode>>();
            foreach (var child in Children)
            {
                edges.Add(new Edge<TreeGraphNode>(this, child));
            }
            return edges;
        }

        public override string ToString()
        {
            return Text;
        }
    }
}
