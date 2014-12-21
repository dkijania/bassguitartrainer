using System;
using BassTrainer.Core.Components;
using BassTrainer.Core.Components.NotesView;
using BassTrainer.Core.Const;
using BassTrainer.Core.Excercise.Options;
using BassTrainer.Core.Utils;
using BassTrainer.Core.Utils.Keyboard;

namespace BassTrainer.Core.Excercise.Collection
{
    public class IdentifyNoteOnFretboard : BaseExcercise, INotesViewListener, ICombinationPressedListener
    {
        private StringFretPair _stringFretToFind;

        protected override void RenewSubscriptions()
        {
            ComponentsLocator.Instance.NotesViewComponent.Subscribe(this);
            ComponentsLocator.Instance.KeyboardEventComponent.OctaveCheckEnabled = false;
            ComponentsLocator.Instance.KeyboardEventComponent.OnCombinationPressedEvent += OnCombinationPressed;
            IntroduceSettingsForNotesViewTipsVisibility();
        }

        public void OnCombinationPressed(Note note)
        {
            OnMouseClick(note);
        }

        protected override ComponentId[] ComponentsVisibleDuringExcercise
        {
            get { return new[] {ComponentId.Fretboard, ComponentId.NotesView}; }
        }

        protected override string[] OptionsToShow
        {
            get { return new[] {Excercise.Options.Options.ShowTips, Excercise.Options.Options.PlayNote}; }
        }

        public IdentifyNoteOnFretboard(Settings.Settings settings, IExcerciseOptionGuiManager guiManager, IVisibilityManager visibilityManager, IComponentModeManager componentModeManager)
            : base(settings, guiManager, visibilityManager,componentModeManager)
        {
        }

        public void OnMouseClick(Note note)
        {
            var noteToFind = FretBoardMapping.GetNote(_stringFretToFind);
            var result = note.EqualsWithoutOctaveNumber(noteToFind);
            if (result)
            {
                FretBoardComponent.FretBoard.RedrawNote(_stringFretToFind, true);
            }
            else
            {
                FretBoardComponent.FretBoard.RedrawNoteWithQuestionMark(_stringFretToFind, false);
            }

            EventHandler actionExectutedAfterDelay = (sender, args) =>
                                                         {
                                                             if (result || ++TryNo == Attempts)
                                                             {
                                                                 FretBoardComponent.FretBoard.ClearView();
                                                                 RegisterResult(result);
                                                                 NextTest();
                                                             }
                                                             else
                                                             {
                                                                 FretBoardComponent.FretBoard.RedrawNoteWithQuestionMark(_stringFretToFind, true);
                                                             }
                                                         };

            WaitXAndCheckResult(actionExectutedAfterDelay, DelayValue);
        }

        protected override void NextTest()
        {
            _stringFretToFind = GetRandomItem();
            PlaySoundIfEnabled(_stringFretToFind);
            FretBoardComponent.FretBoard.RedrawNoteWithQuestionMark(_stringFretToFind);
            ResetTryNo();
        }

        protected override void ContinuteTest()
        {
            FretBoardComponent.FretBoard.DrawNoteWithQuestionMark(_stringFretToFind);
        }
    }
}