using System.Collections.ObjectModel;
using System.Windows.Input;
using DrumMachine.TimeSignature;
using WpfExtensions;
using WpfMetronome;

namespace BassNotesMaster.WpfViews.Metronome
{
    public class TimeSignatureViewModel : BindingDataContextBase, ITimeSignatureViewModel
    {
        public ITimeSignatureView View { get; set; }
        public ObservableCollection<int> PossibleNotesCountPerMeasure { get; private set; }
        public ObservableCollection<int> PosibleNotesTypes { get; private set; }

        private bool _customTimeSignature;
        private bool _useTimeSignature;

        public int CustomUpper
        {
            set
            {
                CustomSignature.NotesPerMeasure = value;
                OnPropertyChanged();
            }
        }
        
        public int CustomLower
        {
            set
            {
                CustomSignature.NoteType = (TimeSignatureOptions.NoteTypeEnum) value;
                OnPropertyChanged();
            }
        }

        public bool EnableTimeSignature
        {
            get { return _useTimeSignature; }
            set
            {
                _useTimeSignature = value;
                View.OnUseTimeSignature(_useTimeSignature);
                OnPropertyChanged();
            }
        }

        public bool UseCustomTimeSignature
        {
            get { return _customTimeSignature; }
            set
            {
                _customTimeSignature = value;
                View.OnUseCustomTimeSignatureChange(value);
                OnPropertyChanged();
            }
        }

        public TimeSignatureOptions StandardSignature = new TimeSignatureOptions();
        public TimeSignatureOptions CustomSignature = new TimeSignatureOptions();

        public TimeSignatureOptions SelectedTimeSignature
        {
            get
            {
                if (!EnableTimeSignature)
                {
                    return TimeSignatureOptions.Unison;
                }
                return UseCustomTimeSignature ? CustomSignature : StandardSignature;
            }
        }

        public ICommand SetStandardSignature { get; private set; }
        public ICommand SetCustomSignature { get; private set; }

        public TimeSignatureViewModel(ITimeSignatureView view)
        {
            View = view;
            EnableTimeSignature = false;
            PosibleNotesTypes = new ObservableCollection<int>(TimeSignatureOptions.PosibleNotesTypes);
            PossibleNotesCountPerMeasure =
                new ObservableCollection<int>(TimeSignatureOptions.PossibleNotesCountPerMeasure);

            SetCustomSignature = new RelayCommand(SetCustomTimeSignature);
            SetStandardSignature = new RelayCommand(SetStandardTimeSignature);
        }

        public event StandardTimeChanged StandardTimeChangedEvent;
        public delegate void StandardTimeChanged(string signature); 


        public void SetStandardTimeSignature(object signatureAsString)
        {
            StandardSignature = TimeSignatureOptions.FromString(signatureAsString.ToString());
            if(StandardTimeChangedEvent != null)
            {
                StandardTimeChangedEvent.Invoke(signatureAsString.ToString());
            }
        }

        public void SetCustomTimeSignature(object signatureAsString)
        {
            CustomSignature = TimeSignatureOptions.FromString(signatureAsString.ToString());
        }
    }
}