using System.Collections.Generic;
using System.IO;
using NAudio.Wave;

namespace WavPlayer
{
    public class WavPlayer : IWavPlayer
    {
        private WaveOut _waveOut;
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

        public PlaybackState PlaybackState
        {
            get { return _waveOut == null ? PlaybackState.Stopped : _waveOut.PlaybackState; }
        }

        public void PlaySoundContiunuosly(LoopStream.OnLoopEndEvent onLoopAction, Stream stream)
        {
            var loop = PreapareStream(stream);
            loop.OnLoopEnd += onLoopAction;
            PlaySoundInContinuosFashion(loop);
        }

        private LoopStream PreapareStream(Stream stream)
        {
            ResetStreamPosition(stream);
            var reader = new WaveFileReader(stream);
            return new LoopStream(reader);
        }

        private void PlaySoundInContinuosFashion(IWaveProvider loop)
        {
            if (_waveOut != null)
            {
                StopCurrentSound();
            }
            if (_waveOut != null) return;
            _waveOut = new WaveOut { Volume = Volume };
            _waveOut.Init(loop);
            _waveOut.Play();
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

        public void PlaySound(Stream wavStream)
        {
            StopCurrentSoundIfNothingIsInQueue();
            ResetStreamPosition(wavStream);
            _reader = new WaveFileReader(wavStream);
            _waveOut = new WaveOut {Volume = Volume};
            PlayNextSoundInQueueIfExists();
            _waveOut.Init(_reader);
            _waveOut.Play();
        }

        private void StopCurrentSoundIfNothingIsInQueue()
        {
            if (_queueStreams.Count == 0)
                StopCurrentSound();
        }

        public void StopCurrentSound()
        {
            if (_waveOut != null)
            {
                _waveOut.Volume = 0;
                _waveOut.Stop();
                _waveOut.Dispose();
                _waveOut = null;
            }
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