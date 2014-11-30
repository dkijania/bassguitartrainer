using System;
using DrumMachine.TimeSignature;

namespace DrumMachine.Engine.Pattern
{
    public class DrumPatternSettings
    {
        public int Measures { get; set; }
        public String NoteType { get; set; }
        public int Bars { get; set; }
        public int Tempo { get; set; }

        public int CalculateNoteInterval()
        {
            NoteTypeEnum noteTypeEnum;
            if (NoteTypeEnum.TryParse(NoteType, true, out noteTypeEnum))
            {
                return (int) noteTypeEnum;
            }
            throw new DrumPatternParsingException("Cannot parse '{0}' as a NoteType", NoteType);
        }

        public int CalculateNoteWeight()
        {
            return (int)NoteTypeEnum.Sixteen/CalculateNoteInterval();
        }
    }
}