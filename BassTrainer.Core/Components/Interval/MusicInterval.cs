using System;
using System.Collections.Generic;
using System.Linq;
using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Components.Interval.Data;
using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.Interval
{
    public class MusicInterval
    {
        private readonly IntervalGuiBuilder _guiBuilder;
        private readonly IntervalCalculator _intervalCalculator = new IntervalCalculator();
        private readonly NotesToStringFretBoardMapping _fretBoardMapping = NotesToStringFretBoardMapping.Instance;
        private readonly IntervalData _data = new IntervalData();

        public MusicInterval(IntervalGuiBuilder guiBuilder)
        {
            _guiBuilder = guiBuilder;
            _guiBuilder.PrepareInfoGuiElements(_data);
            _guiBuilder.PrepareExcerciseGuiElements(_data);
        }

        public void EnableMode(ComponentMode mode)
        {
            _guiBuilder.EnableMode(mode);
        }

        public Note GetRootNote(FretBoard fretBoard)
        {
            var position = fretBoard.GetCurrentlyShownPosition();
            return position.Length == 0 ? null : _fretBoardMapping.GetNote(position[0]);
        }

        public Note Calculate(Note root, int semitone)
        {
            return _intervalCalculator.Calculate(root, semitone);
        }

        public void EnableAllIntervalsButtons()
        {
            _guiBuilder.EnableAllIntervalsButtons();
        }

        public void EnableIntervalsButtonExclusive(IEnumerable<StringFretPair> pairs, StringFretPair intervalStartNote)
        {
            var collection = new HashSet<string>();
            foreach (
                var interaval in
                    pairs.Select(stringFretPair => _intervalCalculator.Calculate(intervalStartNote, stringFretPair)).
                        Select(semitone => _data.BySemitone(semitone)))
            {
                collection.Add(interaval.IntervalName);
            }
            _guiBuilder.EnableIntervalsButtonsExclusive(collection.ToArray());
        }

        public void EnableIntervalsButtonExclusive(IEnumerable<StringFretPair> pairs)
        {
            var combinations = pairs.SelectMany(x => pairs, Tuple.Create);
            var collection = new HashSet<string>();
            foreach (
                var semitone in
                    combinations.Select(
                        combination => _intervalCalculator.Calculate(combination.Item1, combination.Item2)))
            {
                collection.Add(_data.BySemitone(semitone).IntervalName);
            }
            _guiBuilder.EnableIntervalsButtonsExclusive(collection.ToArray());
        }

        public void SetColorForButtonName(IntervalRow row, bool result)
        {
            _guiBuilder.SetColorForButtonName(row, result);
        }

        public void ResetColorForButtonName(IntervalRow row)
        {
            _guiBuilder.ResetColorForButtonName(row);
        }
    }
}
