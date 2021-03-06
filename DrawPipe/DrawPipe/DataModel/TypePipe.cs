using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace DrawPipe.DataModel
{
    public class TypePipe
    {
        public class TypePipeShov
        {
            public string Id { get; private set; }
            public string Name { get; private set; }
            public List<string> KeyList { get; private set; }

            public TypePipeShov(string id, string name)
            {
                Id = id;
                Name = name;
                KeyList = new List<string>();
            }
        }
        public class TypeShov
        {
            public List<TypePipeShov> TypeShovList { get; private set; }

            public TypeShov(string xml)
            {
                TypeShovList = new List<TypePipeShov>();

                XDocument xdoc = XDocument.Parse(xml);

                XElement root = xdoc.Element("pipe");

                foreach (XElement x in root.Elements("pipeType"))
                {
                    string id = x.Attribute("id").Value;
                    string name = x.Attribute("name").Value;

                    TypePipeShov typePipeShov = new TypePipeShov(id, name);

                    foreach (XElement xk in x.Elements("key"))
                    {
                        string keyName = xk.Value;
                        typePipeShov.KeyList.Add(keyName);
                    }

                    TypeShovList.Add(typePipeShov);

                }
            }
        }
    }
}
