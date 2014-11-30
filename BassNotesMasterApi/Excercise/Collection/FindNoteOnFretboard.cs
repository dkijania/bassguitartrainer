using System;
using System.Collections.Generic;
using BassNotesMasterApi.Components.Fretboard;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Excercise.Collection
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
            get { return new[] {Options.ShowTips, Options.PlayNote,Options.HideNoteLabel}; }
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
                FretBoardManager.FretBoard.AlwaysRedrawCollection = new AlwaysRedrawCollection(pairs)
                                                                                            {
                                                                                                IsTransparencyEnabled =
                                                                                                    Options.ShouldShowTips,
                                                                                                    DrawLabels = false
                                                                                            };
            }
            FretBoardManager.ClearView();
        }


        protected override void RenewSubscriptions()
        {
            FretBoardManager.Subscribe(this);
            ManagersLocator.Instance.KeyboardEventManager.OnCombinationPressedEvent += OnCombinationPressed;
        }

        protected override void NextTest()
        {
            _stringFretToFind = GetRandomItem();
            NoteToFind = FretBoardMapping.GetNote(_stringFretToFind);
            ManagersLocator.Instance.MusicNotationManager.RedrawNote(_stringFretToFind);
            PlaySoundIfEnabled(_stringFretToFind);
            ResetTryNo();
        }

        protected override void ContinuteTest()
        {
            ManagersLocator.Instance.MusicNotationManager.RedrawNote(_stringFretToFind);
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