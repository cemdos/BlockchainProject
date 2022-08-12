using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.InfraStructure.Comm
{
    public abstract class NodeRequester : INodeRequester
    {
        private IList<INode>? _Nodes;

        public IList<INode> Nodes
        {
            get
            {
                if (_Nodes == null)
                {
                    _Nodes = new List<INode>();
                    return _Nodes;
                }
                return _Nodes;
            }
            set { _Nodes = value; }
        }

        public void Update(string response)
        {
            throw new NotImplementedException();
        }

        public void RegisterNode(INode neigh)
        {
            if (Nodes == null)
                Nodes = new List<INode>();
            Nodes.Add(neigh);
        }
    }
}
