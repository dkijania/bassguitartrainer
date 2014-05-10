using System;
using NAudio.Wave;

namespace DrumMachine.Engine.Sample
{
    public class MusicSampleProvider : AbstractBeatNotifier,ISampleProvider
    {
        private int _delayBy;
        private int _position;
        private readonly SampleSource _sampleSource;
        private int _counter;
        
        public MusicSampleProvider(SampleSource sampleSource)
        {
            this._sampleSource = sampleSource;
        }

        /// <summary>
        /// Samples to delay before returning anything
        /// </summary>
        public int DelayBy
        {
            get { return _delayBy; }
            set 
            { 
                if (value < 0)
                {
                    throw new ArgumentException("Cannot delay by negative number of samples");
                }
                _delayBy = value; 
            }
        }

        public WaveFormat WaveFormat
        {
            get { return this._sampleSource.SampleWaveFormat; }
        }
        
        public int Read(float[] buffer, int offset, int count)
        {
            int samplesWritten = 0;
            if (_position < _delayBy)
            {
                int zeroFill = Math.Min(_delayBy - _position, count);
                Array.Clear(buffer, offset, zeroFill);
                _position += zeroFill;
                samplesWritten += zeroFill;
             }
            if (samplesWritten < count)
            {
                int samplesNeeded = count - samplesWritten;
                int samplesAvailable = _sampleSource.Length - (_position - _delayBy);
                int samplesToCopy = Math.Min(samplesNeeded, samplesAvailable);
                Array.Copy(_sampleSource.SampleData, PositionInSampleSource, buffer, samplesWritten, samplesToCopy);
                _position += samplesToCopy;
                samplesWritten += samplesToCopy;
                if(_counter++ == 1)
                RaiseOnBeatEvent();
            }
            return samplesWritten;
        }

        private int PositionInSampleSource
        {
            get
            {
                return (_position - _delayBy) + _sampleSource.StartIndex;
            }
        }
    }
}
