using System;
using System.Diagnostics;
using System.Windows.Input;

namespace WpfExtensions
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> execute;
        private readonly Predicate<object> canExecute;

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action execute, Func<bool> canExecute)
            : this((s) => execute(), (s) => canExecute())
        {
        }

        public RelayCommand(Action execute)
            : this((s) => execute(), null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            this.execute = execute;
            this.canExecute = canExecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }

    public class DelegateCommand : ICommand
    {
        private readonly Action action;
        private readonly ICommandExceptionHandler _handler;
        private bool isEnabled;

        public DelegateCommand(Action action,ICommandExceptionHandler handler)
        {
            this.action = action;
            _handler = handler;
            isEnabled = true;
        }

        public DelegateCommand(Action action)
        {
            this.action = action;
            isEnabled = true;
        }


        public void Execute(object parameter)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                if (_handler == null)
                {
                    throw new Exception(ex.Message,ex);
                }
                _handler.OnErrorRaised(ex);
            }
        }

        public bool CanExecute(object parameter)
        {
            return isEnabled;
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    OnCanExecuteChanged();
                }
            }
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            EventHandler handler = CanExecuteChanged;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }

    public interface ICommandExceptionHandler
    {
        void OnErrorRaised(Exception exception);
    }
}