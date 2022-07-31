using System.ComponentModel;
using System.Windows.Forms.Design;

namespace DevExpressWinFormsExtension.DataControls.TreeList.Utils
{
    /// <summary>
    /// Designer for the TreeListSearchable
    /// </summary>
    public class TreeListSearchableDesigner : ControlDesigner
    {
        /// <summary>
        /// Initialization
        /// </summary>
        /// <param name="component"> Component </param>
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            EnableDesignMode(((TreeListSearchable)Control).TreeList, typeof(TreeListDev).Name);
        }
    }
}