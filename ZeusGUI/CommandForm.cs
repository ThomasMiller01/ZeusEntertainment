using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZeusCore;

namespace ZeusGUI
{
    public partial class CommandForm : Form
    {
        MainForm mainForm;
        ConsoleForm consoleForm;

        public CommandForm(MainForm mainForm, ConsoleForm consoleForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.consoleForm = consoleForm;
        }

        //open consoleBox
        private void showConsole_btn_Click(object sender, EventArgs e)
        {
            consoleForm.Show();
        }

        //if escape is pressed hide commandform, else if enter is pressed, call checkCommand from mainform
        private void command_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                TextBox cmd_tb = (TextBox)sender;
                string value = cmd_tb.Text;
                cmd_tb.Text = "";
                mainForm.checkCommand(value);
            }
        }

        //if escape is pressed hide commandform
        private void CommandFrom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        //if escape is pressed hide commandform
        private void showConsole_btn_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }
    }
}
