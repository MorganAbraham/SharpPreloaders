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
            LoadComboBox(comboStyle, typeof(Preloader.AnimationStyle));
            LoadComboBox(comboSpeed, typeof(Preloader.AnimationSpeed));
            LoadComboBox(comboPosition, typeof(Preloader.Position));
            
            
        }

        private void LoadComboBox(ComboBox box, Type type)
        {
            Dictionary<string, object> values = 
                new Dictionary<string, object>();

            foreach (var value in Enum.GetValues(type))
            {
                values.Add(value.ToString(), value);
            }

            box.DataSource = new BindingSource(values, null);
            box.DisplayMember = "Key";
            box.ValueMember = "Value";
            box.SelectedIndex = -1;
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

        private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (preloader != null)
            {
                preloader.StopAnimation();
            }
            if (this.comboSpeed.SelectedIndex != -1 && 
                this.comboStyle.SelectedIndex != -1 && this.comboPosition.SelectedIndex != -1)
            {
                preloader = new Preloader(this, 
                    (Preloader.AnimationStyle)this.comboStyle.SelectedValue, 
                    (Preloader.AnimationSpeed)this.comboSpeed.SelectedValue,
                    (Preloader.Position)this.comboPosition.SelectedValue);
                preloader.StartAnimation();
            }
        }
    }
}
