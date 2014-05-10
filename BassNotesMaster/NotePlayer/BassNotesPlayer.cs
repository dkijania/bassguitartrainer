using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.NotePlayer;
using BassNotesMasterApi.Resources;
using BassNotesMasterApi.Settings;
using BassNotesMasterApi.Utils;

namespace BassNotesMaster.NotePlayer
{
    public class BassNotesPlayer : IBassNotesPlayer
    {
        private readonly NotesToStringFretBoardMapping _notesToStringFretBoardMapping =
            new NotesToStringFretBoardMapping();

        private StringFretPair[] _lastPlayedPositions;
        private readonly Slider _volume;
        private readonly ResourcesManager _resourcesManager = ResourcesManager.Instance;
        private readonly WavPlayer _player = new WavPlayer();

        public BassNotesPlayer(ButtonBase playAgain, Slider volume, ButtonBase mute,
                               Settings settings)
        {
            _volume = volume;
            RegisterEventsAndSetContentForButtons(playAgain, mute);
            SetPlayerAttributes(settings, (ToggleButton) mute);
        }

        private void SetPlayerAttributes(Settings settings, ToggleButton button)
        {
            if (settings.IsPlayerMuted.Value)
            {
                button.IsChecked = true;
                MuteClick(button, null);
            }
            _volume.Value = settings.Volume.Value;
        }

        private void RegisterEventsAndSetContentForButtons(ButtonBase play, ButtonBase mute)
        {
            play.Click += OnPlayAgainClick;
            mute.Click += MuteClick;
        }

        private void MuteClick(object sender, RoutedEventArgs e)
        {
            var button = (ToggleButton) sender;
            var isChecked = (button.IsChecked != null && button.IsChecked.Value);
            button.Content = isChecked ? "Unmute" : "Mute";
            Settings.Instance.IsPlayerMuted.SetNewValue(isChecked);
            _volume.IsEnabled = !isChecked;
        }

        protected void OnPlayAgainClick(object sender, RoutedEventArgs e)
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
            return (float) (_volume.IsEnabled ? _volume.Value : _volume.Minimum);
        }
    }
}