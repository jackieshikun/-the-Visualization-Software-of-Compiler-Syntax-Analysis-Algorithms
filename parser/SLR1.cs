using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

//问题1： 推出空怎么办
//bug： 加了拓广文法后，follow集似乎不对了
namespace ConsoleApplication1.parser
{
    
    public class SLR1:Parsing
    {
        public Dictionary<string, List<string>> closure; // 存I1/S/aBc(S->aBc)
        private Dictionary<string, string> goLog;//转移函数goto
        private Dictionary<string, string> reduceLog;
        private Dictionary<string, List<string>> processedRule;//存储项目集In中已经处理过的规则
        private List<string> queue;//用于广度优先
        private string DOT = ".";
        private string NAME = "I";
        private string START = "start";
        private char SEPERATOR = '_';
        private string ACCEPT = "acc";
        private char gotoSymbol = '#';
        private string[,] action;
        private string[,] goTo;
        private int closureNumber = 1;//项目集的编号，用其记录项目集的编号，如I3
        public SLR1(string f):base(f) {
            closure = new Dictionary<string, List<string>>();       
            goLog = new Dictionary<string, string>();
            reduceLog = new Dictionary<string, string>();          
            processedRule = new Dictionary<string, List<string>>();
            queue = new List<string>();
        }
        

        
        //对给定的rule，没点加点，有点移点，点在rule尾则返回null
        private string DotOperation(string rule){
            string returnString = null;
            string[] production;
            bool isNextNumber = false;
            if (!rule.Contains(DOT))
            {
                returnString = rule.Insert(0,DOT);
                
            }
            else{
                production = rule.Split(' ');
                if (production[production.Count() - 1] == DOT )//是规约式，不需要移动圆点
                    return null;
                for(int i = 0; i < production.Count() ; i++){
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
       
        
        //目的：求出一个非终结符nonTerminal的closure
        //1. 取出nonTerminal对应的一个产生式，打上.，再把这个产生式存到list里
        //2. 取出这个产生式的第一个单词，求这个单词的closure，再把这个closure加到list里
        //3. 如果nonTerminal还有没取出来的产生式，则回到1
        private List<string> GetClosure(string nonTerminal) {
            List<string> productions = new List<string>();
            string firstWord;
            //string toInputLog = "SLR1LR0";
            if (!nonTerminalSet.Contains(nonTerminal))
                return null;
            else {
                foreach (string p in rRule[nonTerminal]) {
                    firstWord = p.Split(' ')[0];
                    string addDotP = DotOperation(p) + SEPERATOR + nonTerminal;
                    if (DotOperation(p) != null && !productions.Contains(addDotP))
                        productions.Add(addDotP);
                    if (!firstWord.Equals(nonTerminal)) {
                        List<string> temp = GetClosure(firstWord);
                        if (temp != null)
                            productions = productions.Union(temp).ToList<string>();
                    }
                    
                }
            
            }
            return productions;
        }
        
        private string RuleExistedInClosure(string rule) {
            foreach (string key in closure.Keys) {
                if (closure[key][0].Equals(rule) ) {
                    return key;
                }
            }
            return null;
        }
        //构建LR0项目簇
        //1.首先找到。对应的位置，从而求出本项目簇的closure集
        //2.然后取出本项目簇的每一个规则，求出下一个项目簇
        //firstProduction是输入的规则，targetClosure表示这个rule应该放在哪个closure里。
        //如果targetClosure为null，表示存在新的closure里，否则存在targetClosure
        //这么做是因为如果一个closure里有S-> .E+T | .E 时，明显S-> E. + T 和 S-> E.应该在一个closure里。
        private void BuildClosure(string firstProduction, string targetClosure) {
            if (!firstProduction.Contains(DOT))
            {
                Console.WriteLine("LR0 输入产生式有问题");
                return;
            }
            string toInputLog = "SLR1LR0";
            //outputLog.ClearLog(toInputLog);
            string number;
            if (targetClosure == null)
            {
                number = NAME + closureNumber;
                //outputLog.InputInfo(toInputLog,number);
                if (!processedRule.ContainsKey(number))
                {
                    List<string> pRule = new List<string>();
                    processedRule.Add(number, pRule);
                }
            }
            else
            {
                number = targetClosure;
            }
            string left = firstProduction.Split(SEPERATOR)[1];
            string right = firstProduction.Split(SEPERATOR)[0];
            if (right.Contains(NULL_SYMBOL))
                right = DOT;
            outputLog.InputInfo(toInputLog, "根据规则" + left + "->" + right + "构建" + number + "的closure集");
            string[] word = right.Split(' ');
            List<string> tempProduction = new List<string>();
            //string firstRule = null;

            for (int i = 0; i < word.Count(); i++)
            {
                if (word[i].Contains(DOT))
                {
                    //firstRule = DotOperation(word[i]); //.不在rule的最后，则firstRule ！= null
                    tempProduction.Add(firstProduction);
                    if (!word[i].Equals(DOT))
                    {
                        List<string> p;
                        p = GetClosure(word[i].Replace(DOT, ""));//word[i]是终结符的话，p=null
                        if (p != null)
                        {
                            tempProduction.AddRange(p);
                        }

                    }
                    if (targetClosure == null)
                        closure.Add(number, tempProduction);
                    else
                    {//项目集已经存在
                        closure[number] = closure[number].Union(tempProduction).ToList<string>();
                    }
                    break;
                }
            }
        }
        private void BuildLR0(string firstProduction , string number) {
            string toInputLog = "SLR1LR0";
            string dotMovedS;
            string target;
            string original = null;
            foreach (string s in closure[number]) {
                if (!processedRule[number].Contains(s)) {
                    processedRule[number].Add(s);
                    string l = s.Split(SEPERATOR)[1];
                    string r = s.Split(SEPERATOR)[0];
                    if (r.Contains(NULL_SYMBOL))
                        r = DOT;
                    foreach (string p in r.Split(' ')) //记录go函数的(In, N)中的N
                        if (p.Contains(DOT))
                        {
                            original = number + SEPERATOR + p.Replace(DOT, "");
                        }
                    dotMovedS = DotOperation(r);
                    if (dotMovedS != null)
                    {//非规约项目
                        foreach (string w in dotMovedS.Split(' '))
                        {
                            if (w.Equals(DOT))
                            {
                                if (!goLog.ContainsKey(original))
                                {
                                    target = RuleExistedInClosure(dotMovedS + SEPERATOR + l);//查看点移动后的项是否存在已有项目集中
                                    if (target == null)
                                    {
                                        closureNumber++;
                                        outputLog.InputInfo(toInputLog, "由" + l + "->" + r + "推出" + original + "->" + NAME + closureNumber);
                                        outputLog.InputInfo(toInputLog,gotoSymbol + original + SEPERATOR + NAME + closureNumber);
                                        goLog.Add(original, NAME + closureNumber);
                                        //BuildLR0(dotMovedS + SEPERATOR + l, null);
                                        //closureNumber++;
                                        BuildClosure(dotMovedS + SEPERATOR + l, null);
                                        queue.Add(dotMovedS + SEPERATOR + l + SEPERATOR + NAME + closureNumber);
                                    }
                                    else
                                    {
                                        outputLog.InputInfo(toInputLog, "由" + l + "->" + r + "推出" + original + "->" + target);
                                        outputLog.InputInfo(toInputLog,gotoSymbol + original + SEPERATOR + target);
                                        goLog.Add(original, target);
                                        //queue.Add(" " + SEPERATOR + " " + SEPERATOR + " " + SEPERATOR + original + SEPERATOR + target + SEPERATOR + l + SEPERATOR + r);
                                    }
                                    
                                }
                                else
                                {
                                    //BuildLR0(dotMovedS + SEPERATOR + l, goLog[original]);
                                    BuildClosure(dotMovedS + SEPERATOR + l, goLog[original]);
                                    queue.Add(dotMovedS + SEPERATOR + l + SEPERATOR + goLog[original]);
                                }
                                
                                

                            }
                            else if (w.Contains(DOT))
                            {
                                string temp = w.Replace(DOT, "");

                                if (IsUpperLetter(temp[0]))
                                {
                                    if (!goLog.ContainsKey(original))
                                    {
                                        target = RuleExistedInClosure(dotMovedS + SEPERATOR + l);//查看点移动后的项是否存在已有项目集中
                                        if (target == null)
                                        {
                                            closureNumber++;
                                            outputLog.InputInfo(toInputLog, "由" + l + "->" + r + "推出" + original + "->" + NAME + closureNumber);
                                            outputLog.InputInfo(toInputLog,gotoSymbol + original + SEPERATOR + NAME + closureNumber);
                                            goLog.Add(original, NAME + closureNumber);
                                            //BuildLR0(dotMovedS + SEPERATOR + l, null);
                                            //closureNumber++;
                                            BuildClosure(dotMovedS + SEPERATOR + l, null);
                                            queue.Add(dotMovedS + SEPERATOR + l + SEPERATOR + NAME + closureNumber);
                                        }
                                        else
                                        {
                                            outputLog.InputInfo(toInputLog, "由" + l + "->" + r + "推出" + original + "->" + target);
                                            outputLog.InputInfo(toInputLog,gotoSymbol + original + SEPERATOR + target);
                                            goLog.Add(original, target);
                                            //queue.Add(" " + SEPERATOR + " " + SEPERATOR + " " + SEPERATOR + original + SEPERATOR + target + SEPERATOR + l + SEPERATOR + r);
                                        }
                                        
                                    }
                                    else
                                    {
                                        //BuildLR0(dotMovedS + SEPERATOR + l, goLog[original]);
                                        BuildClosure(dotMovedS + SEPERATOR + l, goLog[original]);
                                        //queue.Add(dotMovedS + SEPERATOR + l + SEPERATOR + goLog[original]);
                                    }

                                }
                                else
                                {
                                    if (!goLog.ContainsKey(original))
                                    {
                                        target = RuleExistedInClosure(dotMovedS + SEPERATOR + l);//查看点移动后的项是否存在已有项目集中
                                        if (target == null)
                                        {
                                            closureNumber++;
                                            outputLog.InputInfo(toInputLog, "由" + l + "->" + r + "推出" + original + "->" + NAME + closureNumber);
                                            outputLog.InputInfo(toInputLog,gotoSymbol + original + SEPERATOR + NAME + closureNumber);
                                            goLog.Add(original, NAME + closureNumber);
                                            //BuildLR0(dotMovedS + SEPERATOR + l, null);
                                            //closureNumber++;
                                            BuildClosure(dotMovedS + SEPERATOR + l, null);
                                            queue.Add(dotMovedS + SEPERATOR + l + SEPERATOR + NAME + closureNumber);
                                        }
                                        else
                                        {
                                            outputLog.InputInfo(toInputLog, "由" + l + "->" + r + "推出" + original + "->" + target);
                                            outputLog.InputInfo(toInputLog,gotoSymbol + original + SEPERATOR + target);
                                            goLog.Add(original, target);
                                            //queue.Add(" " + SEPERATOR + " " + SEPERATOR + " " + SEPERATOR + original + SEPERATOR + target + SEPERATOR + l + SEPERATOR + r);
                                        }
                                        
                                    }
                                    else
                                    {
                                        //BuildLR0(dotMovedS + SEPERATOR + l, goLog[original]);
                                        BuildClosure(dotMovedS + SEPERATOR + l, goLog[original]);
                                        queue.Add(dotMovedS + SEPERATOR + l + SEPERATOR + goLog[original]);
                                    }

                                }
                            }
                        }


                    }else{//规约项目
                        string p = r.Replace(DOT, "").Trim();
                        reduceLog.Add(number, p + SEPERATOR + l);
                        //p + number
                    }   
                }
            }
            while(queue.Count != 0) {
                string[] items = queue[0].Split('_');
                queue.Remove(queue[0]);
                //closureNumber++;
                BuildLR0(items[0] + SEPERATOR + items[1], items[2]);       
                
            }
                
        }
        private int FindTargetFromGotoColumn(string[,] tempTable, string target)
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
        private int FindTargetFromActionColumn(string[,] tempTable, string target)
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
        private int FindTargetFromRow(string[,] tempTable, string target)
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

        public override bool BuildTable() {
            goTo = new string[closureNumber + 1,nonTerminalSet.Count() + 1];
            action = new string[closureNumber + 1, terminalSet.Count() + 1];
            for (int column = 1; column < nonTerminalSet.Count()+1; column++)
                goTo[0, column] = nonTerminalSet[column-1];
            for (int column = 1; column < terminalSet.Count()+1; column++)
                action[0, column] = terminalSet[column - 1];
            for (int row = 1; row < closureNumber+1; row++) {
                goTo[row, 0] = NAME + (row );
                action[row, 0] = NAME + (row );
            }
            string toLogName = "SLR1table";
            outputLog.ClearLog(toLogName);
            //outputLog.SetNull();

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
                        firstKey = key.Split('_')[0];
                        secondKey = key.Split('_')[1];

                        if (IsUpperLetter(secondKey[0]))
                        {
                            targetColumn = FindTargetFromGotoColumn(goTo, secondKey);
                            targetRow = FindTargetFromRow(goTo, firstKey);
                            if (targetRow != -1 && targetColumn != -1)
                            {
                                if (goTo[targetRow, targetColumn] == null)
                                {
                                    outputLog.InputInfo(toLogName, "根据Automaton,发现" + firstKey + "遇到" + secondKey + "转移到" + goLog[key] + ",填表\r\n");
                                    goTo[targetRow, targetColumn] = goLog[key];
                                    outputLog.InputInfo(toLogName, "#" + "goto" + SEPERATOR + targetRow + SEPERATOR + targetColumn + SEPERATOR + goLog[key]);


                                }
                                else
                                {
                                    //Console.WriteLine("该文法goto表存在冲突:" + key);
                                    outputLog.InputInfo(toLogName, "该文法goto表存在冲突，不是LL1文法\r\n");
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

                                }
                                else
                                {
                                    //Console.WriteLine("1该文法action表存在冲突:" + key);
                                    outputLog.InputInfo(toLogName, "该文法action表存在冲突\r\n");
                                    return false;
                                    //return;
                                }
                            }
                            else
                            {
                                //Console.WriteLine("该文法的action表中不存在:"+ firstKey + " " + secondKey);
                                outputLog.InputInfo(toLogName, "该文法的action表中不存在:" + firstKey + " " + secondKey + "\r\n");
                                return false;
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
                        string left;
                        string right;
                        string key = reduceKey[reduceI];
                        left = reduceLog[key].Split(SEPERATOR)[1];
                        right = reduceLog[key].Split(SEPERATOR)[0];
                        if (left.Equals(START))
                        {
                            targetColumn = FindTargetFromActionColumn(action, "$");
                            targetRow = FindTargetFromRow(action, key);
                            //outputLog.InputInfo(toLogName, key + SEPERATOR + "$" + SEPERATOR + ACCEPT);
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
                            List<string> reduce = follow[left];
                            foreach (string r in reduce)
                            {
                                targetColumn = FindTargetFromActionColumn(action, r);
                                targetRow = FindTargetFromRow(action, key);
                                //outputLog.InputInfo(toLogName, key + SEPERATOR + r + SEPERATOR + reduceLog[key]);
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
             *
             * foreach (string key in goLog.Keys)
                {
                    firstKey = key.Split('_')[0];
                    secondKey = key.Split('_')[1];

                    if (IsUpperLetter(secondKey[0]))
                    {
                        targetColumn = FindTargetFromGotoColumn(goTo, secondKey);
                        targetRow = FindTargetFromRow(goTo, firstKey);
                        if (targetRow != -1 && targetColumn != -1)
                        {
                            if (goTo[targetRow, targetColumn] == null)
                            {
                                outputLog.InputInfo(toLogName, "根据Automaton,发现" + firstKey + "遇到" + secondKey + "转移到" + goLog[key] + "\r\n");
                                goTo[targetRow, targetColumn] = goLog[key];
                                outputLog.InputInfo(toLogName, "#" + "goto" + SEPERATOR + targetRow + SEPERATOR + targetColumn + SEPERATOR + goLog[key]);


                            }
                            else
                            {
                                //Console.WriteLine("该文法goto表存在冲突:" + key);
                                outputLog.InputInfo(toLogName, "该文法goto表存在冲突，不是LL1文法\r\n");
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

                            }
                            else
                            {
                                //Console.WriteLine("1该文法action表存在冲突:" + key);
                                outputLog.InputInfo(toLogName, "该文法action表存在冲突\r\n");
                                return false;
                                //return;
                            }
                        }
                        else
                        {
                            //Console.WriteLine("该文法的action表中不存在:"+ firstKey + " " + secondKey);
                            outputLog.InputInfo(toLogName, "该文法的action表中不存在:" + firstKey + " " + secondKey + "\r\n");
                            return false;
                        }

                    }

                }
             

            string left;
            string right;
            foreach (string key in reduceLog.Keys) {
                left = reduceLog[key].Split(SEPERATOR)[1];
                right = reduceLog[key].Split(SEPERATOR)[0];
                if (left.Equals(START))
                {
                    targetColumn = FindTargetFromActionColumn(action,"$");
                    targetRow = FindTargetFromRow(action, key);
                    //outputLog.InputInfo(toLogName, key + SEPERATOR + "$" + SEPERATOR + ACCEPT);
                    if (targetRow != -1 && targetColumn != -1)
                    {
                        if (action[targetRow, targetColumn] == null)
                        {
                            action[targetRow, targetColumn] = ACCEPT;
                            outputLog.InputInfo(toLogName, "根据Automaton,发现" + key + "可以规约成" + START + "，置acc\r\n");
                            outputLog.InputInfo(toLogName, "#" + "action"+ SEPERATOR +targetRow + SEPERATOR + targetColumn + SEPERATOR + ACCEPT);
                        }
                        else
                        {
                            //Console.WriteLine("3该文法action表存在冲突" + key);
                            outputLog.InputInfo(toLogName, "3该文法action表存在冲突\r\n");
                            return false;
                        }
                    }
                    else {
                        //Console.WriteLine("该文法的action表中不存在:", key);
                        outputLog.InputInfo(toLogName, "该文法的action表中不存在:" + key + "\r\n");
                        return false;
                    }
                    
                }
                else {
                    List<string> reduce = follow[left];
                    foreach (string r in reduce)
                    {
                        targetColumn = FindTargetFromActionColumn(action, r);
                        targetRow = FindTargetFromRow(action, key);
                        //outputLog.InputInfo(toLogName, key + SEPERATOR + r + SEPERATOR + reduceLog[key]);
                        if (targetRow != -1 && targetColumn != -1)
                        {
                            if (action[targetRow, targetColumn] == null)
                            {
                                action[targetRow, targetColumn] = "r" + reduceLog[key];
                                outputLog.InputInfo(toLogName, "根据Automaton，在" + key + "中，"+ left + "的follow集有" + r + ",则" + right + "规约成" + left + "\r\n");
                                outputLog.InputInfo(toLogName, "#" + "action" + SEPERATOR +targetRow + SEPERATOR + targetColumn + SEPERATOR + "r" + left + "->" + right);
                            }
                            else
                            {
                                //Console.WriteLine("2该文法action表存在冲突" + key);
                                outputLog.InputInfo(toLogName, "2该文法action表存在冲突" + key + "\r\n");
                                return false;
                            }
                        }
                        else {
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

        public override void Analyze(string input) {
            Stack<string> stack = new Stack<string>();
            string outputName = "SLR1Analyze";
            outputLog.ClearLog(outputName);
            string[] word;
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
            do{
                if (word[currentWordIndex] != "")
                {
                    //PrintStack(stack);
                    //Console.Write("\t\t输入串:");
                    string inputString = "输入串:";
                    for (int i = currentWordIndex; i < word.Count(); i++)
                    {
                        inputString += word[i];
                        //Console.Write(word[i]);
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
                            if(instruction.Equals(ACCEPT)){
                                //Console.WriteLine("分析成功");
                                outputLog.InputInfo(outputName, "分析成功");
                                break;
                            }else if (instruction[0] == 'r')
                            {
                                string left = instruction.Replace("r", "").Split(SEPERATOR)[1];
                                string right = instruction.Replace("r", "").Split(SEPERATOR)[0];
                                outputLog.InputInfo(outputName, "用" + left + "->" + right + "进行规约");
                                if (right != "") { //如果出现S->.，则不弹出 
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
                                stack.Push(instruction.Replace("s",""));
                                outputLog.InputInfo(outputName, "移进" + inputFirstWord);
                                currentWordIndex++;
                            }
                            else {
                                outputLog.InputInfo(outputName, "action table有问题");
                                //Console.WriteLine("action table有问题");
                                return;
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: action表中没有[{0},{1}]的规则", stackTop, inputFirstWord);
                            outputLog.InputInfo(outputName, "Error: action表中没有[ " +stackTop + "," + inputFirstWord + " ]的规则");
                            return;
                        }
                    }
                    else {
                        Console.WriteLine("Error:原规则式中不存在{0},无法识别", inputFirstWord);
                        outputLog.InputInfo(outputName, "Error:原规则式中不存在" + inputFirstWord + ",无法识别");
                        return;
                    }
                    
                }
                else {
                    currentWordIndex++;
                }
            }while(true);
        
        
        }
        public override Dictionary<string, List<string>> GetRuleSet()
        {

            List<string> temp, production;
            string[] ru = null, tempS;
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
        public void BuildLR0ForView() {
            BuildClosure(DOT + nonTerminalSet[0] + SEPERATOR + START, null);
            BuildLR0(DOT + nonTerminalSet[0] + SEPERATOR + START, "I1");
        }
        public Dictionary<string, List<string>> GetClosure() {
            return closure;
        }
        public string[,] GetGotoTable()
        {
            return goTo;
        }
        public string[,] GetActionTable() {
            return action;
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
                            if( !production.Contains(tempS[1]))
                                production.Add(tempS[1]);
                            rRule[tempS[0]] = production;
                        }
                        else {
                            production = new List<String>();
                            production.Add(tempS[1]);
                            rRule.Add(tempS[0],production);
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
                BuildLR0(DOT + nonTerminalSet[0]+SEPERATOR+START, null );
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
        */
        
        

    }
}
