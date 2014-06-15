﻿using System.Windows.Controls;
using System.Windows.Documents;

namespace WpfSimpleHelpSystem.Gui
{
    /// <summary>
    /// Interaction logic for ContentViewer.xaml
    /// </summary>
    public partial class ContentViewer : UserControl
    {
        public ContentViewer()
        {
            InitializeComponent();
        }

        public FlowDocument Document
        {
            get { return ScrollViewer.Document; }
            set { ScrollViewer.Document = value; }
        }
    }
}
