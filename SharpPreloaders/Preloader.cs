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

        private System.Timers.Timer timer;

        private List<Image> frames;
        private int currentImage = 0;

        private Image originalBackgroundImage;
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
            BallLineToBox
        }

        public Preloader(Control control, AnimationStyle style, AnimationSpeed speed)
        {
            this.control = control;
            if(control == null)
            {
                throw new NullReferenceException("control reference not set to an instance of Control");
            }
            //Make sure control is double buffered
            typeof(Control).InvokeMember("DoubleBuffered", BindingFlags.SetProperty | BindingFlags.Instance |
                  BindingFlags.NonPublic, null, control, new object[] { true });
            control.BackgroundImageLayout = ImageLayout.Stretch;
            this.style = style;
            this.speed = speed;

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
            control.Invoke(new MethodInvoker(delegate { control.BackgroundImage = originalBackgroundImage; }));
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
                default:
                    frameBuilder = null;
                    break;
            }

            Size imageSize = control.Size;
            frames = frameBuilder == null ? new List<Image>() : frameBuilder.GetImages(imageSize);
        }
    }
}
