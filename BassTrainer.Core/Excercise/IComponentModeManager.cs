using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BassTrainer.Core.Components;

namespace BassTrainer.Core.Excercise
{
    public interface IComponentModeManager
    {
        void ApplyMode(ComponentMode mode);
        bool IsMode(ComponentMode mode);
    }
}
