using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BassTrainer.UI.WPF.ShowSelectViewManager
{
    public class ShowSelectViewModel
    {
        private ShowSelectViewComponent _selectViewComponent;

        public ShowSelectViewModel(ShowSelectViewComponent selectViewComponent)
        {
            _selectViewComponent = selectViewComponent;
        }
    }
}
