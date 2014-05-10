using BassNotesMasterApi.Excercise;
using BassNotesMasterApi.Statistics;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi
{
    public class ExcerciseLauncher
    {
        public ExcercisesDictionary ExcercisesDictionary { get; private set; }
        public StatisticsManager StatisticsManager { get; private set; }

        public IExcercise CurrentExcercise { get; set; }
        public IExcercise NewExcercise { get; set; }
        public IExcercise DefaultExcercise { get; set; }

        public ExcerciseLauncher(ExcercisesDictionary excercisesDictionary, StatisticsManager statisticsManager)
        {
            ExcercisesDictionary = excercisesDictionary;
            StatisticsManager = statisticsManager;
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