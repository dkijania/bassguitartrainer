using System;

namespace BassNotesMasterApi.Fretboard
{
    public class FretBoardException : Exception
    {
        public FretBoardException()
        {
        }

        public FretBoardException(string message) : base(message)
        {
        }

        public FretBoardException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
