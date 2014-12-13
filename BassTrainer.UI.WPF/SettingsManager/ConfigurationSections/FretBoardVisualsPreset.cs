using System.Configuration;

namespace BassTrainer.UI.WPF.SettingsManager.ConfigurationSections
{
    public class FretBoardVisualsPreset : ConfigurationElement
    {
        private const string CorrectRectanglePresetElementName = "correctRectanglePreset";
        private const string WrongRectanglePresetElementName = "wrongRectanglePreset";
        private const string DelayTimeElementName = "showVisualFor";


        [ConfigurationProperty(CorrectRectanglePresetElementName, IsRequired = true, DefaultValue = 1)]
        [IntegerValidator(MaxValue = 3, MinValue = 1)]
        public int CorrectVisualPreset
        {
            get { return (int) this[CorrectRectanglePresetElementName]; }
            set { this[CorrectRectanglePresetElementName] = value; }
        }

        [ConfigurationProperty(WrongRectanglePresetElementName, IsRequired = true, DefaultValue = 1)]
        [IntegerValidator(MaxValue = 3, MinValue = 1)]
        public int WrongVisualPreset
        {
            get { return (int) this[WrongRectanglePresetElementName]; }
            set { this[WrongRectanglePresetElementName] = value; }
        }


        [ConfigurationProperty(DelayTimeElementName, IsRequired = true, DefaultValue = 1000)]
        [IntegerValidator(MaxValue = 10000, MinValue = 100)]
        public int ShowResultFor
        {
            get { return (int) this[DelayTimeElementName]; }
            set { this[DelayTimeElementName] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}