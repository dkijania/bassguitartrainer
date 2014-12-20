using System;
using System.Collections.Generic;
using System.ComponentModel;
using DrumMachine;
using DrumMachine.Engine.Model;
using DrumMachine.TimeSignature;
using Metronome.Resources.Metronome.AudioSamples;

namespace Metronome.UI.WPF
{
    public class MetronomeModel
    {
        private DrumMachineKit _drumMachine;
        private readonly IAudioSamplesPresetCollection _samplesPresetCollection;


        public AudioPresetEnum AudioPreset { get; private set; }
        public TimeSignature TimeSignature { get; private set; }
        public BpmModel BpmModel { get; private set; }


        public MetronomeModel(TimeSignature timeSignature, BpmModel bpmModel)
        {
            TimeSignature = timeSignature;
            AudioPreset = AudioPresetEnum.Drum;
            _samplesPresetCollection = new BuiltInAudioSamplePresetCollection();
            BpmModel = bpmModel;
        }

        public void SetAudioPreset(object audioPreset)
        {
            AudioPreset = (AudioPresetEnum) Enum.Parse(typeof (AudioPresetEnum), audioPreset.ToString());
        }

        public void SetTempo(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            if (_drumMachine == null) return;
            _drumMachine.Tempo = BpmModel.BpmValue;
        }

        public void PlayMetronome(params DrumKit.OnBeatHit[] onBeatHitEventHandlers)
        {
            CheckIfTimeSignatureIsSet();
            _drumMachine = new DrumMachineKit(TimeSignature.SelectedTimeSignature,
                _samplesPresetCollection.GetBoth(AudioPreset))
            {
                Tempo = BpmModel.BpmValue
            };
            RegisterAllEventHandlers(onBeatHitEventHandlers);
            _drumMachine.Play();
        }

        private void RegisterAllEventHandlers(IEnumerable<DrumKit.OnBeatHit> onBeatHitEventHandlers)
        {
            foreach (var onBeatHitEventHandler in onBeatHitEventHandlers)
            {
                _drumMachine.OnBeatHitEvent += onBeatHitEventHandler;
            }
        }

        private void CheckIfTimeSignatureIsSet()
        {
            if (TimeSignature.SelectedTimeSignature.Equals(TimeSignatureOptions.NotSet))
            {
                throw new MetronomeException("Time singature not defined");
            }
        }

        public void StopMetronome()
        {
            if (_drumMachine != null)
            {
                _drumMachine.Stop();
            }
        }

        public bool IsStopped
        {
            get { return _drumMachine == null || _drumMachine.IsStopped; }
        }
    }
}