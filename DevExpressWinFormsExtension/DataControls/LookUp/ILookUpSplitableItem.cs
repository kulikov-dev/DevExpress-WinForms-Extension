namespace DevExpressWinFormsExtension.DataControls.LookUp
{
    /// <summary>
    /// Interface for LookUp items, which support splitters
    /// </summary>
    /// <remarks> In case, if need to create last selected items for example </remarks>
    public interface ILookUpSplitableItem
    {
        /// <summary>
        /// Flag if a current item is a last item before splitting
        /// </summary>
        bool IsSplitter { get; set; }
    }
}