using BlockChain.InfraStructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Businesss.Classes
{
    public class Block
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public Transaction[] Transactions { get; set; }
        public string Hash { get; set; }
        public string PreviousHash { get; set; }
        public int Proof { get; set; }
        public Block(int index, List<Transaction> transactions, string previousHash)
        {
            Id = index;
            Transactions = transactions != null ? transactions.ToArray(): new Transaction[0];
            Timestamp = DateTime.Now;
            PreviousHash = previousHash;
        }

        #region Mining
        private bool HashIsValid (string text, int difficulty)
        {
            string hash = HashHelper.CalculateHash(text);
            string zeros = string.Empty.PadLeft(difficulty, '0');
            return hash.StartsWith(zeros);
        }

        public int MineBlock(int difficulty)
        {
            string InitialText = string.Format("{0}{1}{2}", Id, Timestamp, Transactions.Select(async t => t.Hash).Aggregate(async (i, j) => await i + j));
            Proof = 0;
            string text = string.Format("{0}{1}",InitialText,Proof);
            while (!HashIsValid(text,difficulty))
            {
                Proof++;
                text = string.Format("{0}{1}", InitialText, Proof);
            }
            Hash = HashHelper.CalculateHash(text);
            return Proof;

        } 
        #endregion
    }
}
