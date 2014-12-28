using System;

namespace BassTrainer.Core.Components
{
    public class ComponentsLocatorException : Exception
    {
        public ComponentsLocatorException(string message) : base(message)
        {
        }

        public ComponentsLocatorException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}