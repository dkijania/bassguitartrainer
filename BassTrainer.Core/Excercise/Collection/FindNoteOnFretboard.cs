using System;
using System.Collections.Generic;
using BassTrainer.Core.Components;
using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Const;
using BassTrainer.Core.Excercise.Options;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Excercise.Collection
{
    public class FindNoteOnFretboard : BaseExcercise, IFretboardListener
    {
        private StringFretPair _stringFretToFind;

        protected override ComponentId[] ComponentsVisibleDuringExcercise
        {
            get { return new[] {ComponentId.Notation, ComponentId.Fretboard}; }
        }

        protected override string[] OptionsToShow
        {
            get { return new[] {Excercise.Options.Options.ShowTips, Excercise.Options.Options.PlayNote,Excercise.Options.Options.HideNoteLabel}; }
        }

        public FindNoteOnFretboard(Settings.Settings settings, IExcerciseOptionGuiManager guiManager,
                                   IVisibilityManager visibilityManager) : base(settings, guiManager, visibilityManager)
        {
        }

        protected override void AfterStart()
        {
            SetTipsVisibility(StringFretPairs);
        }

        protected override void AfterContinue()
        {
            SetTipsVisibility(StringFretPairs);
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


        protected override void RenewSubscriptions()
        {
            FretBoardComponent.Subscribe(this);
            ComponentsLocator.Instance.KeyboardEventComponent.OnCombinationPressedEvent += OnCombinationPressed;
        }

        protected override void NextTest()
        {
            _stringFretToFind = GetRandomItem();
            NoteToFind = FretBoardMapping.GetNote(_stringFretToFind);
            ComponentsLocator.Instance.MusicNotationComponent.RedrawNote(_stringFretToFind);
            PlaySoundIfEnabled(_stringFretToFind);
            ResetTryNo();
        }

        protected override void ContinuteTest()
        {
            ComponentsLocator.Instance.MusicNotationComponent.RedrawNote(_stringFretToFind);
        }

        public void OnCombinationPressed(Note note)
        {
            
        }

        public void OnMouseClick(StringFretPair stringFretPair, FretBoard fretBoard)
        {
            if (!StringFretPairs.Contains(stringFretPair))
                return;

            var actualNote = FretBoardMapping.GetNote(stringFretPair);
            var result = actualNote.Equals(NoteToFind);
            fretBoard.RedrawNote(stringFretPair, result);

            EventHandler actionExectutedAfterDelay =
                (sender, args) =>
                    {
                        fretBoard.ClearView();
                        if (result || ++TryNo == Attempts)
                        {
                            RegisterResult(result);
                            NextTest();
                        }
                    };
            WaitXAndCheckResult(actionExectutedAfterDelay, DelayValue);
        }
    }
}