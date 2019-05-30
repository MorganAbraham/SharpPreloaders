using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Drawing;
using SharpPreloaders.FrameBuilders;

namespace SharpPreloaders
{
    public class Preloader
    {
        private Form form;
        private AnimationStyle style;
        private AnimationSpeed speed;

        private System.Timers.Timer timer;

        private Image[] frames;

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
            SimpleBallBounce
        }

        public Preloader(Form form, AnimationStyle style, AnimationSpeed speed)
        {
            this.form = form;
            this.style = style;
            this.speed = speed;

            double timerInterval;

            switch (speed)
            {
                case AnimationSpeed.VerySlow:
                    timerInterval = 3000;
                    break;
                case AnimationSpeed.Slow:
                    timerInterval = 1500;
                    break;
                case AnimationSpeed.Fast:
                    timerInterval = 500;
                    break;
                case AnimationSpeed.VeryFast:
                    timerInterval = 250;
                    break;
                case AnimationSpeed.Medium:
                default:
                    timerInterval = 1000;
                    break;
            }

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
            throw new NotImplementedException();
        }

        public void PauseAnimation()
        {
            timer.Stop();
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CreateFrames()
        {
            FrameBuilder frameBuilder;

            switch(style)
            {
                case AnimationStyle.SimpleBallBounce:
                    frameBuilder = new SimpleBallBounceFrameBuilder();
                    break;
                default:
                    frameBuilder = null;
                    break;
            }

            Size imageSize = form.Size;
            frames = frameBuilder == null ? new Image[0] : frameBuilder.GetImages(imageSize);
        }
    }
}
