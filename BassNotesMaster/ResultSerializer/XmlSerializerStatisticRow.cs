using System.Xml.Serialization;

namespace BassNotesMaster.ResultSerializer
{
    [XmlRoot("StatisticRow")]
    [XmlType(TypeName = "StatisticRow")]
    public class XmlSerializerStatisticRow : BassNotesMasterApi.Utils.ResultSerializer.SerializerStatisticRow
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