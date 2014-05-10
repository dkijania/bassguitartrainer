using BassNotesMasterApi.Utils;

namespace BassNotesMasterApi
{
    public interface IManagerModeChangeListener
    {
        void OnModeChanged(ManagerMode mode);
   }
}
