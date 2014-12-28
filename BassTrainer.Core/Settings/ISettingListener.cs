namespace BassTrainer.Core.Settings
{
    public interface ISettingListener
    {
        void Subscribe(Settings settings);
        void OnSettingChanged(Settings settings);
    }
}