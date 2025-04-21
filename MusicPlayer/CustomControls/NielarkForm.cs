using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.ComponentModel;

namespace MusicPlayer.CustomControls
{
    [ToolboxItem(true)] // Allows the form to be added to the toolbox
    public class NielarkForm : Form
    {
        // Fields
        private int borderSize = 2;
        private int borderRadius = 20;
        private Color borderColor = Color.PaleVioletRed;

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

        // Constructor
        public NielarkForm()
        {
            this.FormBorderStyle = FormBorderStyle.None; // Remove default border
            this.DoubleBuffered = true; // Enable double buffering
        }

        // Method to create a rounded rectangle path
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

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Create a rounded rectangle path
            using (GraphicsPath path = GetFigurePath(this.ClientRectangle, borderRadius))
            {
                this.Region = new Region(path); // Set the region for the form

                // Draw the border
                using (Pen pen = new Pen(borderColor, borderSize))
                {
                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias; // Smooth edges
                    e.Graphics.DrawPath(pen, path); // Draw the border path
                }
            }
        }
    }
}
