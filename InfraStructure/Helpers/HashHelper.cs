using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BlockChain.InfraStructure.Helpers
{
    public class HashHelper
    {
        public static string CalculateHash(string text)
        {
            string myHashCalculate = String.Empty;
            using (SHA256 mySHA256 = SHA256.Create())
            {
                byte[] encodeText = new UTF8Encoding().GetBytes(text);
                byte[] myHashArray = mySHA256.ComputeHash(encodeText);
                myHashCalculate = BitConverter.ToString(myHashArray).Replace("-", String.Empty);
                return myHashCalculate;
            }
        }
    }
}
