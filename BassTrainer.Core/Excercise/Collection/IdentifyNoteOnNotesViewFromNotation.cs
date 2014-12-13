using System;
using BassTrainer.Core.Components;
using BassTrainer.Core.Components.Notation;
using BassTrainer.Core.Components.NotesView;
using BassTrainer.Core.Const;
using BassTrainer.Core.Excercise.Options;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Excercise.Collection
{
    public class IdentifyNoteOnNotesViewFromNotation : BaseExcercise, INotesViewListener
    {
        private StringFretPair _stringFretToFind;
        private MusicNotationComponent _musicNotation;

        public IdentifyNoteOnNotesViewFromNotation(Settings.Settings settings, IExcerciseOptionGuiManager guiManager,
                                                   IVisibilityManager visibilityManager)
            : base(settings, guiManager, visibilityManager)
        {
        }

        protected override void RenewSubscriptions()
        {
            ComponentsLocator.Instance.NotesViewComponent.Subscribe(this);
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
            _musicNotation = ComponentsLocator.Instance.MusicNotationComponent;
            _musicNotation.RedrawNote(_stringFretToFind);
            PlaySoundIfEnabled(_stringFretToFind);
        }

        protected override ComponentId[] ComponentsVisibleDuringExcercise
        {
            get { return new[] {ComponentId.NotesView, ComponentId.Notation,}; }
        }

        protected override string[] OptionsToShow
        {
            get { return new[] { Excercise.Options.Options.PlayNote, Excercise.Options.Options.ShowTips}; }
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