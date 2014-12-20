using System;
using System.Collections.Generic;

namespace SimpleHelpSystem.History
{
    public class HelpResourcesDescriptor
    {
        public List<HelpTreeItem> DocumentTreeRoot { get; private set; }
        public HelpTreeItem DefaultItem { get; set; }
        public String DllName { get; private set; }

        public HelpResourcesDescriptor(string dllName)
        {
            DocumentTreeRoot = new List<HelpTreeItem>();
            DllName = dllName;
        }
    }
}