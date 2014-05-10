using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BassNotesMaster.SettingsManager.ConfigurationSections
{
    public class PlayerElement : ConfigurationElement
    {
        private const string MutedElementName = "muted";
        private const string VolumeElementName = "volume";
        private const string SpeedRationElementName = "speedRatio";
        
        [ConfigurationProperty(MutedElementName, IsRequired = true, DefaultValue = false)]
        public bool IsMuted
        {
            get { return (bool)this[MutedElementName]; }
            set { this[MutedElementName] = value; }
        }

        [ConfigurationProperty(VolumeElementName, IsRequired = true, DefaultValue = 50)]
        [IntegerValidator(MaxValue = 100, MinValue = 0)]
        public int Volume
        {
            get { return (int)this[VolumeElementName]; }
            set { this[VolumeElementName] = value; }
        }


        [ConfigurationProperty(SpeedRationElementName, IsRequired = true, DefaultValue = 3)]
        [IntegerValidator(MaxValue = 16, MinValue = 1)]
        public int SpeedRatio
        {
            get { return (int)this[SpeedRationElementName]; }
            set { this[SpeedRationElementName] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}
