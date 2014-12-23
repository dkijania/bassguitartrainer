using System;
using System.Collections.Generic;
using System.Linq;
using BassTrainer.Core.Const;

namespace BassTrainer.Core.Components.Fretboard.SelectionManager
{
    public class SelectionManager
    {
        public readonly IMouseSelectionManager MouseSelectionManager;
        public readonly ScaleSelectionManager ScaleSelectionManager;
        public IGuiSelector GuiSelector { get; set; }
        private readonly List<StringFretPair> _selected;
        private readonly NotesToStringFretBoardMapping _notesToStringFretBoardMapping = NotesToStringFretBoardMapping.Instance;

        public SelectionManager(IGuiSelector guiSelector,
                                IMouseSelectionManager mouseSelectionManager)
        {
            GuiSelector = guiSelector;
            MouseSelectionManager = mouseSelectionManager;
            MouseSelectionManager.SelectionManager = this;
            ScaleSelectionManager = new ScaleSelectionManager();
            _selected = new List<StringFretPair>();
        }

        public IEnumerable<StringFretPair> Selected
        {
            get { return _selected; }
        }

        public void UnselectAllItems()
        {
            GuiSelector.UnselectAllItems();
            CleanUp();
        }

        public void SelectAllItems()
        {
            SelectItems(_notesToStringFretBoardMapping.GetAllPositionsDistinct());
        }

        public void CleanUp()
        {
            MouseSelectionManager.Clear();
            _selected.Clear();
        }

        public void SelectItems(string stringName, int startFret, int endFret)
        {
            var lower = Math.Min(startFret, endFret);
            var higher = Math.Max(startFret, endFret);
            var itemsToSelect= new List<StringFretPair>();
            for (int i = lower; i <= higher; i++)
            {
                var item = new StringFretPair(stringName, i);
                itemsToSelect.Add(item);
            }
            SelectItems(itemsToSelect.ToArray());
        }

        public void SelectItems(Note[] notesToSelect, int startFret, int endFret)
        {
            var fretBoardMapping = NotesToStringFretBoardMapping.Instance;
            var lower = Math.Min(startFret, endFret);
            var higer = Math.Max(startFret, endFret);

            foreach (var stringFretPair in 
                from fretNo in Enumerable.Range(lower, higer)
                from stringNo in Enumerable.Range(0, FretBoardOptions.NoOfStrings)
                select new StringFretPair(stringNo, fretNo)
                into stringFretPair
                let note = fretBoardMapping.GetNote(stringFretPair)
                where notesToSelect.Contains(note) select stringFretPair)
            {
                SelectItems(stringFretPair);
            }
        }

        public void SelectItems(string scaleType, Note rootNote, int startFret, int endFret)
        {
            var scaleTypeEnum = ConvertToScaleType(scaleType);
            var positionCollection = ScaleSelectionManager.SelectScale(scaleTypeEnum, rootNote, startFret,
                                                                       endFret);
            SelectItems(positionCollection.ToArray());
        }

        public void SelectScale(string scaleType, Note root, int position,
                                string scaleFingering)
        {
            var scaleTypeEnum = ConvertToScaleType(scaleType);
            var scaleFingeringTypeEnum = ConvertToScaleFingering(scaleFingering);
            var positionCollection = ScaleSelectionManager.SelectScale(scaleTypeEnum, root, position,
                                                                       scaleFingeringTypeEnum);
            SelectItems(positionCollection.ToArray());
        }


        public void SelectItems(params StringFretPair[] collectionOfStringFretPair)
        {
            _selected.AddRange(collectionOfStringFretPair);
            GuiSelector.SelectItems(collectionOfStringFretPair);
        }

        public void UnselectItems(params StringFretPair[] collectionOfStringFretPair)
        {
            foreach (var stringFretPair in collectionOfStringFretPair)
            {
                _selected.Remove(stringFretPair);
            }
            GuiSelector.UnselectItems(collectionOfStringFretPair);
        }

        private ScaleSelectionManager.ScaleFingering ConvertToScaleFingering(string scaleFingering)
        {
            ScaleSelectionManager.ScaleFingering scaleFingeringType;
            if (!Enum.TryParse(scaleFingering, true, out scaleFingeringType))
            {
                throw new ArgumentException("There is no such scale picking like " + scaleFingering);
            }
            return scaleFingeringType;
        }

        private ScaleType ConvertToScaleType(string scaleType)
        {
            ScaleType scaleTypeEnum;
            if (!Enum.TryParse(scaleType, true, out scaleTypeEnum))
            {
                throw new ArgumentException("There is no such scale type like " + scaleType);
            }
            return scaleTypeEnum;
        }

        public void Reselect()
        {
            GuiSelector.SelectItems(Selected.ToArray());
        }
    }
}