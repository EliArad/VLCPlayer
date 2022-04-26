using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Windows.Forms;

namespace PulseButton
{
    /// <summary>
    /// PulseButton control
    /// <remarks>By Niel M. Thomas 2009</remarks>
    /// </summary>
    public partial class PulseButton : Button
    {
        
        private readonly Timer pulseTimer;
        private RectangleF[] pulses;
        private RectangleF centerRect;
        private Color[] pulseColors;
        private int pulseWidth;
        private bool mouseOver;
        private bool pressed;
        private float pulseSpeed;

        char [] m_stoke = new char[5];

        public enum Shape
        {
            Round,
            Rectangle
        }
        

      

        /// <summary>
        /// Gets or sets the top button color.
        /// </summary>
        /// <value>The top button color.</value>
        [Browsable(true), DefaultValue(typeof(Color), "CornflowerBlue")]
        [Category("Appearance")]
        public Color ButtonColorTop { get; set; }

        /// <summary>
        /// Gets or sets the bottom button color.
        /// </summary>
        /// <value>The bottom button color.</value>
        [Browsable(true), DefaultValue(typeof(Color), "DodgerBlue")]
        [Category("Appearance")]
        public Color ButtonColorBottom { get; set; }

        /// <summary>
        /// Gets or sets the color of the pulse.
        /// </summary>
        /// <value>The color of the pulse.</value>
        [Browsable(true), DefaultValue(typeof(Color), "Black")]
        [Category("Appearance")]
        public Color PulseColor { get; set; }

        /// <summary>
        /// Gets or sets the type of the shape.
        /// </summary>
        /// <value>The type of the shape.</value>
        [Browsable(true), DefaultValue(typeof(Shape), "Round")]
        [Category("Appearance")]
        public Shape ShapeType { get; set; }

        /// <summary>
        /// Gets or sets the corner radius.
        /// </summary>
        /// <value>The corner radius.</value>
        [Browsable(true), DefaultValue(10)]
        [Category("Appearance")]
        public int CornerRadius { get; set; }

        /// <summary>
        /// Gets or sets the color of the focus.
        /// </summary>
        /// <value>The color of the focus.</value>
        [Browsable(true), DefaultValue(typeof(Color), "Orange")]
        [Category("Appearance")]
        public Color FocusColor { get; set; }

        /// <summary>
        /// Gets or sets the foreground color of the control.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The foreground <see cref="T:System.Drawing.Color"/> of the control. The default is the value of the <see cref="P:System.Windows.Forms.Control.DefaultForeColor"/> property.
        /// </returns>
        /// <PermissionSet>
        /// 	<IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Unrestricted="true"/>
        /// </PermissionSet>
        [Browsable(true), DefaultValue(typeof(Color), "White")]
        [Category("Appearance")]
        public new Color ForeColor
        {
            get { return base.ForeColor; } 
            set{ base.ForeColor = value; }
        }

        /// <summary>
        /// Gets or sets the number of pulses.
        /// </summary>
        /// <value>The number of pulses.</value>
        [Browsable(true), DefaultValue(3)]
        [Category("Appearance")]
        public int NumberOfPulses
        {
            get { return pulses.Length; }
            set
            {
                if (value <= 0) return;
                pulses = new RectangleF[value];
                pulseColors = new Color[value];
                ArrangePulses();
            }
        }

        /// <summary>
        /// Gets or sets the width of the pulse.
        /// </summary>
        /// <value>The width of the pulse.</value>
        [Browsable(true), DefaultValue(10)]
        [Category("Appearance")]
        public int PulseWidth
        {
            get { return pulseWidth; }
            set { pulseWidth = value; ArrangePulses(); }
        }

        /// <summary>
        /// Gets or sets the wave speed.
        /// </summary>
        /// <value>The speed of the pulses.</value>
        [Browsable(true), DefaultValue(typeof(float), "0.3f")]
        [Category("Appearance")]
        public float PulseSpeed
        {
            get { return pulseSpeed; }
            set
            {
                if (value <= 0) return;
                pulseSpeed = value;
            }
        }

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>The interval.</value>
        [Browsable(false), DefaultValue(50)]
        public int Interval
        {
            get { return pulseTimer.Interval; }
            set { pulseTimer.Interval = value; }
        }
         
         
        /// <summary>
        /// Initializes a new instance of the <see cref="PulseButton"/> class.
        /// </summary>
        public PulseButton()
        {
            // Control styles
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.UserPaint, true);
            InitializeComponent();
            // Layout & initialization
            SuspendLayout();
            pulseWidth = 10;
            PulseSpeed = .3f;
            ButtonColorTop = Color.CornflowerBlue;
            ButtonColorBottom = Color.DodgerBlue;
            FocusColor = Color.Orange;
            PulseColor = Color.Black;
            ShapeType = Shape.Round;
            CornerRadius = 10;
            Image = null;
            base.TextAlign = ContentAlignment.MiddleCenter;
            Size = new Size(40, 40);
            // Timer
            pulseTimer = new Timer { Interval = 50 };
            pulseTimer.Tick += PulseTimerTick;
            pulses = new RectangleF[3];
            pulseColors = new Color[3];
            ArrangePulses();
            pulseTimer.Enabled = true;
            ResumeLayout(true);
        }
         
 
        /// <summary>
        /// Handles the pulse timer tick.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void PulseTimerTick(object sender, EventArgs e)
        {
            pulseTimer.Enabled = false;
            InflatePulses();
            Invalidate();
            pulseTimer.Enabled = true;
        }
         

        #region - Mouse -

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button != MouseButtons.Left) return;
            pressed = false;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.MouseDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            pressed = true;
        }

        /// <summary>
        /// Raises the <see cref="M:System.Windows.Forms.Control.OnMouseMove(System.Windows.Forms.MouseEventArgs)"/> event.
        /// </summary>
        /// <param name="mevent">A <see cref="T:System.Windows.Forms.MouseEventArgs"/> that contains the event data.</param>
        protected override void OnMouseMove(MouseEventArgs mevent)
        {
            base.OnMouseMove(mevent);
            mouseOver = centerRect.Contains(mevent.Location);
        }

        /// <summary>
        /// Raises the <see cref="Control.OnMouseLeave"/> event.
        /// </summary>
        /// <param name="e">A <see cref="EventArgs"/> that contains the event data.</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mouseOver = false;
            pressed = false;
        }

        #endregion

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.EnabledChanged"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            pulseTimer.Enabled = Enabled;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Resize"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (pulses == null || pulses.Length == 0) return;
            ArrangePulses();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            base.OnPaintBackground(e);
            // Set Graphics interpolation and smoothing
            Graphics g = e.Graphics;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw pulses
            DrawPulses(g);

            if (centerRect.IsEmpty) return;
            // Draw center
            DrawCenter(g);

            // Draw border
            DrawBorder(g);
            // Image
            if (Image != null)
                g.DrawImage(Image, centerRect);

            // Draw highlight
            if (mouseOver)
                DrawHighLight(g);
            // Reflex
            if (!pressed) DrawReflex(g);
            // Text
            DrawText(g);
        }
   
        /// <summary>
        /// Draws the border.
        /// </summary>
        /// <param name="g">The graphics object</param>
        protected virtual void DrawBorder(Graphics g)
        {
            using (var pen = new Pen(!Focused ? Color.FromArgb(60, Color.Black) : FocusColor, 2))
                PaintShape(g, pen, centerRect);
        }

        /// <summary>
        /// Draws the center.
        /// </summary>
        /// <param name="g">The graphics object</param>
        protected virtual void DrawCenter(Graphics g)
        {
            if (Enabled)
            {
                using (var lgb = new LinearGradientBrush(centerRect, ButtonColorTop, ButtonColorBottom,
                                                      LinearGradientMode.Vertical))
                {
                    PaintShape(g, lgb, centerRect);
                }
            }
            else
            {
                using (var lgb = new SolidBrush(Color.Gray))
                    PaintShape(g, lgb, centerRect);

            }
        }

        /// <summary>
        /// Draws the pulses.
        /// </summary>
        /// <param name="g">The graphics object</param>
        protected virtual void DrawPulses(Graphics g)
        {
            if (!Enabled) return;
            for (var i = 0; i < pulses.Length; i++)
            {
                using (var sb = new SolidBrush(pulseColors[i]))
                {
                    PaintShape(g, sb, pulses[i]);
                }
            }
        }

        /// <summary>
        /// Draws the text.
        /// </summary>
        /// <param name="g">The graphics object</param>
        protected virtual void DrawText(Graphics g)
        {
            var format = new StringFormat(StringFormat.GenericDefault) {Trimming = StringTrimming.EllipsisCharacter};
            format.FormatFlags |= StringFormatFlags.MeasureTrailingSpaces;
            format.FormatFlags ^= StringFormatFlags.LineLimit;
            format.HotkeyPrefix = HotkeyPrefix.Show;
            SizeF size = g.MeasureString(Text, Font, new SizeF(centerRect.Width, centerRect.Height), format);
            RectangleF textRect = GetAlignPlacement(TextAlign, centerRect, size);
            using (var sb = new SolidBrush(ForeColor))
                g.DrawString(Text, Font, sb, textRect, format);
        }

        /// <summary>
        /// Draws the reflex.
        /// </summary>
        /// <param name="g">The graphics object</param>
        protected virtual void DrawReflex(Graphics g)
        {
            using (var path = new GraphicsPath())
            {
                RectangleF rect = centerRect;
                rect.Height = rect.Height / 2;
                if (ShapeType == Shape.Round)
                {
                    path.AddArc(centerRect, -180, 180);
                    RectangleF reflexRectangle = rect;
                    reflexRectangle.Offset(0, rect.Height);
                    reflexRectangle.Height /= 6;
                    path.AddArc(reflexRectangle, 0, 180);
                    path.CloseFigure();
                }
                else
                {
                    rect.Height += rect.Height / 6;
                    path.AddRectangle(rect);
                }
                RectangleF area = path.GetBounds();
                using (var lgb = new LinearGradientBrush(area, Color.FromArgb(30, Color.White),
                                                      Color.FromArgb(60, Color.White), -90))
                {
                    g.FillPath(lgb, path);
                }
            }
        }

        /// <summary>
        /// Draws the high light.
        /// </summary>
        /// <param name="g">The graphics object</param>
        protected virtual void DrawHighLight(Graphics g)
        {
            RectangleF highlightRect = centerRect;
            highlightRect.Inflate(-2, -2);
            using (var pen = new Pen(Color.FromArgb(60, Color.White), 4))
            {
                if (ShapeType == Shape.Round)
                    g.DrawEllipse(pen, highlightRect);
                else
                    g.DrawPath(pen, GetRoundRect(g, highlightRect, CornerRadius));
            }
        }

        /// <summary>
        /// Paints the shape.
        /// </summary>
        /// <param name="g">The graphics object</param>
        /// <param name="p">The pen</param>
        /// <param name="rectangle">The rectangle.</param>
        protected virtual void PaintShape(Graphics g, Pen p, RectangleF rectangle)
        {
            if (ShapeType == Shape.Round)
                g.DrawEllipse(p, rectangle);
            else
                using (var path = GetRoundRect(g, rectangle, CornerRadius))
                    g.DrawPath(p, path);
        }

        /// <summary>
        /// Paints the shape.
        /// </summary>
        /// <param name="g">The graphics object</param>
        /// <param name="b">The brush</param>
        /// <param name="rectangle">The rectangle.</param>
        protected virtual void PaintShape(Graphics g, Brush b, RectangleF rectangle)
        {
            if (ShapeType == Shape.Round)
                g.FillEllipse(b, rectangle);
            else
                using (var path = GetRoundRect(g, rectangle, CornerRadius))
                    g.FillPath(b, path);
        }
   
        /// <summary>
        /// Gets a path of a rectangle with round corners.
        /// </summary>
        /// <param name="g">The graphics object</param>
        /// <param name="rect">The rectangle</param>
        /// <param name="radius">The corner radius</param>
        /// <returns></returns>
        public static GraphicsPath GetRoundRect(Graphics g, RectangleF rect, float radius)
        {
            var gp = new GraphicsPath();
            var diameter = radius * 2;
            gp.AddArc(rect.X + rect.Width - diameter, rect.Y, diameter, diameter, 270, 90);
            gp.AddArc(rect.X + rect.Width - diameter, rect.Y + rect.Height - diameter, diameter, diameter, 0, 90);
            gp.AddArc(rect.X, rect.Y + rect.Height - diameter, diameter, diameter, 90, 90);
            gp.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            gp.CloseFigure();
            return gp;
        }

        /// <summary>
        /// Gets the placement.
        /// </summary>
        /// <param name="align">The alignment of the element</param>
        /// <param name="rect">A retangle</param>
        /// <param name="element">The element to be placed</param>
        /// <returns></returns>
        public static RectangleF GetAlignPlacement(ContentAlignment align, RectangleF rect, SizeF element)
        {
            // Left & Top (default)
            float lft = rect.Left;
            float top = rect.Y;
            // Right
            if ((align & (ContentAlignment.BottomRight | ContentAlignment.MiddleRight | ContentAlignment.TopRight)) != 0)
                lft = rect.Right - element.Width;
            // Center
            else if ((align & (ContentAlignment.BottomCenter | ContentAlignment.MiddleCenter | ContentAlignment.TopCenter)) != 0)
                lft = (rect.Width / 2) - (element.Width / 2) + rect.Left;
            // Bottom
            if ((align & (ContentAlignment.BottomCenter | ContentAlignment.BottomLeft | ContentAlignment.BottomRight)) != 0)
                top = rect.Bottom - element.Height;
            // Middle
            else if ((align & (ContentAlignment.MiddleCenter | ContentAlignment.MiddleLeft | ContentAlignment.MiddleRight)) != 0)
                top = (rect.Height / 2) - (element.Height / 2) + rect.Y;

            return new RectangleF(lft, top, element.Width, element.Height);
        }
    
        /// <summary>
        /// Arranges the pulses.
        /// </summary>
        private void ArrangePulses()
        {
            centerRect = new RectangleF(pulseWidth, pulseWidth, Width - 2 * pulseWidth, Height - 2 * pulseWidth);
            for (var i = 1; i <= pulses.Length; i++)
            {
                pulses[i - 1] = new RectangleF(
                    pulseWidth * i / (float)pulses.Length,
                    pulseWidth * i / (float)pulses.Length,
                    Width - 2 * pulseWidth * i / pulses.Length,
                    Height - 2 * pulseWidth * i / pulses.Length
                    );
                pulseColors[i - 1] = Color.FromArgb((int)(Math.Min(pulses[i - 1].X * 255 / pulseWidth, 255)), Color.White);
            }
        }

        /// <summary>
        /// Inflates the pulses.
        /// </summary>
        private void InflatePulses()
        {
            for (var i = 0; i < pulses.Length; i++)
            {
                pulses[i].Inflate(PulseSpeed, PulseSpeed);
                if (pulses[i].Width > Width || pulses[i].Height > Height || pulses[i].X < 0 || pulses[i].Y < 0)
                    pulses[i] = new RectangleF(pulseWidth, pulseWidth, Width - 2 * pulseWidth, Height - 2 * pulseWidth);
                pulseColors[i] = Color.FromArgb((int)(Math.Min(pulses[i].X * 255 / pulseWidth, 255)), PulseColor);
            }
        }
    }
}
