using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace BassTuner.UI.WPF
{
    /// <summary>
    /// Interaction logic for BassTunerView.xaml
    /// </summary>
    public partial class BassTunerView : UserControl
    {
        public BassTunerView()
        {
            InitializeComponent();
            var viewModel = new BassTunerViewModel();
            DataContext = viewModel;
            viewModel.PropertyChanged += viewModel_PropertyChanged;
            FillListOfPossibleInstruments(viewModel.InstrumentsTypes,InstrumentsTypes,viewModel.SetActiveInstrumentTypeCommand);
     //       FillListOfPossibleInstruments(viewModel.InstrumentTuning,InstrumentsTunings,viewModel.ShowTuningReferenceSounds);
        }

        private void FillListOfPossibleInstruments(IEnumerable<String> instrumentsTypes, Panel wrapPanel,ICommand buttonCommand)
        {
            foreach (var instrumentsType in instrumentsTypes)
            {
                wrapPanel.Children.Add(CreateButtonWithContent(instrumentsType,buttonCommand));
            }
        }
        
        private Button CreateButtonWithContent(string content,ICommand command)
        {
            return new Button
            {
                Content = content,
                Height = 40,
                FontSize = 15,
                Command = command
            };
        }


        private void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        //    if (e.PropertyName.Equals("")) ;
        }
    }
}