using System.Collections.Generic;
using System.Xml.Serialization;
using BassTrainer.Core.Utils.ResultSerializer;

namespace BassTrainer.UI.WPF.ResultSerializer
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