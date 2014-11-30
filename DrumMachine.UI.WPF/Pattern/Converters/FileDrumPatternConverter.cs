using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using DrumMachine.Audio;
using DrumMachine.Engine.Pattern;

namespace DrumMachine.UI.WPF.Pattern.Converters
{
    /** Acceptable Formats:
     *   Measures: 3
     *   Bars: 2
     *   NoteType: Whole
     *   Tempo: 120
     *   0 0 0 0 0 0 0 0 0
     *   0 127 0 0 0 0 0 0 0
     */

    public class FileDrumPatternConverter
    {
        private const string ColumnDelimeter = "\t";
        private readonly int _scallingFactor;
        
        public FileDrumPatternConverter(IPatternTilesManipulator tilesManipulator)
        {
            _scallingFactor = tilesManipulator.GetMinimumSpanValue();
        }
       
        public void SaveToFile(DrumPattern drumPattern, string outpuFile)
        {
            var lines = new List<string>();
            SaveSettings(lines, drumPattern.Settings);
            SavePattern(lines, drumPattern.Array);
            File.WriteAllLines(outpuFile, lines);
        }

        private void SaveSettings(ICollection<string> lines, DrumPatternSettings drumPatternSettings)
        {
            lines.Add(String.Format("Measures: {0}", drumPatternSettings.Measures));
            lines.Add(String.Format("Bars: {0}", drumPatternSettings.Bars));
            lines.Add(String.Format("NoteType: {0}", drumPatternSettings.NoteType));
            lines.Add(String.Format("Tempo: {0}", drumPatternSettings.Tempo));
        }

        private void SavePattern(ICollection<string> lines, Byte[,] array)
        {
            for (var x = 0; x < array.GetLength(0); x++)
            {
                var row = WriteRow(array, x);
                lines.Add(row);
            }
        }

        private String WriteRow(Byte[,] array, int x)
        {
            var stringBuilder = new StringBuilder();
            for (var y = 0; y < array.GetLength(1); y += _scallingFactor)
            {
                var cellValue = array[x, y] / DrumPattern.Hit;
                stringBuilder.Append(cellValue).Append(ColumnDelimeter);
            }
            return stringBuilder.ToString();
        }

        public DrumPattern ReadFromFile(string inputFile)
        {
            var lines = File.ReadAllLines(inputFile);
            var drumPatternSetttings = ReadDrumPatternSettings(lines);
            var pattern = ReadDrumPattern(lines.Skip(4).ToArray(), drumPatternSetttings);
            return new DrumPattern(drumPatternSetttings, pattern);
        }

        private byte[,] ReadDrumPattern(IEnumerable<string> lines, DrumPatternSettings drumPatternSettings)
        {
            CheckSize(lines);
            var array = CreateArrayBasedOnSettings(drumPatternSettings,_scallingFactor);
            ConertArrays(lines,array);
            return array;
        }

        private void ConertArrays(IEnumerable<string> lines, byte[,] array)
        {
            for (var i = 0; i < lines.Count(); i++)
            {
                var items = lines.ElementAt(i).Split(Convert.ToChar(ColumnDelimeter));
                var bytes = ConvertToBytes(items);
                ConvertValuesForRow(bytes, array, i);
            }
        }

        private void ConvertValuesForRow(byte[] bytes, byte[,] output, int row)
        {
            for (int j = 0; j < bytes.Length; j++)
            {
                output[row, j * _scallingFactor] = (byte)(bytes[j] * DrumPattern.Hit);
            }
        }

        private byte[,] CreateArrayBasedOnSettings(DrumPatternSettings drumPatternSettings, int scallingFactor)
        {
            var noteInterval = drumPatternSettings.CalculateNoteWeight();
            return new byte[GetExpectedRowsCount(), CalculateRowLength(drumPatternSettings, noteInterval,scallingFactor)];
        }

        private int CalculateRowLength(DrumPatternSettings drumPatternSettings, int noteInterval, int scallingFactor)
        {
            return drumPatternSettings.Bars*drumPatternSettings.Measures*noteInterval*scallingFactor;
        }

        private void CheckSize(IEnumerable<string> lines)
        {
            var samplesCount = GetExpectedRowsCount();
            if (lines.Count() != samplesCount)
                throw new DrumPatternParsingException("Too many or too few drum pattern rows (should be {0})",
                    samplesCount);
        }

        private int GetExpectedRowsCount()
        {
            var audioSampler = DefaultAudioSampler.Instance;
            return audioSampler.Names.Count();
        }

        private byte[] ConvertToBytes(IEnumerable<string> items)
        {
            return items.Where(item => !String.IsNullOrEmpty(item)).Select(item => Convert.ToByte(item)).ToArray();
        }

        private DrumPatternSettings ReadDrumPatternSettings(IEnumerable<string> lines)
        {
            return new DrumPatternSettings
            {
                Measures = Convert.ToInt32(TryToGetNumber(lines.ElementAt(0))),
                Bars = Convert.ToInt32(TryToGetNumber(lines.ElementAt(1))),
                NoteType = TryToGetString(lines.ElementAt(2)),
                Tempo = Convert.ToInt32(TryToGetNumber(lines.ElementAt(3)))
            };
        }

        private string TryToGetString(string elementAt)
        {
            return TryToMatchWithGroup(elementAt, @": (.*)");
        }

        private String TryToGetNumber(string input)
        {
            return TryToMatch(input, @"\d+");
        }

        private string TryToMatch(string input, string regex)
        {
            var value = Regex.Match(input, regex).Value;
            if (String.IsNullOrEmpty(value))
                ThrowException(input, regex);
            return value;
        }

        private string TryToMatchWithGroup(string input, string regex)
        {
            var match = Regex.Match(input, regex);
            if (match.Groups.Count < 2)
                ThrowException(input, regex);
            var value = match.Groups[1].Value;
            if (String.IsNullOrEmpty(value))
                ThrowException(input, regex);
            return value;
        }

        private void ThrowException(string input, string regex)
        {
            throw new DrumPatternParsingException("Couldn't match input {0} with regex {1}", input, regex);
        }
    }
}