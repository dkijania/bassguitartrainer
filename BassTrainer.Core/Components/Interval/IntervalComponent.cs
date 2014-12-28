using System.Collections.Generic;
using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Components.Interval.Data;
using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.Interval
{
    public class IntervalComponent : Component
    {
        private readonly IntervalEventHandler _eventHandler;
        private readonly MusicInterval _musicInterval;
        
        public override void RemoveAllSubscribers()
        {
            _eventHandler.RemoveAllEvents();
        }

        public override void OnModeChanged(ComponentMode mode)
        {
            _musicInterval.EnableMode(mode);
        }

        public IntervalComponent(IntervalGuiBuilder guiBuilder)
        {
            _eventHandler = guiBuilder.IntervalEventHandler;
            _musicInterval = new MusicInterval(guiBuilder);
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
            return _musicInterval.GetRootNote(fretBoard);
        }

        public Note Calculate(Note root, int semitone)
        {
            return _musicInterval.Calculate(root, semitone);
        }

        public void EnableAllIntervalsButtons()
        {
            _musicInterval.EnableAllIntervalsButtons();
        }

        public void EnableIntervalsButtonsExlusive(IEnumerable<StringFretPair> pairs, StringFretPair intervalStartNote)
        {
            _musicInterval.EnableIntervalsButtonExclusive(pairs, intervalStartNote);
        }

        public void EnableIntervalsButtonsExlusive(IEnumerable<StringFretPair> pairs)
        {
            _musicInterval.EnableIntervalsButtonExclusive(pairs);
        }

        public void SetColorForButtonName(IntervalRow row, bool result)
        {
            _musicInterval.SetColorForButtonName(row, result);
        }

        public void ResetColorForButtonName(IntervalRow row)
        {
            _musicInterval.ResetColorForButtonName(row);
        }
    }
}