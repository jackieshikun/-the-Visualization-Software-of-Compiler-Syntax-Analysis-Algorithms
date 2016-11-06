using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace ConsoleApplication1.parser
{
    //bug： 如果某个非终结符能推出空的话，那么getclosure可能出现死循环，因为combine函数中，并没有对加入的字符串进行去重操作
    
    public class LR1 : Parsing
    {
        public Dictionary<string, List<string>> closure; // 存I1/S/aBc(S->aBc)
        public Dictionary<string, string> goLog;//转移函数goto
        public Dictionary<string, string> reduceLog;
        public Dictionary<string, List<string>> processedRule;//存储项目集In中已经处理过的规则
        private List<string> queue;
        public string DOT = ".";
        public string NAME = "I";
        public string START = "start";
        public char SEPERATOR = '_';
        public char OR = '/';//LR1中，用于分割项目集中的向前符号串 A->a.B, a1/a2/a3...
        public string ACCEPT = "acc";
        public char gotoSymbol = '#';
        public string[,] action;
        public string[,] goTo;
        public int closureNumber = 1;//项目集的编号，用其记录项目集的编号，如I3
        
        public LR1(string f):base(f){
            closure = new Dictionary<string, List<string>>();
            goLog = new Dictionary<string, string>();
            reduceLog = new Dictionary<string, string>();
            processedRule = new Dictionary<string, List<string>>();
            queue = new List<string>();
        }

        
        public int FindTargetFromGotoColumn(string[,] tempTable, string target)
        {
            int column;
            for (column = 1; column < nonTerminalSet.Count() + 1; column++)
            {
                if (tempTable[0, column].Equals(target))
                {
                    return column;
                }
            }
            return -1;
        }
        public int FindTargetFromActionColumn(string[,] tempTable, string target)
        {
            int column;
            for (column = 1; column < terminalSet.Count() + 1; column++)
            {
                if (tempTable[0, column].Equals(target))
                {
                    return column;
                }
            }
            return -1;
        }
        public int FindTargetFromRow(string[,] tempTable, string target)
        {
            int row;
            for (row = 1; row < closureNumber + 1; row++)
            {
                if (tempTable[row, 0].Equals(target))
                {
                    return row;
                }
            }
            return -1;
        }

        //对给定的rule，没点加点，有点移点，点在rule尾则返回null
        public string DotOperation(string rule)
        {
            string returnString = null;
            string[] production;
            bool isNextNumber = false;
            if (!rule.Contains(DOT))
            {
                returnString = rule.Insert(0, DOT);

            }
            else
            {
                production = rule.Split(' ');
                if (production[production.Count() - 1] == DOT)//是规约式，不需要移动圆点
                    return null;
                for (int i = 0; i < production.Count(); i++)
                {
                    if (production[i].Contains(DOT))
                    {
                        returnString += production[i].Replace(DOT, "") + " ";
                        isNextNumber = true;
                        if (i + 1 == production.Count())
                        {
                            returnString += DOT;

                        }
                    }
                    else
                    {
                        if (isNextNumber)
                        {
                            production[i] = DOT + production[i];
                            isNextNumber = false;
                        }
                        returnString += production[i] + " ";

                    }
                }
            }

            return returnString.Trim();
        } 
        //把target中的项一一加到source中，用OR分割。
        //用于把某非终结符的first集加到向前看符号中去
        public string CombineLookAhead(List<string> target,string source) {
            source = source.Trim();
            target.Remove(NULL_SYMBOL);
            foreach(string word in target){
                if (!source.Contains(word))
                {
                    if (source.Equals(""))
                    {
                        source = word;
                    }
                    else
                        source = source + OR + word;
                }
            }
            return source;
        }
        public string CombineLookAhead(string target, string source)
        {
            foreach (string s in target.Split(OR))
            {
                if (!source.Contains(s))
                {
                    source = source + OR + s;
                }
            }
            return source;
        }
        

        //去掉构建Closure中可能出现的重复项，比如该文法 L ->*R R->L (出现这个的原因是每一项后面加了_lookahead)
        public List<string> RemoveRepeatedTerm(List<string> original) {
            string reference,referenceLookAhead;
            string target,targetLookAhead;
            for (int i = 0; i < original.Count(); i++) {
                reference = original[i].Split(SEPERATOR)[0] + SEPERATOR + original[i].Split(SEPERATOR)[1];
                referenceLookAhead = original[i].Split(SEPERATOR)[2];
                for (int j = i + 1; j < original.Count(); j++) {
                    target = original[j].Split(SEPERATOR)[0] + SEPERATOR + original[j].Split(SEPERATOR)[1];
                    targetLookAhead = original[j].Split(SEPERATOR)[2];
                    if (reference.Equals(target) && !referenceLookAhead.Equals(targetLookAhead))
                    {
                        referenceLookAhead = CombineLookAhead(referenceLookAhead, targetLookAhead);
                        original[i] = reference + SEPERATOR + referenceLookAhead;
                        //original.Remove(original[j]);
                        original.RemoveAt(j);
                        j--;
                    }
                    else if(original[i].Equals(original[j])){
                        //original.Remove(original[j]);
                        original.RemoveAt(j);
                        j--;
                    }
                }
            }
            return original;
        }
        
        //目的：求出一个非终结符nonTerminal的closure
        //1. 取出nonTerminal对应的一个产生式，打上.，再把这个产生式存到list里
        //2. 取出这个产生式的第一个单词，求这个单词的closure，再把这个closure加到list里
        //3. 如果nonTerminal还有没取出来的产生式，则回到1
        public List<string> GetClosure(string nonTerminal,string nextSymbol, string originalLookAhead, ref List<string> productions)
        {
            //List<string> productions = new List<string>();
            string firstWord;
            string[] word;
            if (!nonTerminalSet.Contains(nonTerminal))
                return null;
            else
            {
                foreach (string p in rRule[nonTerminal])
                {
                    word = p.Split(' ');
                    firstWord = word[0];
                    string lookAhead = originalLookAhead;
                    if (nextSymbol != null)
                    {
                        if (IsUpperLetter(nextSymbol[0]))
                        {
                            if (isToEmpty.ContainsKey(nextSymbol))
                            {
                                lookAhead = CombineLookAhead(first[nextSymbol], originalLookAhead);
                            }
                            else
                            {
                                lookAhead = CombineLookAhead(first[nextSymbol], "");
                            }
                        }
                        else
                        {
                            //lookAhead = CombineLookAhead(nextSymbol, lookAhead);
                            lookAhead = nextSymbol; 
                        }
                    }
                    string addDotP = DotOperation(p) + SEPERATOR + nonTerminal + SEPERATOR + lookAhead;
                    if (!productions.Contains(addDotP)) {
                        productions.Add(addDotP);
                        if (word.Length > 1)
                        {
                            GetClosure(firstWord, word[1], lookAhead,ref productions);
                        }
                        else
                        {
                            GetClosure(firstWord, null, lookAhead, ref productions);
                        }
                    }
                    
                }

            }
            return productions;
        }
        public void BuildLR1ForView()
        {
            BuildClosure(DOT + nonTerminalSet[0] + SEPERATOR + START + SEPERATOR + '$', null);
            BuildLR1(DOT + nonTerminalSet[0] + SEPERATOR + START + SEPERATOR + '$', "I1");
        }
        private void BuildClosure(string firstProduction, string targetClosure)
        {
            if (!firstProduction.Contains(DOT))
            {
                Console.WriteLine("LR1 输入产生式有问题");
                return;
            }
            string toInputLog = "LR1LR0";
            //outputLog.ClearLog(toInputLog);
            string number;
            if (targetClosure == null)
            {
                number = NAME + closureNumber;
                List<string> pRule = new List<string>();
                processedRule.Add(number, pRule);
            }
            else
            {
                number = targetClosure;
            }
            string left = firstProduction.Split(SEPERATOR)[1];
            string right = firstProduction.Split(SEPERATOR)[0];
            string lookAhead = firstProduction.Split(SEPERATOR)[2];
            if (right.Contains(NULL_SYMBOL))
                right = DOT;
            outputLog.InputInfo(toInputLog, "根据规则" + left + "->" + right + "构建" + number + "的closure集");
            string[] word = right.Split(' ');
            List<string> tempProduction = new List<string>();


            for (int i = 0; i < word.Count(); i++)
            {
                if (word[i].Contains(DOT))
                {
                    //firstRule = DotOperation(word[i]); //.不在rule的最后，则firstRule ！= null
                    tempProduction.Add(firstProduction);
                    if (!word[i].Equals(DOT))//word[i]不是DOT
                    {

                        //把word[i+1]的first集加到/替换到lookahead中
                        List<string> p = new List<string>();
                        if (i + 1 != word.Length)
                        {
                            GetClosure(word[i].Replace(DOT, ""), word[i + 1], lookAhead, ref p);//word[i]是终结符的话，p=null
                        }
                        else
                        {
                            GetClosure(word[i].Replace(DOT, ""), null, lookAhead, ref p);
                        }

                        if (p != null)
                        {
                            p = RemoveRepeatedTerm(p);
                            tempProduction.AddRange(p);
                        }


                    }
                    if (targetClosure == null)
                        closure.Add(number, tempProduction);
                    else//项目集已经存在
                    {
                        closure[number] = closure[number].Union(tempProduction).ToList<string>();
                    }
                    break;
                }
            }
        }
        public void BuildLR1(string firstProduction, string number)
        {
            string toInputLog = "LR1LR0";
            string dotMovedS;
            string target;
            string original = null;
            string targetName;
            string lookAhead;
            foreach (string s in closure[number])
            {
                if (!processedRule[number].Contains(s))
                {
                    processedRule[number].Add(s);
                    string l = s.Split(SEPERATOR)[1];
                    string r = s.Split(SEPERATOR)[0];
                    lookAhead = s.Split(SEPERATOR)[2];
                    if (r.Contains(NULL_SYMBOL))
                        r = DOT;
                    foreach (string p in r.Split(' ')) //记录go函数的(In, N)中的N
                        if (p.Contains(DOT))
                        {
                            original = number + SEPERATOR + p.Replace(DOT, "");
                            break;
                        }
                    dotMovedS = DotOperation(r);
                    targetName = dotMovedS + SEPERATOR + l + SEPERATOR + lookAhead;
                    if (dotMovedS != null)
                    {//非规约项目

                        foreach (string w in dotMovedS.Split(' '))
                        {
                            if (w.Equals(DOT))
                            {
                                if (!goLog.ContainsKey(original))
                                {
                                    target = RuleExistedInClosure(targetName);//查看点移动后的项是否存在已有项目集中
                                    if (target == null)
                                    {
                                        closureNumber++;
                                        outputLog.InputInfo(toInputLog, "由" + l + "->" + r + "推出" + original + "->" + NAME + closureNumber);
                                        outputLog.InputInfo(toInputLog, gotoSymbol + original + SEPERATOR + NAME + closureNumber);
                                        goLog.Add(original, NAME + closureNumber);
                                        BuildClosure(targetName, null);
                                        //BuildLR1(targetName, null);
                                        queue.Add(targetName + SEPERATOR + NAME + closureNumber);
                                    }
                                    else
                                    {
                                        outputLog.InputInfo(toInputLog, "由" + l + "->" + r + "推出" + original + "->" + target);
                                        outputLog.InputInfo(toInputLog, gotoSymbol + original + SEPERATOR + target);
                                        goLog.Add(original, target);
                                    }

                                }
                                else
                                {
                                    //BuildLR1(targetName, goLog[original]);
                                    BuildClosure(targetName, goLog[original]);
                                    queue.Add(targetName + SEPERATOR + goLog[original]);
                                }

                            }
                            else if (w.Contains(DOT))
                            {
                                string temp = w.Replace(DOT, "");

                                if (IsUpperLetter(temp[0]))
                                {
                                    if (!goLog.ContainsKey(original))
                                    {
                                        target = RuleExistedInClosure(targetName);//查看点移动后的项是否存在已有项目集中
                                        if (target == null)
                                        {
                                            closureNumber++;
                                            outputLog.InputInfo(toInputLog, "由" + l + "->" + r + "推出" + original + "->" + NAME + closureNumber);
                                            outputLog.InputInfo(toInputLog, gotoSymbol + original + SEPERATOR + NAME + closureNumber);
                                            goLog.Add(original, NAME + closureNumber);
                                            //BuildLR1(targetName, null);
                                            BuildClosure(targetName, null);
                                            queue.Add(targetName + SEPERATOR + NAME + closureNumber);
                                        }
                                        else
                                        {
                                            outputLog.InputInfo(toInputLog, "由" + l + "->" + r + "推出" + original + "->" + target);
                                            outputLog.InputInfo(toInputLog, gotoSymbol + original + SEPERATOR + target);
                                            goLog.Add(original, target);
                                        }

                                    }
                                    else
                                    {
                                        //BuildLR1(targetName, goLog[original]);
                                        BuildClosure(targetName, goLog[original]);
                                        queue.Add(targetName + SEPERATOR + goLog[original]);
                                    }

                                }
                                else
                                {
                                    if (!goLog.ContainsKey(original))
                                    {
                                        target = RuleExistedInClosure(targetName);//查看点移动后的项是否存在已有项目集中
                                        if (target == null)
                                        {
                                            closureNumber++;
                                            outputLog.InputInfo(toInputLog, "由" + l + "->" + r + "推出" + original + "->" + NAME + closureNumber);
                                            outputLog.InputInfo(toInputLog, gotoSymbol + original + SEPERATOR + NAME + closureNumber);
                                            goLog.Add(original, NAME + closureNumber);
                                            //BuildLR1(targetName, null);
                                            BuildClosure(targetName, null);
                                            queue.Add(targetName + SEPERATOR + NAME + closureNumber);
                                        }
                                        else
                                        {
                                            outputLog.InputInfo(toInputLog, "由" + l + "->" + r + "推出" + original + "->" + target);
                                            outputLog.InputInfo(toInputLog, gotoSymbol + original + SEPERATOR + target);
                                            goLog.Add(original, target);
                                        }

                                    }
                                    else
                                    {
                                        //BuildLR1(targetName, goLog[original]);
                                        BuildClosure(targetName, goLog[original]);
                                        queue.Add(targetName + SEPERATOR + goLog[original]);
                                    }

                                }
                            }
                        }


                    }
                    else
                    {//规约项目
                        string p = r.Replace(DOT, "").Trim();
                        reduceLog.Add(number, p + SEPERATOR + l + SEPERATOR + lookAhead);
                        //p + number
                    }



                }
            }
            while (queue.Count != 0) {
                string[] words = queue[0].Split('_');
                queue.Remove(queue[0]);
                BuildLR1(words[0] + SEPERATOR + words[1] + SEPERATOR + words[2], words[3]);
            }
        }

        public string RuleExistedInClosure(string rule)
        {
            foreach (string key in closure.Keys)
            {
                if (closure[key][0].Equals(rule))
                {
                    return key;
                }
            }
            return null;
        }
        
        public override void Analyze(string input){
            Stack<string> stack = new Stack<string>();
            string[] word;
            string outputName = "LR1Analyze";
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
                    //Console.ReadKey();
                    //Console.WriteLine();


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
        public string[,] GetGotoTable() {
            return goTo;
        }
        public string[,] GetActionTable() {
            return action;
        }
        public override bool BuildTable() {
            goTo = new string[closureNumber + 1, nonTerminalSet.Count() + 1];
            action = new string[closureNumber + 1, terminalSet.Count() + 1];
            for (int column = 1; column < nonTerminalSet.Count() + 1; column++)
                goTo[0, column] = nonTerminalSet[column - 1];
            for (int column = 1; column < terminalSet.Count() + 1; column++)
                action[0, column] = terminalSet[column - 1];
            for (int row = 1; row < closureNumber + 1; row++)
            {
                goTo[row, 0] = NAME + (row);
                action[row, 0] = NAME + (row);
            }
            string toLogName = "LR1table";

            string firstKey, secondKey;
            int targetRow, targetColumn;
            int goI = 0, reduceI = 0;
            List<string> goKey = new List<string>(goLog.Keys);
            List<string> reduceKey = new List<string>(reduceLog.Keys);
            for (int i = 1; i <= closureNumber; i++) {
                while (goI < goKey.Count) {
                    string first = goKey[goI].Split('_')[0];
                    if (first.Equals(NAME + i))
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
                                    outputLog.InputInfo(toLogName, "该文法goto表存在冲突:\r\n");
                                    return false;
                                    //Console.WriteLine("该文法goto表存在冲突:" + key);
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
                                outputLog.InputInfo(toLogName, "该文法的action表中不存在:" + firstKey + " " + secondKey + "\r\n");
                                return false;
                                //Console.WriteLine("该文法的action表中不存在:" + firstKey + " " + secondKey);
                            }

                        }
                        goI++;
                    }
                    else
                        break;
                }
                while (reduceI < reduceKey.Count) {
                    string first = reduceKey[reduceI].Split('_')[0];
                    if (first.Equals(NAME + i))
                    {
                        string key = reduceKey[reduceI];
                        string right;
                        string left;
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
                                    outputLog.InputInfo(toLogName, "根据Automaton,发现" + key + "可以规约成" + START + "，置acc,填表\r\n");
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
                            outputLog.InputInfo(toLogName, "该文法goto表存在冲突:\r\n");
                            return false;
                            //Console.WriteLine("该文法goto表存在冲突:" + key);
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
                        outputLog.InputInfo(toLogName, "该文法的action表中不存在:" + firstKey + " " + secondKey + "\r\n");
                        return false;
                        //Console.WriteLine("该文法的action表中不存在:" + firstKey + " " + secondKey);
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
        public override void Run() {
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

                Console.WriteLine("\n\t\t\t\tgoto关系\t\t\t\t");
                PrintDic(goLog);
                Console.WriteLine("\n\t\t\t\treduce关系\t\t\t");
                PrintDic(reduceLog);

                BuildTable();

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
