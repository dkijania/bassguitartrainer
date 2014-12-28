using BassTrainer.Core.Const;

namespace BassTrainer.Core.Utils.Keyboard
{
    public interface ICombinationPressedListener
    {
        void OnCombinationPressed(Note note);
    }
}