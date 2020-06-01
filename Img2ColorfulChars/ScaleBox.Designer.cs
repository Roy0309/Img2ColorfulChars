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
            this.tb_Scale = new System.Windows.Forms.TextBox();
            this.btn_Set = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tb_Scale
            // 
            this.tb_Scale.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.tb_Scale.Location = new System.Drawing.Point(14, 14);
            this.tb_Scale.MaxLength = 4;
            this.tb_Scale.Multiline = true;
            this.tb_Scale.Name = "tb_Scale";
            this.tb_Scale.Size = new System.Drawing.Size(176, 26);
            this.tb_Scale.TabIndex = 0;
            this.tb_Scale.WordWrap = false;
            this.tb_Scale.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tb_Scale_KeyPress);
            // 
            // btn_Set
            // 
            this.btn_Set.Location = new System.Drawing.Point(196, 14);
            this.btn_Set.Name = "btn_Set";
            this.btn_Set.Size = new System.Drawing.Size(61, 26);
            this.btn_Set.TabIndex = 1;
            this.btn_Set.Text = "Set";
            this.btn_Set.UseVisualStyleBackColor = true;
            this.btn_Set.Click += new System.EventHandler(this.btn_Set_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 30);
            this.label1.TabIndex = 2;
            this.label1.Text = "1. Scale should be positive integar.\r\n2. Larger scale gets smaller image.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Location = new System.Drawing.Point(11, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(246, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Given is suggested scale (no need to scroll).";
            // 
            // ScaleBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 105);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Set);
            this.Controls.Add(this.tb_Scale);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ScaleBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Set scale";
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