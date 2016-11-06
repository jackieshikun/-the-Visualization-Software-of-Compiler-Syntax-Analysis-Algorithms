using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using ConsoleApplication1.view;

namespace ConsoleApplication1.parser
{
    public class Parsing
    {
        public Dictionary<string,List<string>> rRule;
        public Dictionary<string, List<string>> follow;
        public Dictionary<string, bool> isFollowSolved;
        public Dictionary<string, List<string>> first;
        public Dictionary<string, bool> isFirstSolved; //该string的first集是否求过
        public Dictionary<string, bool> isToEmpty;
        public List<string> terminalSet;
        public List<string> nonTerminalSet;
        public List<string> rightNonTerminalSet;//存储规则右边的非终结符,检查产生式的右侧的非终结符是否出现在左侧过

        enum FORM {DIC, LIST }
        public LogOutput outputLog;//用于后端向view层传递消息

        private string fileName;
        public static string NULL_SYMBOL = "^";
        public Parsing(string f) {
            fileName = f;

            rRule = new Dictionary<string, List<string>>();
            terminalSet = new List<string>();
            nonTerminalSet = new List<string>();
            rightNonTerminalSet = new List<string>();
            follow = new Dictionary<string, List<string>>();
            isFollowSolved = new Dictionary<string, bool>();
            first = new Dictionary<string, List<string>>();
            isToEmpty = new Dictionary<string, bool>();
            isFirstSolved = new Dictionary<string, bool>();
            outputLog = new LogOutput();
        }

        public List<string> ReadRuleByFile()
        {
            int line = 1;
            String rule;
            Regex reg = new Regex("^[A-Za-z]*'?->");
            List<string> tempList = new List<string>();
            StreamReader fileReader;
            if (System.IO.File.Exists(fileName))
            {
                fileReader = new StreamReader(fileName,Encoding.UTF8);
            }
            else
            {
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

        public String[] RuleSplit(String originalRule)
        {
            //List<String> splitRuleList = new List<string>();
            //String[] splitRule = originalRule.Split('|');
            String startString = originalRule.Substring(0, originalRule.IndexOf('>') + 1); //取规则开始的 X->
            String rightProduction = originalRule.Substring(originalRule.IndexOf('>') + 1,originalRule.Length-startString.Length);
            rightProduction = RemoveSpace(rightProduction);
            //startString = startString + rightProduction;
            String[] splitRule = (startString + " " + rightProduction).Split('|');
            String trueString;
            for (int i = 0; i < splitRule.Length; i++)
            {
                if (!splitRule[i].StartsWith(startString))
                {
                    trueString = startString + splitRule[i];
                }
                else
                {
                    trueString = splitRule[i];
                }
                splitRule[i] = trueString;
            }
            return splitRule;
        }

        public bool IsUpperLetter(char ch)
        {
            if (ch >= 'A' && ch <= 'Z')
                return true;
            else
                return false;

        }
        public bool GetTerminalAndNonTerminalSet(string[] originalRule)
        {
            foreach (string rule in originalRule)
            {
                string[] tempid = rule.Split(' ');
                foreach (string id in tempid)
                {
                    string word = id.Trim();
                    if (word != "")
                    {
                        if (word.Contains("->"))
                        {
                            word = word.Replace("->", "");
                            char ch = word[0];
                            if (IsUpperLetter(ch))
                            {
                                if (nonTerminalSet.Contains(word) == false)
                                {
                                    nonTerminalSet.Add(word);
                                    rightNonTerminalSet.Add(word);//如果没有出现在nonTerminalSet，则一定没有出现在rightNonTerminalSet
                                }
                            }
                            else
                            {
                                Console.WriteLine("error: 规则左边应该是非终结符");
                                return false;
                            }

                        }
                        else
                        {
                            char ch = word[0];
                            if (IsUpperLetter(ch))
                            {
                                if (rightNonTerminalSet.Contains(word) == false)
                                {
                                    rightNonTerminalSet.Add(word);
                                    if (nonTerminalSet.Contains(word) == false)
                                        nonTerminalSet.Add(word);
                                }
                            }
                            else
                            {
                                if (word != NULL_SYMBOL && terminalSet.Contains(word) == false)
                                    terminalSet.Add(word);
                            }
                        }


                    }
                    //Console.WriteLine(s);
                }

            }
            if (!terminalSet.Contains("$"))
                terminalSet.Add("$");
            return true;


        }

        public bool IsRightNonTerminalSetEqualNonTerminalSet()
        {
            foreach (string word in nonTerminalSet) {
                bool isKey = false;
                foreach (string key in rRule.Keys) {
                    if (word.Equals(key)) {
                        isKey = true;
                        break;
                    }
                }
                if (isKey == false)
                    return false;
            }
            return true;
            /*
            List<string> test = nonTerminalSet.Except(rightNonTerminalSet).ToList<string>();
            if (test.Count() != 0)
            {
                Console.WriteLine("error:两侧元素不同,右侧的非终结符没有出现在左边");
                return false;
            }
            return true;
            */
        }
        /*
        public void PrintTerminalAndNonTerminalSet()
        {
            Console.WriteLine("终结符集：");
            foreach (string s in terminalSet)
            {
                Console.Write(s + " ");
            }
            Console.WriteLine("\n非终结符集:");
            foreach (string s in nonTerminalSet)
            {
                Console.Write(s + " ");
            }
        }
         * */
        //去掉一个规则右部多余的空格
        public string RemoveSpace(string st)
        {
            string[] word = st.Split(' ');
            string cleanedSt = null;
            foreach (string s in word)
            {
                if (s != "")
                {
                    cleanedSt += s.Trim() + " ";
                }
            }
            return cleanedSt.Trim();
        }
        /*
           1.把$放到开始符的follow集中
        *  2. 遍历规则, 找word[i] == nonTerminal
        *  3. 如果word[i+1] 是终结符，则把word[i+1]加入follow[nonTerminal]中
        *  4. 如果word[i+1] 是非终结符,
         
        */
        public List<string> FollowSet(string nonTerminal)
        {
            List<string> tempFollow;
            string[] word;
            string toLogName = "follow";
            //outputLog.ClearLog(toLogName);
            int next;//word[i + next]为word[i]后第一个非""的string
            outputLog.InputInfo(toLogName, "求" + nonTerminal + "的follow集:\r\n");
            if (isFollowSolved.ContainsKey(nonTerminal)){//如果求过，则无需再求
                outputLog.InputInfo(toLogName, nonTerminal + "的follow集求过，为:" + PrintList(first[nonTerminal]));
                return follow[nonTerminal];
            }
            if (follow.ContainsKey(nonTerminal))
            {
                tempFollow = follow[nonTerminal];
            }
            else
            {
                tempFollow = new List<string>();
                if (follow.Count() == 0)
                {
                    tempFollow.Add("$");
                    outputLog.InputInfo(toLogName, "添加$到第一个非终结符" + nonTerminal + "的follow集");
                }
                follow.Add(nonTerminal, tempFollow);
                outputLog.InputInfo(toLogName, "#" + Dic2String(follow));
            }
            foreach (string key in rRule.Keys)
            {
                foreach (string production in rRule[key])
                {
                    outputLog.InputInfo(toLogName, "对规则式:" + key + "->" + production + "查找右部是否有" + nonTerminal + "\r\n");
                    word = production.Split(' ');
                    for (int i = 0; i < word.Length; i++)
                    {
                        if (word[i] != "")
                        {
                            if (word[i].Equals(nonTerminal))
                            {
                                if (i + 1 == word.Length)
                                {
                                    //nonTerminal在产生式的最后,把产生式左侧的follow集加入nonTerminal
                                    
                                    if (!nonTerminal.Equals(key))
                                    {
                                        outputLog.InputInfo(toLogName, nonTerminal + "在最后，把" + key + "的follow集加入" + nonTerminal);
                                        //tempFollow.AddRange(FollowSet(key));
                                        //以下为了防止E->bE' E'->aE的死循环的出现
                                        if (!isFollowSolved.ContainsKey(nonTerminal))
                                        {

                                            follow[nonTerminal] = tempFollow;
                                            isFollowSolved.Add(nonTerminal, true);
                                        }
                                        List<string> leftSymbolFollow;
                                        if (isFollowSolved.ContainsKey(key))
                                            leftSymbolFollow = follow[key];
                                        else
                                        {
                                            leftSymbolFollow = FollowSet(key);
                                        }
                                        tempFollow = tempFollow.Union(leftSymbolFollow).ToList<string>();
                                        follow[nonTerminal] = tempFollow;
                                        outputLog.InputInfo(toLogName, "#" + Dic2String(follow));
                                    }
                                    if (isToEmpty.ContainsKey(word[i])) {
                                        for (int j = i-1; j >= 0; j--) { 
                                             
                                            if(IsUpperLetter(word[j][0])){
                                                outputLog.InputInfo(toLogName, nonTerminal + "能推出空，把" + key + "的follow集加入" + word[j]);
                                                if (follow.ContainsKey(word[j]))
                                                {
                                                    List<string> temp = follow[word[j]];
                                                    temp = temp.Union(follow[key]).ToList<string>();
                                                    follow[word[j]] = temp;
                                                    outputLog.InputInfo(toLogName, "#" + Dic2String(follow));
                                                }
                                                else {
                                                    follow.Add(word[j], follow[key]);
                                                }
                                                if (!isToEmpty.ContainsKey(word[j]))
                                                    break;
                                            }else{
                                                outputLog.InputInfo(toLogName, word[j] + "是终结符，跳出循环");
                                                break;
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    for (next = i + 1; next < word.Length; next++)
                                    {
                                        if (word[next] != "")
                                        {
                                            if (IsUpperLetter(word[next][0]))
                                            {
                                                outputLog.InputInfo(toLogName, "后一个符号是非终结符，把" + word[next][0] + "的first集加入" + nonTerminal);
                                                tempFollow = tempFollow.Union(first[word[next]]).ToList<string>();
                                                tempFollow.Remove(NULL_SYMBOL);
                                                if (!isToEmpty.ContainsKey(word[next]))
                                                {
                                                    follow[nonTerminal] = tempFollow;
                                                    outputLog.InputInfo(toLogName, "#" + Dic2String(follow));
                                                    break;
                                                }
                                                follow[nonTerminal] = tempFollow;
                                                outputLog.InputInfo(toLogName, "#" + Dic2String(follow));

                                            }
                                            else
                                            {
                                                if (!tempFollow.Contains(word[next]))
                                                {
                                                    tempFollow.Add(word[next]);
                                                    outputLog.InputInfo(toLogName, "后一个符号是终结符，把" + word[next] + "加入" + nonTerminal + "的follow集");
                                                    follow[nonTerminal] = tempFollow;
                                                    outputLog.InputInfo(toLogName, "#" + Dic2String(follow));
                                                }
                                                break;
                                            }

                                        }
                                    }
                                    //自己是最后一个符号
                                    if (next + 1 == word.Length && isToEmpty.ContainsKey(word[next]))
                                    {
                                        if (!word[next].Equals(nonTerminal))
                                        {
                                            if (!isFollowSolved.ContainsKey(nonTerminal))
                                            {
                                                follow[nonTerminal] = tempFollow;
                                                isFollowSolved.Add(nonTerminal, true);
                                            }
                                            List<string> leftSymbolFollow;
                                            if (isFollowSolved.ContainsKey(key))
                                                leftSymbolFollow = follow[key];
                                            else
                                            {
                                                leftSymbolFollow = FollowSet(key);
                                            }
                                            tempFollow = tempFollow.Union(leftSymbolFollow).ToList<string>();
                                            follow[nonTerminal] = tempFollow;
                                            outputLog.InputInfo(toLogName, nonTerminal + "是最后一个符号，把" + key + "的follow集加入" + nonTerminal + "的follow集");
                                            outputLog.InputInfo(toLogName, "#" + Dic2String(follow));
                                        }
                                    }

                                }

                            }

                        }
                    }
                }

            }
            follow[nonTerminal] = tempFollow;
            if (!isFollowSolved.ContainsKey(nonTerminal))
                isFollowSolved.Add(nonTerminal, true);
            return tempFollow;
        }
        private void InputDicToLog(Dictionary<string, List<string>> dic) { 
        
        }


        public List<string> FirstSet(string nonTerminal)
        {
            string[] productionFirstWord;
            string firstWord, word;
            string toLogName = "first";
            //outputLog.ClearLog(toLogName);
            List<string> tempfirst, rightNonTerminalFirst;
            //Console.WriteLine("目标字符:" + nonTerminal);
            outputLog.InputInfo(toLogName,"求" + nonTerminal + "的first集:\r\n");
            if (isFirstSolved.ContainsKey(nonTerminal))//如果求过，则无需再求
            {
                outputLog.InputInfo(toLogName,nonTerminal+"的first集求过，为:" + PrintList(first[nonTerminal]));
                //Console.Write("该字符的first集求过，为:");
                //PrintList(first[nonTerminal]);
                return first[nonTerminal];
            }
            if (!first.ContainsKey(nonTerminal))
            {
                tempfirst = new List<string>();
                first.Add(nonTerminal, tempfirst);
            }
            tempfirst = first[nonTerminal];
            foreach (string production in rRule[nonTerminal])
            {
                //Console.WriteLine("对规则式:" + nonTerminal + "->" + production);
                outputLog.InputInfo(toLogName,"对规则式:" + nonTerminal + "->" + production);

                productionFirstWord = production.Split(' ');
                firstWord = productionFirstWord[0];
                if (IsUpperLetter(firstWord[0]))
                {
                    //不是左递归
                    if (!firstWord.Equals(nonTerminal))
                    {
                        //Console.WriteLine("首字母是非终结符，需要求" + firstWord[0] + "的first集");
                        outputLog.InputInfo(toLogName,"首字母是非终结符，求" + firstWord[0] + "的first集");
                        outputLog.InputInfo(toLogName, "#" + Dic2String(first));
                        rightNonTerminalFirst = FirstSet(firstWord);
                        outputLog.InputInfo(toLogName, "把" + firstWord + "的first集加入" + nonTerminal);
                        if (rightNonTerminalFirst != null)
                        {
                            //tempfirst.AddRange(rightNonTerminalFirst);
                            if (isToEmpty.ContainsKey(firstWord))
                            {
                                //rightNonTerminalFirst.Remove(NULL_SYMBOL);
                                for (int i = 0; i < productionFirstWord.Length; i++)
                                {
                                    word = productionFirstWord[i];
                                    if (word != "")
                                    {
                                        if (IsUpperLetter(firstWord[0]))
                                        {
                                            List<string> rightTempFirst = FirstSet(word);
                                            outputLog.InputInfo(toLogName, "把" + word + "的first集加入" + nonTerminal );
                                            if (isToEmpty.ContainsKey(word))
                                            {
                                                //rightTempFirst.Remove(NULL_SYMBOL);
                                                rightNonTerminalFirst = rightNonTerminalFirst.Union(rightTempFirst).ToList<string>();
                                            }
                                            else
                                            {
                                                rightTempFirst.Remove(NULL_SYMBOL);
                                                rightNonTerminalFirst = rightNonTerminalFirst.Union(rightTempFirst).ToList<string>();
                                                break;
                                            }

                                        }
                                        else
                                        {
                                            rightNonTerminalFirst.Remove(NULL_SYMBOL);
                                            if (!rightNonTerminalFirst.Contains(word))
                                                rightNonTerminalFirst.Add(word);
                                            break;
                                        }
                                    }
                                    if (i + 1 == productionFirstWord.Length)
                                    {
                                        rightNonTerminalFirst.Add(NULL_SYMBOL);
                                        if (!isToEmpty.ContainsKey(nonTerminal))
                                            isToEmpty.Add(nonTerminal, true);
                                    }
                                }
                            }
                            tempfirst = tempfirst.Union(rightNonTerminalFirst).ToList<string>();
                            first[nonTerminal] = tempfirst;
                            outputLog.InputInfo(toLogName, "#" + Dic2String(first));
                        }
                    }
                    else
                        outputLog.InputInfo(toLogName,"左递归，跳过");
                        //Console.WriteLine("左递归，跳过");

                }
                else
                {
                    //Console.WriteLine("首字母是终结符，把" + firstWord[0] +"加入first集");
                    outputLog.InputInfo(toLogName,"首字母是终结符，把" + firstWord[0] + "加入" + nonTerminal + "的first集");
                    if (!tempfirst.Contains(firstWord))
                    {
                        tempfirst.Add(firstWord);
                        first[nonTerminal] = tempfirst;
                        outputLog.InputInfo(toLogName, "#" + Dic2String(first));
                        if (firstWord.Equals(NULL_SYMBOL))
                            if (!isToEmpty.ContainsKey(nonTerminal))
                                isToEmpty.Add(nonTerminal, true);
                    }

                }
                

            }
            //first[nonTerminal] = tempfirst;
            //Console.Write(nonTerminal + "的first集:");
            //PrintList(first[nonTerminal]);

            isFirstSolved.Add(nonTerminal, true);
            return tempfirst;
        }
        public void SolveFirstSet() {
            string toLogName = "first";
            foreach (string key in rRule.Keys)
            {
                //outputLog.InputInfo(key +"的first集为:\r\n");
                //i++;
                FirstSet(key);
                outputLog.InputInfo(toLogName,"\r\n");
            }
        }
        public void SolveFollowSet() {
            foreach (string key in rRule.Keys)
                FollowSet(key);
        }
        private string Dic2String(Dictionary<string, List<string>> dic) {
            string s = "";
            foreach (string key in dic.Keys) {
                s += key + ":";
                foreach (string symbol in dic[key]) { 
                    s += symbol + " ";
                }
                s += "\r\n";
            }
            return s;
        }

        public virtual Dictionary<string, List<string>> GetRuleSet() {

            List<string> temp, production;
            string[] ru = null, tempS;
            temp = ReadRuleByFile(); // 读取文件中的规则

            if (temp != null)
            {
                foreach (string tempRule in temp) //处理规则，即把 A-> a | b分成 A-> a A-> b
                {
                    //Console.WriteLine(tempRule);
                    ru = RuleSplit(tempRule);
                    if (GetTerminalAndNonTerminalSet(ru) == false)
                        return null;
                    foreach (string s in ru)
                    {
                        tempS = Regex.Split(s, "->", RegexOptions.IgnoreCase);
                        tempS[1] = tempS[1].Trim();
                        if (rRule.ContainsKey(tempS[0]))
                        {
                            production = rRule[tempS[0]];
                            if (!production.Contains(tempS[1]))
                                production.Add(tempS[1]);
                            rRule[tempS[0]] = production;
                        }
                        else
                        {
                            production = new List<String>();
                            production.Add(tempS[1]);
                            rRule.Add(tempS[0], production);
                        }

                    }

                }
                if (IsRightNonTerminalSetEqualNonTerminalSet() == false)
                    return null;
                return rRule;
            }
            else
                return null;
        
        }
        public Dictionary<string, List<string>> GetFirstSet() {
            return first;
        }
        public Dictionary<string, List<string>> GetFollowSet() {
            return follow;
        }
        public string Stack2String(Stack<string> stack)
        {
            string output = "栈:";
            foreach (string s in stack.Reverse())
            {
                output += s + " ";
            }
            return output;
        }
        public string PrintList(List<string> lis)
        {
            string temp = "";
            foreach (string s in lis)
                temp = temp + s + ", ";
            return temp;
        }
        /*
        public void PrintStack(Stack<string> stack)
        {
            Console.Write("栈:");
            foreach (string s in stack.Reverse())
                Console.Write("{0} ", s);
        }
        

        public void PrintDic(Dictionary<string, string> dic)
        {
            foreach (string key in dic.Keys)
                Console.Write(key + " | " + dic[key] + "  \n");

        }
        public void PrintDic(Dictionary<string, List<string>> dic)
        {
            foreach (string key in dic.Keys)
            {
                Console.Write(key + ": ");
                foreach (string word in dic[key])
                {
                    Console.Write(word + " | ");
                }
                Console.WriteLine();
            }
        }

        
        */
        public virtual void Analyze(string input){
            
        }

        public virtual bool BuildTable()
        {
            return false;
        }

        public virtual void Run() { 
        
        }
        public Dictionary<string, List<string>> FetchOutput() {
            return outputLog.GetLog();
        }
        public void ClearOutput() {
            outputLog.SetNull();
        }
    }
}
