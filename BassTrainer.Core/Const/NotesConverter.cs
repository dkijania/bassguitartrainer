namespace BassTrainer.Core.Const
{
    public class NotesConverter
    {
        public static string SharpSuffix = "#";
        public static string BemolSuffix = "b";


        public static string ConvertSharpToBemol(string sharpNote)
        {
            if (sharpNote.Equals(AddSharpSuffix("A")))
                return AddBemolSuffix("B");
            if (sharpNote.Equals(AddSharpSuffix("C")))
                return AddBemolSuffix("D");
            if (sharpNote.Equals(AddSharpSuffix("D")))
                return AddBemolSuffix("E");
            if (sharpNote.Equals(AddSharpSuffix("F")))
                return AddBemolSuffix("G");
            if (sharpNote.Equals(AddSharpSuffix("G")))
                return AddBemolSuffix("A");
            return sharpNote;
        }

        private static string AddSharpSuffix(string sharpNote)
        {
            return sharpNote + SharpSuffix;
        }

        private static string AddBemolSuffix(string sharpNote)
        {
            return sharpNote + BemolSuffix;
        }

        public static string ConvertBemolToSharp(string sharpNote)
        {
            if (sharpNote.Equals(AddBemolSuffix("B")))
                return AddSharpSuffix("A");
            if (sharpNote.Equals(AddBemolSuffix("D")))
                return AddSharpSuffix("C");
            if (sharpNote.Equals(AddBemolSuffix("E")))
                return AddSharpSuffix("D");
            if (sharpNote.Equals(AddBemolSuffix("G")))
                return AddSharpSuffix("F");
            if (sharpNote.Equals(AddBemolSuffix("A")))
                return AddSharpSuffix("G");
            return sharpNote;
        }
    }
}