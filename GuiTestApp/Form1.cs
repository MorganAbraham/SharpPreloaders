using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SharpPreloaders;

namespace GuiTestApp
{
    public partial class Form1 : Form
    {
        private Preloader preloader;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Dictionary<string, Preloader.AnimationSpeed> speeds = new Dictionary<string, Preloader.AnimationSpeed>();
            foreach(Preloader.AnimationSpeed animationSpeed in Enum.GetValues(typeof(Preloader.AnimationSpeed)))
            {
                speeds.Add(animationSpeed.ToString(), animationSpeed);
            }

            comboSpeed.DataSource = new BindingSource(speeds, null);
            comboSpeed.DisplayMember = "Key";
            comboSpeed.ValueMember = "Value";
            comboSpeed.SelectedIndex = -1;

            Dictionary<string, Preloader.AnimationStyle> styles = new Dictionary<string, Preloader.AnimationStyle>();
            foreach (Preloader.AnimationStyle animationStyle in Enum.GetValues(typeof(Preloader.AnimationStyle)))
            {
                styles.Add(animationStyle.ToString(), animationStyle);
            }

            comboStyle.DataSource = new BindingSource(styles, null);
            comboStyle.DisplayMember = "Key";
            comboStyle.ValueMember = "Value";
            comboStyle.SelectedIndex = -1;
            
        }


        private void btnStart_Click(object sender, EventArgs e)
        {
            preloader.StartAnimation();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            preloader.StopAnimation();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            preloader.PauseAnimation();
        }

        private void comboStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (preloader != null)
            {
                preloader.StopAnimation();
            }
            if (this.comboSpeed.SelectedIndex != -1 && this.comboStyle.SelectedIndex != -1)
            {
                preloader = new Preloader(this, (Preloader.AnimationStyle)this.comboStyle.SelectedValue, (Preloader.AnimationSpeed)this.comboSpeed.SelectedValue);
                preloader.StartAnimation();
            }
        }

        private void comboSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(preloader != null)
            {
                preloader.StopAnimation();
            }
            if (this.comboSpeed.SelectedIndex != -1 && this.comboStyle.SelectedIndex != -1)
            {
                preloader = new Preloader(this, (Preloader.AnimationStyle)this.comboStyle.SelectedValue, (Preloader.AnimationSpeed)this.comboSpeed.SelectedValue);
                preloader.StartAnimation();
            }
        }
    }
}
