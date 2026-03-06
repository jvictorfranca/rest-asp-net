using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace RestASPNet.Tests.IntegrationTests.Tools
{
    public static class XmlHelper
    {
        public static StringContent SerializeToXml<T>(T obj)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty); // Remove namespaces from XML

            using var stringWriter = new UTF8StringWriter();
            serializer.Serialize(stringWriter, obj, ns);
            var xmlString = stringWriter.ToString();
            return new StringContent(xmlString, Encoding.UTF8, "application/xml");
        }

        public static async Task<T?> DeserializeFromXmlAsync<T>(HttpResponseMessage response)
        {
            var serializer = new XmlSerializer(typeof(T));
            await using var stream = await response.Content.ReadAsStreamAsync();
            return (T?)serializer.Deserialize(stream);
        }

        private class UTF8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}
