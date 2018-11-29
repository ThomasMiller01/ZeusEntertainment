using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeusCore
{
    public interface ICheckCommand
    {
        string[] checkCommand(string cmd);
        List<string[]> getCommands();
        Dictionary<int, string> getCommandDic();
        bool checkSimilarity(string source, string target, double percentage);
    }
}
