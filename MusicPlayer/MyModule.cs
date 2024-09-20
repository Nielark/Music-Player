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
