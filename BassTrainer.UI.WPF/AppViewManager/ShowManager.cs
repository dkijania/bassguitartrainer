using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BassTrainer.Core.Components;
using BassTrainer.Core.Utils;

namespace BassTrainer.UI.WPF.AppViewManager
{
    public class ShowManager : IVisibilityManager
    {
        public Dictionary<ComponentId, FrameworkElement> VisibilityDictionary =
            new Dictionary<ComponentId, FrameworkElement>();

        public void SetVisible(params ComponentId[] toVisible)
        {
           SetVisibility(Visibility.Visible,toVisible);
        }

        public void ShowAll()
        {
            SetVisible(AllComponents);
        }


        public void SetVisibleExlusive(params ComponentId[] toVisible)
        {
            foreach (
                var componentIdEnumValue in
                    AllComponents.Where(
                        componentIdEnumValue => !toVisible.Any(x => x.Equals(componentIdEnumValue))))
            {
                Hide(componentIdEnumValue);
            }
        }

        public void SetHideAll()
        {
            Hide(AllComponents);
        }

        public void SetHideExlusive(params ComponentId[] toVisible)
        {
            throw new NotImplementedException();
        }

        public void SetEnabledAll()
        {
            SetEnabled(AllComponents);
        }

        public void SetEnabled(params ComponentId[] toVisible)
        {
            foreach (var componentId in toVisible)
            {
                VisibilityDictionary[componentId].IsEnabled = true;
            }
        }

        public void Hide(params ComponentId[] components)
        {
            SetVisibility(Visibility.Hidden,components);
        }

        public void HideAll()
        {
            SetVisibility(Visibility.Hidden,AllComponents);
        }

        public void SetVisibility(Visibility visibility,params ComponentId[] toVisible)
        {
            foreach (var componentId in toVisible)
            {
                VisibilityDictionary[componentId].Visibility = visibility;
                foreach (
                    var child in from object child in LogicalTreeHelper.GetChildren(VisibilityDictionary[componentId])
                                 let childFrameworkElement = child as FrameworkElement
                                 where childFrameworkElement != null
                                 select child)
                {
                    ((FrameworkElement) child).Visibility = visibility;
                }
            }
        }

        public void SetOnTop(ComponentId[] toVisible)
        {
            foreach (var componentId in toVisible)
            {
                SetOnTop(componentId);
            }
        }

        public void SetOnTop(ComponentId toTop)
        {
            if (VisibilityDictionary[toTop] as TabItem == null) 
                return;
            var tabItem = ((TabItem)VisibilityDictionary[toTop]);
            tabItem.IsSelected = true;
            tabItem.TabIndex = 0;
        }

        protected ComponentId[] AllComponents
        {
            get
            {
                return new[]
                           {
                               ComponentId.Fretboard, ComponentId.Notation, ComponentId.NotesView, ComponentId.Intervals
                               , ComponentId.Player
                           };
            }
        }
    }
}