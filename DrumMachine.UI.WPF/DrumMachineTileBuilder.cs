using System.Linq;
using DrumMachine.Resources.DrumMachine.Audio;
using DrumMachine.UI.WPF.Pattern;

namespace DrumMachine.UI.WPF
{
    //TODO: remove
    public class DrumMachineTileBuilder
    {
        private static readonly WavPlayer.WavPlayer WavPlayer = new WavPlayer.WavPlayer();

        public static DrumMachineTile Build(int row, int column)
        {
            var drumMachineTile = new DrumMachineTile(row, column);
         //   drumMachineTile.OnSelectEvent += drumMachineTile_OnSelectEvent;
            return drumMachineTile;
        }

      /*  private static void drumMachineTile_OnSelectEvent(int row, int column,bool isSelected)
        {
            if(!isSelected)
                return;

            var sampleName = DefaultAudioSampler.Instance.Names.ElementAt(row);
            var sample = DefaultAudioSampler.Instance[sampleName];
            WavPlayer.Volume = 0.5f;
            WavPlayer.Play(sample.RawStream);
        }*/
    }
}