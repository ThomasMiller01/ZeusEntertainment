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
using System.Speech.Synthesis;
using System.Diagnostics;

namespace ZeusGUI
{
    public partial class ConsoleForm : Form
    {
        SpeechSynthesizer speechSynthesizer;
        Dictionary<int, string> languageDict = new Dictionary<int, string>();        

        public ConsoleForm()
        {
            InitializeComponent();
            this.speechSynthesizer = new SpeechSynthesizer();
            setup();
        }

        void setup()
        {           
            //add voices if installed for the SpeechSynthesizer
            languageDict.Add(0, null);
            languageDict.Add(1, null);
            //check if voice languages de and en are installed
            foreach (var voices in speechSynthesizer.GetInstalledVoices())
            {
                if (voices.VoiceInfo.Culture.ToString().Equals("de-DE"))
                {
                    languageDict[0] = voices.VoiceInfo.Name;
                }
                else if (voices.VoiceInfo.Culture.ToString().Equals("en-US"))
                {
                    languageDict[1] = voices.VoiceInfo.Name;
                }                
            }
            //if the languages are not installed set language to the first installed language
            if(languageDict[0] == null)
            {
                languageDict[0] = speechSynthesizer.GetInstalledVoices()[0].VoiceInfo.Name;                
            }
            else if (languageDict[1] == null)
            {
                languageDict[0] = speechSynthesizer.GetInstalledVoices()[0].VoiceInfo.Name;                
            }
            print("ZeusEntertainment presents ", false, 0);
            println("[Zeus 2.0]", false, 0);
            println("(c) ZeusEntertainment. Alle Rechte vorbehalten.", false, 0);
            println("", false, 0);
        }

        //call speechsynthesizer to read a string
        public void speechSynthesizerFunction(string text)
        {
            speechSynthesizer.SpeakAsync(text);
        }

        //prints text into the consoleBox
        public void print(string text, bool speak, int language)
        {
            console_tb.AppendText(text);
            if (speak)
            {
                speechSynthesizer.SelectVoice(languageDict[language]);
                speechSynthesizerFunction(text);
            }
        }        

        public void println(string text, bool speak, int language)
        {
            console_tb.AppendText(text + "\r");
            if (speak)
            {
                speechSynthesizer.SelectVoice(languageDict[language]);
                speechSynthesizerFunction(text);
            }            
        }

        //clears the consolebox
        public void clear()
        {
            console_tb.Clear();
        }

        //if escape is pressed hide the consoleBox
        private void console_tb_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        private void ConsoleForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Hide();
            }
        }

        //if text is printed to the console-box, scroll to the bottom
        private void console_tb_TextChanged(object sender, EventArgs e)
        {
            console_tb.SelectionStart = console_tb.Text.Length;
            console_tb.ScrollToCaret();
        }

        //if link is clicked open it in firefox
        private void console_tb_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start("Firefox.exe", e.LinkText);
        }
    }
}
