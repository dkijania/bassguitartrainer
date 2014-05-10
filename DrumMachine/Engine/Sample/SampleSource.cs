using System;
using System.IO;
using NAudio.Wave;

namespace DrumMachine.Engine.Sample
{
    public class SampleSource
    {
        public static SampleSource CreateFromWaveFile(string fileName)
        {
            var memoryStream = new MemoryStream();
            using (var fileStream = new FileStream(fileName,FileMode.Open))
            {
                fileStream.Position = 0;
                fileStream.CopyTo(memoryStream);
            }

            using (var reader = new WaveFileReader(fileName))
            {
                var sp = reader.ToSampleProvider();
                var sourceSamples = (int)(reader.Length / (reader.WaveFormat.BitsPerSample / 8));
                var sampleData = new float[sourceSamples];
                int n = sp.Read(sampleData, 0, sourceSamples);
                if (n != sourceSamples)
                {
                    throw new InvalidOperationException(String.Format("Couldn't read the whole sample, expected {0} samples, got {1}", n, sourceSamples));
                }
                var ss = new SampleSource(sampleData, sp.WaveFormat,memoryStream);
                return ss;
            }
        }

        public SampleSource(float[] sampleData, WaveFormat waveFormat,Stream rawStream) :
            this(sampleData, waveFormat, 0, sampleData.Length)
        {
            RawStream = rawStream;
        }

        public SampleSource(float[] sampleData, WaveFormat waveFormat, int startIndex, int length)
        {
            this.SampleData = sampleData;
            this.SampleWaveFormat = waveFormat;
            this.StartIndex = startIndex;
            this.Length = length;
        }


        public Stream RawStream { get; private set; }
        /// <summary>
        /// Sample data
        /// </summary>
        public float[] SampleData { get; private set; }
        /// <summary>
        /// Format of sampleData
        /// </summary>
        public WaveFormat SampleWaveFormat { get; private set; }
        /// <summary>
        /// Index of the first sample to play
        /// </summary>
        public int StartIndex { get; private set; }
        /// <summary>
        /// Number of valid samples
        /// </summary>
        public int Length { get; private set; }
    }
}
