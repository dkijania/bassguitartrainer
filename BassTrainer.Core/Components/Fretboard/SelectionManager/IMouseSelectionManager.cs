using System.Windows;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.Fretboard.SelectionManager
{
    public interface IMouseSelectionManager
    {
        SelectionManager SelectionManager { set; }
        void Clear();
        void Prepare();
        void FinishSelectionMode(Point point);
        void OnMouseMove(Point point);
        void StartSelectionMode(Point point);
    }
}