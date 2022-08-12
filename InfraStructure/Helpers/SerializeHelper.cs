using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlockChain.InfraStructure.Helpers
{
    public class SerializeHelper<T> where T : class
    {
        public string Serialize(T objectInstanceted)
        {
            string objectString = string.Empty;
            using (var stringWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(objectInstanceted.GetType());
                serializer.Serialize(stringWriter, objectInstanceted);
                objectString = stringWriter.ToString();
            }
            return objectString;
        }

        public T Deserialize(string objectString)
        {
            T? objectInstanceted = null;
            using (var stringReader =new StringReader(objectString.Trim()))
            {
                var serializer = new XmlSerializer(typeof(T));
                objectInstanceted = serializer.Deserialize(stringReader) as T;
            }
            return objectInstanceted;
        }
    }
}
