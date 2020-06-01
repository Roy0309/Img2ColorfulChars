using System;
using System.Drawing;
using System.Windows.Forms;

namespace Img2ColorfulChars
{
    public partial class ScaleBox : Form
    {
        public int HScale { get; private set; }

        public ScaleBox()
        {
            InitializeComponent();
        }

        public ScaleBox(int suggestedScale)
        {
            InitializeComponent();
            tb_Scale.Text = suggestedScale.ToString();
        }

        private void ScaleBox_Load(object sender, EventArgs e)
        {
            // Focus this window
            NativeMethods.SetForegroundWindow(Handle);
        }

        private void tb_Scale_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < '0' || e.KeyChar > '9') && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }

        private void btn_Set_Click(object sender, EventArgs e)
        {
            bool validScale = int.TryParse(tb_Scale.Text, out int hScale);
            if (!validScale)
            {
                label1.ForeColor = Color.Red;
                return;
            }
            HScale = hScale;
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
