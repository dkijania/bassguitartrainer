using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using SimpleHelpSystem.DocumentLoader;
using SimpleHelpSystem.History;
using WpfExtensions;

namespace SimpleHelpSystem.UI.WPF
{
    public class DocumentLoader<T> : AbstractDocumentLoader<T> where T : DependencyObject
    {
        public DocumentLoader(HelpResourcesDescriptor helpResourcesDescriptor, HelpHistoryPropertyNotifier<T> helpHistory, IDocumentPresenter<T> presenter) 
            : base(helpResourcesDescriptor,helpHistory,presenter)
        {
            HelpHistory = helpHistory;
        }

        public override T LoadAndEnableNavigation(Uri uri)
        {
            var document = LoadXaml(uri);
            EnableHyperlinkTracebility(document);
            HelpHistory.Add(document);
            return document;
        }

        private T LoadXaml(Uri uri)
        {
            var uriPath = Path.Combine(UriBase.OriginalString, uri.OriginalString);
            return (T) Application.LoadComponent(new Uri(uriPath, UriKind.Relative));
        }

        private void EnableHyperlinkTracebility(DependencyObject document)
        {
            var hyperlinks = document.GetVisuals().OfType<Hyperlink>();
            foreach (var link in hyperlinks)
            {
                link.RequestNavigate -= LinkRequestNavigate;
                link.RequestNavigate += LinkRequestNavigate;
            }
        }

        private void LinkRequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var uri = e.Uri;
            if (IsExternal(uri))
            {
                LaunchLinkInBrowser(uri);
            }
            else
            {
                Presenter.PresentContent(LoadAndEnableNavigation(e.Uri));
            }
        }

        private bool IsExternal(Uri uri)
        {
            return uri.IsAbsoluteUri;
        }

        private void LaunchLinkInBrowser(Uri uri)
        {
            System.Diagnostics.Process.Start(uri.AbsoluteUri);
        }
        
    }
}