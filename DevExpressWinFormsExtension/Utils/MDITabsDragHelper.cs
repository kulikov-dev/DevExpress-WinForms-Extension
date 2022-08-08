using DevExpress.Utils.Mdi;
using DevExpress.XtraBars.Docking2010;
using DevExpress.XtraBars.Docking2010.DragEngine;
using DevExpress.XtraBars.Docking2010.Views.Tabbed;
using System;
using System.Windows.Forms;

namespace DevExpressWinFormsExtension.Utils
{
    /// <summary>
    /// Helper to process draggable operation between MDI tabs
    /// </summary>
    internal class MDITabsDragHelper : IDisposable
    {
        /// <summary>
        /// Document manager
        /// </summary>
        private DocumentManager _manager;

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="manager"> Document manager </param>
        public MDITabsDragHelper(DocumentManager manager)
        {
            this._manager = manager;
        }

        /// <summary>
        /// Set up dragging opportunity
        /// </summary>
        /// <remarks> Note: have to uncheck it before MDI form closing to unsubscribe </remarks>
        public bool SelectNewPageOnDrag
        {
            set
            {
                MdiClient.DragOver -= OnMdiClientDragOver;
                if (value)
                {
                    MdiClient.AllowDrop = true;
                    MdiClient.DragOver += OnMdiClientDragOver;
                }
            }
        }

        /// <summary>
        /// MDI client
        /// </summary>
        private MdiClient MdiClient
        {
            get
            {
                return MdiClientSubclasser.GetMdiClient(_manager.MdiParent);
            }
        }

        /// <summary>
        /// Event on dipose
        /// </summary>
        public void Dispose()
        {
            if (MdiClient == null)
            {
                return;
            }

            SelectNewPageOnDrag = false;
        }

        /// <summary>
        /// Event on MDI client drag over
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        protected void OnMdiClientDragOver(object sender, DragEventArgs e)
        {
            var point = _manager.ScreenToClient(new System.Drawing.Point(e.X, e.Y));
            var hitInfo = _manager.CalcHitInfo(point) as TabbedViewHitInfo;
            if (hitInfo.Info.HitTest == LayoutElementHitTest.Header && hitInfo.Document != null)
            {
                _manager.View.ActivateDocument(hitInfo.Document.Control);
            }
        }
    }
}