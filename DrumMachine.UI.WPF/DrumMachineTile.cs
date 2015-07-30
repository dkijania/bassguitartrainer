namespace DrumMachine.UI.WPF
{
    public class DrumMachineTile
    {
        public int Row { get; protected set; }
        public int Column { get; protected set; }
        public event OnSelect OnSelectEvent;

        public virtual void InvokeOnSelectEvent()
        {
            var handler = OnSelectEvent;
            if (handler != null) handler(Row, Column);
        }

        public delegate void OnSelect(int row, int column);

        public DrumMachineTile(int row, int column)
        {
            Row = row;
            Column = column;
        }
    }
}