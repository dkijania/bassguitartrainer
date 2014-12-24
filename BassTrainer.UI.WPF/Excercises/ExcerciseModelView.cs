using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using BassTrainer.Core.Components;
using WpfExtensions;

namespace BassTrainer.UI.WPF.Excercises
{
    public class ExcerciseModelView : BindingDataContextBase
    {
        private readonly ComponentsViewModelsLocator _locator;

        private bool _isExcerciseEndEnabled;
        private bool _isExcerciseStartEnabled;
        private bool _isExcercisePauseContEnabled;
        private bool _isExcercisesTypesEnabled;
        private bool _isChooseSelectionEnabled;
        private bool _areOptionsEnabled;
        
        private string _excerciseChooseSelectionContent;
        private string _excercisePauseContinueContent;
        private string _excerciseStartContent;

        private string _selectedExcercise;

        public ExcerciseModelView(ComponentsViewModelsLocator locator)
        {
            _locator = locator;

            Excercises = _locator.Launcher.ExcercisesDictionary.Keys;
            SelectedExcercise = Excercises.First();
            IsExcerciseEndEnabled = false;
            IsExcercisePauseContEnabled = false;
            IsChooseSelectionEnabled = true;
            IsExcercisesTypesEnabled = true;
            IsExcerciseStartEnabled = false;
            AreOptionsEnabled = true;

            ExcerciseStartContent = "Start";
            ExcercisePauseContinueContent = "Pause";
            ExcerciseChooseSelectionContent = "Choose Selection";

            ChooseSelectionCommand = new DelegateCommand(ChooseSelection);
            ExcercisePauseCommand = new DelegateCommand(ExcercisePause);
            ExcerciseStopCommand = new DelegateCommand(ExcerciseStop);
            ExcerciseStartCommand = new DelegateCommand(ExcerciseStart);
        }

        public ICommand ChooseSelectionCommand { get; private set; }
        public ICommand ExcercisePauseCommand { get; private set; }
        public ICommand ExcerciseStopCommand { get; private set; }
        public ICommand ExcerciseStartCommand { get; private set; }

        private void ExcercisePause()
        {
            IsExcerciseEndEnabled = true;
           
            if (_locator.IsCurrentExcercisePaused)
            {
                _locator.ContinueCurrentExcercise();
                ExcercisePauseContinueContent = "Pause";
                IsExcerciseStartEnabled = true;
                AreOptionsEnabled = false;
            }
            else
            {
                _locator.PauseCurrentExcercise();
                ExcercisePauseContinueContent = "Continue";
                IsExcerciseStartEnabled = false;
                AreOptionsEnabled = true;
            }
        }

        private void ChooseSelection()
        {
            if (!_locator.IsMode(ComponentMode.Selection))
            {
                ApplySelectionMode();
            }
            else
            {
                ApplyInfoMode();
            }
        }

        private void ApplySelectionMode()
        {
            ExcerciseChooseSelectionContent = "Cancel";
            IsExcercisesTypesEnabled = false;
            IsExcerciseStartEnabled = true;
            _locator.ApplyMode(ComponentMode.Selection);
        }

        private void ApplyInfoMode()
        {
            ExcerciseChooseSelectionContent = "Choose Selection";
            IsExcercisesTypesEnabled = true;
            IsExcerciseStartEnabled = false;
            _locator.ApplyMode(ComponentMode.Info);
        }

        private void ExcerciseStop()
        {
           IsExcerciseStartEnabled = false;
            IsExcerciseEndEnabled = false;
            IsExcercisePauseContEnabled = false;
            IsChooseSelectionEnabled = true;
            ExcercisePauseContinueContent = "Pause";
            ExcerciseStartContent = "Start";
            IsExcercisesTypesEnabled = true;
            AreOptionsEnabled = true;
            _locator.StopExcerciseAndStartDefault();
        }

        private void ExcerciseStart()
        {
            if (_locator.IsMode(ComponentMode.Excercise))
            {
                _locator.Launcher.CurrentExcercise.Skip();
                return;
            }
            try
            {
                _locator.StartNewExcercise();
            }
            catch (Exception ex)
            {
                ApplySelectionMode();
                throw ex;
            }
            IsExcercisePauseContEnabled = true;
            IsExcerciseEndEnabled = true;
            IsChooseSelectionEnabled = false;
            IsExcercisesTypesEnabled = false;
            AreOptionsEnabled = false;

            ExcerciseChooseSelectionContent = "Choose Selection";
            ExcerciseStartContent = "Skip";
        }
        
        public IEnumerable<string> Excercises { get; private set; }

        public string SelectedExcercise
        {
            get { return _selectedExcercise; }
            set
            {
                _selectedExcercise = value;
                OnPropertyChanged();
                _locator.SetNextExcercise(SelectedExcercise);
            }
        }

        public bool IsExcerciseEndEnabled
        {
            get { return _isExcerciseEndEnabled; }
            set
            {
                _isExcerciseEndEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsExcerciseStartEnabled
        {
            get { return _isExcerciseStartEnabled; }
            set
            {
                _isExcerciseStartEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool IsChooseSelectionEnabled
        {
            get { return _isChooseSelectionEnabled; }
            set
            {
                _isChooseSelectionEnabled = value; 
                OnPropertyChanged();
            }
        }

        public bool IsExcercisePauseContEnabled
        {
            get { return _isExcercisePauseContEnabled; }
            set
            {
                _isExcercisePauseContEnabled = value;
                OnPropertyChanged();
            }
        }

        public string ExcerciseChooseSelectionContent
        {
            get { return _excerciseChooseSelectionContent; }
            set
            {
                _excerciseChooseSelectionContent = value;
                OnPropertyChanged();
            }
        }

        public string ExcercisePauseContinueContent
        {
            get { return _excercisePauseContinueContent; }
            set
            {
                _excercisePauseContinueContent = value;
                OnPropertyChanged();
            }
        }

        public string ExcerciseStartContent
        {
            get { return _excerciseStartContent; }
            set
            {
                _excerciseStartContent = value;
                OnPropertyChanged();
            }
        }

        public bool IsExcercisesTypesEnabled
        {
            get { return _isExcercisesTypesEnabled; }
            set
            {
                _isExcercisesTypesEnabled = value;
                OnPropertyChanged();
            }
        }

        public bool AreOptionsEnabled
        {
            get { return _areOptionsEnabled; }
            set
            {
                _areOptionsEnabled = value;
                OnPropertyChanged();
            }
        }
    }
}