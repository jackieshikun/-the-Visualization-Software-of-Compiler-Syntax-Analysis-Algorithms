using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleApplication1.view
{
    public partial class FirstAndFollow : Form
    {
        Dictionary<string, List<string>> output;
        Dictionary<string, List<string>> first;
        int firstLine;
        int followLine;
        public FirstAndFollow()
        {
            InitializeComponent();
        }

        public void PrintFirstAndFollow(Dictionary<string, List<string>> first, Dictionary<string, List<string>> follow)
        {
            foreach (string key in first.Keys)
            {
                firstSetOutput.AppendText(key + ": ");
                foreach (string s in first[key])
                {
                    firstSetOutput.AppendText(  s + ", ");
                }
                firstSetOutput.AppendText("\r\n");
            }

            foreach (string key in follow.Keys)
            {
                followSetOutput.AppendText(key + ": ");
                foreach (string s in follow[key])
                {
                    followSetOutput.AppendText( s + ", ");
                }
                followSetOutput.AppendText("\r\n");
            }
        }
        public void InputList(Dictionary<string, List<string>> temp) {
            output = temp;
            firstLine = 0;
            followLine = 0;
        }
        public void InputFirstSet(Dictionary<string,List<string>> temp){
            first = temp;
            foreach(string key in temp.Keys){
                firstSet.AppendText(key + ": ");
                foreach (string a in temp[key]) {
                    firstSet.AppendText(a + "  ");
                }
                firstSet.AppendText("\r\n");
            }
        }
        public void InputFollowSet(Dictionary<string, List<string>> temp) {
            foreach (string key in temp.Keys)
            {
                followSet.AppendText(key + ": ");
                foreach (string a in temp[key])
                {
                    followSet.AppendText(a + "  ");
                }
                followSet.AppendText("\r\n");
            }
        }
        private void FirstAndFollow_Load(object sender, EventArgs e)
        {

        }

        private void firstNextStep_Click(object sender, EventArgs e)
        {
            //Console.WriteLine("click");
            if (firstLine < output["first"].Count())
            {
                //Console.WriteLine("Hello " + output["first"][firstLine]);
                string inputString = output["first"][firstLine];
                if (inputString[0] == '#')
                    firstSet.Clear();
                while(inputString[0] == '#'){
                    string dicString = inputString.Replace("#", "");
                    firstSet.AppendText(dicString);
                    firstLine++;
                    inputString = output["first"][firstLine];
                }
                
                firstSetOutput.AppendText(output["first"][firstLine] + "\r\n");
                firstLine++;
                //firstSetOutput.Refresh();
            }
            else {
                firstSetOutput.AppendText("结束\r\n");
                FirstFinishButton.Enabled = false;
                firstNextStep.Enabled = false;
            }
        }

        private void followNextStep_Click(object sender, EventArgs e)
        {
            if (followLine < output["follow"].Count())
            {
                //Console.WriteLine("Hello " + output["follow"][followLine]);
                string inputString = output["follow"][followLine];
                if (inputString[0] == '#')
                    followSet.Clear();
                while (inputString[0] == '#')
                {
                    string dicString = inputString.Replace("#", "");
                    followSet.AppendText(dicString);
                    followLine++;
                    inputString = output["follow"][followLine];
                }
                followSetOutput.AppendText(output["follow"][followLine] + "\r\n");
                followLine++;
                //firstSetOutput.Refresh();
            }
            else
            {
                followSetOutput.AppendText("结束\r\n");
                FollowFinishButton.Enabled = false;
                followNextStep.Enabled = false;
            }
        }

        private void FirstFinishButton_Click(object sender, EventArgs e)
        {
            while (firstLine < output["first"].Count())
            {
                string inputString = output["first"][firstLine];
                if (inputString[0] == '#')
                    firstSet.Clear();
                while(inputString[0] == '#'){
                    string dicString = inputString.Replace("#", "");
                    firstSet.AppendText(dicString);
                    firstLine++;
                    inputString = output["first"][firstLine];
                }
                
                firstSetOutput.AppendText(output["first"][firstLine] + "\r\n");
                firstLine++;
            }
            FirstFinishButton.Enabled = false;
            firstNextStep.Enabled = false;
        }

        private void FollowFinishButton_Click(object sender, EventArgs e)
        {
            while (followLine < output["follow"].Count())
            {
                string inputString = output["follow"][followLine];
                if (inputString[0] == '#')
                    followSet.Clear();
                while (inputString[0] == '#')
                {
                    string dicString = inputString.Replace("#", "");
                    followSet.AppendText(dicString);
                    followLine++;
                    inputString = output["follow"][followLine];
                }
                followSetOutput.AppendText(output["follow"][followLine] + "\r\n");
                followLine++;
            }
            FollowFinishButton.Enabled = false;
            followNextStep.Enabled = false;
        }
    }
}
