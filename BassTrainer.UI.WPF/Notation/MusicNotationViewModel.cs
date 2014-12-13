using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BassTrainer.Core.Components.Notation;

namespace BassTrainer.UI.WPF.Notation
{
    public class MusicNotationViewModel
    {
        private MusicNotationComponent _musicNotationComponent;

        public MusicNotationViewModel(MusicNotationComponent musicNotationComponent)
        {
            _musicNotationComponent = musicNotationComponent;
        }
    }
}
