using System;
using System.Configuration;

namespace BassTrainer.UI.WPF.SettingsManager.ConfigurationSections
{
    public class FontElement : ConfigurationElement
    {
        private const string NameAttribute = "name";
        private const string SizeAttribute = "size";


        [ConfigurationProperty(NameAttribute, IsRequired = true, DefaultValue = "Calibre")]
        [StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\", MinLength = 1, MaxLength = 60)]
        public String Name
        {
            get { return (String) this[NameAttribute]; }
            set { this[NameAttribute] = value; }
        }

        [ConfigurationProperty(SizeAttribute, IsRequired = true, DefaultValue = 12)]
        [IntegerValidator(MaxValue = 24, MinValue = 6)]
        public int Size
        {
            get { return (int) this[SizeAttribute]; }
            set { this[SizeAttribute] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}