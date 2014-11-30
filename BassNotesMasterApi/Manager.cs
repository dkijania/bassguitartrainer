using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi
{
    public abstract class Manager : IManagerModeChangeListener
    {
        public abstract ManagerMode Mode { get; set; }
        public abstract void RemoveAllSubscribers();
        public abstract void OnModeChanged(ManagerMode mode);
        public virtual void Init(){}
    }
}