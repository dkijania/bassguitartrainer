namespace BassNotesMasterApi.Utils
{
    public interface IVisibilityManager
    {
        void SetVisible(params ComponentId[] toVisible);
        void SetVisibleExlusive(params ComponentId[] toVisible);
        void SetHideExlusive(params ComponentId[] toVisible);
        void SetEnabled(params ComponentId[] toVisible);
        void Hide(params ComponentId[] toVisible);
        void SetOnTop(ComponentId toVisible);
        void SetOnTop(ComponentId[] toVisible);
        void HideAll();
        void ShowAll();
        void SetEnabledAll();
    }
}