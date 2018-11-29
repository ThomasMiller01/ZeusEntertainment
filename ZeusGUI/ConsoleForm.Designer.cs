namespace ZeusGUI
{
    partial class ConsoleForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConsoleForm));
            this.console_tb = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // console_tb
            // 
            this.console_tb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.console_tb.BackColor = System.Drawing.Color.Black;
            this.console_tb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.console_tb.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.console_tb.ForeColor = System.Drawing.Color.White;
            this.console_tb.Location = new System.Drawing.Point(12, 12);
            this.console_tb.Name = "console_tb";
            this.console_tb.ReadOnly = true;
            this.console_tb.Size = new System.Drawing.Size(857, 471);
            this.console_tb.TabIndex = 0;
            this.console_tb.Text = "";
            this.console_tb.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.console_tb_LinkClicked);
            this.console_tb.TextChanged += new System.EventHandler(this.console_tb_TextChanged);
            this.console_tb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.console_tb_KeyDown);
            // 
            // ConsoleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(881, 495);
            this.ControlBox = false;
            this.Controls.Add(this.console_tb);
            this.ForeColor = System.Drawing.Color.White;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ConsoleForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ConsoleWindow";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ConsoleForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox console_tb;
    }
}