using System.ComponentModel;

namespace BassNotesMasterApi.Statistics
{
    public class StatisticRow : INotifyPropertyChanged

    {
        public StatisticRow(string excerciseName)
        {
            Excercise = excerciseName;
        }

        public string Excercise { get; private set; }
        public int Questions { get; private set; }
        public int Correct { get; private set; }
        public int Wrong { get; private set; }
        public int Skipped { get; private set; }

       

        public void ResetAll()
        {
            Questions = 0;
            Correct = 0;
            Wrong = 0;
            Skipped = 0;
            RaisePropertyChanged("Questions");
            RaisePropertyChanged("Correct");
            RaisePropertyChanged("Wrong"); 
            RaisePropertyChanged("Skipped");
        }

        public void AddResult(bool result)
        {
            ++Questions;
            RaisePropertyChanged("Questions");
            if (result)
            {
                ++Correct;
                RaisePropertyChanged("Correct");
            }
            else
            {
                ++Wrong;
                RaisePropertyChanged("Wrong");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Skip()
        {
            ++Questions;
            RaisePropertyChanged("Questions");
            ++Skipped;
            RaisePropertyChanged("Skipped");
             }
    }
}