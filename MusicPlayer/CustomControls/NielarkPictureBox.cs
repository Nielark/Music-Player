using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;

namespace MusicPlayer.CustomControls
{
    public class NielarkPictureBox : PictureBox
    {
        // Fields
        private int borderSize = 0;
        private int borderRadius = 0;
        private Color borderColor = Color.PaleVioletRed;
        private Color hoverBackgroundColor = Color.LightSlateGray;
        private Color hoverBorderColor = Color.LightCoral;
        private Color originalBackgroundColor;
        private Color originalBorderColor;
        private bool isHovering = false; // Track if the mouse is hovering

        // Padding for the image inside the PictureBox
        private Padding imagePadding = new Padding(0);

        // Properties
        [Category("Nielark Code")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Invalidate(); // Redraw the control
            }
        }

        [Category("Nielark Code")]
        public int BorderRadius
        {
            get { return borderRadius; }
            set
            {
                borderRadius = value;
                this.Invalidate(); // Redraw the control
            }
        }

        [Category("Nielark Code")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                this.Invalidate(); // Redraw the control
            }
        }

        [Category("Nielark Code")]
        public Color HoverBackgroundColor
        {
            get { return hoverBackgroundColor; }
            set { hoverBackgroundColor = value; }
        }

        [Category("Nielark Code")]
        public Color HoverBorderColor
        {
            get { return hoverBorderColor; }
            set { hoverBorderColor = value; }
        }

        [Category("Nielark Code")]
        public Padding ImagePadding
        {
            get { return imagePadding; }
            set
            {
                imagePadding = value;
                this.Invalidate(); // Redraw the control
            }
        }

        // Constructor
        public NielarkPictureBox()
        {
            this.SizeMode = PictureBoxSizeMode.StretchImage; // Adjust size mode as needed
            this.DoubleBuffered = true; // Enable double buffering
            this.Resize += PictureBox_Resize;

            // Subscribe to hover events
            this.MouseEnter += OnMouseEnter;
            this.MouseLeave += OnMouseLeave;

            // Optimize painting for better performance
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
            // Set the initial background color in design mode
            if (this.DesignMode)
            {
                this.BackColor = Color.Transparent;
            }
        }

        // Methods
        private GraphicsPath GetFigurePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            float curveSize = radius * 2F;

            path.StartFigure();
            path.AddArc(rect.X, rect.Y, curveSize, curveSize, 180, 90);
            path.AddArc(rect.Right - curveSize, rect.Y, curveSize, curveSize, 270, 90);
            path.AddArc(rect.Right - curveSize, rect.Bottom - curveSize, curveSize, curveSize, 0, 90);
            path.AddArc(rect.X, rect.Bottom - curveSize, curveSize, curveSize, 90, 90);
            path.CloseFigure();
            return path;
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);

            Rectangle rectSurface = this.ClientRectangle;
            Rectangle rectBorder = Rectangle.Inflate(rectSurface, -borderSize, -borderSize);

            // Set background color according to hover state
            Color backgroundColor = isHovering ? hoverBackgroundColor : this.BackColor;

            // Fill the control background
            using (SolidBrush brush = new SolidBrush(backgroundColor))
            {
                pevent.Graphics.FillRectangle(brush, rectSurface);
            }

            if (borderRadius > 2) // Rounded PictureBox
            {
                using (GraphicsPath pathSurface = GetFigurePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetFigurePath(rectBorder, borderRadius - borderSize))
                using (Pen penSurface = new Pen(this.Parent?.BackColor ?? Color.White, 0)) // Use parent back color for surface
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    this.Region = new Region(pathSurface);

                    // Draw surface border for HD result
                    pevent.Graphics.DrawPath(penSurface, pathSurface);

                    // Draw control border
                    if (borderSize >= 1)
                        pevent.Graphics.DrawPath(penBorder, pathBorder);
                }
            }
            else // Normal PictureBox
            {
                this.Region = new Region(rectSurface);
                if (borderSize >= 1)
                {
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }
            }

            // Draw the image on top, if it exists, applying padding
            if (this.Image != null)
            {
                Rectangle innerImageRect = new Rectangle(
                    borderSize + imagePadding.Left,
                    borderSize + imagePadding.Top,
                    this.ClientRectangle.Width - 2 * borderSize - imagePadding.Horizontal,
                    this.ClientRectangle.Height - 2 * borderSize - imagePadding.Vertical);

                pevent.Graphics.DrawImage(this.Image, innerImageRect);
            }
        }

        private void PictureBox_Resize(object sender, EventArgs e)
        {
            if (borderRadius > this.Height)
                borderRadius = this.Height; // Ensure border radius doesn't exceed height
            this.Invalidate();
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            isHovering = true;  // Set hover state
            originalBackgroundColor = this.BackColor;
            originalBorderColor = this.borderColor; // Store original border color
            this.borderColor = hoverBorderColor;    // Change to hover border color
            this.Invalidate(); // Redraw the control
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            isHovering = false;  // Reset hover state
            this.BackColor = originalBackgroundColor;
            this.borderColor = originalBorderColor; // Restore original border color
            this.Invalidate(); // Redraw the control
        }
    }
}
