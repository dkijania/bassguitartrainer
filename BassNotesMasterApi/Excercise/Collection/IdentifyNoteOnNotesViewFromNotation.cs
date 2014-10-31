using System;
using BassNotesMasterApi.Notation;
using BassNotesMasterApi.NotesView;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Excercise.Collection
{
    public class IdentifyNoteOnNotesViewFromNotation : BaseExcercise, INotesViewListener
    {
        private StringFretPair _stringFretToFind;
        private MusicNotation _musicNotation;

        public IdentifyNoteOnNotesViewFromNotation(Settings.Settings settings, IExcerciseOptionGuiManager guiManager,
                                                   IVisibilityManager visibilityManager)
            : base(settings, guiManager, visibilityManager)
        {
        }

        protected override void RenewSubscriptions()
        {
            ManagersLocator.Instance.NotesViewManager.Subscribe(this);
            IntroduceSettingsForNotesViewTipsVisibility();
        }

        protected override void NextTest()
        {
            ResetTryNo();
            _stringFretToFind = GetRandomItem();
            ContinuteTest();
        }

        protected override void ContinuteTest()
        {
            NoteToFind = FretBoardMapping.GetNote(_stringFretToFind);
            _musicNotation = ManagersLocator.Instance.MusicNotationManager;
            _musicNotation.RedrawNote(_stringFretToFind);
            PlaySoundIfEnabled(_stringFretToFind);
        }

        protected override ComponentId[] ComponentsVisibleDuringExcercise
        {
            get { return new[] {ComponentId.NotesView, ComponentId.Notation,}; }
        }

        protected override string[] OptionsToShow
        {
            get { return new[] { Options.PlayNote, Options.ShowTips}; }
        }

        public void OnMouseClick(Note note)
        {
            var result = note.EqualsWithoutOctaveNumber(NoteToFind);
            if (result == false)
                _musicNotation.RedrawNote(_stringFretToFind, false);

            EventHandler actionExectutedAfterDelay =
                (sender, args) =>
                    {
                        _musicNotation.RedrawNote(_stringFretToFind); 
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