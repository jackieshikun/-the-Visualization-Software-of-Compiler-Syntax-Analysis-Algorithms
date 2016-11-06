using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApplication1
{
    class ReadRule
    {
        private List<String> ruleList;
        public ReadRule() {
            ruleList = new List<String>();
        }
        private List<string> ReadRuleByFile(){
            int line = 1;
            String rule;
            Regex reg = new Regex("^[A-Za-z]*->");
            List<string> tempList = new List<string>();
            StreamReader fileReader;
            if(System.IO.File.Exists("rule.txt")){
                fileReader = new StreamReader("rule.txt");
            }else{
                Console.WriteLine("rule文件不存在");
                return null;
            }

            while (fileReader.Peek() != -1)
            {
                rule = fileReader.ReadLine();
                if (reg.IsMatch(rule) == false)
                {
                    Console.WriteLine("line " + line + ": rule isn't match expected pattern");
                    return null;
                }
                else
                {
                    tempList.Add(rule);
                }

                line++;
            }
            fileReader.Close();
            return tempList;
        }
        private String[] RuleSplit(String originalRule) {
            //List<String> splitRuleList = new List<string>();
            String[] splitRule = originalRule.Split('|');
            String startString = originalRule.Substring(0,originalRule.IndexOf('>') + 1 ); //取规则开始的 X->
            String trueString;
            for (int i = 0; i < splitRule.Length; i++) {
                if (!splitRule[i].StartsWith(startString))
                {
                    trueString = startString + splitRule[i];
                }
                else {
                    trueString = splitRule[i];
                }
                splitRule[i] = trueString;
            }
            return splitRule;
        }

        public void Run() {
            List<string> temp;
            string[] rule;
            temp = ReadRuleByFile();
            if (temp != null) { 
                foreach(string tempRule in temp){
                    //Console.WriteLine(tempRule);
                    rule = RuleSplit(tempRule);
                    foreach (string i in rule)
                        Console.WriteLine(i);
                }
            }
        
        }

    }
}
