using System;
using System.Collections.Generic;

namespace Gui
{
    interface INodeFromIdProvider
    {
        Node FromId(int id);
        bool isValidId(int id);
        int getNodesCount();
    }
    class NodeFromIdProvider  : INodeFromIdProvider
    {
        private List<Node> nodes;
        public NodeFromIdProvider(List<Node> nodes)
        {
            this.nodes = nodes;
        }

        public Node FromId(int id)
        {
           return nodes[id-1];
        }

        public int getNodesCount()
        {
            return nodes.Count;
        }

        public bool isValidId(int id)
        {
            return id < nodes.Count;
        }
    }
}