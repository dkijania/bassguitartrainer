using System;
using System.Collections.Generic;
using System.Linq;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Fretboard;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Interval
{
    public class IntervalManager : Manager
    {
        public readonly IntervalGuiBuilder GuiBuilder;
        private readonly IntervalEventHandler _eventHandler;
        private readonly IntervalCalculator _intervalCalculator = new IntervalCalculator();
        private readonly NotesToStringFretBoardMapping _fretBoardMapping = new NotesToStringFretBoardMapping();
        private readonly IntervalData _data = new IntervalData();
        private ManagerMode _mode;

        public override ManagerMode Mode
        {
            get { return _mode; }
            set
            {
                _mode = value;
                OnModeChanged(_mode);
            }
        }

        public override void RemoveAllSubscribers()
        {
            _eventHandler.RemoveAllEvents();
        }

        public override void OnModeChanged(ManagerMode mode)
        {
            GuiBuilder.EnableMode(mode);
        }

        public IntervalManager(IntervalGuiBuilder guiBuilder)
        {
            GuiBuilder = guiBuilder;
            _eventHandler = GuiBuilder.IntervalEventHandler;
            GuiBuilder.PrepareInfoGuiElements(_data);
            GuiBuilder.PrepareExcerciseGuiElements(_data);
        }

        public void Subscribe(IIntervalListener intervalListener)
        {
            _eventHandler.RegisterInfoEvents(intervalListener);
        }

        public void Unsubscribe(IIntervalListener intervalListener)
        {
            _eventHandler.UnregisterInfoEvents(intervalListener);
        }

        public void Subscribe(IExcerciseIntervalListener intervalListener)
        {
            _eventHandler.RegisterExcerciseEvents(intervalListener);
        }

        public void Unsubscribe(IExcerciseIntervalListener intervalListener)
        {
            _eventHandler.UnregisterExcerciseEvents(intervalListener);
        }

        public Note GetRootNote(FretBoard fretBoard)
        {
            var position = fretBoard.FretBoardGuiBuilder.GetCurrentlyShownPosition();
            if (position.Length == 0)
                return null;

            return _fretBoardMapping.GetNote(position[0]);
        }

        public Note Calculate(Note root, int semitone)
        {
            return _intervalCalculator.Calculate(root, semitone);
        }

        public void EnableAllIntervalsButtons()
        {
            GuiBuilder.EnableAllIntervalsButtons();
        }

        public void EnableIntervalsButtonsExlusive(IEnumerable<StringFretPair> pairs, StringFretPair intervalStartNote)
        {
            var collection = new HashSet<string>();
            foreach (
                var interaval in
                    pairs.Select(stringFretPair => _intervalCalculator.Calculate(intervalStartNote, stringFretPair)).
                        Select(semitone => _data.BySemitone(semitone)))
            {
                collection.Add(interaval.IntervalName);
            }
            GuiBuilder.EnableIntervalsButtonsExclusive(collection.ToArray());
        }

        public void EnableIntervalsButtonsExlusive(IEnumerable<StringFretPair> pairs)
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
            GuiBuilder.EnableIntervalsButtonsExclusive(collection.ToArray());
        }
    }
}