using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BassTrainer.Core.Components
{
    class ComponentModeManager : IComponentModeChangeListener
    {
        public void OnModeChanged(ComponentMode mode)
        {
            
        }

        /*
        public event ModeChanged ModeChangedEvent;

        public void OnModeChangedEvent(ComponentMode mode)
        {
            var handler = ModeChangedEvent;
            if (handler != null) handler(mode);
        }

        public delegate void ModeChanged(ComponentMode mode);
     */   
   
    }
}
