using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpPreloaders.FrameBuilders;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SharpPreloaders
{
    class SpinningGearsFrameBuilder : FrameBuilder
    {
        public override List<Image> GetImages(System.Drawing.Size imageSize, System.Drawing.Color backColor, ImageBoundry boundry)
        {
            this.imageSize = imageSize;
            this.backColor = backColor;
            this.boundry = boundry;
            images = new List<Image>();

            for (int i = 0; i < 180; i++)
            {
                AddImage(i);
            }
            return images;
        }

        private void AddImage(int spinAmount)
        {
            Bitmap bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    graphics.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);
                }

                const float radius = 50;
                const float toothLength = 10;
                float x = boundry.Center.X - radius - toothLength - 1;
                float y = boundry.Center.Y - radius - toothLength - 1;
                
                DrawGear(graphics, Brushes.Black, Brushes.LightBlue, Pens.Blue, new PointF(x, y), radius, toothLength, 10, 5, true, spinAmount);

                x += 2 * radius + toothLength + 2;
                DrawGear(graphics, Brushes.Black, Brushes.LightGreen,Pens.Green, new PointF(x, y),radius, toothLength, 10, 5, false, -spinAmount);

                y += 2 * radius + toothLength + 2;
                DrawGear(graphics, Brushes.Black, Brushes.Pink,Pens.Red, new PointF(x, y), radius, toothLength, 10, 5, true, spinAmount);


            }
            images.Add(bitmap);
        }

        /*
         * The methodology used to draw gears was adapted from Rod Stephens here:
         * http://csharphelper.com/blog/2016/10/draw-gears-in-c/
         * 
         * I added a spinning mechanism to his existing drawing
         */
        private void DrawGear(Graphics graphics, Brush axleBrush,
            Brush gearBrush, Pen gearPen, PointF center,
            float radius, float toothlength, int teethCount,
            float axleRadius, bool startWithTooth, int spinAmount)
        {
            float dtheta = (float)(Math.PI / teethCount);
            // dtheta in degrees.
            float dthetaDegrees = (float)(dtheta * 180 / Math.PI);

            const float chamfer = 2;
            float toothWidth = radius * dtheta - chamfer;
            float alpha = toothWidth / (radius + toothlength);
            float alphaDegrees = (float)(alpha * 180 / Math.PI);
            float phi = (dtheta - alpha) / 2;

            // Set theta for the beginning of the first tooth.
            float theta = startWithTooth ?  theta = dtheta / 2 : -dtheta / 2;

            // Make rectangles to represent
            // the gear's inner and outer arcs.
            RectangleF inner_rect = new RectangleF(center.X - radius, center.Y - radius, 2 * radius, 2 * radius);
            RectangleF outer_rect = new RectangleF(center.X - radius - toothlength, center.Y - radius - toothlength,
                2 * (radius + toothlength), 2 * (radius + toothlength));

            // Make a path representing the gear.
            using(GraphicsPath path = new GraphicsPath())
            {
                for (int i = 0; i < teethCount; i++)
                {
                    // Move across the gap between teeth.
                    float degrees = (float)(theta * 180 / Math.PI) + spinAmount;
                    path.AddArc(inner_rect, degrees, dthetaDegrees);
                    theta += dtheta;

                    // Move across the tooth's outer edge.
                    degrees = (float)((theta + phi) * 180 / Math.PI) + spinAmount;
                    path.AddArc(outer_rect, degrees, alphaDegrees);
                    theta += dtheta;
                }

            // Draw the gear.
            graphics.FillPath(gearBrush, path);
            graphics.DrawPath(gearPen, path);
            graphics.FillEllipse(axleBrush,
                center.X - axleRadius, center.Y - axleRadius,
                2 * axleRadius, 2 * axleRadius);
            }
        }
    }
}
