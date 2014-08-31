using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using BassNotesMasterApi.Statistics;
using BassNotesMasterApi.Utils.ResultSerializer;

namespace BassNotesMaster.ResultSerializer
{
    public class XmlResultSerializer : IResultSerializer
    {
        public FileInfo File = new FileInfo(@".\save.xml");
      
        public void Save(StatisticRow statisticData)
        {
            if(IsDataEmpty(statisticData))
            {
                return;
            }
            var serializer = new XmlSerializer(typeof (WpfSerializerRoot));
            var root = Read();
            root.AddRow(statisticData);
            var streamWriter = new StreamWriter(File.FullName);
            serializer.Serialize(streamWriter, root);
            streamWriter.Close();
        }

        private bool IsDataEmpty(StatisticRow statisticData)
        {
            return statisticData.Questions == 0;
        }

        public SerializerRoot Read()
        {
            return File.Exists ? ReadFromXmlFile() : new WpfSerializerRoot{Data = new List<XmlSerializerExcerciseData>()};
        }

        private SerializerRoot ReadFromXmlFile()
        {
            var serializer = new XmlSerializer(typeof (WpfSerializerRoot));
            var reader = new StreamReader(File.FullName);
            var root = (WpfSerializerRoot) serializer.Deserialize(reader);
            reader.Close();
            return root;
        }
    }
}