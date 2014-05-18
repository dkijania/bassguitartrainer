using System;
using SimpleHelpSystem.History;

namespace SimpleHelpSystem.DocumentLoader
{
    public abstract class AbstractDocumentLoader<T>
    {
        public HelpResourcesDescriptor HelpResourcesDescriptor { get; protected set; }
        public IDocumentPresenter<T> Presenter { get; protected set; }
        protected HelpHistoryPropertyNotifier<T> HelpHistory;
        protected Uri UriBase;
        
        protected AbstractDocumentLoader(HelpResourcesDescriptor helpResourcesDescriptor, HelpHistoryPropertyNotifier<T> helpHistory, IDocumentPresenter<T> presenter)
        {
            HelpResourcesDescriptor = helpResourcesDescriptor;
            Presenter = presenter;
            HelpHistory = helpHistory;
            UriBase = CreateUriToClientDll(HelpResourcesDescriptor);
        }

        protected Uri CreateUriToClientDll(HelpResourcesDescriptor helpResourcesDescriptor)
        {
            return new Uri(helpResourcesDescriptor.DllName + @";;;component", UriKind.Relative);
        }

        public abstract T LoadAndEnableNavigation(Uri documentUri);
    }
}