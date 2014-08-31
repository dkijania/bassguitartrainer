using System.Windows.Input;
using WpfExtensions;

namespace Metronome.UI.WPF
{
    public class BpmModelView : BindingDataContextBase
    {
        public readonly BpmModel Model = new BpmModel();

        public ICommand Increment { get; private set; }
        public ICommand Decrement { get; private set; }
        public ICommand Set { get; private set; }


        public int BpmValue
        {
            get { return Model.BpmValue; }
            set
            {
                Model.BpmValue = value;
                OnPropertyChanged();
            }
        }

        public BpmModelView()
        {
            Increment = new DelegateCommand(IncrementBpm);
            Decrement = new DelegateCommand(DecrementBpm);
            Set = new RelayCommand(SetBpm);
        }

        public void IncrementBpm()
        {
            Model.Increment();
            OnPropertyChanged("BpmValue");
        }

        public void DecrementBpm()
        {
            Model.Decrement();
            OnPropertyChanged("BpmValue");
        }

        public void SetBpm(object x)
        {
            Model.Set(int.Parse(x.ToString()));
            OnPropertyChanged("BpmValue");
        }
    }
}