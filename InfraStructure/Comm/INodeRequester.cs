using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.InfraStructure.Comm
{
    public interface INodeRequester
    {
        void Update(string response);
    }
}
