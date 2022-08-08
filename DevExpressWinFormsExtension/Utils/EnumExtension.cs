using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpressWinFormsExtension.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DevExpressWinFormsExtension.Utils
{
    /// <summary>
    /// Extension for Enums
    /// </summary>
    /// <remarks> 
    /// 1. Allows to get caption/enum value by Description Attribute;
    /// 2. Fill different DevExpress editors with enum values.
    /// </remarks>
    public static class EnumExtension
    {
        /// <summary>
        /// Get an enum caption by DescriptionAttribute
        /// </summary>
        /// <typeparam name="T"> Description attribute </typeparam>
        /// <param name="value"> Enum value </param>
        /// <returns> A caption </returns>
        public static string GetCaption<T>(this Enum value) where T : DescriptionAttribute
        {
            var attribute = value.GetAttribute<T>();
            return attribute == null ? value.ToString() : attribute.Description;
        }

        /// <summary>
        /// Get an enum caption by DescriptionAttribute
        /// </summary>
        /// <param name="value"> Enum value </param>
        /// <returns> Caption </returns>
        public static string GetCaption(this Enum value)
        {
            return GetCaption<DescriptionAttribute>(value);
        }

        /// <summary>
        /// Get an enum value by the caption
        /// </summary>
        /// <typeparam name="T"> Description attribute </typeparam>
        /// <param name="type"> Enum type </param>
        /// <param name="caption"> Enum value caption </param>
        /// <param name="defaultValue"> Default value </param>
        /// <returns> An enum value </returns>
        /// <exception cref="InvalidEnumArgumentException"> Exception on not found enum by caption </exception>
        public static object FromCaption<T>(Type type, string caption, object defaultValue = null)
            where T : DescriptionAttribute
        {
            if (TryGetCaption<T>(type, caption, out object value))
            {
                return value;
            }

            if (defaultValue == null)
            {
                throw new InvalidEnumArgumentException(string.Format("Can't find a caption '{0}' for the enum '{1}'.", caption, type.ToString()));
            }

            return defaultValue;
        }

        /// <summary>
        /// Get an enum value by the caption
        /// </summary>
        /// <typeparam name="T"> Enum type </typeparam>
        /// <param name="caption"> Enum value caption </param>
        /// <returns> An enum value </returns>
        public static T FromCaption<T>(string caption)
            where T : struct, IConvertible
        {
            return (T)FromCaption<DescriptionAttribute>(typeof(T), caption);
        }

        /// <summary>
        /// Get an enum value by the caption
        /// </summary>
        /// <typeparam name="T"> Enum type </typeparam>
        /// <param name="caption"> Enum value caption </param>
        /// <param name="defaultValue"> Default value </param>
        /// <returns> An enum value </returns>
        public static T FromCaption<T>(string caption, T defaultValue)
            where T : struct, IConvertible
        {
            return (T)FromCaption<DescriptionAttribute>(typeof(T), caption, defaultValue);
        }

        /// <summary>
        /// Get all captions for the enum values
        /// </summary>
        /// <typeparam name="T"> Description attribute </typeparam>
        /// <param name="type"> Enum type </param>
        /// <returns> List of enum captions </returns>
        public static List<string> GetAllCaptions<T>(Type type)
            where T : DescriptionAttribute
        {
            var result = new List<string>();
            foreach (var value in Enum.GetValues(type))
            {
                result.Add((value as Enum).GetCaption<T>());
            }

            return result;
        }

        /// <summary>
        /// Get all captions for the enum values
        /// </summary>
        /// <typeparam name="T"> Enum type </typeparam>
        /// <returns> List of enum captions </returns>
        public static List<string> GetAllCaptions<T>()
            where T : struct, IConvertible
        {
            return GetAllCaptions<DescriptionAttribute>(typeof(T));
        }

        /// <summary>
        /// Get enum wrapper for editors
        /// </summary>
        /// <typeparam name="T"> Enum type </typeparam>
        /// <param name="enumValue"> Enum value </param>
        /// <returns> Wrapped enum item </returns>
        public static EnumEditorItem<T> GetEditorItem<T>(this T enumValue)
            where T : Enum
        {
            return new EnumEditorItem<T>(enumValue);
        }

        /// <summary>
        /// Fill RepositoryItemComboBox by enum values
        /// </summary>
        /// <typeparam name="T"> Enum type </typeparam>
        /// <param name="repository"> Repository </param>
        public static void FillRepository<T>(RepositoryItemComboBox repository)
            where T : Enum
        {
            repository.BeginUpdate();
            try
            {
                repository.Items.Clear();
                foreach (T enumItem in Enum.GetValues(typeof(T)))
                {
                    var enumValue = new EnumEditorItem<T>(enumItem);
                    repository.Items.Add(enumValue);
                }
            }
            finally
            {
                repository.EndUpdate();
            }
        }

        /// <summary>
        /// Fill RepositoryItemCheckedComboBoxEdit by enum values
        /// </summary>
        /// <typeparam name="T"> Enum type </typeparam>
        /// <param name="repository"> Repository </param>
        /// <param name="checkedItems"> Checked enum values </param>
        public static void FillRepository<T>(RepositoryItemCheckedComboBoxEdit repository, IEnumerable<T> checkedItems)
            where T : Enum
        {
            repository.BeginUpdate();
            try
            {
                repository.Items.Clear();
                foreach (T enumItem in Enum.GetValues(typeof(T)))
                {
                    var enumValue = new EnumEditorItem<T>(enumItem);
                    repository.Items.Add(enumValue, checkedItems != null && checkedItems.Contains(enumItem));
                }
            }
            finally
            {
                repository.EndUpdate();
            }
        }

        /// <summary>
        /// Fill ComboBoxEdit by enum values
        /// </summary>
        /// <typeparam name="T"> Enum type </typeparam>
        /// <param name="control"> Control </param>
        /// <param name="selectedIndex"> Selected index </param>
        public static void FillComboBox<T>(ComboBoxEdit control, int selectedIndex = 0)
            where T : Enum
        {
            control.Properties.BeginUpdate();
            try
            {
                control.Properties.Items.Clear();
                foreach (T enumItem in Enum.GetValues(typeof(T)))
                {
                    var enumValue = new EnumEditorItem<T>(enumItem);
                    control.Properties.Items.Add(enumValue);
                }

                control.SelectedIndex = control.Properties.Items.Count > 0 && selectedIndex < control.Properties.Items.Count ? selectedIndex : -1;
            }
            finally
            {
                control.Properties.EndUpdate();
            }
        }

        /// <summary>
        /// Fill ListBoxControl by enum values
        /// </summary>
        /// <typeparam name="T"> Enum type </typeparam>
        /// <param name="control"> Control </param>
        public static void FillListBox<T>(ListBoxControl control)
            where T : Enum
        {
            control.Items.BeginUpdate();
            try
            {
                control.Items.Clear();
                foreach (T enumItem in Enum.GetValues(typeof(T)))
                {
                    var enumValue = new EnumEditorItem<T>(enumItem);
                    control.Items.Add(enumValue);
                }
            }
            finally
            {
                control.EndUpdate();
            }
        }

        /// <summary>
        /// Fill CheckedListBoxControl by enum values
        /// </summary>
        /// <typeparam name="T"> Enum type </typeparam>
        /// <param name="control"> Control </param>
        /// <param name="checkedItems"> Checked enum values </param>
        public static void FillListBox<T>(CheckedListBoxControl control, IEnumerable<T> checkedItems)
            where T : Enum
        {
            control.BeginUpdate();
            try
            {
                control.Items.Clear();
                foreach (T enumItem in Enum.GetValues(typeof(T)))
                {
                    var enumValue = new EnumEditorItem<T>(enumItem);
                    control.Items.Add(enumValue, checkedItems != null && checkedItems.Contains(enumItem));
                }
            }
            finally
            {
                control.EndUpdate();
            }
        }

        /// <summary>
        /// Get enum attribute
        /// </summary>
        /// <typeparam name="T"> Attribute type </typeparam>
        /// <param name="value"> Enum value </param>
        /// <returns> Attribute </returns>
        private static T GetAttribute<T>(this Enum value)
            where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);
            return (T)attributes[0];
        }

        /// <summary>
        /// Get an enum value by the caption
        /// </summary>
        /// <typeparam name="T"> Description attribute </typeparam>
        /// <param name="type"> Enum type </param>
        /// <param name="caption"> Enum value caption </param>
        /// <param name="value"> Enum value </param>
        /// <returns> Flag if success </returns>
        private static bool TryGetCaption<T>(Type type, string caption, out object value) where T : DescriptionAttribute
        {
            var clearCaption = string.IsNullOrWhiteSpace(caption) ? string.Empty : caption.Trim();
            value = null;
            foreach (Enum enumValue in Enum.GetValues(type))
            {
                var enumCaption = enumValue.GetCaption<T>();
                if (clearCaption.Equals(enumCaption, StringComparison.OrdinalIgnoreCase))
                {
                    value = enumValue;
                    return true;
                }
            }

            return false;
        }
    }
}