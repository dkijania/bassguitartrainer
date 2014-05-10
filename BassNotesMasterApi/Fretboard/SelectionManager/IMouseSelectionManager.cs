using System.Windows;

namespace BassNotesMasterApi.Fretboard.SelectionManager
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