using System.Collections.Generic;
using System.IO;
using NAudio.Wave;

namespace BassNotesMasterApi.NotePlayer
{
    public class WavPlayer
    {
        private WaveOut _waveOut = new WaveOut();
        private WaveFileReader _reader;
        private readonly Stack<Stream> _queueStreams = new Stack<Stream>();
        
        public void Play(Stream stream)
        {
            PlaySound(stream);
        }

        public void Play(Stream[] streams)
        {
            FillQueue(streams);
            PlaySound(_queueStreams.Pop());
        }

        private void FillQueue(IEnumerable<Stream> streams)
        {
            _queueStreams.Clear();
            foreach (var stream in streams)
            {
                _queueStreams.Push(stream);
            }
        }
        
        public float Volume { get; set; }

        private void PlaySound(Stream wavStream)
        {
            StopCurrentSoundIfNothingIsInQueue();
            ResetStreamPosition(wavStream);
            _reader = new WaveFileReader(wavStream);
            _waveOut = new WaveOut { Volume = Volume};
            PlayNextSoundInQueueIfExists();
            _waveOut.Init(_reader);
            _waveOut.Play();
        }

        private void StopCurrentSoundIfNothingIsInQueue()
        {
            if (_queueStreams.Count == 0)
                StopCurrentSound();
        }
        
        private void StopCurrentSound()
        {
            _waveOut.Volume = 0;
            _waveOut.Stop();
            _waveOut.Dispose();
            _waveOut = null;
            if (_reader == null) return;
            _reader.Dispose();
            _reader = null;
        }

        private static void ResetStreamPosition(Stream stream)
        {
            stream.Position = 0;
        }

        private void PlayNextSoundInQueueIfExists()
        {
            if (_queueStreams.Count >= 1)
            {
                _waveOut.PlaybackStopped += WaveOutPlaybackStopped;
            }
        }

        private void WaveOutPlaybackStopped(object sender, StoppedEventArgs e)
        {
            _waveOut.Stop();
            if (_queueStreams.Count > 0)
            {
                PlaySound(_queueStreams.Pop());
            }
        }
    }
}