using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BassTrainer.Core.Components.Fretboard;

namespace BassTrainer.UI.WPF.FretBoard
{
    public class FretboardViewModel
    {
        private FretboardComponent _fretboardComponent;

        public FretboardViewModel(FretboardComponent fretboardComponent)
        {
            _fretboardComponent = fretboardComponent;
        }
    }
}
