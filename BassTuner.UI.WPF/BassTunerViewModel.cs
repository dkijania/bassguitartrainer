using System;
using System.Collections.Generic;
using System.Windows.Input;
using BassTrainer.Core.Const.BassGuitar.Parameters;
using WpfExtensions;

namespace BassTuner.UI.WPF
{
    public class BassTunerViewModel : BindingDataContextBase
    {
        private String _currentlyActiveInstrumentType;
        private String _currentlyActiveInstrumentTuning;

        private bool _instrumentTypeVisible;
        private bool _instrumentTuningVisible;
        private bool _tuningSoundsVisible;
        
        public IList<String> InstrumentTuning { get; private set; }
        public IList<String> InstrumentsTypes { get; private set; }

        public ICommand SetActiveInstrumentTypeCommand { get; private set; }
        public ICommand SetActiveInstrumentTuningCommand { get; private set; }
        
        private readonly BassGuitarTypes _bassGuitarTypes = new BassGuitarTypes();
        private readonly BassGuitar4StringTuning _bassGuitarTunings = new BassGuitar4StringTuning();
        
        public BassTunerViewModel()
        {
            InstrumentsTypes = new List<string>(_bassGuitarTypes.Values);
            SetActiveInstrumentTypeCommand = new DelegateCommand(SetActiveInstrumentType);
            SetActiveInstrumentTuningCommand = new DelegateCommand(SetActiveInstrumentTuning);
            
            
            InstrumentTuning = new List<string>(_bassGuitarTunings.Values);
           
            InstrumentTypeVisible = true;
            InstrumentTuningVisible = false;
        }


        private void SetActiveInstrumentType()
        {
            CurrentlyActiveInstrumentType = InstrumentsTypes[0];
            InstrumentTuningVisible = true;
        }

        private void SetActiveInstrumentTuning()
        {
            CurrentlyActiveInstrumentTuning = InstrumentTuning[0];
        }

        public string CurrentlyActiveInstrumentType
        {
            get { return _currentlyActiveInstrumentType; }
            set
            {
                _currentlyActiveInstrumentType = value;
                OnPropertyChanged();
            }
        }

        public string CurrentlyActiveInstrumentTuning
        {
            get { return _currentlyActiveInstrumentTuning; }
            set
            {
                _currentlyActiveInstrumentTuning = value;
                OnPropertyChanged();
            }
        }

        public bool InstrumentTuningVisible
        {
            get { return _instrumentTuningVisible; }
            set
            {
                _instrumentTuningVisible = value;
                OnPropertyChanged();
            }
        }

        public bool InstrumentTypeVisible
        {
            get { return _instrumentTypeVisible; }
            set
            {
                _instrumentTypeVisible = value;
                OnPropertyChanged();
            }
        }

        public bool TuningSoundsVisible
        {
            get { return _tuningSoundsVisible; }
            set
            {
                _tuningSoundsVisible = value;
                OnPropertyChanged();
            }
        }
    }
}
