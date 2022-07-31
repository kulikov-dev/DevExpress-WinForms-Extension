namespace DevExpressWinFormsExtension.Interfaces
{
    /// <summary>
    /// Interface for control's which suppport AllowClosing query
    /// </summary>
    public interface IClosableControl
    {
        /// <summary>
        /// Confirmation of form closing event
        /// </summary>
        /// <returns> True, if form can be closed </returns>
        bool AllowClose();
    }
}