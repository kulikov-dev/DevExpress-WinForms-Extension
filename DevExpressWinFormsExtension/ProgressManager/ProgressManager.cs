using System;
using System.Windows.Forms;

namespace DevExpressWinFormsExtension.ProgressManager
{
    /// <summary>
    /// Progress manager
    /// </summary>
    /// <remarks> Used to create/drop new progress for a control </remarks>
    public static class ProgressManager
    {
        /// <summary>
        /// Add marquee progress to the control
        /// </summary>
        /// <param name="control"> Conrol </param>
        /// <returns> Manager for control the created progress </returns>
        public static IProgressControlHandler InitMarquee(Control control)
        {
            if (control.InvokeRequired)
            {
                return (IProgressControlHandler)control.Invoke(
                    new Func<IProgressControlHandler>(() =>
                    {
                        return SafeInit(control, isMarquee: true);
                    }));
            }
            else
            {
                return SafeInit(control, isMarquee: true);
            }
        }

        /// <summary>
        /// Add progress to the control
        /// </summary>
        /// <param name="control"> Conrol </param>
        /// <returns> Manager to control the created progress </returns>
        public static IProgressControlHandler Init(Control control)
        {
            if (control == null)
            {
                return null;
            }

            if (control.InvokeRequired)
            {
                return (IProgressControlHandler)control.Invoke(
                    new Func<IProgressControlHandler>(() =>
                    {
                        return SafeInit(control, isMarquee: false);
                    }));
            }
            else
            {
                return SafeInit(control, isMarquee: false);
            }
        }

        /// <summary>
        /// Remove progress
        /// </summary>
        /// <param name="manager"> Manager for a progress </param>
        public static void Drop(IProgressControlHandler manager)
        {
            manager?.Drop();
        }

        /// <summary>
        /// Add progress to the control
        /// </summary>
        /// <param name="control"> Control </param>
        /// <param name="isMarquee"> Flag if marquee </param>
        /// <returns> Manager to control the created progress </returns>
        private static IProgressControlHandler SafeInit(Control control, bool isMarquee)
        {
            var progressControl = new ProgressControl(isMarquee: isMarquee)
            {
                Parent = control
            };
            progressControl.BringToFront();
            progressControl.Handler.SetMessage("Please wait...");
            progressControl.Dock = DockStyle.Fill;
            return progressControl.Handler;
        }
    }
}