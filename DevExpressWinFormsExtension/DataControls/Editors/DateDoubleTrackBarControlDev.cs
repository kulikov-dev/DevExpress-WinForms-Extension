using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;

namespace DevExpressWinFormsExtension.DataControls.Editors
{
    /// <summary>
    /// The visual component, which allows users to select range of dates: start, end and date between them.
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(RangeControl))]
    [Description("The visual component, which allows users to select range of dates: start, end and date between them.")]
    public partial class DateDoubleTrackBarControlDev : UserControlDev
    {
        #region Field's

        /// <summary>
        /// Height
        /// </summary>
        private const int barHeight = 7;

        /// <summary>
        /// Left scale limit
        /// </summary>
        private int minScaleLimit;

        /// <summary>
        /// Right scale limit
        /// </summary>
        private int maxScaleLimit;

        /// <summary>
        /// Left date in days count
        /// </summary>
        private int leftDateDays;

        /// <summary>
        /// Right date in days count
        /// </summary>
        private int rightDateDays;

        /// <summary>
        /// Middle date in days count
        /// </summary>
        private int middleDateDays;

        /// <summary>
        /// Updates count for begin/end update pattern
        /// </summary>
        private int updatesCount;

        /// <summary>
        /// Last mouse down position
        /// </summary>
        private Point mouseDownPosition;

        /// <summary>
        /// An active button, which is moving now
        /// </summary>
        private SimpleButton movingButton;

        /// <summary>
        /// Appearance info about track buttons
        /// </summary>
        private SkinElement seTrack;

        /// <summary>
        /// Appearance info about ticks
        /// </summary>
        private SkinElement seTick;

        /// <summary>
        /// Vertical center of the component
        /// </summary>
        private int componentVerticalCenter;

        /// <summary>
        /// Track left position
        /// </summary>
        private int trackLeft;

        /// <summary>
        /// Track buttons width
        /// </summary>
        private int trackWidth;

        /// <summary>
        /// Date buttons size
        /// </summary>
        private Size buttonSize = new Size(12, 26);

        /// <summary>
        /// Label top position
        /// </summary>
        private int labelTop;

        /// <summary>
        /// Tick top position
        /// </summary>
        private int tickTop;

        /// <summary>
        /// List of ticks
        /// </summary>
        private List<TickStruct> ticks;

        #endregion

        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DateDoubleTrackBarControlDev()
        {
            InitializeComponent();

            MinDateLimit = DateTime.Now.AddYears(-1);
            MaxDateLimit = DateTime.Now;
            SetupDates(MinDateLimit, MaxDateLimit);

            LookAndFeelOnStyleChanged(null, null);
            LookAndFeel.StyleChanged += LookAndFeelOnStyleChanged;
            ButtonSize = ButtonSize;
        }

        /// <summary>
        ///  Finalizes an instance of the DateDoubleTrackBarControlDev class.
        /// </summary>
        ~DateDoubleTrackBarControlDev()
        {
            if (LookAndFeel != null)
            {
                LookAndFeel.StyleChanged -= LookAndFeelOnStyleChanged;
            }
        }

        /// <summary>
        /// On dates changed
        /// </summary>
        public event EventHandler DatesChanged;

        #region Properties

        /// <summary>
        /// Left scale limit
        /// </summary>
        [Browsable(true)]
        [Category("Data")]
        [Description("Left scale limit")]
        public DateTime MinDateLimit
        {
            get
            {
                return ConvertDaysCountToDate(MinScaleLimit);
            }

            set
            {
                MinScaleLimit = ConvertDateToDaysCount(value);
            }
        }

        /// <summary>
        /// Right scale limit
        /// </summary>
        [Browsable(true)]
        [Category("Data")]
        [Description("Right scale limit")]
        public DateTime MaxDateLimit
        {
            get
            {
                return ConvertDaysCountToDate(MaxScaleLimit);
            }

            set
            {
                MaxScaleLimit = ConvertDateToDaysCount(value);
            }
        }

        /// <summary>
        /// Left date
        /// </summary>
        [Browsable(true)]
        [Category("Data")]
        [Description("Left date")]
        public DateTime LeftDate
        {
            get
            {
                return ConvertDaysCountToDate(LeftDateDays);
            }

            set
            {
                LeftDateDays = ConvertDateToDaysCount(value);
            }
        }

        /// <summary>
        /// Right date
        /// </summary>
        [Browsable(true)]
        [Category("Data")]
        [Description("Right date")]
        public DateTime RightDate
        {
            get
            {
                return ConvertDaysCountToDate(RightDateDays);
            }

            set
            {
                RightDateDays = ConvertDateToDaysCount(value);
            }
        }

        /// <summary>
        /// Middle date
        /// </summary>
        [Browsable(true)]
        [Category("Data")]
        [Description("Middle date")]
        public DateTime MiddleDate
        {
            get
            {
                return ConvertDaysCountToDate(MiddleDateDays);
            }

            set
            {
                MiddleDateDays = ConvertDateToDaysCount(value);
            }
        }

        /// <summary>
        /// Visibility of the middle button
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Visibility of the middle button")]
        public bool MiddleButtonVisibility
        {
            get
            {
                return !btnMiddle.Visible;
            }

            set
            {
                btnMiddle.Visible = !value;
            }
        }

        /// <summary>
        /// Minimum length between track buttons
        /// </summary>
        [Browsable(true)]
        [Category("Layout")]
        [Description("Minimum length between track buttons")]
        public int LengthMinimum { get; set; }

        /// <summary>
        /// Show tooltip on moving track buttons
        /// </summary>
        [Browsable(true)]
        [Category("Layout")]
        [Description("Show tooltip on moving track buttons")]
        public bool ShowTooltip { get; set; }

        /// <summary>
        /// Draggable button's size
        /// </summary>
        [Browsable(true)]
        [Category("Layout")]
        [Description("Draggable button's size")]
        public Size ButtonSize
        {
            get
            {
                return buttonSize;
            }

            set
            {
                buttonSize = value;
                btnLeftDate.Size = btnRightDate.Size = btnMiddle.Size = buttonSize;
            }
        }

        /// <summary>
        /// Flag of auto-setup position of middle date, based on start/end dates.
        /// </summary>
        private bool autoMiddleDate;

        /// <summary>
        /// Flag of auto-setup position of middle date, based on start/end dates.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Flag of auto-setup position of middle date, based on start/end dates.")]
        public bool AutoMiddleDate
        {
            get
            {
                return autoMiddleDate;
            }

            set
            {
                autoMiddleDate = value;
                btnMiddle.Enabled = !value;
            }
        }

        /// <summary>
        /// Size of small ticks
        /// </summary>
        private int TickSmall { get; set; } = 2;

        /// <summary>
        /// Size of large ticks
        /// </summary>
        private int TickLarge { get; set; } = 5;

        /// <summary>
        /// Left scale limit
        /// </summary>
        private int MinScaleLimit
        {
            get
            {
                return minScaleLimit;
            }

            set
            {
                minScaleLimit = value;
                if (LeftDateDays < minScaleLimit)
                {
                    LeftDateDays = minScaleLimit;
                }

                CreateTicks();
                UpdateButtonPositions();
            }
        }

        /// <summary>
        /// Right scale limit
        /// </summary>
        private int MaxScaleLimit
        {
            get
            {
                return maxScaleLimit;
            }

            set
            {
                maxScaleLimit = value;
                if (RightDateDays > maxScaleLimit)
                {
                    RightDateDays = maxScaleLimit;
                }

                CreateTicks();
                UpdateButtonPositions();
            }
        }

        /// <summary>
        /// Start date in days count
        /// </summary>
        private int LeftDateDays
        {
            get
            {
                return leftDateDays;
            }

            set
            {
                var newValue = Math.Max(MinScaleLimit, Math.Min(MaxScaleLimit, value));
                newValue = Math.Min(newValue, rightDateDays - LengthMinimum);
                if (newValue == leftDateDays)
                {
                    return;
                }

                leftDateDays = newValue;
                if (AutoMiddleDate)
                {
                    CalcMiddleDateDays();
                }
                else
                {
                    MiddleDateDays = Math.Max(MiddleDateDays, LeftDateDays);
                }

                UpdateButtonPositions();
                InvokeChanged();
            }
        }

        /// <summary>
        /// End date in days count
        /// </summary>
        private int RightDateDays
        {
            get
            {
                return rightDateDays;
            }

            set
            {
                var newValue = Math.Max(MinScaleLimit, Math.Min(MaxScaleLimit, value));
                if (newValue > leftDateDays)
                {
                    rightDateDays = newValue;
                    if (AutoMiddleDate)
                    {
                        CalcMiddleDateDays();
                    }
                    else
                    {
                        MiddleDateDays = Math.Min(MiddleDateDays, RightDateDays);
                    }
                }

                UpdateButtonPositions();
                InvokeChanged();
            }
        }

        /// <summary>
        /// Middle date in days count
        /// </summary>
        private int MiddleDateDays
        {
            get
            {
                return middleDateDays;
            }

            set
            {
                middleDateDays = Math.Max(leftDateDays, Math.Min(rightDateDays, value));
                UpdateButtonPositions();
                InvokeChanged();
            }
        }

        #endregion

        /// <summary>
        /// Invoker
        /// </summary>
        public void InvokeChanged()
        {
            if (IsLockUpdating)
            {
                updatesCount++;
            }
            else
            {
                updatesCount = 0;
                DatesChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Initialize left/right dates
        /// </summary>
        /// <param name="leftDate"> Left date </param>
        /// <param name="rightDate"> Right date </param>
        public void SetupDates(DateTime leftDate, DateTime rightDate)
        {
            leftDateDays = ConvertDateToDaysCount(leftDate);
            rightDateDays = ConvertDateToDaysCount(rightDate);
            CalcMiddleDateDays();
        }

        /// <summary>
        /// Initialize left/right dates
        /// </summary>
        /// <param name="leftDate"> Left date </param>
        /// <param name="rightDate"> Right date </param>
        /// <param name="middleDate"> Middle date </param>
        public void SetupDates(DateTime leftDate, DateTime rightDate, DateTime middleDate)
        {
            leftDateDays = ConvertDateToDaysCount(leftDate);
            rightDateDays = ConvertDateToDaysCount(rightDate);
            middleDateDays = ConvertDateToDaysCount(middleDate);
        }

        /// <summary>
        /// Event on ending update
        /// </summary>
        protected override void OnEndUpdate()
        {
            base.OnEndUpdate();
            if (updatesCount > 0 && !IsLockUpdating)
            {
                InvokeChanged();
            }
        }

        /// <summary>
        /// Event on the component resize
        /// </summary>
        /// <param name="e"> Parameters </param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            SizesChanged();
        }

        /// <summary>
        /// Event on the component redraw
        /// </summary>
        /// <param name="e"> Parameters </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            BeginUpdate();
            try
            {
                const int trackPadHorz = 6;
                var rectangle = new Rectangle(trackLeft - trackPadHorz, componentVerticalCenter - (barHeight / 2), trackWidth + (2 * trackPadHorz), barHeight);
                var skinTrack = new SkinElementInfo(seTrack, rectangle);
                ObjectPainter.DrawObject(new GraphicsCache(e.Graphics), SkinElementPainter.Default, skinTrack);

                var ticksArea = RectangleF.Empty;
                using (var pen = new Pen(seTick.Color.GetForeColor()))
                {
                    foreach (var tick in ticks)
                    {
                        ticksArea = DrawTick(tick, e.Graphics, pen, ticksArea);
                    }
                }

                DrawTrackButtons(e, rectangle);
            }
            finally
            {
                EndUpdate();
            }
        }

        /// <summary>
        /// Draw track buttons
        /// </summary>
        /// <param name="e"> Source </param>
        /// <param name="rectangle"> Drawing rectangle </param>
        private void DrawTrackButtons(PaintEventArgs e, Rectangle rectangle)
        {
            var xFirst = ConvertDateToPosition(leftDateDays);
            var xMiddle = ConvertDateToPosition(middleDateDays);
            var xLast = ConvertDateToPosition(rightDateDays);

            var rect1 = new Rectangle(xFirst, rectangle.Top + 1, xMiddle - xFirst, Math.Max(rectangle.Height - 2, 2));
            var rect2 = new Rectangle(xMiddle, rectangle.Top + 1, xLast - xMiddle, Math.Max(rectangle.Height - 2, 2));
            var rect11 = rect1;
            rect11.Inflate(1, 1);
            var rect12 = rect2;
            rect12.Inflate(1, 1);
            if (rect1.Width > 0)
            {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(rect11, Color.Maroon, Color.OrangeRed, System.Drawing.Drawing2D.LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, rect1);
                }
            }

            if (rect2.Width > 0)
            {
                using (var brush = new System.Drawing.Drawing2D.LinearGradientBrush(rect12, Color.DarkGreen, Color.LimeGreen, System.Drawing.Drawing2D.LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(brush, rect2);
                }
            }
        }

        /// <summary>
        /// Get days count from date
        /// </summary>
        /// <param name="date"> Date </param>
        /// <returns> Days cont </returns>
        private int ConvertDateToDaysCount(DateTime date)
        {
            return (int)(date - DateTime.MinValue).TotalDays;
        }

        /// <summary>
        /// Convert days count to Date
        /// </summary>
        /// <param name="daysCount"> Days count </param>
        /// <returns> Date </returns>
        private DateTime ConvertDaysCountToDate(int daysCount)
        {
            var newDate = DateTime.MinValue.AddDays(daysCount);
            if (newDate.Year == 9999)
            {
                newDate = DateTime.Now.Date;
            }

            return newDate;
        }

        /// <summary>
        /// Convert position to date
        /// </summary>
        /// <param name="position"> Position </param>
        /// <returns> Date in days </returns>
        private int ConvertPositionToDate(int position)
        {
            var koef = 1.0 * (position - trackLeft) / trackWidth;
            return (int)(minScaleLimit + ((maxScaleLimit - minScaleLimit) * koef));
        }

        /// <summary>
        /// Convert date in days to position
        /// </summary>
        /// <param name="days"> Value </param>
        /// <returns> Position </returns>
        private int ConvertDateToPosition(int days)
        {
            var koef = 1.0 * (days - minScaleLimit) / (maxScaleLimit - minScaleLimit);
            return (int)(trackLeft + (trackWidth * koef));
        }

        /// <summary>
        /// Event on size changed
        /// </summary>
        private void SizesChanged()
        {
            componentVerticalCenter = Padding.Top + (ButtonSize.Height / 2);
            trackLeft = Padding.Left + (ButtonSize.Width / 2);
            trackWidth = Width - Padding.Left - Padding.Right - ButtonSize.Width;
            tickTop = componentVerticalCenter + (ButtonSize.Height / 2) + 2;
            labelTop = tickTop + TickLarge + 2;
            UpdateButtonPositions();
        }

        /// <summary>
        /// Update moving button position
        /// </summary>
        private void UpdateButtonPositions()
        {
            if (btnLeftDate == null)
            {
                return;
            }

            btnLeftDate.Top = componentVerticalCenter - (ButtonSize.Height / 2);
            btnMiddle.Top = componentVerticalCenter - (ButtonSize.Height / 2);
            btnRightDate.Top = componentVerticalCenter - (ButtonSize.Height / 2);

            btnLeftDate.Left = ConvertDateToPosition(LeftDateDays) - (ButtonSize.Width / 2);
            btnMiddle.Left = ConvertDateToPosition(MiddleDateDays) - (ButtonSize.Width / 2);
            btnRightDate.Left = ConvertDateToPosition(RightDateDays) - (ButtonSize.Width / 2);
            Invalidate();
        }

        /// <summary>
        /// Event on the component style changed
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="eventArgs"> Parameters </param>
        private void LookAndFeelOnStyleChanged(object sender, EventArgs eventArgs)
        {
            var skinEditors = EditorsSkins.GetSkin(LookAndFeel);

            seTrack = skinEditors[EditorsSkins.SkinTrackBarTrack];
            seTick = skinEditors[EditorsSkins.SkinTrackBarTickLine];
        }

        /// <summary>
        /// Ticks drawing
        /// </summary>
        /// <param name="tick"> Tick info </param>
        /// <param name="graphics"> Drawing context </param>
        /// <param name="pen"> Pen </param>
        /// <param name="drawedArea"> Ticks drawing area </param>
        /// <returns> Tecks drawing rectangle </returns>
        private RectangleF DrawTick(TickStruct tick, Graphics graphics, Pen pen, RectangleF drawedArea)
        {
            var left = ConvertDateToPosition(ConvertDateToDaysCount(tick.Date));
            var tickSize = tick.IsBig ? TickLarge : TickSmall;
            graphics.DrawLine(pen, left, tickTop, left, tickTop + tickSize);
            if (!tick.IsDrawTitle)
            {
                return drawedArea;
            }

            var title = tick.ToString();
            var size = graphics.MeasureString(title, Font);
            var tickPoint = new PointF(tick.Alignment == StringAlignment.Near ? left : tick.Alignment == StringAlignment.Far ? left - size.Width : left - (size.Width / 2), labelTop);
            var area = new RectangleF(tickPoint.X, tickPoint.Y, size.Width, size.Height);

            if (area.IntersectsWith(drawedArea))
            {
                return drawedArea;
            }

            graphics.DrawString(title, Font, CommonBrushes.GetBrushByColor(ForeColor), tickPoint);
            return RectangleF.Union(drawedArea, area);
        }

        /// <summary>
        /// Create ticks
        /// </summary>
        private void CreateTicks()
        {
            ticks = new List<TickStruct>();
            var yearsCount = (MaxDateLimit - MinDateLimit).TotalDays / 365;
            var halfWidth = Width * 0.5f;
            var tickWidthSum = 0;

            ticks.Add(new TickStruct { Date = MinDateLimit, Alignment = StringAlignment.Near, IsDrawTitle = true, Format = "MMM yyyy", IsBig = true });

            DateTime startDate;
            int monthInc;
            string format;

            if (yearsCount > 20)
            {
                startDate = new DateTime(((MinDateLimit.Year / 5) + 1) * 5, 1, 1);
                monthInc = 60;
                format = "yyyy";
            }
            else if (yearsCount > 1.5)
            {
                startDate = new DateTime(MinDateLimit.Year + 1, 1, 1);
                monthInc = 12;
                format = "yyyy";
            }
            else
            {
                startDate = new DateTime(MinDateLimit.Year, MinDateLimit.Month, 1).AddMonths(1);
                monthInc = 1;
                format = "MMM yyyy";
            }

            while (true)
            {
                if (startDate >= MaxDateLimit)
                {
                    break;
                }

                tickWidthSum += (int)(trackWidth / (yearsCount * 12) * monthInc);
                var item = new TickStruct { Date = startDate, Alignment = StringAlignment.Center, IsDrawTitle = false, Format = format, IsBig = false };
                if (tickWidthSum > halfWidth)
                {
                    item.IsBig = true;
                    item.IsDrawTitle = true;
                    tickWidthSum = 0;
                }

                ticks.Add(item);
                startDate = startDate.AddMonths(monthInc);
            }

            ticks.Add(new TickStruct { Date = MaxDateLimit, Alignment = StringAlignment.Far, IsDrawTitle = true, Format = "MMM yyyy", IsBig = true });
        }

        /// <summary>
        /// Event on mouse down
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void dragButton_MouseDown(object sender, MouseEventArgs e)
        {
            var senderButton = (SimpleButton)sender;
            if (e.Button == MouseButtons.Left)
            {
                mouseDownPosition = e.Location;
                movingButton = senderButton;
                movingButton.Capture = true;
            }
        }

        /// <summary>
        /// Event on mouse move
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void dragButton_MouseMove(object sender, MouseEventArgs e)
        {
            var senderButton = sender as SimpleButton;
            if (movingButton != null && senderButton == movingButton)
            {
                var newPosition = movingButton.Left + e.X - mouseDownPosition.X;
                TryMoveButton(newPosition);

                if (ShowTooltip)
                {
                    ShowToolTip(senderButton, e.Location);
                }

                Invalidate();
            }
        }

        /// <summary>
        /// Show ToolTip for the button
        /// </summary>
        /// <param name="sender"> Button </param>
        private void ShowToolTip(SimpleButton sender, Point location)
        {
            var dateInDays = ConvertPositionToDate(sender.Location.X);
            var date = ConvertDaysCountToDate(dateInDays);
            toolTipController.ShowHint(date.ToShortDateString(), sender.PointToScreen(location));
        }

        /// <summary>
        /// Event on mouse up
        /// </summary>
        /// <param name="sender"> Source </param>
        /// <param name="e"> Parameters </param>
        private void dragButton_MouseUp(object sender, MouseEventArgs e)
        {
            movingButton = null;
        }

        /// <summary>
        /// Apply new position to the button
        /// </summary>
        /// <param name="newPos"> X position of the button </param>
        private void TryMoveButton(int newPos)
        {
            var coord = newPos + (ButtonSize.Width / 2);
            var value = ConvertPositionToDate(coord);
            if (movingButton == btnLeftDate)
            {
                LeftDateDays = value;
            }
            else if (movingButton == btnRightDate)
            {
                RightDateDays = value;
            }
            else if (movingButton == btnMiddle)
            {
                MiddleDateDays = value;
            }

            UpdateButtonPositions();
        }

        /// <summary>
        /// Calc auto middle date
        /// </summary>
        private void CalcMiddleDateDays()
        {
            MiddleDateDays = (int)Math.Round(leftDateDays + ((rightDateDays - leftDateDays) / 2d));
        }

        /// <summary>
        /// Struct to store information about ticks
        /// </summary>
        private struct TickStruct
        {
            /// <summary>
            /// Date
            /// </summary>
            public DateTime Date;

            /// <summary>
            /// DateFormat
            /// </summary>
            public string Format;

            /// <summary>
            /// Label alignment
            /// </summary>
            public StringAlignment Alignment;

            /// <summary>
            /// Flag, bigger tickmark
            /// </summary>
            public bool IsBig;

            /// <summary>
            /// Flag, if draw title
            /// </summary>
            public bool IsDrawTitle;

            /// <summary>
            /// Get string representation of the class
            /// </summary>
            /// <returns> String representation </returns>
            public override string ToString()
            {
                return Date.ToString(Format);
            }
        }
    }
}