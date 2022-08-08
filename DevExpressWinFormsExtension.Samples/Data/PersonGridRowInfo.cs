namespace DevExpressWinFormsExtension.Samples.Data
{
    /// <summary>
    /// Row info about test person
    /// </summary>
    internal sealed class PersonGridRowInfo
    {
        /// <summary>
        /// Full name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Average salary
        /// </summary>
        public double AverageSalary { get; set; }

        /// <summary>
        /// Is person valid
        /// </summary>
        public bool IsValid { get; set; }
    }
}