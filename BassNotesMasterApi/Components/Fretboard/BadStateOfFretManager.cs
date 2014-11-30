using System;

namespace BassNotesMasterApi.Components.Fretboard
{
    public class BadStateOfFretManager : Exception
    {
        public BadStateOfFretManager(string setSelectionModeBefore) : base(setSelectionModeBefore)
        {
        }
    }
}