using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1.parser
{
    public class LogOutput
    {
        Dictionary<string,List<string>> log;
        enum FORM { DIC, LIST }
        public LogOutput() {
            log = new Dictionary<string,List<string>>();
        }

        public void SetNull() {
            log.Clear();
        }
        public void ClearLog(string listName) {
            if(log.ContainsKey(listName))
                log[listName].Clear();
        }

        public void InputInfo(string name,string info) {
            if (log.ContainsKey(name))
            {
                log[name].Add(info);
            }
            else {
                log.Add(name, new List<string>());
                log[name].Add(info);
            }
        }
        public Dictionary<string,List<string>> GetLog() {
            return log;
        }
        /*
        public void InputDicInfo(Dictionary<string,List<string>> info) {
            foreach (string key in info.Keys) {
                log.Add(FORM.DIC.ToString());
                log.AddRange(info[key]);
                log.Add(FORM.DIC.ToString());
            
            }

        }
         * */
    }
}
