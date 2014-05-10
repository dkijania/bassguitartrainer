using System.Configuration;

namespace BassNotesMaster.SettingsManager.ConfigurationSections
{
    public class BassNotesMasterOptionSection : ConfigurationSection
    {
        private const string FontElementName = "font";
        private const string OthersElementName = "others";
        private const string FretboardVisualPresetElementName = "fretboardVisual";
        private const string PlayerElementName = "player";


        [ConfigurationProperty(PlayerElementName)]
        public PlayerElement Player
        {
            get { return (PlayerElement)this[PlayerElementName]; }
            set { this[PlayerElementName] = value; }
        }

        [ConfigurationProperty(FontElementName)]
        public FontElement Font
        {
            get { return (FontElement) this[FontElementName]; }
            set { this[FontElementName] = value; }
        }

        [ConfigurationProperty(FretboardVisualPresetElementName)]
        public FretBoardVisualsPreset FretBoardVisualsPreset
        {
            get { return (FretBoardVisualsPreset) this[FretboardVisualPresetElementName]; }
            set { this[FretboardVisualPresetElementName] = value; }
        }

        [ConfigurationProperty(OthersElementName)]
        public Others Others
        {
            get { return (Others) this[OthersElementName]; }
            set { this[OthersElementName] = value; }
        }
        
        public override bool IsReadOnly()
        {
            return false;
        }
    }
}