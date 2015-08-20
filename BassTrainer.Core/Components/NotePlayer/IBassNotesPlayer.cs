using BassTrainer.Core.Const;
using WavPlayer;

namespace BassTrainer.Core.Components.NotePlayer
{
    public interface IBassNotesPlayer
    {
        void SetPlayerAttributes(Settings.Settings settings);
        void ChangeMuteState();
        bool IsPlaying();
        void StopIfPlaying();
        void SetMuteText();
        void PlayAgain();
        void PlayNote(params Note[] notes);
        void Stop();
        void PlayNote(params StringFretPair[] stringFretPairs);
        void PlayNoteContinuosly(LoopStream.OnLoopEndEvent onLoopAction = null,params StringFretPair[] stringFretPairs);
        double Volume { get; set; }
        bool IsMuted { get; set; }
        string MuteText { get; set; }
    }
}