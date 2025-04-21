using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer
{
    internal class MyModule
    {
        public static bool dragHeader = false;
        public static Point startPoint = new Point(0, 0);

        public static void StartDrag(MouseEventArgs e)
        {
            // Start dragging
            dragHeader = true;
            startPoint = new Point(e.X, e.Y);
        }

        public static void MovedDrag(Form form,MouseEventArgs e)
        {
            // Move the form
            if (dragHeader)
            {
                // Calculate the new position of the form
                int newLeft = form.Left + e.X - startPoint.X;
                int newTop = form.Top + e.Y - startPoint.Y;

                // Apply constraint to prevent the form from moving above the top edge
                form.Left = newLeft;  // No constraints on left
                form.Top = Math.Max(0, newTop);  // Constraint on top (can't move above the screen)

                // UNCOMMENT THE CODE BELOW TO ALLOW CONSTRAINTS IN EVERY EDGE OF THE SCREEN

                // Ensure the form stays within screen bounds
                // Rectangle screenBounds = Screen.FromControl(this).Bounds;
                //int maxX = screenBounds.Right - this.Width;
                //int maxY = screenBounds.Bottom - this.Height;

                //this.Left = Math.Max(screenBounds.Left, Math.Min(newLeft, maxX));
                //this.Top = Math.Max(screenBounds.Top, Math.Min(newTop, maxY));

                //int maxLeft = Screen.PrimaryScreen.WorkingArea.Width - this.Width;
                //int maxTop = Screen.PrimaryScreen.WorkingArea.Height - this.Height;
            }
        }

        public static void EndDrag()
        {
            // Stop dragging
            dragHeader = false;
        }

        public static void SetRoundedCorners(Control ctrl, int topLeftRadius, int topRightRadius, int bottomRightRadius, int bottomLeftRadius)
        {
            GraphicsPath gp = new GraphicsPath();
            Rectangle r = new Rectangle(0, 0, ctrl.Width, ctrl.Height);

            // Top-left corner
            if (topLeftRadius > 0)
            {
                gp.AddArc(r.X, r.Y, topLeftRadius * 2, topLeftRadius * 2, 180, 90);
            }
            else
            {
                gp.AddLine(r.X, r.Y, r.X, r.Y);
            }

            // Top-right corner
            if (topRightRadius > 0)
            {
                gp.AddArc(r.Right - topRightRadius * 2, r.Y, topRightRadius * 2, topRightRadius * 2, 270, 90);
            }
            else
            {
                gp.AddLine(r.Right, r.Y, r.Right, r.Y);
            }

            // Bottom-right corner
            if (bottomRightRadius > 0)
            {
                gp.AddArc(r.Right - bottomRightRadius * 2, r.Bottom - bottomRightRadius * 2, bottomRightRadius * 2, bottomRightRadius * 2, 0, 90);
            }
            else
            {
                gp.AddLine(r.Right, r.Bottom, r.Right, r.Bottom);
            }

            // Bottom-left corner
            if (bottomLeftRadius > 0)
            {
                gp.AddArc(r.X, r.Bottom - bottomLeftRadius * 2, bottomLeftRadius * 2, bottomLeftRadius * 2, 90, 90);
            }
            else
            {
                gp.AddLine(r.X, r.Bottom, r.X, r.Bottom);
            }

            gp.CloseFigure();
            ctrl.Region = new Region(gp);
        }

        public static void MakeCircular(Control ctrl)
        {
            int diameter = Math.Min(ctrl.Width, ctrl.Height);
            Rectangle r = new Rectangle(0, 0, diameter, diameter);
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(r);
            ctrl.Region = new Region(gp);
        }
    }
}
