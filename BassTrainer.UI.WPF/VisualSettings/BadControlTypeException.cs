using System;
using System.Runtime.Serialization;

namespace BassTrainer.UI.WPF.VisualSettings
{
    public class BadControlTypeException : Exception
    {
        public BadControlTypeException()
        {
        }

        public BadControlTypeException(string message) : base(message)
        {
        }

        public BadControlTypeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BadControlTypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
