namespace SimpleHelpSystem.DocumentLoader
{
    public interface IDocumentPresenter<in T>
    {
        void PresentContent(T document);
    }
}