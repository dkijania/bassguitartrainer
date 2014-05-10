using System;
using System.Collections.Generic;
using System.Linq;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Fretboard;
using BassNotesMasterApi.Interval;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Excercise.Collection
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
            get { return new[] {Options.ShowTips, Options.AlwaysStartFromLowestNoteParamName}; }
        }

        private IntervalManager IntervalManager
        {
            get { return ManagersLocator.Instance.IntervalManager; }
        }

        public IntervalsExcercise(Settings.Settings settings, IExcerciseOptionGuiManager guiManager,
                                  IVisibilityManager visibilityManager) : base(settings, guiManager, visibilityManager)
        {
        }

        protected override void AfterStart()
        {
            IntroduceSettings(StringFretPairs);
        }

        private void SubscribeToManagersEvents()
        {
            IntervalManager.Subscribe(this);
            FretBoardManager.EventHandler.Subscribe(this);
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
                FretBoardManager.FretBoard.FretBoardGuiBuilder.AlwaysRedrawCollection = new AlwaysRedrawCollection(pairs)
                {
                    IsTransparencyEnabled =
                        Options.ShouldShowTips,
                    DrawLabels = false
                };
            }
            FretBoardManager.ClearView();
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
                    IntervalManager.EnableIntervalsButtonsExlusive(pairs, _intervalStartNote);
                else
                    IntervalManager.EnableIntervalsButtonsExlusive(pairs);
            }
            else
            {
                IntervalManager.EnableAllIntervalsButtons();
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
                FretBoardManager.FretBoard.FretBoardGuiBuilder.DrawNote(firstItem);
            }
            else
            {
                firstItem = GetRandomItem();
                _currentlySeekIndex = 0;
            }
            _intervalsToFind = new[] {firstItem, secondItem};
            ManagersLocator.Instance.PlayerManager.PlayNote(firstItem, secondItem);
        }

        protected override void ContinuteTest()
        {
        }

        public void IntervalExcerciseEvent(IntervalRow row)
        {
            var startNote = FretBoardMapping.GetNote(_intervalsToFind[0]);
            var endNote = FretBoardMapping.GetNote(_intervalsToFind[1]);
            var result = Math.Abs(_notesInfo.CalculateDistanceFromNote(startNote, endNote)) == row.Semitone;

            IntervalManager.GuiBuilder.SetColorForButtonName(row, result);

            EventHandler actionExectutedAfterDelay = (sender, args) =>
                                                         {
                                                             FretBoardManager.FretBoard.ClearView();
                                                             IntervalManager.GuiBuilder.ResetColorForButtonName(row);
                                                             if (!result && ++TryNo != Attempts) return;
                                                             NextTest();
                                                             RegisterResult(result);
                                                         };
            WaitXAndCheckResult(actionExectutedAfterDelay, 1.0);
        }

        public void OnMouseClick(StringFretPair stringFretPair, FretBoard fretBoard)
        {
            var result = stringFretPair.Equals(_intervalsToFind[_currentlySeekIndex]);
            FretBoardManager.FretBoard.FretBoardGuiBuilder.RedrawNote(stringFretPair, result);

            EventHandler actionExectutedAfterDelay = (sender, args) =>
                                                         {
                                                             FretBoardManager.FretBoard.ClearView();
                                                             if (!result && ++TryNo != Attempts) return;
                                                             if (++_currentlySeekIndex == _intervalsToFind.Count())
                                                                 NextTest();
                                                         };
            WaitXAndCheckResult(actionExectutedAfterDelay, DelayValue);
        }
    }
}