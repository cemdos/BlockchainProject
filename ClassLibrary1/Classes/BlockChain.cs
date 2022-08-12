using BlockChain.InfraStructure.Comm;
using BlockChain.InfraStructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Businesss.Classes
{
    public class BlockChain : NodeRequester, INode
    {
        public List<Block> Blocks { get; set; }
        public List<Transaction> TempTransactions { get; set; }
        public const int Difficulty = 4;
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public BlockChain()
        {
            Blocks = new List<Block>();
            TempTransactions = new List<Transaction>();
            GenerateKeys();
        }

        public void NewTransaction(string sender, string receiver, decimal amount, bool fromNet = false)
        {
            Transaction newTransaction = new Transaction(sender, receiver, amount);
            newTransaction.Id = TempTransactions.Count;
            newTransaction.Sign(PublicKey, PrivateKey);
            if (newTransaction.IsValid())
            {
                TempTransactions.Add(newTransaction);
                if (!fromNet)
                {
                    foreach (INode node in Nodes)
                    {
                        node.PerformTransactionNet(this, sender, receiver, amount);
                    }
                }
            }
        }

        private void GenerateKeys()
        {
            string[] keys = RSAHelper.GenerateKeys();
            PublicKey = keys[0];
            PrivateKey = keys[1];
        }

        public void NewBlock()
        {
            string previousHash = string.Empty;
            if (Blocks.Count > 0)
                previousHash = Blocks[Blocks.Count - 1].Hash;

            Block newBlock = new Block(Blocks.Count, TempTransactions, previousHash);
            newBlock.MineBlock(Difficulty);
            Blocks.Add(newBlock);
            TempTransactions = new List<Transaction>();
        }

        #region Comm

        public void PerformTransactionNet(INodeRequester requester, string sender, string receiver, decimal amount)
        {
            NewTransaction(sender, receiver, amount, true);
        }

        public void PrepareInfoToSend(INodeRequester requester, string code)
        {
            if (code.Equals("EntireBlockchain"))
            {
                SerializeHelper<BlockChain> sh = new SerializeHelper<BlockChain>();
                string response = sh.Serialize(this);
                requester.Update(response);
            }
        }

        public void PerformConsensus()
        {
            foreach (INode node in Nodes)
            {
                node.PrepareInfoToSend(this, "EntireBlockChain");

            }
        }

        public void Update(string response)
        {
            SerializeHelper<BlockChain> sh = new SerializeHelper<BlockChain>();
            BlockChain neigh = sh.Deserialize(response);
            if (neigh.Blocks.Count > Blocks.Count)
            {
                string prevHash = string.Empty;
                Blocks = neigh.Blocks; 
            }
        }

        #endregion
    }
}
