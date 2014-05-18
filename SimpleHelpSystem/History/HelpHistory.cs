using System.Collections.Generic;
using System.Linq;

namespace SimpleHelpSystem.History
{
    public class HelpHistory<T> : IHelpHistory<T>
    {
        private readonly List<T> _history = new List<T>();
        private int _lastIndex = -1;

        public bool CanUndo
        {
            get { return _lastIndex > 0; }
        }

        public bool CanRedo
        {
            get { return _lastIndex < _history.Count - 1; }
        }

        public void Add(T t)
        {
            if (IsTheSamePageAlreadyAddedAtTheEnd(t))
            {
                return;
            }
            _history.Add(t);
            _lastIndex++;
        }

        private bool IsTheSamePageAlreadyAddedAtTheEnd(T t)
        {
            return _history.Any() && _history.Last().Equals(t);
        }

        public T Undo()
        {
            if (!CanUndo) throw new HelpHistoryException("Cannot Undo");
            return _history[--_lastIndex];
        }

        public T Redo()
        {
            if (!CanRedo) throw new HelpHistoryException("Cannot Redo");
            return _history[++_lastIndex];
        }
    }
}