using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


/*
    Program() -> id Block().
 *  Block() -> begin Compound_tail()
 *  Compound_tail() -> Statement() end | Statement() ; Compound_tail() | Integer : Statement() end | Integer : Statement() ; Compound_tail()
 *  Statement() -> Variable() := Expression() | id | id ( Expression() ) | begin Compound_tail() | if Expression() then Statement() | if Expression() then Statement() else Statement()
 *  | while Expression() do Statement()
 *  Expression() -> Simple_expression()
 *  
 *  
 
 
 
 */






namespace ConsoleApplication1
{
    using BufferType = System.Char;

    class Parser
    {
        private StreamReader fileReader;
        private String currentToken;
        private int currentTokenType;
        public Parser() {
            fileReader = new StreamReader("token.txt");
        
        }

        private bool CatchNextWord() {
            char ch;
            int num = 0;
            String tempToken = "";

            if (fileReader.Peek() == -1) {
                return false;
            }
            ch = (char)fileReader.Read();
            while (ch != ',')
            {
                tempToken += ch;
                ch = (char)fileReader.Read();
            }
            ch = (char)fileReader.Read();
            if (ch == ',')//防止出现 ,,MARK.Comma的情况
            {
                tempToken = ",";
            }
            else
            {
                String tempnum = "";
                while (ch != '\n')
                {
                    tempnum += ch;
                    ch = (char)fileReader.Read(); 
                }
                num = Int32.Parse(tempnum);
            }
            currentToken = tempToken;
            currentTokenType = num;
            return true;
        
        }
        private void Match(int tk) {
            if (currentTokenType == tk)
            {
                CatchNextWord();
            }
            else {
                Console.WriteLine("Parser Error, current Token isn't expected Token");
            }
        
        }

        public void Run() {
            StreamWriter fileWriter = new StreamWriter("parser.txt");
            while(CatchNextWord()){
                fileWriter.WriteLine(currentToken + ","+ currentTokenType);
            
            }
            fileWriter.Close();
        }
    }
}
