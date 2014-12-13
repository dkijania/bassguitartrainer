using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BassTrainer.Core.Components.NotesView;

namespace BassTrainer.UI.WPF.NotesView
{
    public class NotesViewModel
    {
        private NotesViewComponent _notesViewComponent;

        public NotesViewModel(NotesViewComponent notesViewComponent)
        {
            _notesViewComponent = notesViewComponent;
        }
    }
}
