using System.Collections.ObjectModel;
using System.Windows.Input;
using DrumMachine.TimeSignature;
using WpfExtensions;

namespace DrumMachine.UI.WPF.TimeSignature
{
    public class TimeSignatureViewModel : BindingDataContextBase
    {
        public ObservableCollection<int> PossibleNotesCountPerMeasure { get; private set; }
        public ObservableCollection<int> PosibleNotesTypes { get; private set; }
        public DrumMachine.TimeSignature.TimeSignature TimeSignature = new DrumMachine.TimeSignature.TimeSignature();
     
        public int CustomUpper
        {
            set
            {
                TimeSignature.CustomSignature.NotesPerMeasure = value;
                OnPropertyChanged();
            }
        }
        
        public int CustomLower
        {
            set
            {
                TimeSignature.CustomSignature.NoteType = (NoteTypeEnum)value;
                OnPropertyChanged();
            }
        }

        public bool EnableTimeSignature
        {
            get { return TimeSignature.IsTimeSignatureEnabled; }
            set
            {
                TimeSignature.IsTimeSignatureEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool UseCustomTimeSignature
        {
            get { return TimeSignature.IsCustomTimeSignatureEnabled; }
            set
            {
                TimeSignature.IsCustomTimeSignatureEnabled = value;
                OnPropertyChanged();
            }
        }


        public ICommand SetStandardSignature { get; private set; }
        public ICommand SetCustomSignature { get; private set; }

        public TimeSignatureViewModel()
        {
            EnableTimeSignature = false;
            UseCustomTimeSignature = false;
            PosibleNotesTypes = new ObservableCollection<int>(TimeSignatureOptions.PosibleNotesTypes);
            PossibleNotesCountPerMeasure =
                new ObservableCollection<int>(TimeSignatureOptions.PossibleNotesCountPerMeasure);

            SetCustomSignature = new RelayCommand(SetCustomTimeSignature);
            SetStandardSignature = new RelayCommand(SetStandardTimeSignature);
        }

        public delegate void StandardTimeChanged(string signature);
        public event StandardTimeChanged StandardTimeChangedEvent;
     
        public void SetStandardTimeSignature(object signatureAsString)
        {
            TimeSignature.StandardSignature = TimeSignatureOptions.FromString(signatureAsString.ToString());
            if(StandardTimeChangedEvent != null)
            {
                StandardTimeChangedEvent.Invoke(signatureAsString.ToString());
            }
        }

        public void SetCustomTimeSignature(object signatureAsString)
        {
            TimeSignature.CustomSignature = TimeSignatureOptions.FromString(signatureAsString.ToString());
        }
    }
}