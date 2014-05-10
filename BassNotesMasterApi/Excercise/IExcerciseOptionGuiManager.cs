namespace BassNotesMasterApi.Excercise
{
    public interface IExcerciseOptionGuiManager
    {
        void Build();
        void AddOption(string alwaysStartFromLowestNoteParamName, bool b);
        bool GetBooleanOption(string alwaysStartFromLowestNoteParamName);
        void Clear();
    }
}