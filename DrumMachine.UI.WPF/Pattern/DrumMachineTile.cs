namespace DrumMachine.UI.WPF.Pattern
{
    public class DrumMachineTile
    {
        public int Row { get; protected set; }
        public int Column { get; protected set; }
        public event OnSelect OnSelectEvent;
        public event IgnoreMouseClick OnIgnoreMouseClick;

        public virtual bool InvokeOnIgnoreMouseClick()
        {
            IgnoreMouseClick handler = OnIgnoreMouseClick;
            return handler != null && handler();
        }

        //check if function is valid
        public virtual void InvokeOnSelectEvent(int row, int column, bool isSelected)
        {
            var handler = OnSelectEvent;
            if (handler != null)
             handler(row,column,isSelected);
            
        }

        public delegate void OnSelect(int row, int column,bool isSelected);
        public delegate bool IgnoreMouseClick();


        public DrumMachineTile(int row, int column)
        {
            Row = row;
            Column = column;
        }


    }
}
    