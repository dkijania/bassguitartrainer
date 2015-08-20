using System.IO;
using NAudio.Wave;

namespace WavPlayer
{
    public interface IWavPlayer
    {
        void Play(Stream stream);
        void Play(Stream[] streams);
        PlaybackState PlaybackState { get; }
        float Volume { get; set; }
        void PlaySoundContiunuosly(LoopStream.OnLoopEndEvent onLoopAction, Stream stream);
        void PlaySound(Stream wavStream);
        void StopCurrentSound();


    }
}