using System;

namespace DrumMachine.TimeSignature
{
    public class TimeSignatureException : Exception
    {
        public TimeSignatureException()
        {
        }

        public TimeSignatureException(string message) : base(message)
        {
        }
    }
}