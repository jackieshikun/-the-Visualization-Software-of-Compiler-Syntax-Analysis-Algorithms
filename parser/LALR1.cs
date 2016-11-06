using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace ConsoleApplication1.parser
{
    public class LALR1 : LR1
    {
        //private Dictionary<string, List<string>> closure; // 存I1/S/aBc(S->aBc)
        //private Dictionary<string, string> goLog;//转移函数goto
        //private Dictionary<string, string> reduceLog;
        //private Dictionary<string, List<string>> processedRule;//存储项目集In中已经处理过的规则
        private Dictionary<string, bool> isRuleCombined;
        public Dictionary<string, List<string>> combinedClosure;
        //private string DOT = ".";
        //private string NAME = "I";
        //private string START = "start";
        //private char SEPERATOR = '_';
        //private char OR = '/';//LR1中，用于分割项目集中的向前符号串 A->a.B, a1/a2/a3...
        //private string ACCEPT = "acc";
        //private string[,] action;
        //private string[,] goTo;
        //private int closureNumber = 1;//项目集的编号，用其记录项目集的编号，如I3

        public LALR1(string f):base(f){
            //closure = new Dictionary<string, List<string>>();
            //goLog = new Dictionary<string, string>();
            //reduceLog = new Dictionary<string, string>();
            //processedRule = new Dictionary<string, List<string>>();
            isRuleCombined = new Dictionary<string, bool>();
            combinedClosure = new Dictionary<string, List<string>>();
        }
        
        public override void Analyze(string input)
        {
            Stack<string> stack = new Stack<string>();
            string[] word;
            string outputName = "LALR1Analyze";
            outputLog.ClearLog(outputName);
            int currentWordIndex = 0, targetRow, targetColumn;
            string stackTop = null, inputFirstWord = null, instruction;
            stack.Push("I1");
            Console.WriteLine("\n\t\t\t\t预测分析程序\t\t\t\t\n");
            word = input.Split(' ');
            if (word[word.Count() - 1] != "$")
            {
                //Console.WriteLine("Error: 输入串应该以$结尾");
                outputLog.InputInfo(outputName, "Error: 输入串应该以$结尾");
                return;
            }
            do
            {
                if (word[currentWordIndex] != "")
                {
                    //PrintStack(stack);
                    //Console.Write("\t\t输入串:");
                    string inputString = "输入串:";
                    for (int i = currentWordIndex; i < word.Count(); i++)
                    {
                        inputString += word[i];
                    }
                    outputLog.InputInfo(outputName, "#" + Stack2String(stack) + SEPERATOR + inputString);
                    /*
                    for (int i = currentWordIndex; i < word.Count(); i++)
                    {
                        Console.Write(word[i]);
                    }
                    Console.ReadKey();
                    Console.WriteLine();
                    */

                    stackTop = stack.Peek();
                    inputFirstWord = word[currentWordIndex];
                    if (terminalSet.Contains(inputFirstWord))
                    {
                        targetColumn = FindTargetFromActionColumn(action, inputFirstWord);
                        targetRow = FindTargetFromRow(action, stackTop);
                        instruction = action[targetRow, targetColumn];
                        if (instruction != null)
                        {
                            if (instruction.Equals(ACCEPT))
                            {
                                //Console.WriteLine("分析成功");
                                outputLog.InputInfo(outputName, "分析成功");
                                break;
                            }
                            else if (instruction[0] == 'r')
                            {
                                string left = instruction.Replace("r", "").Split(SEPERATOR)[1];
                                string right = instruction.Replace("r", "").Split(SEPERATOR)[0];
                                outputLog.InputInfo(outputName, "用" + left + "->" + right + "进行规约");
                                if (right != "")
                                { //如果出现S->.，则不弹出 
                                    foreach (string s in right.Split(' '))
                                    {
                                        stack.Pop();
                                        stack.Pop();
                                    }
                                }

                                stackTop = stack.Peek();

                                targetColumn = FindTargetFromGotoColumn(goTo, left);
                                targetRow = FindTargetFromRow(goTo, stackTop);
                                instruction = goTo[targetRow, targetColumn];
                                if (instruction != null)
                                {
                                    stack.Push(left);
                                    stack.Push(instruction);
                                }
                                else
                                {
                                    //Console.WriteLine("Error: goto表中没有[{0},{1}]的规则", stackTop, left);
                                    outputLog.InputInfo(outputName, "Error: goto表中没有[ " + stackTop + "," + left + " ]的规则");
                                    return;
                                }


                            }
                            else if (instruction[0] == 's')
                            {
                                stack.Push(inputFirstWord);
                                stack.Push(instruction.Replace("s", ""));
                                outputLog.InputInfo(outputName, "移进" + inputFirstWord);
                                currentWordIndex++;
                            }
                            else
                            {
                                //Console.WriteLine("action table有问题");
                                outputLog.InputInfo(outputName, "action table有问题");
                                return;
                            }
                        }
                        else
                        {
                            //Console.WriteLine("Error: action表中没有[{0},{1}]的规则", stackTop, inputFirstWord);
                            outputLog.InputInfo(outputName, "Error: action表中没有[ " + stackTop + "," + inputFirstWord + " ]的规则");
                            return;
                        }
                    }
                    else
                    {
                        //Console.WriteLine("Error:原规则式中不存在{0},无法识别", inputFirstWord);
                        outputLog.InputInfo(outputName, "Error:原规则式中不存在" + inputFirstWord + ",无法识别");
                        return;
                    }

                }
                else
                {
                    currentWordIndex++;
                }
            } while (true);
        }
         
        public void ConvertLR1ToLALR1() {
            string reference, compare,name;
            string[] word;
            string lookAhead;
            string toInputLog = "LALR1Convert";
            outputLog.ClearLog(toInputLog);
            Dictionary<string, List<string>> tempClosure = new Dictionary<string,List<string>>();
            List<string> tempProduction;
            string delClousreName;
            //Console.WriteLine("\t\t\t\t合并项目集\t\t\t\t");
            List<string> key = new List<string>(closure.Keys);
            //找同心集
            for (int i = 0; i < closure.Count(); i++) {
                if (!isRuleCombined.ContainsKey(key[i]))
                {
                    isRuleCombined.Add(key[i], true);
                    reference = closure[key[i]][0];
                    name = key[i];
                    word = reference.Split(SEPERATOR);
                    lookAhead = word[2];
                    compare = word[0] + SEPERATOR + word[1];
                    tempProduction = new List<string>();
                    for (int j = i + 1; j < closure.Count(); j++)
                    {
                        if (!isRuleCombined.ContainsKey(key[j]))
                        {
                            string target = closure[key[j]][0].Split(SEPERATOR)[0] + SEPERATOR + closure[key[j]][0].Split(SEPERATOR)[1];
                            if (target.Equals(compare))
                            {
                                delClousreName = name;
                                isRuleCombined.Add(key[j], true);
                                name = name +  OR  + key[j];
                                lookAhead =  CombineLookAhead(lookAhead,closure[key[j]][0].Split(SEPERATOR)[2]);
                                outputLog.InputInfo(toInputLog, "发现" + delClousreName + "和" + key[j] + "是同心集,合并项目集");
                                string a = "#" + delClousreName + SEPERATOR + key[j] + SEPERATOR + name + SEPERATOR + compare + SEPERATOR + lookAhead;
                                outputLog.InputInfo(toInputLog, "#" + delClousreName + SEPERATOR + key[j] + SEPERATOR + name + SEPERATOR + compare + SEPERATOR + lookAhead);
                                //Console.WriteLine(key[i] + " == " + key[j]);
                            }
                        }
                    }
                    tempProduction.Add(compare + SEPERATOR + lookAhead);
                    tempClosure.Add(name, tempProduction);

                }
            }
            
            Dictionary<string, string> updatedGoLog = new Dictionary<string, string>();
            Dictionary<string, string> updatedReduceLog = new Dictionary<string, string>();
            string originalClosure = null, targetClosure = null;
            //用同心集替换goLog的对应项
            foreach (string tempKey in goLog.Keys){
                    originalClosure = tempKey;
                    targetClosure = goLog[tempKey];
                    foreach (string tempClosureKey in tempClosure.Keys){
                        string targetKey = tempKey.Split(SEPERATOR)[0];
                        foreach (string dividedClosure in tempClosureKey.Split(OR))
                        {
                            if (targetKey.Equals(dividedClosure))
                            { //要分割一下
                                originalClosure = tempClosureKey + SEPERATOR + tempKey.Split(SEPERATOR)[1];
                                break;
                            }
                        }

                        foreach (string dividedClosure in tempClosureKey.Split(OR))
                        {
                            if (goLog[tempKey].Equals(dividedClosure))
                            { //要分割一下
                                targetClosure = tempClosureKey;
                                break;
                            }
                        } 

                       
                }
                if (!updatedGoLog.ContainsKey(originalClosure))
                     updatedGoLog.Add(originalClosure, targetClosure);
            }
            goLog = updatedGoLog;

            originalClosure = null;
            targetClosure = null;
            //用同心集替换reduce表中的相应项
            foreach (string tempKey in reduceLog.Keys)
            {
                originalClosure = tempKey;
                targetClosure = reduceLog[tempKey];
                foreach (string tempClosureKey in tempClosure.Keys)
                {
                    //string targetKey = tempKey.Split(SEPERATOR)[0];
                    foreach (string dividedClosure in tempClosureKey.Split(OR))
                    {
                        if (tempKey.Equals(dividedClosure))
                        { //要分割一下
                            originalClosure = tempClosureKey;
                            string s = targetClosure.Split(SEPERATOR)[0] + SEPERATOR +targetClosure.Split(SEPERATOR)[1];
                            foreach (string tempDividedClosure in tempClosureKey.Split(OR)) {//合并项目集时，需要合并lookahead
                                foreach (string temp in closure[tempDividedClosure])
                                {
                                    string tempS = temp.Replace(" "+DOT,"");
                                    if (tempS.Contains(s))
                                    {
                                        targetClosure = s + SEPERATOR + CombineLookAhead(targetClosure.Split(SEPERATOR)[2],temp.Split(SEPERATOR)[2]);
                                    }
                                }
                            }
                            
                        }
                    }

                }
                if (!updatedReduceLog.ContainsKey(originalClosure))
                    updatedReduceLog.Add(originalClosure, targetClosure);
            }
            reduceLog = updatedReduceLog;

            combinedClosure = tempClosure;
            
            //Console.WriteLine("\t\t\t\t新的reduceLog");
            ///PrintDic(updatedReduceLog);
            
            //Console.WriteLine("\t\t\t\t新的goLog");
            //PrintDic(updatedGoLog);
            //Console.WriteLine("\t\t\t\tLALR1项目集\t\t\t\t");
            //PrintDic(tempClosure);
        }

        public override bool BuildTable()
        {
            goTo = new string[combinedClosure.Count() + 1, nonTerminalSet.Count() + 1];
            action = new string[combinedClosure.Count() + 1, terminalSet.Count() + 1];
            for (int column = 1; column < nonTerminalSet.Count() + 1; column++)
                goTo[0, column] = nonTerminalSet[column - 1];
            for (int column = 1; column < terminalSet.Count() + 1; column++)
                action[0, column] = terminalSet[column - 1];
            int j = 1;
            foreach (string key in combinedClosure.Keys)
            {
                goTo[j,0] = key;
                action[j,0] = key;
                j++;
            }
            string toLogName = "LALR1table";
            outputLog.ClearLog(toLogName);


            string firstKey, secondKey;
            int targetRow, targetColumn;
            int goI = 0, reduceI = 0;
            List<string> goKey = new List<string>(goLog.Keys);
            List<string> reduceKey = new List<string>(reduceLog.Keys);
            foreach (string referenceKey in combinedClosure.Keys) {
                while (goI < goKey.Count) {
                    string first = goKey[goI].Split('_')[0];
                    if (first.Equals(referenceKey))
                    {
                        string key = goKey[goI];
                        firstKey = key.Split(SEPERATOR)[0];
                        secondKey = key.Split(SEPERATOR)[1];
                        if (IsUpperLetter(secondKey[0]))
                        {
                            targetColumn = FindTargetFromGotoColumn(goTo, secondKey);
                            targetRow = FindTargetFromRow(goTo, firstKey);
                            if (targetRow != -1 && targetColumn != -1)
                            {
                                if (goTo[targetRow, targetColumn] == null)
                                {
                                    goTo[targetRow, targetColumn] = goLog[key];
                                    outputLog.InputInfo(toLogName, "根据Automaton,发现" + firstKey + "遇到" + secondKey + "转移到" + goLog[key] + ",填表\r\n");
                                    outputLog.InputInfo(toLogName, "#" + "goto" + SEPERATOR + targetRow + SEPERATOR + targetColumn + SEPERATOR + goLog[key]);
                                }
                                else
                                {
                                    //Console.WriteLine("该文法goto表存在冲突:" + key);
                                    outputLog.InputInfo(toLogName, "该文法goto表存在冲突:\r\n");
                                    return false;
                                    //return;
                                }
                            }
                            else
                            {
                                //Console.WriteLine("该文法的goto表中不存在:" + firstKey + " " + secondKey);
                                outputLog.InputInfo(toLogName, "该文法的goto表中不存在:" + firstKey + " " + secondKey + "\r\n");
                                return false;
                            }

                        }
                        else
                        {
                            targetColumn = FindTargetFromActionColumn(action, secondKey);
                            targetRow = FindTargetFromRow(action, firstKey);
                            if (targetColumn != -1 && targetRow != -1)
                            {
                                if (action[targetRow, targetColumn] == null)
                                {
                                    action[targetRow, targetColumn] = "s" + goLog[key];
                                    outputLog.InputInfo(toLogName, "#" + "action" + SEPERATOR + targetRow + SEPERATOR + targetColumn + SEPERATOR + "s" + goLog[key]);
                                    outputLog.InputInfo(toLogName, "根据Automaton,发现" + firstKey + "遇到" + secondKey + "转移到" + goLog[key] + ",填表\r\n");
                                }
                                else
                                {
                                    //Console.WriteLine("1该文法action表存在冲突:" + key);
                                    outputLog.InputInfo(toLogName, "1该文法action表存在冲突\r\n");
                                    return false;
                                    //return;
                                }
                            }
                            else
                            {
                                //Console.WriteLine("该文法的action表中不存在:" + firstKey + " " + secondKey);
                                outputLog.InputInfo(toLogName, "该文法的action表中不存在:" + firstKey + " " + secondKey + "\r\n");
                                return false;
                            }

                        }
                        goI++;
                    }
                    else
                        break;
                }

                while (reduceI < reduceLog.Count) {
                    string first = reduceKey[reduceI].Split('_')[0];
                    if (first.Equals(referenceKey))
                    {
                        string left;
                        string right;
                        string key = reduceKey[reduceI];
                        right = reduceLog[key].Split(SEPERATOR)[0];
                        left = reduceLog[key].Split(SEPERATOR)[1];
                        if (left.Equals(START))
                        {
                            targetColumn = FindTargetFromActionColumn(action, "$");
                            targetRow = FindTargetFromRow(action, key);
                            if (targetRow != -1 && targetColumn != -1)
                            {
                                if (action[targetRow, targetColumn] == null)
                                {
                                    action[targetRow, targetColumn] = ACCEPT;
                                    outputLog.InputInfo(toLogName, "根据Automaton,发现" + key + "可以规约成" + START + "，置acc\r\n");
                                    outputLog.InputInfo(toLogName, "#" + "action" + SEPERATOR + targetRow + SEPERATOR + targetColumn + SEPERATOR + ACCEPT);
                                }
                                else
                                {
                                    //Console.WriteLine("3该文法action表存在冲突" + key);
                                    outputLog.InputInfo(toLogName, "3该文法action表存在冲突\r\n");
                                    return false;
                                }
                            }
                            else
                            {
                                //Console.WriteLine("该文法的action表中不存在:", key);
                                outputLog.InputInfo(toLogName, "该文法的action表中不存在:" + key + "\r\n");
                                return false;
                            }

                        }
                        else
                        {
                            //List<string> reduce = follow[right];
                            string[] reduce = reduceLog[key].Split(SEPERATOR)[2].Split(OR);
                            foreach (string r in reduce)
                            {
                                targetColumn = FindTargetFromActionColumn(action, r);
                                targetRow = FindTargetFromRow(action, key);
                                if (targetRow != -1 && targetColumn != -1)
                                {
                                    if (action[targetRow, targetColumn] == null)
                                    {
                                        action[targetRow, targetColumn] = "r" + reduceLog[key];
                                        outputLog.InputInfo(toLogName, "根据Automaton，在" + key + "中，" + left + "的follow集有" + r + ",则" + right + "规约成" + left + ",填表\r\n");
                                        outputLog.InputInfo(toLogName, "#" + "action" + SEPERATOR + targetRow + SEPERATOR + targetColumn + SEPERATOR + "r" + left + "->" + right);
                                    }
                                    else
                                    {
                                        //Console.WriteLine("2该文法action表存在冲突" + key);
                                        outputLog.InputInfo(toLogName, "2该文法action表存在冲突" + key + "\r\n");
                                        return false;
                                    }
                                }
                                else
                                {
                                    //Console.WriteLine("2该文法action表存在冲突" + key);
                                    outputLog.InputInfo(toLogName, "该文法action表不存在" + key + "\r\n");
                                    return false;
                                }

                            }
                        }
                        reduceI++;
                    }
                    else
                        break;
                }
            }
            /*
            foreach (string key in goLog.Keys)
            {
                firstKey = key.Split(SEPERATOR)[0];
                secondKey = key.Split(SEPERATOR)[1];
                if (IsUpperLetter(secondKey[0]))
                {
                    targetColumn = FindTargetFromGotoColumn(goTo, secondKey);
                    targetRow = FindTargetFromRow(goTo, firstKey);
                    if (targetRow != -1 && targetColumn != -1)
                    {
                        if (goTo[targetRow, targetColumn] == null)
                        {
                            goTo[targetRow, targetColumn] = goLog[key];
                            outputLog.InputInfo(toLogName, "根据Automaton,发现" + firstKey + "遇到" + secondKey + "转移到" + goLog[key] + "\r\n");
                            outputLog.InputInfo(toLogName, "#" + "goto" + SEPERATOR + targetRow + SEPERATOR + targetColumn + SEPERATOR + goLog[key]);
                        }
                        else
                        {
                            //Console.WriteLine("该文法goto表存在冲突:" + key);
                            outputLog.InputInfo(toLogName, "该文法goto表存在冲突:\r\n");
                            return false;
                            //return;
                        }
                    }
                    else
                    {
                        //Console.WriteLine("该文法的goto表中不存在:" + firstKey + " " + secondKey);
                        outputLog.InputInfo(toLogName, "该文法的goto表中不存在:" + firstKey + " " + secondKey + "\r\n");
                        return false;
                    }

                }
                else
                {
                    targetColumn = FindTargetFromActionColumn(action, secondKey);
                    targetRow = FindTargetFromRow(action, firstKey);
                    if (targetColumn != -1 && targetRow != -1)
                    {
                        if (action[targetRow, targetColumn] == null)
                        {
                            action[targetRow, targetColumn] = "s" + goLog[key];
                            outputLog.InputInfo(toLogName, "#" + "action" + SEPERATOR + targetRow + SEPERATOR + targetColumn + SEPERATOR + "s" + goLog[key]);
                            outputLog.InputInfo(toLogName, "根据Automaton,发现" + firstKey + "遇到" + secondKey + "转移到" + goLog[key] + "\r\n");
                        }
                        else
                        {
                            //Console.WriteLine("1该文法action表存在冲突:" + key);
                            outputLog.InputInfo(toLogName, "1该文法action表存在冲突\r\n");
                            return false;
                            //return;
                        }
                    }
                    else
                    {
                        //Console.WriteLine("该文法的action表中不存在:" + firstKey + " " + secondKey);
                        outputLog.InputInfo(toLogName, "该文法的action表中不存在:" + firstKey + " " + secondKey + "\r\n");
                        return false;
                    }

                }

            }

            string right;
            string left;
            foreach (string key in reduceLog.Keys)
            {
                right = reduceLog[key].Split(SEPERATOR)[0];
                left = reduceLog[key].Split(SEPERATOR)[1];
                if (left.Equals(START))
                {
                    targetColumn = FindTargetFromActionColumn(action, "$");
                    targetRow = FindTargetFromRow(action, key);
                    if (targetRow != -1 && targetColumn != -1)
                    {
                        if (action[targetRow, targetColumn] == null)
                        {
                            action[targetRow, targetColumn] = ACCEPT;
                            outputLog.InputInfo(toLogName, "根据Automaton,发现" + key + "可以规约成" + START + "，置acc\r\n");
                            outputLog.InputInfo(toLogName, "#" + "action" + SEPERATOR + targetRow + SEPERATOR + targetColumn + SEPERATOR + ACCEPT);
                        }
                        else
                        {
                            //Console.WriteLine("3该文法action表存在冲突" + key);
                            outputLog.InputInfo(toLogName, "3该文法action表存在冲突\r\n");
                            return false;
                        }
                    }
                    else
                    {
                        //Console.WriteLine("该文法的action表中不存在:", key);
                        outputLog.InputInfo(toLogName, "该文法的action表中不存在:" + key + "\r\n");
                        return false;
                    }

                }
                else
                {
                    //List<string> reduce = follow[right];
                    string[] reduce = reduceLog[key].Split(SEPERATOR)[2].Split(OR);
                    foreach (string r in reduce)
                    {
                        targetColumn = FindTargetFromActionColumn(action, r);
                        targetRow = FindTargetFromRow(action, key);
                        if (targetRow != -1 && targetColumn != -1)
                        {
                            if (action[targetRow, targetColumn] == null)
                            {
                                action[targetRow, targetColumn] = "r" + reduceLog[key];
                                outputLog.InputInfo(toLogName, "根据Automaton，在" + key + "中，" + left + "的follow集有" + r + ",则" + right + "规约成" + left + "\r\n");
                                outputLog.InputInfo(toLogName, "#" + "action" + SEPERATOR + targetRow + SEPERATOR + targetColumn + SEPERATOR + "r" + left + "->" + right);
                            }
                            else
                            {
                                //Console.WriteLine("2该文法action表存在冲突" + key);
                                outputLog.InputInfo(toLogName, "2该文法action表存在冲突" + key + "\r\n");
                                return false;
                            }
                        }
                        else
                        {
                            //Console.WriteLine("2该文法action表存在冲突" + key);
                            outputLog.InputInfo(toLogName, "该文法action表不存在" + key + "\r\n");
                            return false;
                        }

                    }
                }

            }
            */
            return true;
           
        }
        /*
        public override void Run()
        {
            List<string> temp, production;
            string[] ru = null, tempS;
            //string[,] anaTable;
            bool isStartSym = true;
            temp = ReadRuleByFile(); // 读取文件中的规则

            if (temp != null)
            {
                foreach (string tempRule in temp) //处理规则，即把 A-> a | b分成 A-> a A-> b
                {
                    //Console.WriteLine(tempRule);
                    ru = RuleSplit(tempRule);
                    GetTerminalAndNonTerminalSet(ru);
                    foreach (string s in ru)
                    {
                        tempS = Regex.Split(s, "->", RegexOptions.IgnoreCase);

                        if (isStartSym)
                        { //拓广文法
                            List<string> p = new List<string>();
                            string right = tempS[0];

                            //Console.WriteLine("aaaaa" + right);
                            p.Add(right);
                            rRule.Add(START, p);
                            isStartSym = false;
                        }

                        tempS[1] = RemoveSpace(tempS[1]).Trim();
                        //tempS[1] = DotOperation(tempS[1]);
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
                PrintTerminalAndNonTerminalSet();

                IsRightNonTerminalSetEqualNonTerminalSet();
                Console.WriteLine("\n\t\t\t\tFirst集\t\t\t\t");
                foreach (string key in rRule.Keys)
                    FirstSet(key);
                PrintDic(first);
                Console.WriteLine("\t\t\t\tFollow集\t\t\t\t");
                foreach (string key in rRule.Keys)
                    FollowSet(key);
                PrintDic(follow);

                Console.ReadKey();



                Console.WriteLine("\n\t\t\t\t项目集\t\t\t\t");

                BuildLR1(DOT + nonTerminalSet[0] + SEPERATOR + START + SEPERATOR + '$', null);
                PrintDic(closure);

                ConvertLR1ToLALR1();

                Console.WriteLine("\n\t\t\t\tgoto关系\t\t\t\t");
                PrintDic(goLog);
                Console.WriteLine("\n\t\t\t\treduce关系\t\t\t");
                PrintDic(reduceLog);

                


                BuildTable();
                //Console.ReadKey();

                StreamReader fileReader;//分析输入串
                if (System.IO.File.Exists("input.txt"))
                {
                    fileReader = new StreamReader("input.txt");
                }
                else
                {
                    Console.WriteLine("input文件不存在");
                    return;
                }
                while (fileReader.Peek() != -1)
                {
                    string input = fileReader.ReadLine();
                    Analyze(input);
                }
            }
        }
         * */

    }
}
