using System;
using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Components.Interval;
using BassTrainer.Core.Components.Notation;
using BassTrainer.Core.Components.NotePlayer;
using BassTrainer.Core.Components.NotesView;
using BassTrainer.Core.Components.Statistics;
using BassTrainer.Core.Utils.Keyboard;
using BassTrainer.Core.Utils.ResultSerializer;

namespace BassTrainer.Core.Components
{
    public class ComponentsLocator
    {
        public FretboardComponent FretboardComponent { get; set; }
        public IntervalComponent IntervalComponent { get; set; }
        public MusicNotationComponent MusicNotationComponent { get; set; }
        public NotesViewComponent NotesViewComponent { get; set; }
        public Component ShowSelectComponent { get; set; }
        public StatisticsComponent StatisticsComponent { get; set; }
        public BassNotesPlayer PlayerManager { get; set; }
        public IResultSerializer ResultSerializer { get; set; }
        public AbstractKeyboardEventComponent KeyboardEventComponent { get; set; }
        
        private static ComponentsLocator _locator;
        
        private ComponentsLocator()
        {
        }

        public static ComponentsLocator Instance
        {
            get { return _locator ?? (_locator = new ComponentsLocator()); }
        }

        public void RemoveAllEvents()
        {
            DoForAllPropertiesDerivedFrom<Component>(manager => manager.RemoveAllSubscribers());
        }

        public void CheckIfAllPropertiesAreNotNull()
        {
            DoForAllPropertiesDerivedFrom<Component>(CheckIfNotNull);
        }

        private void CheckIfNotNull(Component component)
        {
            if (component == null)
            {
                throw new ComponentsLocatorException("property of type " + typeof (Component) +
                                                   " wasn't set during configuration.");
            }
        }
        
        public void DoForAllPropertiesDerivedFrom<T>(Action<T> action)
        {
            foreach (
                var propertyInfo in
                    GetType().GetProperties())
            {
                var type = propertyInfo.PropertyType;

                if (typeof (T).IsAssignableFrom(type))
                {
                    var manager = propertyInfo.GetValue(this);
                    action((T) manager);
                }
            }
        }

        public void Init()
        {
            CheckIfAllPropertiesAreNotNull();
            DoForAllPropertiesDerivedFrom<Component>(component => component.Init());
        }
    }
}