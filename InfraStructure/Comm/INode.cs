using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.InfraStructure.Comm
{
    public interface INode
    {
        void PerformTransactionNet(INodeRequester requester, string sender, string receiver, decimal amount);
        void PrepareInfoToSend(INodeRequester requester,string code);
    }
}
