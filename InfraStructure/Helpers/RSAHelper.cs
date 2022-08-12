using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlockChain.InfraStructure.Helpers
{
    public class RSAHelper
    {
        public static string[] GenerateKeys()
        {
            string[] keys = new string[2];
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                //public key
                keys[0] = SerializeRSAParameters(RSA.ExportParameters(false));
                //private key
                keys[1] = SerializeRSAParameters(RSA.ExportParameters(true));
            }
            return keys;
        }

        public static byte[] Sign(string text, string privateKey)
        {
            RSAParameters key = DeserializeRSAParameters(privateKey);
            byte[]? signature = null;
                
            using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
            {
                var encoder = new UTF8Encoding();
                byte[] originalData = encoder.GetBytes(text);
                try
                {
                    rsa.ImportParameters(key);
                    signature = rsa.SignData(originalData, CryptoConfig.MapNameToOID("SHA512"));
                }
                catch (CryptographicException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
                return signature;
            }
        }

        public static bool Verify(string data, byte[] signature, string publicKey)
        {
            bool success = false;
            try
            {
                RSAParameters key = DeserializeRSAParameters(publicKey);
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    var encoder = new UTF8Encoding();
                    byte[] dataBytes = encoder.GetBytes(data);
                    try
                    {
                        rsa.ImportParameters(key);
                        success = rsa.VerifyData(dataBytes, CryptoConfig.MapNameToOID("SHA512"), signature);

                    }
                    catch (CryptographicException ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        rsa.PersistKeyInCsp = false;
                    }
                }
                return success;
            }
            catch (Exception)
            {
                return success;
            }
        }

        #region Private Method
        private static string SerializeRSAParameters(RSAParameters parameters)
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, parameters);

            return sw.ToString();
        }

        private static RSAParameters DeserializeRSAParameters(string strKey)
        {
            var sr = new StringReader(strKey);
            var xr = new XmlSerializer(typeof(RSAParameters));

            return (RSAParameters)xr.Deserialize(sr);
        }
        #endregion

    }
}
