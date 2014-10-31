using System.Collections.Generic;
using System.Xml.Serialization;
using BassNotesMasterApi.Utils.ResultSerializer;

namespace BassNotesMaster.ResultSerializer
{
    [XmlType(TypeName = "ExcerciseData")]
    [XmlRoot("ExcerciseData")]
    public class XmlSerializerExcerciseData : SerializerExcerciseData
    {
       [XmlAttribute]
        public string Name { get; set; }

        [XmlArrayItem]
        public List<XmlSerializerStatisticRow> Data { get; set; }

        public override string GetName()
        {
            return Name;
        }

        public override List<SerializerStatisticRow> GetData()
        {
            return new List<SerializerStatisticRow>(Data);
        }
    }
}