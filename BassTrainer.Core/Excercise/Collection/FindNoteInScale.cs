using System;
using System.Linq;
using BassTrainer.Core.Components;
using BassTrainer.Core.Const;
using BassTrainer.Core.Excercise.Options;
using BassTrainer.Core.Excercise.SelectionSetters;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Excercise.Collection
{
    public class FindNoteInScale : BaseExcercise
    {
        private StringFretPair _stringFretToFind;

        protected override ComponentId[] ComponentsVisibleDuringExcercise
        {
            get { return new[] { ComponentId.Fretboard}; }
        }

        protected override string[] OptionsToShow
        {
            get { return new[] {Excercise.Options.Options.PlayNote,Excercise.Options.Options.Scale}; }
        }

        public FindNoteInScale(Settings.Settings settings, IExcerciseOptionGuiManager guiManager,
                               IVisibilityManager visibilityManager,IComponentModeManager componentModeManager)
            : base(settings, guiManager, visibilityManager,componentModeManager, new OnlyValidRootsSelectionSetter())
        {
        }

        protected override void RenewSubscriptions()
        {
        }

        protected override void ContinuteTest()
        {
            RunTestForCurrentStringFret();
        }

        public void OnMouseClick(StringFretPair stringFretPair)
        {
            var musicNotation = ComponentsLocator.Instance.MusicNotationComponent;

            var possibleAnwsers = FretBoardMapping.GetAllEquivalentPositions(stringFretPair);
            var result = possibleAnwsers.Contains(_stringFretToFind);
            musicNotation.RedrawNote(stringFretPair, result);

            EventHandler actionExectutedAfterDelay = (sender, args) =>
                                                         {
                                                             musicNotation.ResetGui();
                                                             if (result || ++TryNo == Attempts)
                                                             {
                                                                 RegisterResult(result);
                                                                 FretBoardComponent.FretBoard.ClearView();
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
            FretBoardComponent.FretBoard.DrawNote(_stringFretToFind);
        }
    }
}