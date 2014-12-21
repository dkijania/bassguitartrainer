using BassTrainer.Core.Components;
using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Components.Interval;
using BassTrainer.Core.Components.Interval.Data;
using BassTrainer.Core.Components.Notation;
using BassTrainer.Core.Components.NotesView;
using BassTrainer.Core.Const;

namespace BassTrainer.Core.Excercise.Collection.Tutorial
{
    public class Tutorial : AbstractTutorial, IFretboardListener, INotesViewListener, IMusicNotationListener,
                            IIntervalListener
    {
        private readonly NotesToStringFretBoardMapping _fretBoardMapping = NotesToStringFretBoardMapping.Instance;
        private readonly ComponentsLocator _componentsLocator = ComponentsLocator.Instance;

        public Tutorial(IComponentModeManager componentModeManager) : base(componentModeManager)
        {
        }


        public override void BeforeStart()
        {
            _componentsLocator.FretboardComponent.Subscribe(this);
            _componentsLocator.NotesViewComponent.Subscribe(this);
            _componentsLocator.MusicNotationComponent.Subscribe(this);
            _componentsLocator.IntervalComponent.Subscribe(this);
        }
        
        public void IntervalShowEvent(IntervalRow row)
        {

            var fretboard = _componentsLocator.FretboardComponent;
            var intervalManager = _componentsLocator.IntervalComponent;
            var rootNote = intervalManager.GetRootNote(fretboard.FretBoard);
            if(rootNote == null)
            {
                throw new ExcerciseException("You must select root note (by clicking on fretboard)");
            }
            var outputNote = intervalManager.Calculate(rootNote, row.Semitone);
            fretboard.FretBoard.DrawAllMatchingNotes(outputNote);
        }

        public void IntervalPlayEvent(IntervalRow row)
        {
            var fretboard = _componentsLocator.FretboardComponent;
            var intervalManager = _componentsLocator.IntervalComponent;
            var bassNotesPlayer = _componentsLocator.PlayerManager;

            var rootNote = intervalManager.GetRootNote(fretboard.FretBoard);
            var outputNote = intervalManager.Calculate(rootNote, row.Semitone);
            bassNotesPlayer.PlayNote(outputNote, rootNote);
        }

        public void OnMouseClick(StringFretPair stringFretPair)
        {
            var fretboard = _componentsLocator.FretboardComponent;
            var bassNotesPlayer = _componentsLocator.PlayerManager;
            
            var collection = _fretBoardMapping.GetAllEquivalentPositions(stringFretPair);
            fretboard.FretBoard.RedrawNotes(collection);
            bassNotesPlayer.PlayNote(stringFretPair);
        }

        public void OnMouseClick(StringFretPair stringFretPair, FretBoard fretBoard)
        {
            var fretboard = _componentsLocator.FretboardComponent;
            var musicNotation= _componentsLocator.MusicNotationComponent;
            var bassNotesPlayer = _componentsLocator.PlayerManager;
            
            fretboard.FretBoard.RedrawNote(stringFretPair, isCorrect: true);
            bassNotesPlayer.PlayNote(stringFretPair);
            musicNotation.RedrawNote(stringFretPair);
        }


        public void OnMouseClick(Note note)
        {
            var fretboard = _componentsLocator.FretboardComponent;
            fretboard.ClearView();
            fretboard.FretBoard.DrawAllMatchingNoteWithoutOctaveCheck(note);
        }
    }
}