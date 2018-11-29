using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ZeusCore
{
    class CommandDefinition
    {
        public List<string[]> commandsDict = new List<string[]>();
        public Dictionary<int, string> commandDict = new Dictionary<int, string>();
        public Dictionary<string, string> programNamesDict = new Dictionary<string, string>();

        public CommandDefinition()
        {
            XDocument commandsFile = XDocument.Load(Properties.Settings.Default.commandPath);
            var items = commandsFile.Descendants("item");
            //fill commands list with data from commandDefinition.xml
            foreach(var item in items)
            {
                string name = "";
                string command = "";
                string action = "";
                string usesCore = "";
                string definition = "";
                string answers = "";
                string hasParams = "";
                var nameItem = item.Descendants("name");
                var commandItem = item.Descendants("command");
                var actionItem = item.Descendants("action");
                var usesCoreItem = item.Descendants("usesCore");
                var definitionItem = item.Descendants("definition");
                var answerItem = item.Descendants("answers");
                var hasParamsItem = item.Descendants("needsParameter");
                foreach (var value in nameItem)
                {
                    name = value.Value;
                }
                foreach (var value in commandItem)
                {
                    command = value.Value;
                }
                foreach (var value in actionItem)
                {
                    action = value.Value;
                }
                foreach (var value in usesCoreItem)
                {
                    usesCore = value.Value;
                }
                foreach (var value in definitionItem)
                {
                    definition = value.Value;
                }
                foreach (var value in answerItem)
                {
                    answers = value.Value;
                }
                foreach (var value in hasParamsItem)
                {
                    hasParams = value.Value;
                }
                commandsDict.Add(new string[] { name, command, action, usesCore, definition, answers, hasParams});
            }
            //fill commandDic with data from commandDefinition.xml
            for(int i = 0; i < commandsDict.Count(); i++)
            {
                commandDict.Add(i, commandsDict[i][0]);
            }
            fillProgramNamesDict();
        }

        void fillProgramNamesDict()
        {
            programNamesDict.Add("ProgramName", "ProgramPath");            
        }
    }
}
