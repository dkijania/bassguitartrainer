using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BassTrainer.Core.Components;
using BassTrainer.Core.Excercise.Collection;
using BassTrainer.Core.Excercise.Collection.Tutorial;
using BassTrainer.Core.Excercise.Options;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Excercise
{
    public class ExcercisesDictionary
    {
        private readonly Dictionary<string,IExcercise> _excercises = new Dictionary<string, IExcercise>();
      
        public IExcercise DefaultExcercise
        {
            get { return _excercises["Tutorial"]; }
        } 

        public ExcercisesDictionary(IExcerciseOptionGuiManager guiManager, IVisibilityManager visibilityManager)
        {
            _excercises.Add("Tutorial", new Tutorial());
            _excercises.Add("Find Note Position", new FindNoteOnFretboard(Settings.Settings.Instance, guiManager,visibilityManager));
            _excercises.Add("Identify Note On fretboard", new IdentifyNoteOnFretboard(Settings.Settings.Instance, guiManager, visibilityManager));
            _excercises.Add("Identify Note On fretboard (Notation)", new FindNotationForPosition(Settings.Settings.Instance, guiManager, visibilityManager));
            _excercises.Add("Identify Note by interval", new IntervalsExcercise(Settings.Settings.Instance, guiManager, visibilityManager));   
            _excercises.Add("Find Note Position from Notes View", new FindNoteOnFretboardFromNotesView(Settings.Settings.Instance, guiManager, visibilityManager));   
            _excercises.Add("Identify Note Shown on Notation", new IdentifyNoteOnNotesViewFromNotation(Settings.Settings.Instance, guiManager, visibilityManager));   
        }

        public IExcercise this[String key]
        {
            get
            {
                var excercise = _excercises[key];
                AttachStatistics(excercise,key);
                excercise.ShowOptions();
                return excercise;
            }
        }

        private void AttachStatistics(IExcercise excercise, String key)
        {
            excercise.StatisticData = ComponentsLocator.Instance.StatisticsComponent.GetStatisticForExcercise(key);
        }

        public ICollection Keys
        {
            get { return _excercises.Keys.Skip(1).ToArray(); }
        }
    }
}
