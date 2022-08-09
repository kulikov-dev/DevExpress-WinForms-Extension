using DevExpressWinFormsExtension.DataControls.LookUp;

namespace DevExpressWinFormsExtension.Samples.Data
{
    /// <summary>
    /// Music style item
    /// </summary>
    internal struct MusicStyleDataItem : ILookUpSplitableItem
    {
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="name"> Style name </param>
        /// <param name="description"> Hint description </param>
        /// <param name="isSplitter"> If item is a last item before splitting </param>
        public MusicStyleDataItem(string name, string description, bool isSplitter = false)
        {
            Name = name;
            Description = description;
            IsSplitter = isSplitter;
        }

        /// <summary>
        /// Style name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Hint description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Flag if a current item is a last item before splitting
        /// </summary>
        public bool IsSplitter { get; set; }
    }
}