using BassNotesMasterApi.Const;
using BassNotesMasterApi.Fretboard;
using BassNotesMasterApi.Interval;
using BassNotesMasterApi.Notation;
using BassNotesMasterApi.NotesView;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Excercise
{
      /*public class ExcerciseSubscriptionManager
    {
        private readonly AttributeReader _attributeReader = new AttributeReader();
        
        public void SubscribeExcerciseToManagers(IExcercise excercise)
        {
            var componentsToSubscribe = _attributeReader.ReadClassAttribute(excercise);
            
        }
    }





    public class UsingManager : Attribute
    {
        public ComponentId[] Components { get; private set; }

        public UsingManager(params ComponentId[] components)
        {
            Components = components;
        }
    }

    public class AttributeReaderException : Exception
    {
        public AttributeReaderException(string message) : base(message)
        {
        }

        public AttributeReaderException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    public class AttributeReader
    {
        public ComponentId[] ReadClassAttribute(object attributeOwner)
        {
            var attributeOwnerType = attributeOwner.GetType(); 
            foreach (var usingAttribute in attributeOwnerType.GetCustomAttributes(true).OfType<UsingManager>())
            {
                return usingAttribute.Components;
            }
            throw new AttributeReaderException("Couldn't find UsingManager class attribute for type" + attributeOwnerType.FullName);
        }
    }

  [UsingManager(ComponentId.NotesView,
        ComponentId.Notation,
        ComponentId.Intervals,
        ComponentId.Fretboard)]
   */ public class Tutorial : AbstractTutorial, IFretboardListener, INotesViewListener, IMusicNotationListener,
                            IIntervalListener
    {
        private readonly NotesToStringFretBoardMapping _fretBoardMapping = new NotesToStringFretBoardMapping();
        private readonly ManagersLocator _managersLocator = ManagersLocator.Instance;
            

        public override void BeforeStart()
        {
            _managersLocator.FretboardManager.Subscribe(this);
            _managersLocator.NotesViewManager.Subscribe(this);
            _managersLocator.MusicNotationManager.Subscribe(this);
            _managersLocator.IntervalManager.Subscribe(this);
        }


        public void IntervalShowEvent(IntervalRow row)
        {

            var fretboard = _managersLocator.FretboardManager;
            var intervalManager = _managersLocator.IntervalManager;
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
            var fretboard = _managersLocator.FretboardManager;
            var intervalManager = _managersLocator.IntervalManager;
            var bassNotesPlayer = _managersLocator.PlayerManager;

            var rootNote = intervalManager.GetRootNote(fretboard.FretBoard);
            var outputNote = intervalManager.Calculate(rootNote, row.Semitone);
            bassNotesPlayer.PlayNote(outputNote, rootNote);
        }

        public void OnMouseClick(StringFretPair stringFretPair)
        {
            var fretboard = _managersLocator.FretboardManager;
            var bassNotesPlayer = _managersLocator.PlayerManager;
            
            var collection = _fretBoardMapping.GetAllEquivalentPositions(stringFretPair);
            fretboard.FretBoard.FretBoardGuiBuilder.RedrawNotes(collection);
            bassNotesPlayer.PlayNote(stringFretPair);
        }

        public void OnMouseClick(StringFretPair stringFretPair, FretBoard fretBoard)
        {
            var fretboard = _managersLocator.FretboardManager;
            var musicNotation= _managersLocator.MusicNotationManager;
            var bassNotesPlayer = _managersLocator.PlayerManager;
            
            fretboard.FretBoard.FretBoardGuiBuilder.RedrawNote(stringFretPair, isCorrect: true);
            bassNotesPlayer.PlayNote(stringFretPair);
            musicNotation.RedrawNote(stringFretPair);
        }


        public void OnMouseClick(Note note)
        {
            var fretboard = _managersLocator.FretboardManager;
            fretboard.ClearView();
            fretboard.FretBoard.DrawAllMatchingNoteWithoutOctaveCheck(note);
        }
    }
}