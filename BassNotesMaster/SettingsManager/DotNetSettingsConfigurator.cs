using System;
using System.Configuration;
using BassNotesMaster.SettingsManager.ConfigurationSections;
using BassNotesMasterApi.Const;
using BassNotesMasterApi.Settings;

namespace BassNotesMaster.SettingsManager
{
    public class DotNetSettingsConfigurator : ISettingsConfigurator
    {
        public int FretBoardCorrectVisualPreset { get; private set; }
        public int FretboardWrongVisualPreset { get; private set; }
        public string FontName { get; set; }
        public int FontSize { get; set; }
        public int NumberOfTries { get; set; }
        public double ShowResultFor { get; set; }
        public FretBoardOptions FretboardShow { get; set; }
        public string ThemeName { get; set; }
        public bool IsPlayerMuted { get; set; }
        public double Volume { get; set; }
        public int SpeedRatio { get; set; }
        public int Timeout { get; set; }

        public void Read(Settings settings)
        {
            var assemblyConfiguration = GetConfiguration();
            
            FontName = assemblyConfiguration.Font.Name;
            FontSize = assemblyConfiguration.Font.Size;
            FretBoardCorrectVisualPreset = assemblyConfiguration.FretBoardVisualsPreset.CorrectVisualPreset;
            FretboardWrongVisualPreset = assemblyConfiguration.FretBoardVisualsPreset.WrongVisualPreset;
            NumberOfTries = assemblyConfiguration.Others.NumberOfTries;
            ShowResultFor = (double)assemblyConfiguration.FretBoardVisualsPreset.ShowResultFor/1000;
            FretboardShow = new FretBoardOptions();
            ThemeName = assemblyConfiguration.Others.ThemeName;
            IsPlayerMuted = assemblyConfiguration.Player.IsMuted;
            Volume = (double)assemblyConfiguration.Player.Volume/100;
            SpeedRatio = assemblyConfiguration.Player.SpeedRatio;
            Timeout = assemblyConfiguration.Others.ExcerciseTimeout/1000;
            settings.Configure(this);
        }


        public void Save(Settings settings)
        {
            var conf = GetConfiguration();
            conf.FretBoardVisualsPreset.CorrectVisualPreset = settings.CorrectRectanglePreset.Value;
            conf.FretBoardVisualsPreset.WrongVisualPreset = settings.WrongRectanglePreset.Value;
            conf.FretBoardVisualsPreset.ShowResultFor = (int)(settings.DelayTime.Value * 1000);

            conf.Font.Name = settings.FontFamilyName.Value;
            conf.Font.Size = settings.FontSize.Value;

            conf.Others.NumberOfTries = settings.AttemptsCount.Value;
          //  conf.Others.ThemeName = Enum.GetName(typeof(MetroThemes), settings.GuiTheme.Value);
            conf.Others.Show = Enum.GetName(typeof(FretBoardShow), settings.FretBoardOptions.Value.Show);

            conf.Player.IsMuted = settings.IsPlayerMuted.Value;
            conf.Player.Volume = (int)(settings.Volume.Value * 100);
            conf.Player.SpeedRatio = settings.SpeedRatio.Value;

            _exeConfiguration.Save(ConfigurationSaveMode.Modified);
        }

        private readonly Configuration _exeConfiguration = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        private BassNotesMasterOptionSection GetConfiguration()
        {
            return (BassNotesMasterOptionSection)_exeConfiguration.GetSection("settings");
        }
    }
}
