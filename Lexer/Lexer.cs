using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using ConsoleApplication1;


/*
 程序结构：
 * 一个大循环套两个小循环
 * 小循环1：程序把字符从文件读到缓冲区，直到缓冲区满或文件读完
 * 小循环2：程序把缓冲区内的字符转换成标记符，直到读到文件终止符或者缓冲区空
 * 判断： 如果文件未读完，则继续循环
 
 */



// BUG

// 未做事项： 1. 注释过滤
//            2. 错误处理

namespace ConsoleApplication1{
    using BufferType = System.Char;//不知道怎么把Buffer.cs中重命名的BufferType拿过来，于是又写了一遍

     class Lexer
    {
        private Hashtable idTable;
        private Queue queue;
        private StreamReader fileReader;
        private static int MAX_WORD_LENGTH = 50;
        private BufferType[] wordArray;
       
        public static string [] keyWord = {"begin","if","then","else","program","while","do","and","procedure","function","var","or","array","for", "end"};
        public enum MARK {Begin, If, Then, Else, Program, While, Do, And, Procedure, Function, Var, Or, Array, For, End,
            Id, Num, Assign_op, Add, Sub, Mul, Div, Lt, Le, Eq, Ne, Gt, Ge, Vir, Lbr, Rbr, Quo, Sem, Col, Com, Stop, Exc }
         /* Assign_op -- :=, Lt -- <, Le -- <=, Eq -- =, Ne -- <>, Gt -- >, Ge -- >=, Virgule -- /, Left_bracket -- (, Rbr -- ), Quo -- ', 
          * 
            Semicolon -- ;, Colon -- :, Comma -- , Stop -- ., Excalmation -- !,
          */
        enum ERROR {WordLengthExceed }
        public Lexer()
	    {
            idTable = new Hashtable();
            queue = new Queue();
            wordArray = new BufferType[MAX_WORD_LENGTH];
            fileReader = new StreamReader("pascal.txt");
        
	    }
        private bool IsDigit(char ch) {
            if (ch >= '0' && ch <= '9')
                return true;
            else
                return false;
    
        }

        private bool IsLetter(char ch) {
            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || ch == '_')
                return true;
            else
                return false;
    
        }
        
        private String ArrayToString(BufferType[] buffer) {
            String ss = "";
            for (int i = 0; buffer[i] != '\0'; i++) {
                ss += buffer[i];
            }
            return ss;
        }
        /*
           扫描器，一次扫描出一个单词，扫描前会去掉单词前的空格,\t,\n
         
        */
        private String Scaner(ref int wordType) { 
            char ch;
            String word;
            BufferType[] wordArray = new BufferType[MAX_WORD_LENGTH];
            do {
                ch = queue.DeQueue();
            }
            while ((ch == ' ') || (ch == '\t') || (ch == '\n'));
            
            if(IsDigit(ch)){
                //整数？负数？浮点数？
                String tempNum = "";
                tempNum += ch;
                bool isFirstDot = true;
                while (true) {
                    ch = queue.GetQueueFrontElement();
                    if (IsDigit(ch)) {
                        ch = queue.DeQueue();
                        tempNum += ch;
                    }
                    else if (ch == '.' && isFirstDot)
                    {
                        isFirstDot = false;
                        ch = queue.DeQueue();
                        tempNum += ch;
                    }
                    else {
                        break;
                    }
                }
                /*
                int num = 0;
                bool isFirstDot = true;
                int factor = 1;//正负系数
                int multiple = 10;//小数点后变为为0.1，小数点前为10
                if (ch == '-') {
                    factor = -1;
                    ch = queue.DeQueue();
                }
                while (true) { 
                    num = factor*num*10 + (ch - '0');
                    ch = queue.GetQueueFrontElement();
                    if (IsDigit(ch))
                    {
                        ch = queue.DeQueue();
                    }
                    else if (ch == '.' && isFirstDot) { 
                        
                    }else
                        break;
                
                */
                wordType = (int)MARK.Num;
                word = tempNum;
                //word = num.ToString();
                
                //Console.Write(word + ",");
                return word;

            }else if (IsLetter(ch)){
                int arrayNum = 0;
                
                while(true){
                    //Console.Write(ch);
                    wordArray[arrayNum++] = ch;
                    ch = queue.GetQueueFrontElement();
                    
                    if (IsLetter(ch) || IsDigit(ch))
                    {
                        ch = queue.DeQueue();
                    }
                    else
                        break;
                }
                
                    
              
                wordArray[arrayNum] = '\0';
                wordType = (int)MARK.Id;
                word = ArrayToString(wordArray);
                //Console.Write(word + ",");
                return word;
            }else{
                int arrayNum = 0;
                switch(ch){
                    case '+':
                        wordType = (int)MARK.Add;
                        wordArray[arrayNum] = '+';
                        break;
                    case ';': 
                        wordType = (int)MARK.Sem;
                        wordArray[arrayNum] = ';';
                        break;
                    case '-':
                        wordType = (int)MARK.Sub;
                        wordArray[arrayNum] = '-';
                        break;
                    case '*':
                        wordType = (int)MARK.Mul;
                        wordArray[arrayNum] = '*';
                        break;
                    case '/':
                        wordType = (int)MARK.Div;
                        wordArray[arrayNum] = '/';
                        break;
                    case '\'':
                        wordType = (int)MARK.Quo;
                        wordArray[arrayNum] = '\'';
                        break;
                    case ':':
                        ch = queue.GetQueueFrontElement();
                        if (ch == '=')
                        {
                            wordArray[arrayNum++] = ':';
                            ch = queue.DeQueue();
                            wordArray[arrayNum] = '=';
                            wordType = (int)MARK.Assign_op;
                        }
                        else {
                            wordType = -1;
                        }
                        break;
                    case ')':
                        wordType = (int)MARK.Rbr;
                        wordArray[arrayNum] = ')';
                        break;
                    case '(':
                        wordType = (int)MARK.Lbr;
                        wordArray[arrayNum] = '(';
                        break;
                    case '=':
                        ch = queue.GetQueueFrontElement();
                        if (ch == '=')
                        {
                            wordArray[arrayNum++] = '=';
                            ch = queue.DeQueue();
                            wordArray[arrayNum] = '=';
                            wordType = (int)MARK.Eq;
                        }
                        else {
                            wordType = -1;
                        }
                        break;
                    case ',':
                        wordType = (int)MARK.Com;
                        wordArray[arrayNum] = ',';
                        break;
                    case '.':
                        wordType = (int)MARK.Stop;
                        wordArray[arrayNum] = '.';
                        break;
                    case '!':
                        wordType = (int)MARK.Exc;
                        wordArray[arrayNum] = '!';
                        break;
                    default:
                        wordType = -1;
                        break;
                }
                wordArray[++arrayNum] = '\0';
                word = ArrayToString(wordArray);
                return word;
                
            
            }

        }
        private void ConvertLetter2Word(){
            //取出Word
            //判断关键字
            //生成token流，放到文件里
            int wordType = -1;
            String word;
            StreamWriter fileWriter = new StreamWriter("token.txt");
            //Console.WriteLine("Hello Word");
            do
            {
                word = Scaner(ref wordType);
                word = word.ToLower(); //pascal不区分大小写
                //Console.Write(word);
                if (wordType == (int)MARK.Num) {
                    fileWriter.WriteLine(word + "," + wordType);
                }
                else if (wordType == (int)MARK.Id)
                {
                    //是否是关键字
                    for (int i = (int)MARK.Begin; i < (int)MARK.End; i++ )
                    {
                        if (word.Equals(keyWord[i]) == true) {
                            wordType = i;
                        }

                    }
                    fileWriter.WriteLine(word + "," + wordType);
                }

                else {
                    if (wordType != -1)
                    {
                        fileWriter.WriteLine(word + "," + wordType);
                    }
                    else if (!word.Equals(""))
                        Error(word);

                    
                
                }
            }
            while (!queue.IsQueueEmpty());
                

            fileWriter.Close();
        }
        private void Error(String word) {
            Console.WriteLine("Error: " + word);
        
        }

        public void Run() { 
            bool isFileEnd = false;
            int ch;
            while(!isFileEnd){
                //ch = fileReader.Read();
                while((fileReader.Peek() != -1) && !queue.IsQueueFull() ){
                    ch = fileReader.Read();
                    queue.EnQueue((BufferType)ch);
                }
                queue.ReadQueue();
                if (fileReader.Peek() == -1)
                    isFileEnd = true;

                ConvertLetter2Word();
            
            }
            fileReader.Close();
            
        }
        
    
    }

}