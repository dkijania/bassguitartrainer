using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using BassTrainer.Core.Components.Statistics;
using BassTrainer.Core.Utils.ResultSerializer;

namespace BassTrainer.UI.WPF.ResultSerializer
{
    [XmlRoot("Root")]
    public class WpfSerializerRoot : SerializerRoot
    {
        [XmlArray]
        public List<XmlSerializerExcerciseData> Data { get; set; }


        public override List<SerializerExcerciseData> GetData()
        {
            return new List<SerializerExcerciseData>(Data);
        }

        public override void AddRow(StatisticRow statisticData)
        {
            var dataToAdd = Data.FirstOrDefault(
                x => String.Equals(x.Name, statisticData.Excercise, StringComparison.InvariantCultureIgnoreCase));

            if (dataToAdd == null)
            {
                dataToAdd = new XmlSerializerExcerciseData
                                {Name = statisticData.Excercise, 
                                 Data = new List<XmlSerializerStatisticRow>()};

                var statisticRowData = new XmlSerializerStatisticRow
                                           {
                                               TestNo = 1,
                                               Failed = statisticData.Wrong,
                                               Passed = statisticData.Correct,
                                               Skipped = statisticData.Skipped,
                                           };
                dataToAdd.Data.Add(statisticRowData);
                Data.Add(dataToAdd);
            }
            else
            {
                var statisticRowData = new XmlSerializerStatisticRow
                {
                    TestNo = dataToAdd.Data.Count + 1,
                    Failed = statisticData.Wrong,
                    Passed = statisticData.Correct,
                    Skipped = statisticData.Skipped,
                };
                dataToAdd.Data.Add(statisticRowData);
            }
        }
    }
}