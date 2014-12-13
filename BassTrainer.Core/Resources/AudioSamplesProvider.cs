using System;
using System.Collections.Generic;
using System.IO;
using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Resources
{
    public class AudioSamplesProvider
    {
        private const string DirectoryName = "Resources/Sounds";
        private readonly DirectoryInfo _relateSoundsLocation = new DirectoryInfo(DirectoryName);
        private readonly Dictionary<Note, Stream> _streamsDictionary = new Dictionary<Note, Stream>();

        public AudioSamplesProvider()
        {
            LoadWavStreamsToMemory();
        }

        public Stream this[Note key]
        {
            get { return _streamsDictionary[key]; }
        }

        private void LoadWavStreamsToMemory()
        {
            var fretBoardMapping = NotesToStringFretBoardMapping.Instance;
            foreach (var note in fretBoardMapping.ValuesDistinct)
            {
                var file = GetFile(note);
                var memoryStream = new MemoryStream();
                using (var fileStream = new FileStream(file, FileMode.Open))
                {
                    fileStream.Position = 0;
                    fileStream.CopyTo(memoryStream);
                }
                _streamsDictionary.Add(note, memoryStream);
            }
        }
        
        private string GetFile(Note note)
        {
            return ConstructPathToAudioSampleForNote(note); 
        }

        private string ConstructPathToAudioSampleForNote(Note note)
        {
            var audioFileNameWithoutExtension = GetNoteId(note);
            var matchingFileCollection = ApplyRegexForDirectory(audioFileNameWithoutExtension);
            CheckIfCollectionHasCorrectLength(matchingFileCollection, audioFileNameWithoutExtension);
            return string.Format("{0}/{1}", DirectoryName, matchingFileCollection[0].Name);
        }

        private FileInfo[] ApplyRegexForDirectory(string audioFileNameWithoutExtension)
        {
            var regex = String.Format("{0}.*", audioFileNameWithoutExtension);
            return _relateSoundsLocation.GetFiles(regex);
        }

        private void CheckIfCollectionHasCorrectLength(FileInfo[] collection, string audioFileNameWithoutExtension)
        {
            if (collection.Length == 0)
            {
                throw new ResourceNotFoundException(
                    string.Format("Couldn't find any audio sample for given expression {0} in {1}",
                                  audioFileNameWithoutExtension, _relateSoundsLocation.FullName));
            }
            if (collection.Length > 1)
            {
                throw new ResourceNotFoundException(
                    string.Format("There are more than on audio sample for expression {0}",
                                  audioFileNameWithoutExtension));
            }
        }

        private string GetNoteId(Note note)
        {
            return String.Format("{0}{1}", note.SharpOrRegularRepresenation, note.OctaveNumber);
        }
    }
}