using System;

namespace SimpleHelpSystem.History
{
    public class HelpHistoryException : Exception
    {
        public HelpHistoryException(string message)
            : base(message)
        {
        }
    }
}