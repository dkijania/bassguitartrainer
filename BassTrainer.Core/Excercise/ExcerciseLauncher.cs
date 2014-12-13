using BassTrainer.Core.Components.Statistics;
using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Excercise
{
    public class ExcerciseLauncher
    {
        public ExcercisesDictionary ExcercisesDictionary { get; private set; }
        public StatisticsComponent StatisticsComponent { get; private set; }

        public IExcercise CurrentExcercise { get; set; }
        public IExcercise NewExcercise { get; set; }
        public IExcercise DefaultExcercise { get; set; }

        public ExcerciseLauncher(ExcercisesDictionary excercisesDictionary, StatisticsComponent statisticsComponent)
        {
            ExcercisesDictionary = excercisesDictionary;
            StatisticsComponent = statisticsComponent;
            DefaultExcercise = ExcercisesDictionary.DefaultExcercise;
        }

        public void RunDefaultExcercise()
        {
            CurrentExcercise = ExcercisesDictionary.DefaultExcercise;
            CurrentExcercise.Start(null);
            DefaultExcercise = CurrentExcercise;
        }

        public void SetNextExcercise(string byName)
        {
            NewExcercise = ExcercisesDictionary[byName];
        }

        public void StopCurrentExcercise()
        {
            CurrentExcercise.Stop();
        }

        public void StartNewExcercise(StringFretPair[] selectedItems)
        {
            CurrentExcercise = NewExcercise;
            CurrentExcercise.Start(selectedItems);
        }

        public void StopExcerciseAndStartDefault()
        {
            CurrentExcercise.Stop();
            CurrentExcercise = DefaultExcercise;
            CurrentExcercise.Start(null);
        }
    }
}