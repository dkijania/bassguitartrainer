using System;
using System.Collections.Generic;
using BassTrainer.Core.Components;
using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Components.NotesView;
using BassTrainer.Core.Const;
using BassTrainer.Core.Excercise.Options;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Excercise.Collection
{
    public class FindNoteOnFretboardFromNotesView : BaseExcercise, IFretboardListener,INotesViewListener
    {
        private readonly NotesViewComponent _notesViewComponent = ComponentsLocator.Instance.NotesViewComponent;
        private readonly FretboardComponent _fretboardComponent = ComponentsLocator.Instance.FretboardComponent;

         private StringFretPair _curentlySeekPosition;

        public FindNoteOnFretboardFromNotesView(Settings.Settings settings, IExcerciseOptionGuiManager guiManager, IVisibilityManager visibilityManager, IComponentModeManager componentModeManager)
             : base(settings, guiManager, visibilityManager, componentModeManager)
        {
        }

        protected override void RenewSubscriptions()
        {
            _fretboardComponent.FretBoard.AlwaysRedrawCollection.DrawLabels = false;
            _fretboardComponent.Subscribe(this);
            ComponentsLocator.Instance.NotesViewComponent.Subscribe(this);
        }

        public void OnMouseClick(Note note)
        {
        }

        public void OnMouseClick(StringFretPair stringFretPair, FretBoard fretBoard)
        {
            if (!StringFretPairs.Contains(stringFretPair))
                return;
            var actualNote = FretBoardMapping.GetNote(stringFretPair);
            var result = CheckResult(actualNote);
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

        public bool CheckResult(Note actualNote)
        {
            return Options.ShouldRequireCorrectOctave ? actualNote.Equals(NoteToFind) : actualNote.EqualsWithoutOctaveNumber(NoteToFind);
        }

        protected void IntroduceSettings()
        {
            if (Options.ShouldShowTips)
            {
                SetTipsVisibility(StringFretPairs);
            }
        }

        private void SetTipsVisibility(IEnumerable<StringFretPair> pairs)
        {
            FretBoardComponent.FretBoard.AlwaysRedrawCollection = new AlwaysRedrawCollection(pairs)
                                                                                        {
                                                                                            IsTransparencyEnabled =
                                                                                                Options.ShouldShowTips,
                                                                                            DrawLabels = false
                                                                                        };
            FretBoardComponent.ClearView();
        }

        protected override void NextTest()
        {
            _curentlySeekPosition = GetRandomItem();
            NoteToFind = FretBoardMapping.GetNote(_curentlySeekPosition);
            ResetTryNo();
            RunTextForChosenPosition();
        }

        protected override void ContinuteTest()
        {
            IntroduceSettings();
            RunTextForChosenPosition();
        }

        private void RunTextForChosenPosition()
        {
            var note = FretBoardMapping.GetNote(_curentlySeekPosition);
            var noteInTheMiddle = new Note("F");
            _notesViewComponent.ShowButtonExclusive(noteInTheMiddle);
            _notesViewComponent.SetTextForButton(noteInTheMiddle, note,
                                               withOctaveNumber: Options.ShouldRequireCorrectOctave);
            PlaySoundIfEnabled(_curentlySeekPosition);
        }

        protected override void AfterStop()
        {
            RestoreView();
        }

        protected override void BeforeContinue()
        {
            RestoreView();
        }

        protected override void AfterStart()
        {
            IntroduceSettings();
        }

        private void RestoreView()
        {
            _notesViewComponent.RevertGui();
        }

        protected override ComponentId[] ComponentsVisibleDuringExcercise
        {
            get { return new[] {ComponentId.Fretboard, ComponentId.NotesView}; }
        }

        protected override string[] OptionsToShow
        {
            get { return new[] { Excercise.Options.Options.PlayNote, Excercise.Options.Options.ShowTips, Excercise.Options.Options.RequireCorrectOctave, Excercise.Options.Options.HideNoteLabel }; }
        }
    }
}