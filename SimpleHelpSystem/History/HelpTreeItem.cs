using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SimpleHelpSystem.History
{
    public class HelpTreeItem
    {
        public HelpTreeItem(string header, Uri documentUri)
        {
            Header = header;
            DocumentUri = documentUri;
            Children = new ObservableCollection<HelpTreeItem>();
        }

        public HelpTreeItem(string header, string relativeLocation = "") :
            this(header, new Uri(relativeLocation, UriKind.Relative))
        {
        }

        public String Header { get; set; }
        public Uri DocumentUri { get; set; }
        public ObservableCollection<HelpTreeItem> Children { get; private set; }
    }
}