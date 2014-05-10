using System;

namespace BassNotesMasterApi.Fretboard
{
    public class BadStateOfFretManager : Exception
    {
        public BadStateOfFretManager(string setSelectionModeBefore) : base(setSelectionModeBefore)
        {
        }
    }
}