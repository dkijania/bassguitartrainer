using System;
using System.IO;
using System.Linq;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Resources;
using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.NotePlayer
{
    public class BassNotesPlayer : PropertyChangedNotifier
    {
        private double _volume;

        public double Volume
        {
            get { return _volume; }
            set
            {
                _volume = value;
                OnPropertyChanged();
            }
        }

        private bool _isMuted;

        public bool IsMuted
        {
            get { return _isMuted; }
            set
            {
                _isMuted = value;
                OnPropertyChanged();
            }
        }

        private string _muteText;

        public string MuteText
        {
            get { return _muteText; }
            set
            {
                _muteText = value;
                OnPropertyChanged();
            }
        }
        private readonly NotesToStringFretBoardMapping _notesToStringFretBoardMapping =
            NotesToStringFretBoardMapping.Instance;

        private StringFretPair[] _lastPlayedPositions;

        private readonly ResourcesManager _resourcesManager = ResourcesManager.Instance;
        private readonly WavPlayer.WavPlayer _player = new WavPlayer.WavPlayer();

        public BassNotesPlayer(Settings.Settings settings)
        {
            SetPlayerAttributes(settings);
        }

        private void SetPlayerAttributes(Settings.Settings settings)
        {
            IsMuted = settings.IsPlayerMuted.Value;
            ChangeMuteState();
            Volume = settings.Volume.Value;
        }

        public void ChangeMuteState()
        {
            IsMuted = !IsMuted;
            MuteText = IsMuted ? "Unmute" : "Mute";
            Settings.Settings.Instance.IsPlayerMuted.SetNewValue(IsMuted);
        }

        public void PlayAgain()
        {
            PlayNote(_lastPlayedPositions);
        }

        public void PlayNote(params Note[] notes)
        {
            if (notes == null || notes.Length == 0)
                return;

            var stringFretPairs = _notesToStringFretBoardMapping.Convert(notes);
            PlayNote(stringFretPairs);
        }

        public void PlayNote(params StringFretPair[] stringFretPairs)
        {
            if (stringFretPairs == null || stringFretPairs.Length == 0)
                return;

            RegisterLastPlayedPosition(stringFretPairs);
            var listToPlay = FillQueueOfStreams(stringFretPairs);
            _player.Volume = GetVolume();
            _player.Play(listToPlay);
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
    }
}