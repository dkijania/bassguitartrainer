namespace BassTrainer.Core.Settings
{
    public class UndoManager<T>
    {
        private T _value;
        private bool _lastChangeResult;

        public bool LastChangeResult
        {
            get { return _lastChangeResult; }
            set { _lastChangeResult = value; }
        }

        public T Value
        {
            get { return _value; }
        }

        public UndoManager(T value)
        {
            _value = value;
        }

        public void SetNewValue(T newValue)
        {
            _lastChangeResult = SetNewValueAndSetResult(newValue);
        }

        private bool SetNewValueAndSetResult(T newValue)
        {
            if (_value.Equals(newValue))
                return false;

            _value = newValue;
            return true;
        }
    }
}