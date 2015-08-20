using System;
using System.Collections;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using BassTrainer.Core.Const;
using WpfExtensions;

namespace BassTuner.UI.WPF
{
    /// <summary>
    /// Interaction logic for BassTunerView.xaml
    /// </summary>
    public partial class BassTunerView : UserControl
    {
        private const String StoryBoardName = "Storyboard";

        public BassTunerView()
        {
            InitializeComponent();
            var viewModel = new BassTunerViewModel();
            DataContext = viewModel;
            viewModel.PropertyChanged += viewModel_PropertyChanged;
            InstrumentsTypes.FillWithComponents(viewModel.InstrumentsTypes,
                viewModel.SetActiveInstrumentTypeCommand,
                CreateButtonWithContent);
        }

        private static Control CreateButtonWithContent(ICommand command, object content)
        {
            var button = new Button
            {
                Content = content,
                Height = 40,
                FontSize = 15,
                Command = command,
                CommandParameter = content
            };
            return button;
        }
        
        private void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var viewModel = sender as BassTunerViewModel;
            if (e.PropertyName.Equals("CurrentlyActiveInstrumentTuning"))
            {
                FillIntrumentsTuningNotes(viewModel.CurrentlyActiveInstrumentTuning.Notes, viewModel.PlayNoteCommand);
            }
            if (e.PropertyName.Equals("LastPlayedNote"))
            {
                StartAnimation(viewModel.LastPlayedNote);
           }
        }

        private void StartAnimation(Note note)
        {
            StartAnimationFor(x => x.Content.ToString().Equals(note.ToString()), StoryBoardName);
        }

        private void StartAnimationFor(Func<Button, bool> expression, String storyBoardName)
        {
            var sb = FindResource(storyBoardName) as Storyboard;
            var button = InstrumentsTunings.FindButton(expression);
            button.StartStoryboardAnimation(sb);
        }

        private void FillIntrumentsTuningNotes(IEnumerable notes,ICommand command)
        {
            InstrumentsTunings.FillWithComponents(notes,command, CreateButtonWithContent);
        }
    }
}