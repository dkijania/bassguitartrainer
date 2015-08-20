using System;
using System.Collections;
using System.IO;
using System.Linq;
using BassTrainer.Core.Const;
using BassTrainer.Core.Resources;
using BassTrainer.Core.Utils;
using NAudio.Wave;
using WavPlayer;

namespace BassTrainer.Core.Components.NotePlayer
{
    public class BassNotesPlayer : PropertyChangedNotifier, IBassNotesPlayer
    {
        private readonly NotesToStringFretBoardMapping _notesToStringFretBoardMapping =
            NotesToStringFretBoardMapping.Instance;
        private StringFretPair[] _lastPlayedPositions;
        private readonly ResourcesManager _resourcesManager = ResourcesManager.Instance;
        private readonly IWavPlayer _player = new WavPlayer.WavPlayer();

        private double _volume;
        private bool _isMuted;
        private string _muteText;
       
        public BassNotesPlayer(Settings.Settings settings)
        {
            SetPlayerAttributes(settings);
        }

        public void SetPlayerAttributes(Settings.Settings settings)
        {
            IsMuted = settings.IsPlayerMuted.Value;
            SetMuteText();
            Volume = settings.Volume.Value;
        }

        public void ChangeMuteState()
        {
            IsMuted = !IsMuted;
            SetMuteText();
            Settings.Settings.Instance.IsPlayerMuted.SetNewValue(IsMuted);
        }


        public bool IsPlaying()
        {
            return _player.PlaybackState == PlaybackState.Playing;
        }

        public void StopIfPlaying()
        {
            if (IsPlaying())
            {
                Stop();
            }
        }

        public void SetMuteText()
        {
            MuteText = IsMuted ? Properties.Resources.UnMute : Properties.Resources.Mute;
        }

        public void PlayAgain()
        {
            PlayNote(_lastPlayedPositions);
        }

        public void PlayNote(params Note[] notes)
        {
            var stringFretPairs = _notesToStringFretBoardMapping.Convert(notes);
            PlayNote(stringFretPairs);
        }

        public void PlayNote(params StringFretPair[] stringFretPairs)
        {
            if (IsListEmpty(stringFretPairs))
                return;

            var listToPlay = GetList(stringFretPairs);
            _player.Volume = GetVolume();
            _player.Play(listToPlay);
        }
        
        public void PlayNoteContinuosly(LoopStream.OnLoopEndEvent onLoopAction = null,params Note[] notes)
        {
            var stringFretPairs = _notesToStringFretBoardMapping.Convert(notes);
            PlayNoteContinuosly(onLoopAction,stringFretPairs);
        }

        public void PlayNoteContinuosly(LoopStream.OnLoopEndEvent onLoopAction = null, params StringFretPair[] stringFretPairs)
        {
            if (IsListEmpty(stringFretPairs))
                return;

            var listToPlay = GetList(stringFretPairs);
            _player.Volume = GetVolume();
            _player.PlaySoundContiunuosly(onLoopAction, listToPlay.First());
        }

        public void Stop()
        {
            _player.StopCurrentSound();
        }
        
        private bool IsListEmpty(ICollection stringFretPairs)
        {
            return stringFretPairs == null || stringFretPairs.Count == 0;
        }

        private Stream[] GetList(StringFretPair[] stringFretPairs)
        {
            RegisterLastPlayedPosition(stringFretPairs);
            return FillQueueOfStreams(stringFretPairs);
        }

        private Stream[] FillQueueOfStreams(StringFretPair[] stringFretPairs)
        {
            Array.Reverse(stringFretPairs);
            return stringFretPairs.Select(GetStreamOfSample).ToArray();
        }

        private void RegisterLastPlayedPosition(StringFretPair[] stringFretPairs)
        {
            _lastPlayedPositions = stringFretPairs;
        }

        private Stream GetStreamOfSample(StringFretPair stringFretPair)
        {
            var note = _notesToStringFretBoardMapping.GetNote(stringFretPair);
            return (Stream) _resourcesManager[note];
        }

        private float GetVolume()
        {
            return (float) (IsMuted ? 0 : Volume);
        }

        public double Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                OnPropertyChanged();
            }
        }

        public bool IsMuted
        {
            get { return _isMuted; }
            set
            {
                _isMuted = value;
                OnPropertyChanged();
            }
        }
        
        public string MuteText
        {
            get { return _muteText; }
            set
            {
                _muteText = value;
                OnPropertyChanged();
            }
        }
    }
}