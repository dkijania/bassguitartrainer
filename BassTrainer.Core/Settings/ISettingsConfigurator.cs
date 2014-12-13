using BassTrainer.Core.Const;

namespace BassTrainer.Core.Settings
{
    public interface ISettingsConfigurator
    {
        int FretBoardCorrectVisualPreset { get; }
        int FretboardWrongVisualPreset { get; }
        string FontName { get; set; }
        int FontSize { get; set; }
        int NumberOfTries { get; set; }
        double ShowResultFor { get; set; }
        FretBoardOptions FretboardShow { get; set; }
        string ThemeName { get; set; }
        bool IsPlayerMuted { get; set; }
        double Volume { get; set; }
        int SpeedRatio { get; set; }
        int Timeout { get; set; }
        void Save(Settings settings);
        void Read(Settings settings);
    }
}