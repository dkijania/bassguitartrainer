using System;
using BassTrainer.Core.Const;

namespace BassTrainer.Core.Settings
{
    public class Settings
    {
        public UndoManager<int> CorrectRectanglePreset;
        public UndoManager<int> WrongRectanglePreset;
        public UndoManager<FretBoardOptions> FretBoardOptions;
        public UndoManager<string> FontFamilyName;
        public UndoManager<int> FontSize;
        public UndoManager<int> AttemptsCount;
        public UndoManager<double> DelayTime;
        public UndoManager<double> Timeout;
        public UndoManager<bool> IsPlayerMuted;
        public UndoManager<double> Volume;
        public UndoManager<int> SpeedRatio;
        public UndoManager<bool> ShowCorrectAnswer;

        public event SettingsChanged SettingChangedEvent;

        public delegate void SettingsChanged(Settings settings);

        private static Settings _instance;

        public void Configure(ISettingsConfigurator conf)
        {
            CorrectRectanglePreset = new UndoManager<int>(conf.FretBoardCorrectVisualPreset);
            WrongRectanglePreset = new UndoManager<int>(conf.FretboardWrongVisualPreset);
            FontFamilyName = new UndoManager<string>(conf.FontName);
            FontSize = new UndoManager<int>(conf.FontSize);
            AttemptsCount = new UndoManager<int>(conf.NumberOfTries);
            DelayTime = new UndoManager<double>(conf.ShowResultFor);
            FretBoardOptions = new UndoManager<FretBoardOptions>(conf.FretboardShow);
            IsPlayerMuted = new UndoManager<bool>(conf.IsPlayerMuted);
            Volume = new UndoManager<double>(conf.Volume);
            SpeedRatio = new UndoManager<int>(conf.SpeedRatio);
            Timeout = new UndoManager<double>(conf.Timeout);
            ShowCorrectAnswer = new UndoManager<bool>(conf.ShowCorrectAnswer);
        }

        public static Settings Instance
        {
            get { return _instance ?? (_instance = new Settings()); }
        }

        public void OnSettingChangedEvent(Settings settings)
        {
            var handler = SettingChangedEvent;
            if (handler != null) handler(settings);
        }

        public void FireSettingsChangedEvent()
        {
            OnSettingChangedEvent(this);
        }

        public void ForceUpdateAllFields()
        {
            CorrectRectanglePreset.LastChangeResult = true;
            WrongRectanglePreset.LastChangeResult = true;
            FontFamilyName.LastChangeResult = true;
            FontSize.LastChangeResult = true;
            AttemptsCount.LastChangeResult = true;
            DelayTime.LastChangeResult = true;
            FretBoardOptions.LastChangeResult = true;
            IsPlayerMuted.LastChangeResult = true;
            Volume.LastChangeResult = true;
            SpeedRatio.LastChangeResult = true;
            Timeout.LastChangeResult = true;
            ShowCorrectAnswer.LastChangeResult = true;
            OnSettingChangedEvent(this);
        }
    }
}