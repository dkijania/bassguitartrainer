namespace BassNotesMasterApi.Interval
{
    public class IntervalEventHandler
    {
        public IntervalData IntervalData { get; set; }

        protected event IntervalShowEvent ShowEvent;
        protected event IntervalPlayEvent PlayEvent;
        protected event IntervalExcerciseEvent ExcerciseEvent;

        public delegate void IntervalExcerciseEvent(IntervalRow row);

        public delegate void IntervalShowEvent(IntervalRow row);

        public delegate void IntervalPlayEvent(IntervalRow row);

        public void RegisterInfoEvents(IIntervalListener intervalListener)
        {
            ShowEvent += intervalListener.IntervalShowEvent;
            PlayEvent += intervalListener.IntervalPlayEvent;
        }

        public void UnregisterInfoEvents(IIntervalListener intervalListener)
        {
            ShowEvent -= intervalListener.IntervalShowEvent;
            PlayEvent -= intervalListener.IntervalPlayEvent;
        }

        public void RegisterExcerciseEvents(IExcerciseIntervalListener intervalListener)
        {
            ExcerciseEvent += intervalListener.IntervalExcerciseEvent;
        }

        public void UnregisterExcerciseEvents(IExcerciseIntervalListener intervalListener)
        {
            ExcerciseEvent -= intervalListener.IntervalExcerciseEvent;
        }


        protected void RaisePlayEvent(IntervalRow intervalRow)
        {
            var evt = PlayEvent;
            if (evt != null)
            {
                evt(intervalRow);
            }
        }

        protected void RaiseShowEvent(IntervalRow intervalRow)
        {
            var evt = ShowEvent;
            if (evt != null)
            {
                evt(intervalRow);
            }
        }

        protected void RaiseExcerciseEvent(IntervalRow intervalRow)
        {
            var evt = ExcerciseEvent;
            if (evt != null)
            {
                evt(intervalRow);
            }
        }

        public void RemoveAllEvents()
        {
            ExcerciseEvent = null;
            ShowEvent = null;
        }
    }
}