namespace ZeusGUI
{
    partial class CommandForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommandForm));
            this.command_tb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.showConsole_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // command_tb
            // 
            this.command_tb.Location = new System.Drawing.Point(78, 12);
            this.command_tb.Name = "command_tb";
            this.command_tb.Size = new System.Drawing.Size(203, 20);
            this.command_tb.TabIndex = 0;
            this.command_tb.KeyDown += new System.Windows.Forms.KeyEventHandler(this.command_tb_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Command:";
            // 
            // showConsole_btn
            // 
            this.showConsole_btn.Location = new System.Drawing.Point(188, 38);
            this.showConsole_btn.Name = "showConsole_btn";
            this.showConsole_btn.Size = new System.Drawing.Size(90, 21);
            this.showConsole_btn.TabIndex = 2;
            this.showConsole_btn.Text = "Show Console";
            this.showConsole_btn.UseVisualStyleBackColor = true;
            this.showConsole_btn.Click += new System.EventHandler(this.showConsole_btn_Click);
            this.showConsole_btn.KeyDown += new System.Windows.Forms.KeyEventHandler(this.showConsole_btn_KeyDown);
            // 
            // CommandForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(293, 69);
            this.ControlBox = false;
            this.Controls.Add(this.showConsole_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.command_tb);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "CommandForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ExecutionWindow";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.CommandFrom_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox command_tb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button showConsole_btn;
    }
}

