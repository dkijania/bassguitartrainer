using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components
{
    public abstract class Component : IComponentModeChangeListener
    {
        public abstract void RemoveAllSubscribers();
        public abstract void OnModeChanged(ComponentMode mode);
        public virtual void Init(){}
    }
}