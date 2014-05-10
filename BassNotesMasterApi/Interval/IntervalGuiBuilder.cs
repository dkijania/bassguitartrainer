using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi.Interval
{
    public abstract class IntervalGuiBuilder
    {
        public readonly IntervalEventHandler IntervalEventHandler;

        protected IntervalGuiBuilder(IntervalEventHandler intervalEventHandler)
        {
            IntervalEventHandler = intervalEventHandler;
        }
        
        public void EnableMode(ManagerMode mode)
        {
            switch (mode)
            {
                case ManagerMode.Info:
                    ShowInfoPanel();
                    HideExcercisePanel();
                    break;
                case ManagerMode.Excercise:
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