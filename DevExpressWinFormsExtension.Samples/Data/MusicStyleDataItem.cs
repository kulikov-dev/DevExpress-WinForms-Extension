namespace DevExpressWinFormsExtension.Samples.Data
{
    /// <summary>
    /// Music style item
    /// </summary>
    internal struct MusicStyleDataItem
    {
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="name"> Style name </param>
        /// <param name="description"> Hint description </param>
        public MusicStyleDataItem(string name, string description)
        {
            Name = name;
            Description = description;
        }

        /// <summary>
        /// Style name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Hint description
        /// </summary>
        public string Description { get; set; }
    }
}