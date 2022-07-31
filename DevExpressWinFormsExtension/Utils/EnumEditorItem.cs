using System;

namespace DevExpressWinFormsExtension.Helpers
{
    /// <summary>
    /// Wrapper for comfort using enum values in DevExpress editors
    /// </summary>
    /// <typeparam name="T"> Enum </typeparam>
    public class EnumEditorItem<T>
        where T : Enum
    {
        /// <summary>
        /// Constructor with parameters
        /// </summary>
        /// <param name="value"> Enum value </param>
        public EnumEditorItem(T value)
        {
            EnumValue = value;
        }

        /// <summary>
        /// Enum value
        /// </summary>
        public T EnumValue { get; private set; }

        /// <summary>
        /// Get string representation of the class
        /// </summary>
        /// <returns> String representation </returns>
        public override string ToString()
        {
            return EnumValue.GetCaption();
        }
    }
}