﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using BassTrainer.Core.Components.Fretboard;
using BassTrainer.Core.Components.Fretboard.SelectionManager;
using BassTrainer.Core.Const;
using BassTrainer.Core.Settings;
using BassTrainer.Core.Utils;
using BassTrainer.UI.WPF.FretBoard.FretBoardView;

namespace BassTrainer.UI.WPF.FretBoard.SelectionManager
{
    public class GuiSelector : IGuiSelector
    {
        protected readonly BorderStyleCollection BorderStyleCollection = BorderStyleCollection.Instance;
        protected readonly Settings Settings;
        protected readonly FretBoardGuiBuilder FretBoardGuiBuilder;
        protected readonly Panel DrawArea;

        public GuiSelector(Settings settings, IFretBoardGuiBuilder fretBoardGuiBuilder)
        {
            Settings = settings;
            FretBoardGuiBuilder = fretBoardGuiBuilder as FretBoardGuiBuilder;
            if (FretBoardGuiBuilder != null) DrawArea = FretBoardGuiBuilder.DrawArea;
        }


        public void UnselectAllItems()
        {
            foreach (var border in DrawArea.Children.OfType<Border>())
            {
                UnselectItem(border);
            }
        }

        public void SelectItems(params StringFretPair[] collectionOfStringFretPair)
        {
            foreach (var border in collectionOfStringFretPair.Select(item => FretBoardGuiBuilder.GetBorderForCords(item)))
            {
                if(border != null)
                SelectItem(border);
            }
        }

        public void UnselectItems(params StringFretPair[] collectionOfStringFretPair)
        {
            foreach (var item in collectionOfStringFretPair)
            {
                var border = FretBoardGuiBuilder.GetBorderForCords(item);
                UnselectItem(border);
                
            }
        }

        public void SelectAllItems()
        {
            foreach (var border in DrawArea.Children.OfType<Border>())
            {
                SelectItem(border);
            }
        }

        public void SelectMany(IEnumerable<Border> borders)
        {
            foreach (var border in borders)
            {
                SelectItem(border);
            }
        }

        public virtual void SelectItem(Border element)
        {
            element.Background = BorderStyleCollection.BackgroundBrushAfterSelection;
        }

        public virtual void UnselectItem(Border element)
        {
            element.Background = BorderStyleCollection.
                GetCorrectBorderStyle(Settings.CorrectRectanglePreset.Value).Background;
        }
    }
}
