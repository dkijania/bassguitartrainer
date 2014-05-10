using System;

namespace BassNotesMasterApi
{
    public class ManagersLocatorException : Exception
    {
        public ManagersLocatorException(string message) : base(message)
        {
        }

        public ManagersLocatorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}