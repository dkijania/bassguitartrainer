using System;

namespace BassNotesMasterApi.Fretboard
{
    public class CoordinatesOutsideTheFretBoardException : Exception
    {
        public CoordinatesOutsideTheFretBoardException()
        {
        }

        public CoordinatesOutsideTheFretBoardException(string message) : base(message)
        {
        }

        public CoordinatesOutsideTheFretBoardException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}