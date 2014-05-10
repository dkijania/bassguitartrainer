using System;
using System.Runtime.Serialization;

namespace BassNotesMasterApi.Excercise
{
    public class ExcerciseException : Exception
    {
        public ExcerciseException(string message) : base(message)
        {
        }

        public ExcerciseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExcerciseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
