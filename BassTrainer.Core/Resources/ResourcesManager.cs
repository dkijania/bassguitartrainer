using System;
using System.Collections.Generic;
using System.IO;
using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Resources
{
    public class ResourcesManager
    {
        public DirectoryInfo Root = new DirectoryInfo("Resources");

        private readonly Dictionary<ResourceId, String> _resources = new Dictionary<ResourceId, String>();
        private readonly AudioSamplesProvider _audioSamplesProvider = new AudioSamplesProvider();
        
        public enum ResourceId
        {
            BassClefImage,
            FlatNoteImage,
            QuarterNoteStemDownwardImage,
            QuarterNoteStemUpwardImage,
            SharpNoteImage,
            ErrorImage,
            WrongQuarterNoteStemDownwardImage,
            WrongQuarterNoteStemUpwardImage,
            WrongFlatNoteImage,
            WrongSharpNoteImage,
            PlayIcon,
            MutedIcon,
            UnmutedIcon,
            LayoutFile,
            DefaultLayoutFile,
            FretboardImage
        }

        public void AddNamedResource(ResourceId key, String value)
        {
            _resources.Add(key, value);
        }

        private ResourcesManager()
        {
              
        }

        private static ResourcesManager _instance;

        public static ResourcesManager Instance
        {
            get { return _instance ?? (_instance = new ResourcesManager()); }
        }

        public object this[object key]
        {
            get
            {
                if (key is ResourceId)
                    return new FileInfo(_resources[(ResourceId)key]).FullName;
                if (key is Note)
                   return _audioSamplesProvider[(Note)key];
                throw new ResourceNotFoundException("Wrong type of key: " + key.GetType());
            }
        }

        
    }
}