using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZeusCore;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Threading;
using Google.Cloud.Speech.V1;
using Google.Apis.Auth.OAuth2;

namespace ZeusGUI
{
    public partial class MainForm : Form
    {
        CommandForm commandForm;
        ConsoleForm consoleForm;
        SpeechRecognizer speechRecognizer;

        List<string> responses = new List<string>();

        public MainForm()
        {
            InitializeComponent();
            //create object of console-form and command-form
            consoleForm = new ConsoleForm();
            commandForm = new CommandForm(this, consoleForm);
            //set up the speech-recognition-engine
            speechRecognizer = new SpeechRecognizer();
            SpeechRecognitionEngine rec = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("de-DE"));
            //set up global grammar
            DictationGrammar grammar = new DictationGrammar();
            /* Grammar grammar = new Grammar(new GrammarBuilder("zeus")); */
            //load grammar
            rec.LoadGrammarAsync(grammar);
            rec.SetInputToDefaultAudioDevice();            
            //Create event when speech was recognized
            rec.SpeechRecognized += rec_SpeechRecognized;
            rec.RecognizeAsync(RecognizeMode.Multiple);
        }

        //function that checks the commands for the speech recognizer engine
        void rec_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            string r = e.Result.Text;            
            //only if recognized speech contains signalword 'zeus' call checkCommand
            if (r.ToLower().Contains("zeus"))
            {
                //plays activate-sound and visual-effect before sound is send to the google-speech-api
                /* sound needs to be implemented */

                //Google-Speech-Api needs to be finished              
                //await GoogleSpeechApiStreamingAsync(15);
                if (Properties.Settings.Default.debug)
                {
                    consoleForm.println(r.ToLower(), false, 0);
                }
                //if isListening is true or 'r' contains cmd to turn listening on
                if (Properties.Settings.Default.speechRecognitionIsActive || CheckCommand.Instance.checkSimilarity(CheckCommand.Instance.getCommandDic()[19], responses.Last().ToLower(), 0.6))
                {
                    checkCommand(r.ToLower());
                }
            }
        }

        //function that checks the commands with the checkfunction of zeus core
        public string[] checkCommand(string cmd)
        {
            //call checkCommand() from ZeusCore and get answer-data
            string[] checkCommandData = CheckCommand.Instance.checkCommand(cmd);
            if (checkCommandData != null)
            {
                //if command-function defined in commandDefinition.xml is called in core, go to else { }
                if (checkCommandData[3].Equals("false"))
                {
                    try
                    {
                        //call command-function defined in commandDefinition.xml
                        MethodInfo methodInfo = this.GetType().GetMethod(checkCommandData[2]);
                        string returnValue = (string)methodInfo.Invoke(this, null);
                        consoleForm.println("", false, 0);
                        if (Properties.Settings.Default.debug)
                        {
                            consoleForm.println("Command: " + checkCommandData[0], false, 0);
                            consoleForm.print("Answer: ", false, 0);
                        }                        
                        consoleForm.println(getRandomCommand(checkCommandData[5]), true, 1);
                        if (!checkForSpecialFormat(returnValue, checkCommandData[0]))
                        {
                            if (Properties.Settings.Default.debug)
                            {
                                consoleForm.print("FunctionAnswer: ", false, 0);                                
                            }
                            consoleForm.println(returnValue, true, 1);
                        }            
                    }
                    catch(Exception e)
                    {
                        consoleForm.print("Error: ", true, 1);
                        consoleForm.println(e.Message, false, 0);                        
                    }                    
                }
                else
                {
                    //else print the answer of the command-function called in ZeusCore
                    consoleForm.println("", false, 0);
                    if (Properties.Settings.Default.debug)
                    {
                        consoleForm.println("Command: " + checkCommandData[0], false, 0);
                        consoleForm.print("Answer: ", false, 0);
                    }                    
                    consoleForm.println(getRandomCommand(checkCommandData[5]), true, 1);
                    if (Properties.Settings.Default.debug)
                    {
                        consoleForm.println("CmdError: " + checkCommandData[8], false, 0);
                    }                    
                    if (!checkForSpecialFormat(checkCommandData[9], checkCommandData[0]))
                    {
                        if (Properties.Settings.Default.debug)
                        {
                            consoleForm.print("FunctionAnswer: ", false, 0);
                        }                        
                        consoleForm.println(checkCommandData[9], true, 1);
                    }                
                }
                return checkCommandData;
            }
            else
            {
                consoleForm.println("", false, 0);
                consoleForm.println("Unknown command", true, 1);
                return null;
            }
        }       

        //Google-Speech-Api
        async Task<object> GoogleSpeechApiStreamingAsync(int seconds)
        {
            if (NAudio.Wave.WaveIn.DeviceCount < 1)
            {
                Console.WriteLine("No microphone!");
                return -1;
            }
            var speech = SpeechClient.Create();
            var streamingCall = speech.StreamingRecognize();
            // Write the initial request with the config.
            await streamingCall.WriteAsync(
                new StreamingRecognizeRequest()
                {
                    StreamingConfig = new StreamingRecognitionConfig()
                    {
                        Config = new RecognitionConfig()
                        {
                            Encoding = RecognitionConfig.Types.AudioEncoding.Linear16,
                            SampleRateHertz = 16000,
                            LanguageCode = "de"
                        },
                        InterimResults = true,
                        SingleUtterance = true,
                    }
                });
            // Print responses as they arrive.
            Task printResponses = Task.Run(async () =>
            {
                List<string> responses = new List<string>();
                while (await streamingCall.ResponseStream.MoveNext(
                    default(CancellationToken)))
                {
                    foreach (var result in streamingCall.ResponseStream
                        .Current.Results)
                    {
                        foreach (var alternative in result.Alternatives)
                        {
                            responses.Add(alternative.Transcript);
                            Console.WriteLine(alternative.Transcript);
                            //consoleForm.println(alternative.Transcript, false, 0);
                        }
                    }                   
                }
                Console.WriteLine("----------------------");
                Console.WriteLine("Last: " + responses.Last());
                Console.WriteLine("----------------------");
                //callCore(responses.Last());
            });
            // Read from the microphone and stream to API.
            object writeLock = new object();
            bool writeMore = true;
            var waveIn = new NAudio.Wave.WaveInEvent();
            waveIn.DeviceNumber = 0;
            waveIn.WaveFormat = new NAudio.Wave.WaveFormat(16000, 1);
            waveIn.DataAvailable +=
                (object sender, NAudio.Wave.WaveInEventArgs args) =>
                {
                    lock (writeLock)
                    {
                        if (!writeMore) return;
                        streamingCall.WriteAsync(
                            new StreamingRecognizeRequest()
                            {
                                AudioContent = Google.Protobuf.ByteString
                                    .CopyFrom(args.Buffer, 0, args.BytesRecorded)
                            }).Wait();
                    }
                };
            waveIn.StartRecording();
            Console.WriteLine("Speak now.");
            consoleForm.println("Speak now.", false, 0);
            await Task.Delay(TimeSpan.FromSeconds(seconds));
            // Stop recording and shut down.
            waveIn.StopRecording();
            lock (writeLock) writeMore = false;
            await streamingCall.WriteCompleteAsync();
            await printResponses;
            return 0;
        }

        //methods according to the commands.xml file
        public string clear()
        {
            consoleForm.clear();
            return "";
        }
        public string startConsole()
        {
            consoleForm.Show();
            return "";
        }
        public string closeConsole()
        {
            consoleForm.Hide();
            return "";
        }
        public string startZeus()
        {
            Properties.Settings.Default.speechRecognitionIsActive = true;
            return "";
        }
        public string stopZeus()
        {
            Properties.Settings.Default.speechRecognitionIsActive = false;
            return "";
        }
        public string closeZeus()
        {
            Close();
            return "";
        }
        public string toggleDebug()
        {
            if (Properties.Settings.Default.debug)
            {
                Properties.Settings.Default.debug = false;
            }
            else
            {
                Properties.Settings.Default.debug = true;
            }
            Properties.Settings.Default.Save(); 
            return "done";
        }        
        //end of the methods according to the commands.xml file

        //checks if the answer string have to be a special format
        bool checkForSpecialFormat(string cmdAnswer, string cmdName)
        {
            //defines what command has to be another format
            if (CheckCommand.Instance.getCommandDic()[0].Equals(cmdName)) //possibleCommands
            {
                try
                {
                    consoleForm.println("You have to say the keyword: 'zeus' before every command", false, 0);
                    string[] cmdArray = cmdAnswer.Split(';');
                    foreach (var str in cmdArray)
                    {
                        string[] tmp = str.Split(':');
                        consoleForm.print("Name: " + tmp[0], false, 0);
                        consoleForm.println(", Cmd: " + tmp[1], false, 0);
                    }
                    consoleForm.println(cmdArray.Length + " possible commands", false, 0);
                    return true;
                }
                catch (Exception e)
                {
                    consoleForm.println("Error: ", true, 1);
                    consoleForm.println(e.Message, false, 0);
                    return false;
                }
            }
            else if (CheckCommand.Instance.getCommandDic()[1].Equals(cmdName)) //getTime
            {
                if (Properties.Settings.Default.debug)
                {
                    consoleForm.print("FunctionAnswer: ", false, 0);
                }
                consoleForm.println(cmdAnswer, true, 0);
                return true;
            }
            else if (CheckCommand.Instance.getCommandDic()[2].Equals(cmdName)) //getDay
            {
                if (Properties.Settings.Default.debug)
                {
                    consoleForm.print("FunctionAnswer: ", false, 0);
                }                
                consoleForm.println(cmdAnswer, true, 0);
                return true;
            }
            else if (CheckCommand.Instance.getCommandDic()[7].Equals(cmdName)) //Wikipedia-Search
            {
                if (Properties.Settings.Default.debug)
                {
                    consoleForm.println("FunctionAnswer:", false, 0);
                }
                string[] answer = cmdAnswer.Split('~');
                consoleForm.println("Test: " + answer[1], false, 0);
                try
                {
                    answer[1] = answer[1].Split('(')[0] + answer[1].Split('(')[1].Split(new[] { ") " }, StringSplitOptions.None)[1];
                }
                catch (Exception) { }
                consoleForm.println(answer[0], false, 0);
                consoleForm.println(answer[1], true, 0);
                consoleForm.println(answer[2], false, 0);
                return true;
            }
            else if (CheckCommand.Instance.getCommandDic()[9].Equals(cmdName)) //weather
            {
                if (Properties.Settings.Default.debug)
                {
                    consoleForm.print("FunctionAnswer:", false, 0);
                }
                string[] cmdAnswerSplit = cmdAnswer.Split('~');
                consoleForm.println(cmdAnswerSplit[0], true, 1);
                consoleForm.println(cmdAnswerSplit[1], false, 0);
                return true;
            }
            else if (CheckCommand.Instance.getCommandDic()[10].Equals(cmdName)) //news
            {
                string[] articleList = cmdAnswer.Split(new[] { "join1%%" }, StringSplitOptions.None);
                int i = 0;
                foreach(var article in articleList)
                {
                    bool readOut = false;
                    //only read out the first three article-titles
                    if (i < 3)
                    {
                        readOut = true;
                        i++;
                    }
                    consoleForm.println("", false, 0);
                    string[] articleArray = article.Split(new[] { "join2%%" }, StringSplitOptions.None);
                    switch (articleArray[0])
                    {
                        case "WORLD":
                            consoleForm.println("Welt:", readOut, 0); //Searchparameter
                            break;
                        case "ENTERTAINMENT":
                            consoleForm.println("Entertainment:", readOut, 0); //Searchparameter
                            break;
                        case "TECHNOLOGY":
                            consoleForm.println("Technologie: ", readOut, 0); //Searchparameter
                            break;
                    }
                    consoleForm.println(articleArray[1], readOut, 0); //title
                    consoleForm.println(articleArray[2], false, 0); //link
                    //consoleForm.println("ID: " + articleArray[3], false, 0);
                    consoleForm.println(articleArray[4], false, 0); //PublishingDate
                    //consoleForm.println("Description: " + articleArray[5], false, 0);                    
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        //choose a random answer, string answer has the format: 'answer1;answer2;answer3'
        string getRandomCommand(string answer)
        {
            Random random = new Random();
            string[] answerArray = answer.Split(';');
            int randomNumber = random.Next(0, answerArray.Length);
            return answerArray[randomNumber];
        }

        //functions for the notify icon
        private void executeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            commandForm.Show();
        }

        private void consoleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            consoleForm.Show();
        }

        private void mainFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Visible = true;
            this.ShowInTaskbar = true;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ZeusGuiIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                commandForm.Show();
            }
        }

        //if escape is pressed hide mainframe
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Visible = false;
                this.ShowInTaskbar = false;
            }
        }
    }
}
