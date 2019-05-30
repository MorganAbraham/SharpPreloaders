using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace SharpPreloaders.FrameBuilders
{
    /// <summary>
    /// This should make a set of images where a set of circles start in a line and then end up in a square shape then shift back again
    /// </summary>
    class BallLineToBoxFrameBuilder : FrameBuilder 
    {
        public override List<Image> GetImages(Size imageSize)
        {
            this.imageSize = imageSize;
            images = new List<Image>();
            /*
             * The ball should start at center screen. Then it should go up for 10 frames, then back down for 10 frames
             * The ball should flatten and get more oblong when it hits the ground
             */

            float circleSize = imageSize.Width * .05f;
            float midScreenX = imageSize.Width / 2;
            float lineY = imageSize.Height * .5F;
            int ballCount = 4;
            float lineSize = imageSize.Width * .8f;

            int changeFrameCount = 5;
            

            float startingX = imageSize.Width - lineSize;
            startingX -= (circleSize / 2);
            float distance = lineSize / ballCount;

            float ballStartY = lineY - (circleSize / 2f);
            /*
             * Start all of the balls in 1 line
             */
            #region Create Ball Objects
            BallInfo ball1 = new BallInfo()
            {
                X = startingX,
                Y = ballStartY,
                Height = circleSize,
                Width = circleSize,
                BallColor = Color.Red
            };

            BallInfo ball2 = new BallInfo()
            {
                X = ball1.X + distance,
                Y = ballStartY,
                Height = circleSize,
                Width = circleSize,
                BallColor = Color.Blue
            };

            BallInfo ball3 = new BallInfo()
            {
                X = ball2.X + distance,
                Y = ballStartY,
                Height = circleSize,
                Width = circleSize,
                BallColor = Color.Green
            };

            BallInfo ball4 = new BallInfo()
            {
                X = ball3.X + distance,
                Y = ballStartY,
                Height = circleSize,
                Width = circleSize,
                BallColor = Color.Yellow
            };
            #endregion

            for (int i = 0; i < changeFrameCount * 3; i++)
            {
                AddImage(ball1, ball2, ball3, ball4);
            }

            //Twitch slightly before moving
            for(int i = 0; i < 2; i++)
            {
                ball1.X += .5f;
                ball1.Y -= .5f;

                ball2.X -= .5f;
                ball2.Y += .5f;

                ball3.X += .5f;
                ball3.Y -= .5f;

                ball4.X -= .5f;
                ball4.Y += .5f;

                AddImage(ball1, ball2, ball3, ball4);

                ball1.X -= .5f;
                ball1.Y += .5f;

                ball2.X += .5f;
                ball2.Y -= .5f;

                ball3.X -= .5f;
                ball3.Y += .5f;

                ball4.X += .5f;
                ball4.Y -= .5f;

                AddImage(ball1, ball2, ball3, ball4);
            }

            //Final Position
            //Ball 1 and 3 should be right above the center line and side by side
            //Ball 2 should not move
            //Ball 4 should be right by Ball 2, directly under ball 3

            float ball1XFinal = startingX + distance;
            float ball1YFinal = lineY - circleSize * 2;
            float ball3XFinal = startingX + distance + (distance / 2);
            float ball3YFinal = lineY - circleSize * 2;
            float ball4XFinal = startingX + distance + (distance / 2);
            float ball4YFinal = ballStartY;

            float xChange1 = (ball1XFinal - ball1.X) / changeFrameCount;
            float yChange1 = (ball1.Y - ball1YFinal) / changeFrameCount;
            float xChange3 = (ball3XFinal - ball3.X) / changeFrameCount;
            float yChange3 = (ball3.Y - ball3YFinal) / changeFrameCount;
            float xChange4 = (ball4XFinal - ball4.X) / changeFrameCount;
            float yChange4 = (ball4.Y - ball4YFinal) / changeFrameCount;


            for (int i = 0; i < changeFrameCount; i++)
            {
                ball1.X += xChange1;
                ball1.Y -= yChange1;

                ball3.X += xChange3;
                ball3.Y -= yChange3;

                ball4.X += xChange4;
                ball4.Y -= yChange4;
                AddImage(ball1, ball2, ball3, ball4);
            }

            //Hold here
            for (int i = 0; i < changeFrameCount * 3; i++)
            {
                AddImage(ball1, ball2, ball3, ball4);
            }

            //Twitch slightly before moving
            for (int i = 0; i < 2; i++)
            {
                ball1.X += .5f;
                ball1.Y -= .5f;

                ball2.X -= .5f;
                ball2.Y += .5f;

                ball3.X += .5f;
                ball3.Y -= .5f;

                ball4.X -= .5f;
                ball4.Y += .5f;

                AddImage(ball1, ball2, ball3, ball4);

                ball1.X -= .5f;
                ball1.Y += .5f;

                ball2.X += .5f;
                ball2.Y -= .5f;

                ball3.X -= .5f;
                ball3.Y += .5f;

                ball4.X += .5f;
                ball4.Y -= .5f;

                AddImage(ball1, ball2, ball3, ball4);
            }

            //Change back to a line
            for (int i = 0; i < changeFrameCount; i++)
            {
                ball1.X -= xChange1;
                ball1.Y += yChange1;

                ball3.X -= xChange3;
                ball3.Y += yChange3;

                ball4.X -= xChange4;
                ball4.Y += yChange4;
                AddImage(ball1, ball2, ball3, ball4);
            }
            return images;
        }

        private void AddImage(BallInfo ball1, BallInfo ball2, BallInfo ball3, BallInfo ball4)
        {
            Bitmap bitmap = new Bitmap(imageSize.Width, imageSize.Height);
            using(Graphics graphics = Graphics.FromImage(bitmap))
            {
                using(SolidBrush brush = new SolidBrush(Color.FromArgb(47, 56, 71)))
                {
                    graphics.FillRectangle(brush, 0, 0, bitmap.Width, bitmap.Height);
                }
                using(Pen pen = new Pen(Color.Black, 5f))
                {
                    pen.Color = ball1.BallColor;
                    graphics.DrawEllipse(pen, ball1.X, ball1.Y, ball1.Width, ball1.Height);

                    pen.Color = ball2.BallColor;
                    graphics.DrawEllipse(pen, ball2.X, ball2.Y, ball2.Width, ball2.Height);

                    pen.Color = ball3.BallColor;
                    graphics.DrawEllipse(pen, ball3.X, ball3.Y, ball3.Width, ball3.Height);

                    pen.Color = ball4.BallColor;
                    graphics.DrawEllipse(pen, ball4.X, ball4.Y, ball4.Width, ball4.Height);
                }
            }
            images.Add(bitmap);
        }

        struct BallInfo
        {
            public float X { get; set; }
            public float Y { get; set; }
            public float Width { get; set; }
            public float Height { get; set; }
            public Color BallColor { get; set; }
        }
    }
}
