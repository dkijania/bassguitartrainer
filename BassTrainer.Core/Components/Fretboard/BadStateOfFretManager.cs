using System;

namespace BassTrainer.Core.Components.Fretboard
{
    public class BadStateOfFretManager : Exception
    {
        public BadStateOfFretManager(string setSelectionModeBefore) : base(setSelectionModeBefore)
        {
        }
    }
}