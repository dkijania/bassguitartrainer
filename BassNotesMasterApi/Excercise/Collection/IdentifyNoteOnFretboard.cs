using System;
using System.Collections.Generic;
using System.Linq;
using BassNotesMasterApi.NotesView;
using BassNotesMasterApi.Utils;
using BassNotesMasterApi.Utils.Keyboard;

namespace BassNotesMasterApi.Excercise.Collection
{
    public class IdentifyNoteOnFretboard : BaseExcercise, INotesViewListener, ICombinationPressedListener
    {
        private StringFretPair _stringFretToFind;

        protected override void RenewSubscriptions()
        {
            ManagersLocator.Instance.NotesViewManager.Subscribe(this);
            ManagersLocator.Instance.KeyboardEventManager.OctaveCheckEnabled = false;
            ManagersLocator.Instance.KeyboardEventManager.OnCombinationPressedEvent += OnCombinationPressed;
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
            get { return new[] {Options.ShowTips, Options.PlayNote}; }
        }

        public IdentifyNoteOnFretboard(Settings.Settings settings, IExcerciseOptionGuiManager guiManager,
                                       IVisibilityManager visibilityManager)
            : base(settings, guiManager, visibilityManager)
        {
        }

        public void OnMouseClick(Note note)
        {
            var noteToFind = FretBoardMapping.GetNote(_stringFretToFind);
            var result = note.EqualsWithoutOctaveNumber(noteToFind);
            if (result)
            {
                FretBoardManager.FretBoard.FretBoardGuiBuilder.RedrawNote(_stringFretToFind, true);
            }
            else
            {
                FretBoardManager.FretBoard.FretBoardGuiBuilder.RedrawNoteWithQuestionMark(_stringFretToFind, false);
            }

            EventHandler actionExectutedAfterDelay = (sender, args) =>
                                                         {
                                                             if (result || ++TryNo == Attempts)
                                                             {
                                                                 FretBoardManager.FretBoard.ClearView();
                                                                 RegisterResult(result);
                                                                 NextTest();
                                                             }
                                                             else
                                                             {
                                                                 FretBoardManager.FretBoard.FretBoardGuiBuilder.
                                                                     RedrawNoteWithQuestionMark(_stringFretToFind, true);
                                                             }
                                                         };

            WaitXAndCheckResult(actionExectutedAfterDelay, DelayValue);
        }

        protected override void NextTest()
        {
            _stringFretToFind = GetRandomItem();
            PlaySoundIfEnabled(_stringFretToFind);
            FretBoardManager.FretBoard.FretBoardGuiBuilder.RedrawNoteWithQuestionMark(_stringFretToFind);
            ResetTryNo();
        }

        protected override void ContinuteTest()
        {
            FretBoardManager.FretBoard.FretBoardGuiBuilder.DrawNoteWithQuestionMark(_stringFretToFind);
        }
    }
}