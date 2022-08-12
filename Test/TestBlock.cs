using BlockChain.Businesss.Classes;
using BlockChain.InfraStructure.Helpers;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.Test
{
    [TestFixture]
    public class TestBlock
    {
        [Test]
        public void TestNewBlock()
        {
            Businesss.Classes.BlockChain nodo1 = new Businesss.Classes.BlockChain();
            Assert.IsNotNull(nodo1);

            nodo1.NewTransaction("Cem", "Ali", 25);
            nodo1.NewTransaction("Ahmet", "Ayşe", 20);
            nodo1.NewBlock();

            nodo1.NewTransaction("Mehmet", "Burcu", 15);
            nodo1.NewTransaction("Dogan", "Veli", 10);
            nodo1.NewTransaction("Fatih", "Akif", 5);
            nodo1.NewBlock();

            Assert.That(nodo1.Blocks.Count, Is.EqualTo(2));
            Assert.That(nodo1.Blocks[0].Transactions.Length, Is.EqualTo(2));
            Assert.That(nodo1.Blocks[1].Transactions.Length, Is.EqualTo(3));

            Assert.That(nodo1.Blocks[0].Transactions[0].Amount, Is.EqualTo(25));

            Assert.That(nodo1.Blocks[1].PreviousHash, Is.EqualTo(nodo1.Blocks[0].Hash));
            Assert.That(nodo1.Blocks[0].PreviousHash, Is.EqualTo(string.Empty));
        }

        [Test]
        public void TestHashHelper()
        {
            string text = "Merhaba Dünya";
            Assert.That(HashHelper.CalculateHash(text), Is.EqualTo("51d6575fd179fcd4894cbef4af6bef4b06de5b041390bd659cf209ff1cf3be7d".ToUpper()));
        }

        [Test]
        public void TestMineBlock()
        {
            Businesss.Classes.BlockChain nodo1 = new Businesss.Classes.BlockChain();
            nodo1.NewTransaction("Cem", "Ali", 25);
            nodo1.NewTransaction("Ahmet", "Ayşe", 20);
            nodo1.NewBlock();

            nodo1.Blocks[0].MineBlock(Businesss.Classes.BlockChain.Difficulty);
            Assert.IsTrue(nodo1.Blocks[0].Hash.StartsWith("".PadLeft(Businesss.Classes.BlockChain.Difficulty, '0')));
        }

        [Test]
        public void TestSerialization()
        {
            Transaction tTest = new Transaction("Ali", "Veli", 15);
            SerializeHelper<Transaction> sh = new SerializeHelper<Transaction>();
            Assert.IsNotNull(sh.Serialize((tTest)));
            Assert.IsFalse(string.IsNullOrEmpty(sh.Serialize(tTest)));
        }

        [Test]
        public void TestRSAHelper()
        {
            string[] keys = RSAHelper.GenerateKeys();
            Assert.IsNotNull(keys);
            Assert.That(2, Is.EqualTo(keys.Length));
            Assert.IsFalse(string.IsNullOrEmpty(keys[0]));
            Assert.IsFalse(string.IsNullOrEmpty(keys[1]));

            string text = "HOLA";
            byte[] signature = RSAHelper.Sign(text, keys[1]);
            Assert.IsTrue(RSAHelper.Verify(text, signature, keys[1]));
            Assert.IsFalse(RSAHelper.Verify(text, signature, "k"));

            Transaction t1 = new Transaction("Ahmet", "Mehmet", 12);
            t1.Sign(keys[0], keys[1]);
            Assert.IsTrue(t1.IsValid());

            Transaction t2 = new Transaction("Ahmet", "Mehmet", 12);
            t1.Sign("md", keys[1]);
            Assert.IsFalse(t2.IsValid());
        }

        [Test]
        public void TestTrancsactionNet()
        {
            Businesss.Classes.BlockChain nodo1 = new Businesss.Classes.BlockChain();
            Businesss.Classes.BlockChain node2 = new Businesss.Classes.BlockChain();
            nodo1.RegisterNode(node2);
            node2.RegisterNode(nodo1);
            nodo1.NewTransaction("Cem", "Ali", 5);
            nodo1.NewTransaction("Ali", "Ayşe", 3);

            Assert.That(node2.TempTransactions.Count, Is.EqualTo(nodo1.TempTransactions.Count));
        }

        [Test]
        public void TestConsensus()
        {
            Businesss.Classes.BlockChain node1 = new Businesss.Classes.BlockChain();
            node1.NewTransaction("Cem", "Ali", 5);
            node1.NewTransaction("Ali", "Ayşe", 3);
            node1.NewBlock();
            node1.NewTransaction("Ahmet", "Mehmet", 5);
            node1.NewTransaction("Mehmet", "Sezgin", 3);
            node1.NewBlock();


            Businesss.Classes.BlockChain node2 = new Businesss.Classes.BlockChain();
            node1.RegisterNode(node2);
            node2.RegisterNode(node1);

            node2.PerformConsensus();
            Assert.That(node2.Blocks.Count, Is.EqualTo(node1.Blocks.Count));
        }
    }
}
