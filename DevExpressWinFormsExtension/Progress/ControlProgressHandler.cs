using System;
using System.Threading;
using System.Windows.Forms;

namespace DevExpressWinFormsExtension.Progress
{
    /// <summary>
    /// Progress manager
    /// </summary>
    internal class ControlProgressHandler : IProgressControlHandler
    {
        /// <summary>
        /// Temp progress increment value
        /// </summary>
        /// <remarks> For use with fractional numbers, visual increment for whole numbers only </remarks>
        private float cumulativeIncrementValue = 0f;

        /// <summary>
        /// Progress control
        /// </summary>
        private readonly ProgressControl control;

        /// <summary>
        /// Cancellation token source
        /// </summary>
        private readonly CancellationTokenSource tokenSource;

        /// <summary>
        /// Contstructor with parameters
        /// </summary>
        /// <param name="progressControl"> A progress control </param>
        internal ControlProgressHandler(ProgressControl progressControl)
        {
            control = progressControl;
            Token = progressControl.Token;
            tokenSource = progressControl.TokenSource;
            MessageSuffix = string.Empty;
        }

        /// <summary>
        /// Cancellation token
        /// </summary>
        public CancellationToken Token
        {
            get;

            private set;
        }

        /// <summary>
        /// Message suffix for an iteration progress
        /// </summary>
        public string MessageSuffix { get; set; }

        /// <summary>
        /// Increment a progress
        /// </summary>
        /// <param name="value"> Incrementation value </param>
        public void Increment(float value)
        {
            cumulativeIncrementValue += value;
            var fullPartNumber = Convert.ToInt32(Math.Truncate(cumulativeIncrementValue));
            cumulativeIncrementValue -= fullPartNumber;
            if (fullPartNumber < 1)
            {
                return;
            }

            if (control.InvokeRequired)
            {
                control.BeginInvoke(new Action(() => { control.Increment(fullPartNumber); }));
            }
            else
            {
                control.Increment(fullPartNumber);
                Application.DoEvents();
            }
        }

        /// <summary>
        /// Set user message
        /// </summary>
        /// <param name="message"> User message </param>
        public void SetMessage(string message)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new Action(() => { control.SetMessage(message + " " + MessageSuffix); }));
            }
            else
            {
                control.SetMessage(message + " " + MessageSuffix);
            }
        }

        /// <summary>
        /// Remove a progress control from a parent control
        /// </summary>
        public void Drop()
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new Action(() => { control.Parent = null; }));
            }
            else
            {
                control.Parent = null;
            }
        }

        /// <summary>
        /// Cancel an operation
        /// </summary>
        public void Cancel()
        {
            tokenSource?.Cancel();
        }

        /// <summary>
        /// Set progress value
        /// </summary>
        /// <param name="value"> Value </param>
        public void SetValue(int value)
        {
            if (control.InvokeRequired)
            {
                control.BeginInvoke(new Action(() => { control.SetValue(value); }));
            }
            else
            {
                control.SetValue(value);
            }
        }
    }
}