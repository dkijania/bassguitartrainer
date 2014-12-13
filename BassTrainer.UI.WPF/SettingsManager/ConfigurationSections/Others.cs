using System;
using System.Configuration;

namespace BassTrainer.UI.WPF.SettingsManager.ConfigurationSections
{
    public class Others : ConfigurationElement
    {
        private const string ThemeElementName = "themeName";
        private const string NoOfTriesElementName = "noOfTries";
        private const string ShowElementName = "showNotes";
        private const string TimeoutExcerciseName = "excerciseTimeout";


        [ConfigurationProperty(ThemeElementName, IsRequired = true, DefaultValue = "GenericTheme")]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public String ThemeName
        {
            get { return (String) this[ThemeElementName]; }
            set { this[ThemeElementName] = value; }
        }

        [ConfigurationProperty(ShowElementName, IsRequired = true, DefaultValue = "OnlySharps")]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public String Show{
            get { return (String)this[ShowElementName]; }
            set { this[ShowElementName] = value; }
        }

        [ConfigurationProperty(NoOfTriesElementName, IsRequired = true, DefaultValue = 3)]
        [IntegerValidator(MaxValue = 100, MinValue = 1)]
        public int NumberOfTries
        {
            get { return (int) this[NoOfTriesElementName]; }
            set { this[NoOfTriesElementName] = value; }
        }

        [ConfigurationProperty(TimeoutExcerciseName, IsRequired = true, DefaultValue = 10000)]
        [IntegerValidator(MaxValue = 10000, MinValue = 1)]
        public int ExcerciseTimeout
        {
            get { return (int)this[TimeoutExcerciseName]; }
            set { this[TimeoutExcerciseName] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}