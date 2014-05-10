namespace BassNotesMasterApi.Settings
{
    public interface ISettingListener
    {
        void Subscribe(Settings settings);
        void OnSettingChanged(Settings settings);
    }
}