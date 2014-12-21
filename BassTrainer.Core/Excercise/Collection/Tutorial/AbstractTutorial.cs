using System.Collections.Generic;
using BassTrainer.Core.Components;
using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Components.Statistics;
using BassTrainer.Core.Const;
using BassTrainer.Core.Excercise.SelectionSetters;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Excercise.Collection.Tutorial
{
    public abstract class AbstractTutorial : IExcercise
    {
        protected readonly IComponentModeManager ComponentModeManager;

        protected AbstractTutorial(IComponentModeManager componentModeManager)
        {
            ComponentModeManager = componentModeManager;
            SelectionSetter = new DefaultSelectionSetter();
        }

        public void Start(IEnumerable<StringFretPair> pairs)
        {
            ComponentModeManager.ApplyMode(ComponentMode.Info);
            Start();
        }

        public void Skip()
        {
        }

        public void Pause()
        {}

        public void Continue(StringFretPair[] pairs)
        {}

        public void ShowOptions()
        {}

        public bool IsPaused { get; private set; }
        public StatisticRow StatisticData { get; set; }
        public ISelectionSetter SelectionSetter { get; private set; }


        public void Stop()
        {
            ComponentsLocator.Instance.RemoveAllEvents();
        }

        public void Start()
        {
            BeforeStart();
        }

        public abstract void BeforeStart();
    }
}