using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SharpPreloaders.FrameBuilders
{
    /// <summary>
    /// This should make a simple set of images where when 
    /// animated together it looks like a ball is bouncing up and down
    /// </summary>
    class SimpleBallBounceFrameBuilder : FrameBuilder
    {
        public override List<Image> GetImages(Size imageSize, Color backColor, ImageBoundry boundry)
        {
            this.imageSize = imageSize;
            this.backColor = backColor;
            this.boundry = boundry;

            /*
             * The ball should start at center screen. Then it should go up for 10 frames, 
             * then back down for 10 frames
             * The ball should flatten and get more oblong when it hits the ground
             */
            
            float circleSize = boundry.ImageSize.Width * .05f;
            
            float startPointY = boundry.Center.Y - (boundry.ImageSize.Height / 2);
            float x = (boundry.Center.X) - (circleSize / 2f);
            float midScreenY = (boundry.Center.Y) - (circleSize / 2f);
            float increment = ((midScreenY - startPointY) - (circleSize * .5f)) / 10;
            startPointY += increment;

            float width = circleSize;
            float height = circleSize;

            images = new List<Image>();
            //Add starting ball
            AddImage(x, startPointY, width, height);

            //Add images going down
            float y = startPointY;
            for (int i = 0; i < 9; i++)
            {
                y += increment;
                AddImage(x, y, width, height);
            }

            /*
             * Add Bouncing images
             * When the ball comes in contact with the line is should start to squish/flatten slightly to
             * show that pressure is being put on the ball as it's velocity and momentum try to push it further down. 
             * The ball should unsquish as it recoils back up.
             * 
             * Without the squish the animiation will just show the ball going straight up and down
             */
            //Squish Down
            float SquishWidth = width;
            float squishHeight = height;
            for (int i = 0; i < 2; i++)
            {
                squishHeight *= .85f;
                SquishWidth *= 1.15f;
                x -= .1f;
                y += (increment * .25f);
                AddImage(x, y, SquishWidth, squishHeight);
            }

            //Unsquish
            for (int i = 0; i < 2; i++)
            {
                squishHeight *= 1.15f;
                SquishWidth *= .85f;
                x += .1f;
                y -= increment;
                AddImage(x, y, SquishWidth, squishHeight);
            }
            //Add Images Going up
            for (int i = 0; i < 8; i++)
            {
                y -= increment;
                AddImage(x, y, width, height);
            }

            return images;
        }


        private void AddImage(float x, float y, float width, float height)
        {
            Bitmap bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            
            using(Graphics graphics = Graphics.FromImage(bitmap))
            {
                using (SolidBrush brush = new SolidBrush(backColor))
                {
                    graphics.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);
                }
                using (Pen pen = new Pen(Color.Black, .1f))
                {
                    float lineY = boundry.Center.Y;
                    float startingX = (boundry.Center.X - (boundry.ImageSize.Width / 2)) + 
                        (boundry.ImageSize.Width * .25F);
                    float lineEnd = (boundry.Center.X - (boundry.ImageSize.Width / 2)) +
                        (boundry.ImageSize.Width * .75F);
                    graphics.DrawLine(pen, startingX, lineY, lineEnd, lineY);
                }
                using(Pen pen = new Pen(Color.Blue, .1f))
                {
                    graphics.DrawEllipse(pen, x, y, width, height);
                }
            }
            images.Add(bitmap);
        }



    }
}
