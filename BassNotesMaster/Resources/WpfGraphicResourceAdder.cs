using BassNotesMasterApi.Resources;

namespace BassNotesMaster.Resources
{
    public class WpfGraphicResourceAdder
    {
        public WpfGraphicResourceAdder()
        {
            var resourcesManager = ResourcesManager.Instance;
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.BassClefImage,"Resources/Images/bassClef.png");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.ErrorImage,"Resources/Images/error.png");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.SharpNoteImage,"Resources/Images/sharp.png");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.FlatNoteImage,"Resources/Images/flat.png");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.QuarterNoteStemUpwardImage,"Resources/Images/quarterNote.png");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.QuarterNoteStemDownwardImage,"Resources/Images/quarterNoteWithStemDownward.png");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.WrongSharpNoteImage,"Resources/Images/wrongSharp.png");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.WrongFlatNoteImage,"Resources/Images/wrongFlat.png");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.WrongQuarterNoteStemUpwardImage,"Resources/Images/WrongQuarterNote.png");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.WrongQuarterNoteStemDownwardImage,"Resources/Images/WrongQuarterNoteWithStemDownward.png");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.PlayIcon,"Resources/Icons/play_icon.png");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.MutedIcon,"Resources/Icons/muted_icon.png");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.UnmutedIcon,"Resources/Icons/unmuted_icon.png");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.LayoutFile,"Resources/Layout/CurrentLayout.xml");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.DefaultLayoutFile,"Resources/Layout/DefaultLayout.xml");
      //      resourcesManager.AddNamedResource(ResourcesManager.ResourceId.FretboardImage,"Resources/Images/FretboardBassWNutFLAT.gif");
            resourcesManager.AddNamedResource(ResourcesManager.ResourceId.FretboardImage, "Resources/Images/fretboard.jpg");
            
        }
    }
}