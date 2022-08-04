using DevExpress.XtraEditors;
using DevExpressWinFormsExtension.DataControls.Forms;
using DevExpressWinFormsExtension.Utils;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace DevExpressWinFormsExtension.Progress
{
    /// <summary>
    /// Progress control to show on a XtraUserControl
    /// </summary>
    [ToolboxItem(false)]
    internal partial class ProgressControl : XtraUserControlDev
    {
        /// <summary>
        /// Label offset
        /// </summary>
        private const int labelYOffset = 5;

        /// <summary>
        /// Cancellation token
        /// </summary>
        private readonly CancellationTokenSource cancellationToken;

        /// <summary>
        /// ProgressBar control
        /// </summary>
        private readonly ProgressBarBaseControl activeProgress;

        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="marquee"> Flag if marquee progress </param>
        public ProgressControl(bool isMarquee)
        {
            InitializeComponent();
            cancellationToken = new CancellationTokenSource();
            Handler = new ControlProgressHandler(this);
            marqueeProgress.Visible = progressBar.Visible = false;
            activeProgress = isMarquee ? (ProgressBarBaseControl)marqueeProgress : progressBar;
            activeProgress.Visible = true;
            Appearance.BackColor = SkinHelper.TranslateColor(SystemColors.Control);
        }

        /// <summary>
        /// Progress manager
        /// </summary>
        public IProgressControlHandler Handler
        {
            get; private set;
        }

        /// <summary>
        /// Cancellation token
        /// </summary>
        public CancellationToken Token
        {
            get
            {
                return cancellationToken == null ? CancellationToken.None : cancellationToken.Token;
            }
        }

        /// <summary>
        /// Cancellation token source
        /// </summary>
        public CancellationTokenSource TokenSource
        {
            get
            {
                return cancellationToken;
            }
        }

        /// <summary>
        /// Increment a progress
        /// </summary>
        /// <param name="value"> Incrementation value </param>
        public void Increment(int value)
        {
            if (activeProgress == progressBar)
            {
                progressBar.Increment(value);
            }
        }

        /// <summary>
        /// Set progress value
        /// </summary>
        /// <param name="value"> Value </param>
        public void SetValue(int value)
        {
            if (activeProgress == progressBar)
            {
                var safeValue = Math.Min(Math.Max(value, 0), 100);
                progressBar.EditValue = safeValue;
            }
        }

        /// <summary>
        /// Set user message
        /// </summary>
        /// <param name="message"> User message </param>
        public void SetMessage(string message)
        {
            lblProgressText.Text = message;
            SetControlAtCenter(lblProgressText, (activeProgress.Height / 2) + (lblProgressText.Height / 2) + labelYOffset);
        }

        /// <summary>
        /// Event on the cancel button
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (XtraMessageBox.Show("Please confirm the cancellation", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            cancellationToken?.Cancel();
        }

        /// <summary>
        /// Event on resize
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void ProgressControl_SizeChanged(object sender, EventArgs e)
        {
            if (activeProgress == null)
            {
                return;
            }

            const int cancelButtonOffset = 10;
            SetControlAtCenter(activeProgress, 0);
            SetControlAtCenter(lblProgressText, (activeProgress.Height / 2) + (lblProgressText.Height / 2) + labelYOffset);
            SetControlAtCenter(btnCancel, -((activeProgress.Height / 2) + (btnCancel.Height / 2)) - cancelButtonOffset);
        }

        /// <summary>
        /// Update control position
        /// </summary>
        /// <param name="control"> Control </param>
        /// <param name="offsetY"> Offset </param>
        private void SetControlAtCenter(Control control, int offsetY)
        {
            control.Left = (Width / 2) - (control.Width / 2);
            control.Top = (Height / 2) - (control.Height / 2) - offsetY;
        }
    }
}