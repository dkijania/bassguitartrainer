using System;
using System.Collections.Generic;
using System.Linq;
using BassNotesMasterApi.Fretboard;
using BassNotesMasterApi.NotesView;
using BassNotesMasterApi.Utils;
using BassNotesMasterApi.Utils.Keyboard;

namespace BassNotesMasterApi.Excercise.Collection
{
    public class FindNoteOnFretboardFromNotesView : BaseExcercise, IFretboardListener,INotesViewListener
    {
        private readonly NotesViewManager _notesViewManager = ManagersLocator.Instance.NotesViewManager;
        private readonly FretboardManager _fretboardManager = ManagersLocator.Instance.FretboardManager;

         private StringFretPair _curentlySeekPosition;

        public FindNoteOnFretboardFromNotesView(Settings.Settings settings, IExcerciseOptionGuiManager guiManager,
                                                IVisibilityManager visibilityManager)
            : base(settings, guiManager, visibilityManager)
        {
        }

        protected override void RenewSubscriptions()
        {
            _fretboardManager.FretBoard.FretBoardGuiBuilder.AlwaysRedrawCollection.DrawLabels = false;
            _fretboardManager.EventHandler.Subscribe(this);
            ManagersLocator.Instance.NotesViewManager.Subscribe(this);
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
            fretBoard.FretBoardGuiBuilder.RedrawNote(stringFretPair, result);

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
            FretBoardManager.FretBoard.FretBoardGuiBuilder.AlwaysRedrawCollection = new AlwaysRedrawCollection(pairs)
                                                                                        {
                                                                                            IsTransparencyEnabled =
                                                                                                Options.ShouldShowTips,
                                                                                            DrawLabels = false
                                                                                        };
            FretBoardManager.ClearView();
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
            _notesViewManager.ShowButtonExclusive(noteInTheMiddle);
            _notesViewManager.SetTextForButton(noteInTheMiddle, note,
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
            _notesViewManager.RevertGui();
        }

        protected override ComponentId[] ComponentsVisibleDuringExcercise
        {
            get { return new[] {ComponentId.Fretboard, ComponentId.NotesView}; }
        }

        protected override string[] OptionsToShow
        {
            get { return new[] { Options.PlayNote, Options.ShowTips, Options.RequireCorrectOctave, Options.HideNoteLabel }; }
        }
    }
}