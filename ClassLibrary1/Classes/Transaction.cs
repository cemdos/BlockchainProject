using BlockChain.InfraStructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Businesss.Classes
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string? Sender { get; set; }
        public string? Receiver { get; set; }
        public decimal Amount { get; set; }
        public string Hash => HashHelper.CalculateHash($"{Sender}{Receiver}{Amount}");
        public string PublicKey { get; set; }
        public byte[] Signature { get; set; }

        public Transaction()
        {

        }

        public Transaction(string sender, string receiver, decimal amount)
        {
            Sender = sender;
            Receiver = receiver;
            Amount = amount;
            TimeStamp = DateTime.Now;
        }

        public void Sign(string publicKey,string privateKey)
        {
            this.PublicKey = publicKey;
            Signature = RSAHelper.Sign(Hash,privateKey);
        }

        public bool IsValid()
        {
            return RSAHelper.Verify(Hash, Signature, PublicKey);
        }
    }
}
