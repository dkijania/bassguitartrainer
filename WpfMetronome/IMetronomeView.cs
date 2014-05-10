using System;
using DrumMachine.Engine.Pattern;

namespace WpfMetronome
{
    public interface IMetronomeView : ICommandExceptionHandler
    {
        void OnStartClick();
        void OnStopClick();
        bool IsFullScreen { get; set; }
        bool ShowCounter { get; set; }
        void OnBeat(DrumPattern drumPattern);
    }
}