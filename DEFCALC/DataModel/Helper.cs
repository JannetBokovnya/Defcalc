using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DEFCALC.DataModel
{
    
        public static class Helper
        {
            public static void Serialize(Stream streamObject, object objForSerialization)
            {
                if (objForSerialization == null || streamObject == null)
                    return;

                XmlSerializer serializer = new XmlSerializer(objForSerialization.GetType());
                serializer.Serialize(streamObject, objForSerialization);
            }

            public static object Deserialize(FileStream streamObject, Type serializedObjectType)
            {
                if (serializedObjectType == null || streamObject == null)
                    return null;

                XmlSerializer serializer = new XmlSerializer(serializedObjectType);
                return serializer.Deserialize(streamObject);
            }

        }
    }
