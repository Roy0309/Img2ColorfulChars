namespace Img2ColorfulChars
{
    partial class ScaleBox
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScaleBox));
            this.tb_Scale = new System.Windows.Forms.TextBox();
            this.btn_Set = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tb_Scale
            // 
            resources.ApplyResources(this.tb_Scale, "tb_Scale");
            this.tb_Scale.Name = "tb_Scale";
            this.tb_Scale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_Scale_KeyPress);
            // 
            // btn_Set
            // 
            resources.ApplyResources(this.btn_Set, "btn_Set");
            this.btn_Set.Name = "btn_Set";
            this.btn_Set.UseVisualStyleBackColor = true;
            this.btn_Set.Click += new System.EventHandler(this.btn_Set_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Name = "label2";
            // 
            // ScaleBox
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Set);
            this.Controls.Add(this.tb_Scale);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScaleBox";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ScaleBox_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_Scale;
        private System.Windows.Forms.Button btn_Set;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}