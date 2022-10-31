using DevExpress.XtraEditors;
using DevExpress.XtraTabbedMdi;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace DevExpressWinFormsExtension.Utils
{
    /// <summary>
    /// Class to catch screenshot from any form/control/tab pages/mdi forms
    /// </summary>
    public static class ControlScreenshotCapture
    {
        /// <summary>
        /// Result image
        /// </summary>
        private static Bitmap resultImage;

        /// <summary>
        /// Previous focused control
        /// </summary>
        private static Control previousFocusedControl;

        /// <summary>
        /// Flag of the capture form closing
        /// </summary>
        private static bool isCaptureClosing = false;

        /// <summary>
        /// Link to main application form
        /// </summary>
        private static XtraForm mainAppForm;

        /// <summary>
        /// Capture a control screenshot
        /// </summary>
        /// <param name="mainAppForm"> Link to a main application form </param>
        /// <returns> Screenshot </returns>
        public static Bitmap GetScreenshot(XtraForm mainAppForm)
        {
            previousFocusedControl = null;
            ControlScreenshotCapture.mainAppForm = mainAppForm;

            var screenLeft = SystemInformation.VirtualScreen.Left;
            var screenTop = SystemInformation.VirtualScreen.Top;
            var screenWidth = SystemInformation.VirtualScreen.Width;
            var screenHeight = SystemInformation.VirtualScreen.Height;

            var factor = GetScalingFactor();
            var size = new Size((int)(screenWidth * factor), (int)(screenHeight * factor));
            isCaptureClosing = false;

            var captureForm = new Form
            {
                BackColor = Color.Blue,
                TransparencyKey = Color.Blue,
                AllowTransparency = true,
                Size = new Size(screenWidth, screenHeight),
                StartPosition = FormStartPosition.Manual,
                FormBorderStyle = FormBorderStyle.None,
                Location = new Point(0, 0),
                Cursor = Cursors.Hand,
                TopMost = true
            };

            captureForm.Paint += CaptureFormPaint;
            captureForm.MouseMove += CaptureFormMouseMove;
            captureForm.MouseDown += CaptureFormMouseDown;
            captureForm.Deactivate += CaptureFormDeactivate;
            captureForm.KeyDown += CaptureFormKeyDown;

            if (resultImage != null)
            {
                resultImage.Dispose();
                resultImage = null;
            }

            captureForm.ShowDialog(mainAppForm);

            ControlScreenshotCapture.mainAppForm = null;
            return resultImage;
        }

        /// <summary>
        /// Get control under a main form
        /// </summary>
        /// <param name="mousePosition"> Mouse position</param>
        /// <returns> Control under mouse position </returns>
        private static Control GetControlFromMainForm(Point mousePosition)
        {
            var control = GetControlAtPoint(mainAppForm, mousePosition);
            if (control == null)
            {
                var mdiContainer = (mainAppForm as IMdiContainer)?.GetContainer();
                if (mdiContainer != null)
                {
                    var pages = mdiContainer.Pages;
                    foreach (XtraMdiTabPage page in pages)
                    {
                        control = GetControlAtPoint(page.MdiChild, mousePosition);
                        if (control != null)
                        {
                            break;
                        }
                    }
                }
            }

            return control;
        }

        /// <summary>
        /// Get a control under cursor on the parent control
        /// </summary>
        /// <param name="parentControl"> Parent control </param>
        /// <param name="mousePosition"> Mouse position </param>
        /// <returns> Control under mouse position </returns>
        private static Control GetControlAtPoint(Control parentControl, Point mousePosition)
        {
            var screen = Screen.FromPoint(mousePosition);
            var point = mousePosition;
            if (!screen.Primary)
            {
                point = new Point(mousePosition.X, mousePosition.Y);
            }

            var control = parentControl.GetChildAtPoint(parentControl.PointToClient(point));
            if (control == null)
            {
                if (parentControl == mainAppForm)
                {
                    return null;
                }

                return parentControl;
            }
            else if (control is DevExpress.XtraTab.XtraTabPage)
            {
                control = (parentControl as DevExpress.XtraTab.XtraTabControl).SelectedTabPage;
            }
            else if (control is DevExpress.XtraTab.XtraTabControl)
            {
                var tabControl = control as DevExpress.XtraTab.XtraTabControl;
                var hitInfo = tabControl.CalcHitInfo(tabControl.PointToClient(point));
                if (hitInfo.HitTest == DevExpress.XtraTab.ViewInfo.XtraTabHitTest.PageHeader)
                {
                    tabControl.SelectedTabPage = hitInfo.Page;
                }
            }
            else if (!control.Visible)
            {
                return parentControl;
            }

            return GetControlAtPoint(control, point);
        }

        /// <summary>
        /// Get desktop scaling koefficient
        /// </summary>
        /// <returns> Scaling koefficient </returns>
        private static float GetScalingFactor()
        {
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                IntPtr desktop = graphics.GetHdc();
                try
                {
                    int logicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.Vertres);
                    int physicalScreenHeight = GetDeviceCaps(desktop, (int)DeviceCap.DesktopVertres);
                    float screenScalingFactor = physicalScreenHeight / (float)logicalScreenHeight;
                    return screenScalingFactor;
                }
                finally
                {
                    graphics.ReleaseHdc();
                }
            }
        }

        /// <summary>
        /// Capture form key down event
        /// </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Parameters </param>
        private static void CaptureFormKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ((Form)sender)?.Close();
            }
        }

        /// <summary>
        /// Capture form deactivate event
        /// </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Parameters </param>
        private static void CaptureFormDeactivate(object sender, EventArgs e)
        {
            ((Form)sender)?.Close();
        }

        /// <summary>
        /// Capture form paint event
        /// </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Parameters </param>
        private static void CaptureFormPaint(object sender, PaintEventArgs e)
        {
            if (previousFocusedControl == null || isCaptureClosing)
            {
                return;
            }

            var pointToScreen = previousFocusedControl.PointToScreen(new Point(0, 0));
            var controlRectangle = new Rectangle(pointToScreen.X, pointToScreen.Y, previousFocusedControl.Width, previousFocusedControl.Height);
            var factor = GetScalingFactor();
            var pen = new Pen(Color.FromArgb(40, Color.Red), 0.5f);
            try
            {
                const int k = 2;
                for (var i = -k; i < k; i++)
                {
                    for (var j = -k; j < k; j++)
                    {
                        if (Math.Abs(i) + Math.Abs(j) < k + 1)
                        {
                            e.Graphics.DrawRectangle(pen, (controlRectangle.Left + i) * factor, (controlRectangle.Top + j) * factor, controlRectangle.Width * factor, controlRectangle.Height * factor);
                        }
                    }
                }
            }
            finally
            {
                pen.Dispose();
            }
        }

        /// <summary>
        /// Capture form mouse move event
        /// </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Parameters </param>
        private static void CaptureFormMouseMove(object sender, MouseEventArgs e)
        {
            var control = GetControlFromMainForm(e.Location);
            if (control == null || previousFocusedControl == control)
            {
                return;
            }

            previousFocusedControl = control;
            (sender as Form).Refresh();
        }

        /// <summary>
        /// Capture form mouse down event
        /// </summary>
        /// <param name="sender"> Sender </param>
        /// <param name="e"> Parameters </param>
        private static void CaptureFormMouseDown(object sender, MouseEventArgs e)
        {
            var control = GetControlFromMainForm(e.Location);
            if (control == null)
            {
                return;
            }

            isCaptureClosing = true;
            var form = sender as Form;
            form.Refresh();
            form.Close();

            var pointToScreen = control.PointToScreen(new Point(0, 0));
            var factor = GetScalingFactor();
            resultImage = new Bitmap((int)(control.Width * factor), (int)(control.Height * factor));
            using (var graphics = Graphics.FromImage(resultImage))
            {
                var screen = Screen.FromHandle(control.Handle);
                var controlRectangle = new Rectangle((int)((pointToScreen.X - screen.Bounds.Left) * factor), (int)((pointToScreen.Y - screen.Bounds.Top) * factor), (int)(control.Width * factor), (int)(control.Height * factor));
                graphics.CopyFromScreen(pointToScreen, new Point(0, 0), resultImage.Size, CopyPixelOperation.SourceCopy);
            }
        }

        [DllImport("gdi32.dll")]
        private static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        /// <summary>
        /// Enum for monitor system metrics
        /// </summary>
        private enum DeviceCap
        {
            /// <summary>
            /// VERTRES
            /// </summary>
            Vertres = 10,

            /// <summary>
            /// DESKTOPVERTRES
            /// </summary>
            DesktopVertres = 117,
        }
    }

    /// <summary>
    /// Interface for MdiContainer form
    /// </summary>
    public interface IMdiContainer
    {
        /// <summary>
        /// Get mdi manager
        /// </summary>
        /// <returns> MDI manager </returns>
        XtraTabbedMdiManager GetContainer();
    }
}
