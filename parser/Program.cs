using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using ConsoleApplication1;
using ConsoleApplication1.parser;
using ConsoleApplication1.view;

namespace ConsoleApplication1
{
    
    
    public class Node<T>{
        private T data;
        public T Data {
            get {
                return data;
            }
            set {
                data = value;
            }
        
        }
        public Node() { }
        public Node(T data) {
            this.data = data;
        }
        public Node<T> next = null;
    }
    
    
    public class FileOperation
    {
        static public int MAXBUFFER = 2048;
        private int[] readCharBuffer;
        //private char[] byData;
        StreamReader file = null;
        private int tokenStart, tokenend;
        private int tokenEnd {
            get { return this.tokenend; }
            set { ReloadBuffer(); // 检查是否需要进行缓冲区填充
            tokenend = value;     
            }
        
        }
        private string fileName;

        public FileOperation(string fileName) { 
            readCharBuffer = new int[MAXBUFFER];
            tokenStart = 0;
            tokenEnd = 0;
            this.fileName = fileName;
            file = new StreamReader(fileName);
            ReloadBufferLeft();
           // byData = new char[MAXBUFFER/2];
        }
        ~FileOperation() {
            
            Console.WriteLine("file closed");
        }
        private void ReloadBufferRight() {
            int readCharNum = MAXBUFFER / 2;
            int readCh;
            while ((readCharNum < MAXBUFFER ) && (readCh = file.Read()) != -1)
            {
                readCharBuffer[readCharNum] = readCh;
                readCharNum++;

            }
            if (readCharNum == -1)
            {
                readCharBuffer[readCharNum] = -1;
                for (int i = MAXBUFFER / 2; readCharBuffer[i] != -1; i++)
                {
                    Console.Write((char)readCharBuffer[i]);

                }
            }
        }
        

        private void ReloadBufferLeft() {
            /*
            if (!File.Exists(fileName)) { 
                string[] createText = {"Hello", "World"};
                File.WriteAllLines(fileName,createText,Encoding.UTF8);
            }
            string[] readText = File.ReadAllLines(fileName, Encoding.UTF8);
            int i = 1;
            foreach(string s in readText){
                string[] s1 = s.Split(' ');
                foreach (string printS in s1)
                {
                    Console.WriteLine(i + "." + printS + printS.Length);
                    i++;
                }
            }
             * */
            int readCharNum = 0;
            int readCh = 0;
            while ((readCharNum < MAXBUFFER / 2) && (readCh = file.Read()) != -1) {
                readCharBuffer[readCharNum] = readCh;
                readCharNum++;

            }
            if (readCh == -1) {
                readCharBuffer[readCharNum] = -1;
                for (int i = 0; readCharBuffer[i] != -1; i++) {
                    Console.Write((char)readCharBuffer[i]);
                
                }
            }
                
        }
        private void ReloadBuffer()
        {
            if (tokenEnd == (MAXBUFFER - 1) / 2)
            {
                    //if(tokenStart> MAXBUFFER/2)
                 ReloadBufferRight();
            }
            else if (tokenEnd == MAXBUFFER - 1)
            {
                    //if(tokenStart< MAXBUFFER/2)
                ReloadBufferLeft();
            }
            else                {
                ;
            }
        }
        
        private void ReadFile2Buffer(){
            /*
            while (readCharBuffer[tokenEnd] != ' ')
            {
                if (tokenEnd == (MAXBUFFER -1) / 2)
                {
                    //if(tokenStart> MAXBUFFER/2)
                    ReloadBufferRight();
                    tokenEnd++;
                }
                else if (tokenEnd == MAXBUFFER - 1)
                {
                    //if(tokenStart< MAXBUFFER/2)
                    ReloadBufferLeft();
                    tokenEnd++;
                }
                else
                {
                    tokenEnd++;
                }
            }
             * */
            
        
        }
        /*
        public void ReadFile(string fileName)
        {
            StreamReader file = null;
            //Linklist<char> testList = new Linklist<char>();
            try
            {
                file = new StreamReader(fileName);
                int ch;
                ch = file.Read();
                while (ch != -1 )
                {
                    //testList.AddNode(Convert.ToChar(ch));
                    ch = file.Read();
                }
                //testList.PrintList();
                file.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine("Error occur");
            }


        }
        */
        private void ExtractInteger(){
            char readChar = (char)readCharBuffer[tokenStart];
            tokenEnd = tokenStart;
            while(readChar>= '0' && readChar<= '9'){
                
                tokenEnd++;
                readChar =(char)readCharBuffer[tokenEnd];
            }
            tokenStart = tokenEnd;
            /*
            if (IsRightTerminator(readChar))
            {
                tokenStart = tokenEnd;
                return true;
            }
            else {
                tokenStart = tokenEnd;
                return false;
            }
             * */
        
        }
        private void ExtractIdentifer() {
            char readChar = (char)readCharBuffer[tokenStart];
            tokenEnd = tokenStart;
            while((readChar >= 'a' && readChar <= 'z') || (readChar >= 'A' && readChar <= 'Z' ) || (readChar == '_')){
                tokenEnd++;
                readChar = (char)readCharBuffer[tokenEnd];
                
            }
            tokenStart = tokenEnd;
            /*
            if (IsRightTerminator(readChar))
            {
                tokenStart = tokenEnd;
                return true;
            }
            else {
                tokenStart = tokenEnd;
                return false;
            
            }
            */
        }
        private void ExtractFloat(){
        
        
        }
        private bool IsRightTerminator(char terminator) {
            if (terminator == ':' || terminator == ' ' || terminator == '\n' || terminator == ';')
                return true;
            else
                return false;
        
        }
        public void TokenGenerate()
        {
            char ch;
            StreamWriter fileWriter = new StreamWriter("lexer.txt");
            ReloadBuffer();
            Console.ReadKey();
            while (readCharBuffer[tokenEnd] != -1)
            {
                
                ch = (char)readCharBuffer[tokenStart];
                Console.Write(ch);
                
                if (ch >= '0' && ch <= '9') {
                    ExtractInteger();
                    fileWriter.Write("num");
                    
                   
                    
                }
                else if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || ch == '_')
                {
                    ExtractIdentifer();
                    fileWriter.Write("id");
                }
                else {
                    
                    fileWriter.Write(ch);
                    tokenStart++;
                    tokenEnd = tokenStart;
                }

            }
            fileWriter.Close();   
           
        }

    }
    
    
    public class Program
    {

        [STAThreadAttribute]
        static void Main(string[] args)
        {

            /*
            StreamWriter f = new StreamWriter("output.txt");
            Console.SetOut(f);
            Console.WriteLine("Hello World");
            f.Flush();
            f.Close();
             * */
            //Console.WriteLine("Finished");
            MainWindow form = new MainWindow();
            form.ShowDialog();

            //panelTest a = new panelTest();
            //a.ShowDialog();
            
            //LL1 rule = new LL1("rule.txt");
            //rule.Run();


            //form.Close();
            //Parsing rule = new LL1("rule.txt");
            //rule.Run();
            //Console.Read();
            //LR1 rule = new LR1();

            //FileOperation file = new FileOperation(f);
            //file.ReadFile(f);
            //file.TokenGenerate();

            //file = null;
            //Console.ReadKey();
        }
    }
}
