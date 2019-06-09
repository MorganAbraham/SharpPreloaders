using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Drawing;
using SharpPreloaders.FrameBuilders;
using System.Reflection;

namespace SharpPreloaders
{
    public class Preloader
    {
        private Control control;
        private AnimationStyle style;
        private AnimationSpeed speed;
        private Position position;

        private System.Timers.Timer timer;

        private List<Image> frames;
        private int currentImage = 0;

        private Image originalBackgroundImage;

        public enum Position
        {
            TopLeft,
            MiddleLeft,
            BottomLeft,
            TopCenter,
            MiddleCenter,
            BottomCenter,
            TopRight,
            MiddleRight,
            BottomRight
        }

        public enum AnimationSpeed
        {
            VerySlow,
            Slow,
            Medium,
            Fast,
            VeryFast
        }

        public enum AnimationStyle
        {
            SimpleBallBounce,
            BallLineToBox,
            ShapeShifter
        }

        public Preloader(Control control, AnimationStyle style, AnimationSpeed speed, 
            Position position = Position.MiddleCenter)
        {
            this.control = control;
            if(control == null)
            {
                throw new NullReferenceException("control reference not set to an instance of Control");
            }
            //Make sure control is double buffered
            typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | 
                BindingFlags.Instance | BindingFlags.NonPublic, null, control, new object[] { true });
            control.BackgroundImageLayout = ImageLayout.Stretch;
            this.style = style;
            this.speed = speed;
            this.position = position;

            double timerInterval;

            switch (speed)
            {
                case AnimationSpeed.VerySlow:
                    timerInterval = 500;
                    break;
                case AnimationSpeed.Slow:
                    timerInterval = 250;
                    break;
                case AnimationSpeed.Fast:
                    timerInterval = 50;
                    break;
                case AnimationSpeed.VeryFast:
                    timerInterval = 25;
                    break;
                case AnimationSpeed.Medium:
                default:
                    timerInterval = 100;
                    break;
            }

            CreateFrames();
            this.originalBackgroundImage = control.BackgroundImage;
            timer = new System.Timers.Timer(timerInterval);
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
        }

        public void StartAnimation()
        {
            timer.Start();
        }

        public void StopAnimation()
        {
            PauseAnimation();
            ClearAnimation();
        }

        private void ClearAnimation()
        {
            control.Invoke(new MethodInvoker(delegate 
                { control.BackgroundImage = originalBackgroundImage; }));
        }

        public void PauseAnimation()
        {
            if(timer.Enabled)
            {
                timer.Stop();
            }
            else
            {
                timer.Start();
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            control.Invoke(new MethodInvoker(delegate { control.BackgroundImage = frames[currentImage]; }));
            currentImage = (currentImage + 1) % frames.Count;
        }

        private void CreateFrames()
        {
            FrameBuilder frameBuilder;

            switch(style)
            {
                case AnimationStyle.SimpleBallBounce:
                    frameBuilder = new SimpleBallBounceFrameBuilder();
                    break;
                case AnimationStyle.BallLineToBox:
                    frameBuilder = new BallLineToBoxFrameBuilder();
                    break;
                case AnimationStyle.ShapeShifter:
                    frameBuilder = new ShapeShifterFrameBuilder();
                    break;
                default:
                    frameBuilder = null;
                    break;
            }

            Size imageSize = control.Size;
            Color backColor = control.BackColor;
            frames = frameBuilder == null ? new List<Image>() : 
                frameBuilder.GetImages(imageSize, backColor, GetImageBoundry(imageSize));
        }

        private ImageBoundry GetImageBoundry(Size imageSize)
        {

            /*
             * First find the dead center point
             * The rest of the points will  build from there
             */

            float middleX = imageSize.Width / 2;
            float middleY = imageSize.Height / 2;

            float leftX = middleX / 2;
            float rightX = imageSize.Width - (middleX / 2);

            float topY = middleY / 2;
            float bottomY = imageSize.Height - (middleY / 2);


            string positionName = position.ToString();
            string[] yPositions = new string[] { "Top", "Middle", "Bottom" };
            string[] xPositions = new string[] { "Left", "Center", "Right" };

            string yPositionName = Array.Find(yPositions, x => positionName.StartsWith(x));
            string xPositionName = Array.Find(xPositions, x => positionName.EndsWith(x));

            PointF imageCenter = new PointF();

            switch(xPositionName)
            {
                case "Left":
                    imageCenter.X = leftX;
                    break;
                case "Right":
                    imageCenter.X = rightX;
                    break;
                case "Center":
                default:
                    imageCenter.X = middleX;
                    break;
            }

            switch (yPositionName)
            {
                case "Top":
                    imageCenter.Y = topY;
                    break;
                case "Bottom":
                    imageCenter.Y = bottomY;
                    break;
                case "Middle":
                default:
                    imageCenter.Y = middleY;
                    break;
            }


            int height = (int)(imageSize.Height - (Math.Abs((middleY - imageCenter.Y) * 2)));
            int width = (int)(imageSize.Width - (Math.Abs((middleX - imageCenter.X) * 2)));

            Size internalSize = new Size(width, height);

            return new ImageBoundry()
            {
                Center = imageCenter,
                ImageSize = internalSize
            };
        }
    }
}
