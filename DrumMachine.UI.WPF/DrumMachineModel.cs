using System;
using System.Linq;
using DrumMachine.Engine.Pattern;
using DrumMachine.Resources.DrumMachine.Audio;
using DrumMachine.TimeSignature;
using DrumMachine.UI.WPF.Pattern;
using DrumMachine.UI.WPF.Pattern.Converters;

namespace DrumMachine.UI.WPF
{
    internal class DrumMachineModel
    {
        private static readonly WavPlayer.WavPlayer WavPlayer = new WavPlayer.WavPlayer();
        private DrumMachineKit _drumMachineKit;
        private readonly IPatternTilesManipulator _tilesManipulator;
        private readonly FileDrumPatternConverter _fileDrumPatternConverter;
        private readonly UIDrumPatternConverter _uiDrumPatternConverter;

        public DrumMachineModel(IPatternTilesManipulator tilesManipulator)
        {
            _tilesManipulator = tilesManipulator;
            _fileDrumPatternConverter = new FileDrumPatternConverter(_tilesManipulator);
            _uiDrumPatternConverter = new UIDrumPatternConverter();
        }

        //TODO: add control for volume
        public void PlaySound(int row)
        {
            var audioSampler = DefaultAudioSampler.Instance;
            var sampleName = audioSampler.Names.ElementAt(row);
            var sample = audioSampler[sampleName];
            WavPlayer.Volume = 0.5f;
            WavPlayer.Play(sample.RawStream);
        }

        //TODO: fix bugs connected with higlighter and uncomment line
        public void Play(int tempoValue, string selectedNoteType, int measures)
        {
            var drumPattern = ToDrumPattern(selectedNoteType, measures, tempoValue);
            _drumMachineKit = new DrumMachineKit(drumPattern) {Tempo = tempoValue};
          //  _drumMachineKit.OnBeatHitEvent += _tilesManipulator.PatternHighlighter.HighlightColumnOnBeat;
            _drumMachineKit.Play();
        }

        public DrumPattern ToDrumPattern(string selectedNoteType, int measures, int tempoValue)
        {
            int samplesCount = DefaultAudioSampler.Instance.Samples.Count();
            var noOfBars = _tilesManipulator.BarsCount;
            var noteType = (NoteTypeEnum) Enum.Parse(typeof (NoteTypeEnum), selectedNoteType);
            var drum = new DrumPattern(samplesCount, new TimeSignatureOptions(measures*noOfBars, noteType));
            var drumPatternSettings = new DrumPatternSettings
            {
                Bars = noOfBars,
                Measures = measures,
                NoteType = selectedNoteType,
                Tempo = tempoValue
            };
            _tilesManipulator.FillDrumPattern(drum,_uiDrumPatternConverter);
            drum.Settings = drumPatternSettings;
            return drum;
        }

        public void Stop()
        {
            if (_drumMachineKit != null)
            {
                _drumMachineKit.Stop();
            }
            _tilesManipulator.PatternHighlighter.CleanUpHighlight();
        }

        public void TryToSetTempo(int tempoValue)
        {
            if (_drumMachineKit != null) _drumMachineKit.Tempo = tempoValue;
        }

        public void JoinCell(int row, int column)
        {
            _tilesManipulator.JoinCell(row, column);
        }

        public void SplitCell(int row, int column)
        {
            _tilesManipulator.SplitCell(row, column);
        }

        public void AddBar()
        {
            _tilesManipulator.AddBar();
        }

        public void RemoveBar()
        {
            _tilesManipulator.RemoveBar();
        }

        public void UpdatePatternStructure(int measures, int note)
        {
            _tilesManipulator.ResetBarsCount();
            _tilesManipulator.SetColumnsCount(measures, note);
            _tilesManipulator.SetColumnsSpan(note);
        }

        public void Clear()
        {
            _tilesManipulator.Clear();
        }

        public void ExportDrumPattern(String fileName, string selectedNoteType, int measures, int tempoValue)
        {
            _fileDrumPatternConverter.SaveToFile(ToDrumPattern(selectedNoteType, measures, tempoValue), fileName);
        }

        public DrumPatternSettings ImportDrumPatternAndGetNewSettings(string fileName)
        {
            var drumPattern = _fileDrumPatternConverter.ReadFromFile(fileName);
            _tilesManipulator.ImportToUi(drumPattern,_uiDrumPatternConverter);
            return drumPattern.Settings;
        }
    }
}