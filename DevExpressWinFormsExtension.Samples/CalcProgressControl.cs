using DevExpressWinFormsExtension.DataControls.Forms;
using DevExpressWinFormsExtension.Progress;
using System.Threading.Tasks;

namespace DevExpressWinFormsExtension.Samples
{
    /// <summary>
    /// Sample control to demonstrate progress
    /// </summary>
    public partial class CalcProgressControl : XtraUserControlDev
    {
        public CalcProgressControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event on start calculations
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void btnStartCalc_Click(object sender, System.EventArgs e)
        {
            var handler = ProgressManager.InitMarquee(this);

            //// Perform long business logic
            new Task(() =>
            {
                while (true)
                {
                    if (handler.Token.IsCancellationRequested)
                    {
                        handler.Drop();
                        break;
                    }
                }
            }).Start();
        }
    }
}