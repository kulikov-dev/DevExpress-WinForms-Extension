using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;

namespace DevExpressWinFormsExtension.DataControls.Extensions
{
    /// <summary>
    /// Extension for DateEdit component
    /// </summary>
    public static class DateEditExtension
    {
        /// <summary>
        /// Update view style of DateEdit editor according to the date interval type
        /// </summary>
        /// <param name="editor"> Link to the editor </param>
        /// <param name="intervalType"> Date interval type </param>
        public static void UpdateView(this DateEdit editor, VistaCalendarInitialViewStyle intervalType)
        {
            UpdateView(editor.Properties, intervalType);
        }

        /// <summary>
        /// Update view style of RepositoryItemDateEdit editor according to the date interval type
        /// </summary>
        /// <param name="editor"> Link to the editor </param>
        /// <param name="intervalType"> Date interval type </param>
        public static void UpdateView(this RepositoryItemDateEdit editor, VistaCalendarInitialViewStyle intervalType)
        {
            editor.CalendarView = CalendarView.Vista;
            editor.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            editor.VistaDisplayMode = DevExpress.Utils.DefaultBoolean.True;
            switch (intervalType)
            {
                case VistaCalendarInitialViewStyle.YearView:
                    editor.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.YearsGroupView;
                    editor.VistaCalendarViewStyle = VistaCalendarViewStyle.YearsGroupView;

                    editor.DisplayFormat.FormatString = editor.Mask.EditMask = "yyyy";
                    break;
                case VistaCalendarInitialViewStyle.QuarterView:
                    editor.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.QuarterView;
                    editor.VistaCalendarViewStyle = VistaCalendarViewStyle.QuarterView;

                    editor.DisplayFormat.FormatString = editor.Mask.EditMask = "MMM yyyy";
                    break;
                case VistaCalendarInitialViewStyle.MonthView:
                    editor.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.YearView;
                    editor.VistaCalendarViewStyle = VistaCalendarViewStyle.YearView;

                    editor.DisplayFormat.FormatString = editor.Mask.EditMask = "MM yyyy";
                    break;
                default:
                    editor.VistaCalendarInitialViewStyle = VistaCalendarInitialViewStyle.MonthView;
                    editor.VistaCalendarViewStyle = VistaCalendarViewStyle.All;

                    editor.DisplayFormat.FormatString = editor.Mask.EditMask = "dd MM yyyy";
                    break;
            }
        }
    }
}