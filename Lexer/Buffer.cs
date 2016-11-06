using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    using BufferType = System.Char;
    
    class Queue {
        private static int MAXBUFFER = 4096;
        private BufferType [] buffer = new BufferType[MAXBUFFER];
        private int front, rear;

        public Queue() {
            SetNull();
        
        }
        private void SetNull() {
            front = MAXBUFFER - 1;
            rear = MAXBUFFER - 1;
        }
        public bool IsQueueEmpty() {
            if (front == rear)
                return true;
            return false;
        
        }
        public bool IsQueueFull() {
            if (front == (rear + 1) % MAXBUFFER)
                return true;
            return false;
        }

        public int QueueLength() {
            int num;
            num = rear - front;
            if (num < 0)
                num += MAXBUFFER;
            return num;
        }
        public BufferType GetQueueFrontElement() {
            int n;
            if (IsQueueEmpty())
                return '\0';//???????应该是0，但是0的话会出错
            else
            {
                n = (front + 1) % MAXBUFFER;
                return buffer[n];
            }
            
        }
        public bool EnQueue(BufferType data) {
            if (IsQueueFull())
            {
                return false;
            }
            else {
                rear = (rear + 1) % MAXBUFFER;
                buffer[rear] = data;
                return true;
            }
        
        }

        public BufferType DeQueue() {
            BufferType data;
            if (IsQueueEmpty())
            {
                return '\0';
            }
            else {
                
                front = (front + 1) % MAXBUFFER;
                data = buffer[front];
                return data;
            
            }
        
        }
        public void ReadQueue() {
            int start = front;
            Console.WriteLine("Ready to Work");
            while(start != rear){
                Console.Write(buffer[start]);
                start = (start + 1) % MAXBUFFER;
            }
            Console.Write(buffer[start]);
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    class Buffer
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

        public Buffer(StreamReader file) { 
            readCharBuffer = new int[MAXBUFFER];
            tokenEnd = 0;
            ReloadBufferLeft();
           // byData = new char[MAXBUFFER/2];
        }
        ~Buffer() {
            
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
        /*
        private void ExtractInteger(){
            char readChar = (char)readCharBuffer[tokenStart];
            tokenEnd = tokenStart;
            while(readChar>= '0' && readChar<= '9'){
                
                tokenEnd++;
                readChar =(char)readCharBuffer[tokenEnd];
            }
            tokenStart = tokenEnd;
            //*
            if (IsRightTerminator(readChar))
            {
                tokenStart = tokenEnd;
                return true;
            }
            else {
                tokenStart = tokenEnd;
                return false;
            }
             
        
        }
        */
        /*
        private void ExtractIdentifer() {
            char readChar = (char)readCharBuffer[tokenStart];
            tokenEnd = tokenStart;
            while((readChar >= 'a' && readChar <= 'z') || (readChar >= 'A' && readChar <= 'Z' ) || (readChar == '_')){
                tokenEnd++;
                readChar = (char)readCharBuffer[tokenEnd];
                
            }
            tokenStart = tokenEnd;
            ///*
            if (IsRightTerminator(readChar))
            {
                tokenStart = tokenEnd;
                return true;
            }
            else {
                tokenStart = tokenEnd;
                return false;
            
            }
            
        }
        */
        /*
        private void ExtractFloat(){
        
        
        }
         * /
        /*
        private bool IsRightTerminator(char terminator) {
            if (terminator == ':' || terminator == ' ' || terminator == '\n' || terminator == ';')
                return true;
            else
                return false;
        
        }
         * */
        /*
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
         * */

    }

}
