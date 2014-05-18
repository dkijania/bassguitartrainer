using System.ComponentModel;

namespace SimpleHelpSystem.History
{
    public class HelpHistoryPropertyNotifier<T> : INotifyPropertyChanged
    {
        private readonly HelpHistory<T> _helpHistory = new HelpHistory<T>();

        public void Add(T t)
        {
            _helpHistory.Add(t);
            RaisePropertyChanged();
        }

        private void RaisePropertyChanged()
        {
            OnPropertyChanged("CanUndo");
            OnPropertyChanged("CanRedo");
        }

        public T Undo()
        {
            var document = _helpHistory.Undo();
            RaisePropertyChanged();
            return document;
        }

        public T Redo()
        {
            RaisePropertyChanged();
            var document = _helpHistory.Redo();
            RaisePropertyChanged();
            return document;
        }

        public bool CanUndo
        {
            get { return _helpHistory.CanUndo; }
        }

        public bool CanRedo
        {
            get { return _helpHistory.CanRedo; }
        }

        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}