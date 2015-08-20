using NAudio.Wave;

namespace WavPlayer
{
    public class LoopStream : WaveStream
    {
        private readonly WaveStream _sourceStream;

        public event OnLoopEndEvent OnLoopEnd;
        public delegate void OnLoopEndEvent();

        protected virtual void InvokeOnLoopEnd()
        {
            OnLoopEndEvent handler = OnLoopEnd;
            if (handler != null) handler();
        }

        public LoopStream(WaveStream sourceStream)
        {
            this._sourceStream = sourceStream;
            this.EnableLooping = true;
        }

        public bool EnableLooping { get; set; }

        public override WaveFormat WaveFormat
        {
            get { return _sourceStream.WaveFormat; }
        }

        public override long Length
        {
            get { return _sourceStream.Length; }
        }

        public override long Position
        {
            get { return _sourceStream.Position; }
            set { _sourceStream.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            var totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                var bytesRead = _sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);
                if (bytesRead == 0)
                {
                    if (_sourceStream.Position == 0 || !EnableLooping)
                    {
                        break;
                    }
                    ResetPosition();
                    FireOnLoopEndEvent();
                }
                totalBytesRead += bytesRead;
            }
            return totalBytesRead;
        }

        private void ResetPosition()
        {
            _sourceStream.Position = 0;
        }

        private void FireOnLoopEndEvent()
        {
            InvokeOnLoopEnd();
        }
    }
}