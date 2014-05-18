namespace SimpleHelpSystem.History
{
    public interface IHelpHistory<T>
    {
        bool CanUndo { get; }
        bool CanRedo { get; }
        void Add(T t);
        T Undo();
        T Redo();
    }
}