using System;

namespace SimpleHelpSystem.History
{
    [Serializable]
    public class HelpHistoryException : Exception
    {
        public HelpHistoryException(string message)
            : base(message)
        {
        }
    }
}