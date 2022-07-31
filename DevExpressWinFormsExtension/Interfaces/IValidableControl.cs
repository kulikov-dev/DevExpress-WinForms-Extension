namespace DevExpressWinFormsExtension.Interfaces
{
    /// <summary>
    /// Interface for controls, which support validation
    /// </summary>
    public interface IValidable
    {
        /// <summary>
        /// Control validation
        /// </summary>
        /// <returns> True, if valid </returns>
        bool ValidateData();

        /// <summary>
        /// Setup control's state, depends on validation result
        /// </summary>
        /// <param name="isValid"> Validation result </param>
        void SetControlState(bool isValid);
    }
}