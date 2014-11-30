using System;
using System.Runtime.Serialization;

namespace DrumMachine.Engine.Pattern
{
    public class DrumPatternParsingException : Exception
    {
        public DrumPatternParsingException()
        {
        }

        public DrumPatternParsingException(string format, params object[] objects) 
            : base(string.Format(format,objects))
        {
        }

        public DrumPatternParsingException(string message) : base(message)
        {
        }

        public DrumPatternParsingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DrumPatternParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
