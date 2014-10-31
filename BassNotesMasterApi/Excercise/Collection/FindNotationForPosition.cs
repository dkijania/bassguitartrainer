using System;
using System.Linq;
using BassNotesMasterApi.Notation;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Excercise.Collection
{
    public class FindNotationForPosition : BaseExcercise, IMusicNotationListener
    {
        private StringFretPair _stringFretToFind;

        protected override ComponentId[] ComponentsVisibleDuringExcercise
        {
            get { return new[] {ComponentId.Notation, ComponentId.Fretboard}; }
        }

        protected override string[] OptionsToShow
        {
            get { return new[] {Options.PlayNote}; }
        }

        public FindNotationForPosition(Settings.Settings settings, IExcerciseOptionGuiManager guiManager,
                                       IVisibilityManager visibilityManager)
            : base(settings, guiManager, visibilityManager,new DefaultSelectionSetter())
        {
        }

        protected override void RenewSubscriptions()
        {
            var musicNotation = ManagersLocator.Instance.MusicNotationManager;
            musicNotation.OnModeChanged(ManagerMode.Info);
            musicNotation.Subscribe(this);
        }
        
        protected override void ContinuteTest()
        {
            RunTestForCurrentStringFret();
        }

        public void OnMouseClick(StringFretPair stringFretPair)
        {
            var musicNotation = ManagersLocator.Instance.MusicNotationManager;

            var possibleAnwsers = FretBoardMapping.GetAllEquivalentPositions(stringFretPair);
            var result = possibleAnwsers.Contains(_stringFretToFind);
            musicNotation.RedrawNote(stringFretPair, result);

            EventHandler actionExectutedAfterDelay = (sender, args) =>
                                                         {
                                                             musicNotation.ResetGui();
                                                             if (result || ++TryNo == Attempts)
                                                             {
                                                                 RegisterResult(result);
                                                                 FretBoardManager.FretBoard.ClearView();
                                                                 NextTest();
                                                             }
                                                         };

            WaitXAndCheckResult(actionExectutedAfterDelay, DelayValue);
        }

        protected override void NextTest()
        {
            _stringFretToFind = GetRandomItem();
            ResetTryNo();
            RunTestForCurrentStringFret();
        }

        private void RunTestForCurrentStringFret()
        {
            PlaySoundIfEnabled(_stringFretToFind);
            FretBoardManager.FretBoard.FretBoardGuiBuilder.DrawNote(_stringFretToFind);
        }
    }
}