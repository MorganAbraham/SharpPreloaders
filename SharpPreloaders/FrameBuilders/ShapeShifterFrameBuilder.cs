using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SharpPreloaders.FrameBuilders
{
    class ShapeShifterFrameBuilder : FrameBuilder
    {
        public override List<Image> GetImages(Size imageSize, Color backColor, ImageBoundry boundry)
        {
            images = new List<Image>();
            this.imageSize = imageSize;

            float centerX = imageSize.Width / 2;
            float centerY = imageSize.Height / 2;

            //Start with all points at the center
            PointF point1 = new PointF(centerX, centerY);
            PointF point2 = new PointF(centerX, centerY);
            PointF point3 = new PointF(centerX, centerY);
            PointF point4 = new PointF(centerX, centerY);

            for(int i = 0; i < 10; i++)
            {
                AddImage(point1, point2, point3, point4);
            }
            //Fan out into a square

            point1.X -= 30f;
            point1.Y -= 30f;
            point2.X += 30f;
            point2.Y -= 30f;
            point3.X += 30f;
            point3.Y += 30f;
            point4.X -= 30f;
            point4.Y += 30f;

            for (int i = 0; i < 10; i++)
            {
                AddImage(point1, point2, point3, point4);
            }
            //Round out into a circle

            return images;
        }

        private void AddImage(PointF point1, PointF point2, PointF point3, PointF point4)
        {
            Bitmap bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            PointF[] points = new PointF[] { point1, point2, point3, point4 };
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    graphics.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);
                }

                using (SolidBrush brush = new SolidBrush(Color.Black))
                {
                    foreach (PointF point in points)
                    {
                        DrawPoint(graphics, brush, point);
                    }
                }

                using (Pen pen = new Pen(Color.Black, .1f))
                {
                    graphics.DrawLine(pen, point1, point2);
                    graphics.DrawLine(pen, point2, point3);
                    graphics.DrawLine(pen, point3, point4);
                    graphics.DrawLine(pen, point4, point1);
                }
            }

            images.Add(bitmap);
        }

        private void DrawPoint(Graphics graphics, Brush brush, PointF point)
        {
            
            graphics.FillRectangle(brush, point.X, point.Y, 1, 1);
        }
    }
}
