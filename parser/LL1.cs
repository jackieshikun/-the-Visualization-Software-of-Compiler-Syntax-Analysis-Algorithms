using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;
using ConsoleApplication1.parser;


// 已解决 BUG1 splitRule function中， 如果一个规则后有很多空格，则很难处理 如 A-> a + b           | a

//待做： 出错时自动结束程序
//待做：退出空集的判断没有做



namespace ConsoleApplication1
{
    //using 
    public class LL1:Parsing
    {

        string[,] anaTable;
        char SEPERATOR = '_';
        public LL1(string f):base(f) {
            
        
        }
        
       
        
        private int FindTerminalFromTable(string [,] tempTable, string target) {
            int column;
            for (column = 1; column < terminalSet.Count() + 1; column++) {
                if (tempTable[0, column].Equals(target)) {
                    break;
                }
            }
            return column;
        }
        private int FindNonTerminalFromTable(string[,] tempTable, string target)
        {
            int row;
            for (row = 1; row < terminalSet.Count() + 1; row++)
            {
                if (tempTable[row, 0].Equals(target))
                {
                    break;
                }
            }
            return row;
        }
        public override bool BuildTable() {
            anaTable = new string[nonTerminalSet.Count() + 1, terminalSet.Count() + 1];
            int targetRow, targetColumn;
            for (int row = 1; row < nonTerminalSet.Count() + 1; row++)
                anaTable[row, 0] = nonTerminalSet[row - 1];
            for (int column = 1; column < terminalSet.Count() + 1; column++)
                anaTable[0, column] = terminalSet[column - 1];
            
                

            string toLogName = "LL1table";
            outputLog.ClearLog(toLogName);
            foreach(string key in rRule.Keys){
                foreach (string production in rRule[key]) {
                    string firstWord = production.Split(' ')[0];
                    outputLog.InputInfo(toLogName, "对" + key +  "->" + production);
                    if (IsUpperLetter(firstWord[0]))
                    {
                        
                        if (!isToEmpty.ContainsKey(firstWord))//检查例如 S->A B C中，A,B,C的first集是否都包含空 
                        {
                            foreach (string f in first[firstWord])
                            {
                                outputLog.InputInfo(toLogName, f + "属于首字母" + firstWord + "的first集,填表");
                                if (!f.Equals(NULL_SYMBOL))
                                {
                                    targetRow = FindNonTerminalFromTable(anaTable, key);
                                    targetColumn = FindTerminalFromTable(anaTable, f);
                                    if (anaTable[targetRow, targetColumn] == null)
                                    {
                                        anaTable[targetRow, targetColumn] = production;
                                        outputLog.InputInfo(toLogName, "#" + targetRow + SEPERATOR + targetColumn + SEPERATOR + production);
                                    }
                                    else
                                    {
                                        Console.WriteLine("不是LL1文法");
                                        outputLog.InputInfo(toLogName, "不是LL1文法");
                                        return false;
                                    }
                                }
                            }
                        }
                        else {
                            string[] word = production.Split(' ');
                            int i;
                            outputLog.InputInfo(toLogName, "首字母" + firstWord + "包含空,需要后续字母的first集");
                            for (i = 0; i < word.Count(); i++) {
                                if (IsUpperLetter(word[i][0]))
                                {
                                    foreach (string f in first[word[i]])
                                    {
                                        if (!f.Equals(NULL_SYMBOL))
                                        {
                                            outputLog.InputInfo(toLogName, f + "属于" + word[i] + "的first集，填表");
                                            targetRow = FindNonTerminalFromTable(anaTable, key);
                                            targetColumn = FindTerminalFromTable(anaTable, f);
                                            if (anaTable[targetRow, targetColumn] == null)
                                            {
                                                anaTable[targetRow, targetColumn] = production;
                                                outputLog.InputInfo(toLogName, "#" + targetRow + SEPERATOR + targetColumn + SEPERATOR + production);
                                            }
                                            else
                                            {
                                                Console.WriteLine("不是LL1文法");
                                                outputLog.InputInfo(toLogName, "不是LL1文法");
                                                return false;
                                            }
                                        }
                                    }
                                    if (!isToEmpty.ContainsKey(word[i]))
                                        break;
                                }
                                else {
                                    if (!word[i].Equals(NULL_SYMBOL))
                                    {
                                        outputLog.InputInfo(toLogName, word[i] + "为终结符");
                                        targetRow = FindNonTerminalFromTable(anaTable, key);
                                        targetColumn = FindTerminalFromTable(anaTable, word[i]);
                                        if (anaTable[targetRow, targetColumn] == null)
                                        {
                                            anaTable[targetRow, targetColumn] = production;
                                            outputLog.InputInfo(toLogName, "#" + targetRow + SEPERATOR + targetColumn + SEPERATOR + production);
                                        }
                                        else
                                        {
                                            Console.WriteLine("不是LL1文法");
                                            outputLog.InputInfo(toLogName, "不是LL1文法");
                                            return false;
                                        }
                                        break;
                                    }
                                }
                                if (i + 1 == word.Count()) {
                                    foreach (string f in follow[key])
                                    {
                                        targetRow = FindNonTerminalFromTable(anaTable, key);
                                        targetColumn = FindTerminalFromTable(anaTable, f);
                                        string p = key + "->" + NULL_SYMBOL;
                                        if (anaTable[targetRow, targetColumn] == null)
                                        {
                                            outputLog.InputInfo(toLogName, "#" + targetRow + SEPERATOR + targetColumn + SEPERATOR + production);
                                            anaTable[targetRow, targetColumn] = p;
                                        }
                                        else
                                        {
                                            Console.WriteLine("不是LL1文法");
                                            outputLog.InputInfo(toLogName, "不是LL1文法");
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else {
                        if (!firstWord.Equals(NULL_SYMBOL))
                        {
                            outputLog.InputInfo(toLogName, "首字母" + firstWord + "为终结符，填表");
                            targetRow = FindNonTerminalFromTable(anaTable, key);
                            targetColumn = FindTerminalFromTable(anaTable, firstWord);
                            if (anaTable[targetRow, targetColumn] == null)
                            {
                                anaTable[targetRow, targetColumn] = production;
                                outputLog.InputInfo(toLogName, "#" + targetRow + SEPERATOR + targetColumn + SEPERATOR + production);
                            }
                            else
                            {
                                Console.WriteLine("不是LL1文法");
                                outputLog.InputInfo(toLogName, "不是LL1文法");
                                return false;
                            }
                        }
                        else {
                            outputLog.InputInfo(toLogName, "首字母为空");
                            foreach (string f in follow[key]) {
                                outputLog.InputInfo(toLogName, f + "属于" + key + "的follow集，填表");
                                targetRow = FindNonTerminalFromTable(anaTable, key);
                                targetColumn = FindTerminalFromTable(anaTable, f);
                                if (anaTable[targetRow, targetColumn] == null)
                                {
                                    anaTable[targetRow, targetColumn] = production;
                                    outputLog.InputInfo(toLogName, "#" + targetRow + SEPERATOR + targetColumn + SEPERATOR + production);
                                }
                                else
                                {
                                    Console.WriteLine("不是LL1文法");
                                    outputLog.InputInfo(toLogName, "不是LL1文法");
                                    return false;
                                }
                            }
                        
                        }
                    
                    }
                
                }
            }

            /*
            Console.WriteLine("\n\t\t\t\t预测分析表\t\t\t\t\n");
            for (int i = 0; i < nonTerminalSet.Count() + 1; i++) {
                for (int j = 0; j < terminalSet.Count() + 1; j++) {
                    Console.Write(tempTable[i,j] + "\t");
                }
                Console.WriteLine();
            }
            */
            //anaTable = tempTable;
            return true;
        }
        public override void Analyze(string input) {
            //FileStream fs = new FileStream("output.txt", FileMode.Open, FileAccess.Write, FileShare.ReadWrite);
            //StreamWriter writeFile = new StreamWriter("output.txt");
            //SWriter sw = new StringWriter();
            //Console.SetOut(writeFile);
            Stack<string> stack = new Stack<string>();
            string outputName = "LL1Analyze";
            outputLog.ClearLog(outputName);
            string[] word;
            int currentWordIndex = 0,targetRow, targetColumn;
            string stackTop = null ,inputFirstWord = null,production;
            stack.Push("$");
            stack.Push(nonTerminalSet[0]);
            //Console.WriteLine("\n\t\t\t\t预测分析程序\t\t\t\t\n");
            
            word = input.Split(' ');
            if (word[word.Count() - 1] != "$") {
                outputLog.InputInfo(outputName, "Error: 输入串应该以$结尾");
                //Console.WriteLine("\t\tError: 输入串应该以$结尾");
                //writeFile.Flush();
                //writeFile.Close();
                return;
            }
            do {
                if (word[currentWordIndex] != "")
                {
                    //PrintStack(stack);
                    string inputString = "输入串:";
                    //Console.Write("\t输入串:");
                    for (int i = currentWordIndex; i < word.Count(); i++) {
                        inputString += word[i];
                    }
                    outputLog.InputInfo(outputName, "#" + Stack2String(stack) + SEPERATOR + inputString);
                    //Console.ReadKey();
                    //Console.WriteLine();

                    stackTop = stack.Peek();
                    inputFirstWord = word[currentWordIndex];
                    if (terminalSet.Contains(inputFirstWord))
                    {
                        if (IsUpperLetter(stackTop[0]))//栈顶是非终结符
                        {
                            targetRow = FindNonTerminalFromTable(anaTable, stackTop);
                            targetColumn = FindTerminalFromTable(anaTable, inputFirstWord);
                            production = anaTable[targetRow, targetColumn];
                            if (production != null)
                            {
                                //Console.Write("\t{0}->{1}", stackTop, production);
                                outputLog.InputInfo(outputName, stackTop + "->" + production);
                                if (!production.Equals(NULL_SYMBOL))
                                {

                                    
                                    stack.Pop();
                                    foreach (string s in production.Split(' ').Reverse()) {
                                        if (s != "") {
                                            stack.Push(s);
                                        }
                                    }
                                }
                                else {
                                    stack.Pop();
                                }
                            }

                            else {
                                //Console.Write("\tError: 预测分析表中没有[{0},{1}]的规则",stackTop,inputFirstWord);
                                outputLog.InputInfo(outputName, "Error: 预测分析表中没有[" + stackTop + "," +inputString +"]的规则");
                                break;
                            }
                        }
                        else {
                            if (stackTop.Equals(inputFirstWord))
                            {
                                //Console.Write("\t移进{0}",stackTop);
                                outputLog.InputInfo(outputName, "栈顶匹配" + stackTop );
                                currentWordIndex++;
                                stack.Pop();
                            }
                            else {
                                //Console.Write("\t栈顶的终结符和分析串的终结符不匹配");
                                outputLog.InputInfo(outputName, "栈顶的终结符和分析串的终结符不匹配");
                                break;
                            }
                        }
                    }
                    else {
                        Console.Write("\tError:原规则式中不存在{0},无法识别",inputFirstWord);
                        outputLog.InputInfo(outputName, "Error:原规则式中不存在{0},无法识别" + inputFirstWord );
                        break;
                    }

                    //Console.WriteLine();
                }
                else {
                    currentWordIndex++;
                }
                //writeFile.Flush();
            
            }while(stack.Peek() != "$");
            /*
            PrintStack(stack);
            Console.Write("\t输入串:");
            for (int i = currentWordIndex; i < word.Count(); i++)
            {
                Console.Write(word[i]);
            }
             * */
            //Console.ReadKey();
            //Console.WriteLine();
            if ( stack.Peek().Equals("$") && word[currentWordIndex].Equals("$"))
            {
                //Console.Write("\t分析成功");
                outputLog.InputInfo(outputName, "分析成功");
            }
            else
                outputLog.InputInfo(outputName, "分析失败");
                //Console.Write("\t分析失败");
            
            //File.WriteAllText(@"output.txt",sw.ToString());
            //writeFile.Flush();
            //writeFile.Close();
            
        }
        

        


        /*
        public override void Run()
        {
            List<string> temp, production;
            string[] ru = null, tempS;
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
                        tempS[1] = tempS[1].Trim();
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
                PrintDic(rRule);
                Console.ReadKey();
                PrintTerminalAndNonTerminalSet();
                
                Console.Write("\n非终结符:");
                foreach (string s in nonTerminalSet)
                    Console.Write("{0} ", s);
                Console.Write(";\n终结符:");
                foreach (string s in terminalSet)
                    Console.Write("{0} ", s);
                Console.ReadKey();
                
                IsRightNonTerminalSetEqualNonTerminalSet();
                Console.WriteLine("\n\t\t\t\tFirst集\t\t\t\t");
                foreach(string key in rRule.Keys)
                    FirstSet(key);
                PrintDic(first);
                Console.WriteLine("\t\t\t\tFollow集\t\t\t\t");
                foreach(string key in rRule.Keys)
                    FollowSet(key);
                PrintDic(follow);

                Console.ReadKey();

                BuildTable();//构建分析表

                Console.ReadKey();
                
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
                while (fileReader.Peek() != -1) {
                    string input = fileReader.ReadLine();
                    Analyze(input);
                }
                
                
            }

        }
         * */
        public string[,] GetAnaTable() {
            return anaTable;
        }
        /*
        public Dictionary<string, List<string>> GetFirstSet() {
            return first;
        }
        public Dictionary<string, List<string>> GetFollowSet() {
            return follow;
        }
        */
    }
}
