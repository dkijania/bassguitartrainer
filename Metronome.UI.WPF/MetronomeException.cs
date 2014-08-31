using System;

namespace Metronome.UI.WPF
{
    public class MetronomeException : Exception
    {
        public MetronomeException()
        {
        }

        public MetronomeException(string message) : base(message)
        {
        }
    }
}