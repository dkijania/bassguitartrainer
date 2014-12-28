using BassTrainer.Core.Const;
using BassTrainer.Core.Utils;

namespace BassTrainer.Core.Components.Notation
{
    public interface IMusicNotationListener
    {
        void OnMouseClick(StringFretPair stringFretPair);
    }
}