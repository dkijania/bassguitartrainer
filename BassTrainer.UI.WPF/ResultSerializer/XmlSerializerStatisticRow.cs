using System.Xml.Serialization;
using BassTrainer.Core.Utils.ResultSerializer;

namespace BassTrainer.UI.WPF.ResultSerializer
{
    [XmlRoot("StatisticRow")]
    [XmlType(TypeName = "StatisticRow")]
    public class XmlSerializerStatisticRow : SerializerStatisticRow
    {
        [XmlAttribute]
        public override long TestNo { get; set; }

        [XmlAttribute]
        public override long Passed { get; set; }

        [XmlAttribute]
        public override long Failed { get; set; }

        [XmlAttribute]
        public override long Skipped { get; set; }
        
    }
}