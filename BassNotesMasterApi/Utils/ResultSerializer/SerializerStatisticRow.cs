using System.Xml.Serialization;
using BassNotesMasterApi.Statistics;

namespace BassNotesMasterApi.Utils.ResultSerializer
{
    public class SerializerStatisticRow
    {
        [XmlIgnore]
        public virtual long TestNo { get; set; }
        [XmlIgnore]
        public virtual long Passed { get; set; }
        [XmlIgnore]
        public virtual long Failed { get; set; }
        [XmlIgnore]
        public virtual long Skipped { get; set; }

        public void AddAll(SerializerStatisticRow row)
        {
            Passed += row.Passed;
            Failed += row.Failed;
            Skipped += row.Skipped;
        }
    }
}