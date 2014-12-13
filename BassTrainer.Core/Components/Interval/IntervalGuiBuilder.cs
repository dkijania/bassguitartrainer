using BassTrainer.Core.Components.Interval.Data;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.Interval
{
    public abstract class IntervalGuiBuilder
    {
        public readonly IntervalEventHandler IntervalEventHandler;

        protected IntervalGuiBuilder(IntervalEventHandler intervalEventHandler)
        {
            IntervalEventHandler = intervalEventHandler;
        }
        
        public void EnableMode(ComponentMode mode)
        {
            switch (mode)
            {
                case ComponentMode.Info:
                    ShowInfoPanel();
                    HideExcercisePanel();
                    break;
                case ComponentMode.Excercise:
                    ShowExcercisePanel();
                    HideInfoPanel();
                    break;
            }
        }

        protected abstract void ShowInfoPanel();
        protected abstract void HideExcercisePanel();
        protected abstract void ShowExcercisePanel();
        protected abstract void HideInfoPanel();

        public abstract void PrepareInfoGuiElements(IntervalData data);
        public abstract void PrepareExcerciseGuiElements(IntervalData data);
        public abstract void SetColorForButtonName(IntervalRow row, bool result);
        public abstract void ResetColorForButtonName(IntervalRow row);
        
        public abstract void EnableAllIntervalsButtons();
        public abstract void EnableIntervalsButtons(string[] names);
        public abstract void DisableAllIntervalsButtons();
        public abstract void EnableIntervalsButtonsExclusive(string[] names);
    }
}