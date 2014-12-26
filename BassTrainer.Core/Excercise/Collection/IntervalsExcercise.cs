using System;
using System.Collections.Generic;
using System.Linq;
using BassTrainer.Core.Components;
using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Components.Interval;
using BassTrainer.Core.Components.Interval.Data;
using BassTrainer.Core.Const;
using BassTrainer.Core.Excercise.Options;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Excercise.Collection
{
    public class IntervalsExcercise : BaseExcercise, IExcerciseIntervalListener, IFretboardListener
    {
        private readonly NotesInfo _notesInfo = new NotesInfo();
        private StringFretPair _intervalStartNote;
        private StringFretPair[] _intervalsToFind;
        private int _currentlySeekIndex;

        protected override ComponentId[] ComponentsVisibleDuringExcercise
        {
            get { return new[] {ComponentId.Fretboard, ComponentId.Intervals, ComponentId.Player}; }
        }

        protected override void RenewSubscriptions()
        {
            SubscribeToManagersEvents();
        }

        protected override string[] OptionsToShow
        {
            get { return new[] {Excercise.Options.Options.ShowTips, Excercise.Options.Options.AlwaysStartFromLowestNoteParamName}; }
        }

        private IntervalComponent IntervalComponent
        {
            get { return ComponentsLocator.Instance.IntervalComponent; }
        }

        public IntervalsExcercise(Settings.Settings settings, IExcerciseOptionGuiManager guiManager, IVisibilityManager visibilityManager, IComponentModeManager componentModeManager)
            : base(settings, guiManager, visibilityManager,componentModeManager)
        {
        }

        protected override void AfterStart()
        {
            IntroduceSettings(StringFretPairs);
        }

        private void SubscribeToManagersEvents()
        {
            IntervalComponent.Subscribe(this);
            FretBoardComponent.Subscribe(this);
        }

        private void IntroduceSettings(IEnumerable<StringFretPair> pairs)
        {
            var stringFretPairs = pairs as StringFretPair[] ?? pairs.ToArray();
            SetIntervalRoot(stringFretPairs);
            SetIntervalButtonsAvailability(stringFretPairs);
            SetTipsVisibility(pairs);
        }
        
        private void SetTipsVisibility(IEnumerable<StringFretPair> pairs)
        {
            if (Options.ShouldShowTips)
            {
                FretBoardComponent.FretBoard.AlwaysRedrawCollection = new AlwaysRedrawCollection(pairs)
                {
                    IsTransparencyEnabled =
                        Options.ShouldShowTips,
                    DrawLabels = false
                };
            }
            FretBoardComponent.ClearView();
        }
        private void SetIntervalRoot(IEnumerable<StringFretPair> pairs)
        {
            var stringFretPairs = pairs as StringFretPair[] ?? pairs.ToArray();
            if (!Options.AlwaysStartFromLowestNote) return;
            var notes = stringFretPairs.Select(FretBoardMapping.GetNote);
            var lowestNote = _notesInfo.GetLowestNote(notes);
            _intervalStartNote = FretBoardMapping.GetMatchingNote(lowestNote, stringFretPairs);
        }

        private void SetIntervalButtonsAvailability(IEnumerable<StringFretPair> pairs)
        {
            if (Options.ShouldShowTips)
            {
                if (Options.AlwaysStartFromLowestNote)
                    IntervalComponent.EnableIntervalsButtonsExlusive(pairs, _intervalStartNote);
                else
                    IntervalComponent.EnableIntervalsButtonsExlusive(pairs);
            }
            else
            {
                IntervalComponent.EnableAllIntervalsButtons();
            }
        }

        protected override void AfterContinue()
        {
            IntroduceSettings(StringFretPairs);
        }

        protected override void NextTest()
        {
            var secondItem = GetRandomItem();
            StringFretPair firstItem;
            if (Options.AlwaysStartFromLowestNote)
            {
                firstItem = _intervalStartNote;
                _currentlySeekIndex = 1;
           }
            else
            {
                firstItem = GetRandomItem();
                _currentlySeekIndex = 0;
            }
            _intervalsToFind = new[] {firstItem, secondItem};
            ComponentsLocator.Instance.PlayerManager.PlayNote(firstItem, secondItem);
            ResetTryNo();
        }

        protected override void ContinuteTest()
        {
        }

        public void IntervalExcerciseEvent(IntervalRow row)
        {
            var startNote = FretBoardMapping.GetNote(_intervalsToFind[0]);
            var endNote = FretBoardMapping.GetNote(_intervalsToFind[1]);
            var result = Math.Abs(_notesInfo.CalculateDistanceFromNote(startNote, endNote)) == row.Semitone;

            IntervalComponent.SetColorForButtonName(row, result);

            EventHandler actionExectutedAfterDelay = (sender, args) =>
                                                         {
                                                             FretBoardComponent.FretBoard.ClearView();
                                                             IntervalComponent.ResetColorForButtonName(row);
                                                             if (result || ++TryNo == Attempts)
                                                             {
                                                                 NextTest();
                                                                 RegisterResult(result);
                                                             }
                                                         };
            WaitXAndCheckResult(actionExectutedAfterDelay, 1.0);
        }

        public void OnMouseClick(StringFretPair stringFretPair, FretBoard fretBoard)
        {
            var result = stringFretPair.Equals(_intervalsToFind[_currentlySeekIndex]);
            FretBoardComponent.FretBoard.RedrawNote(stringFretPair, result);

            EventHandler actionExectutedAfterDelay = (sender, args) =>
                                                         {
                                                             FretBoardComponent.FretBoard.ClearView();
                                                             if (!result && ++TryNo != Attempts) return;
                                                             if (++_currentlySeekIndex == _intervalsToFind.Count())
                                                                 NextTest();
                                                         };
            WaitXAndCheckResult(actionExectutedAfterDelay, DelayValue);
        }
    }
}