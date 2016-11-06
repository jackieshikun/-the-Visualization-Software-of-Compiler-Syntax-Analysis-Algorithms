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
    public partial class LR1Automaton : Form
    {
        int closureNum = 0;
        int ClOSUREWIDTH = 120;
        int CLOSUREHEIGHT = 90;
        int LABELHEIGHT = 10;
        int display = 0;
        string inputLogName = "LR1LR0";
        Dictionary<string, List<string>> log;
        Dictionary<string, List<string>> closure;
        Dictionary<string, string> adjRelation;
        Dictionary<string, int> edge;//一个closure总共有几条边
        Dictionary<string, int> drawingCount;//记录画到了closure的第几条边
        double x = 0.5;
        double y = 1;
        
        public LR1Automaton()
        {
            InitializeComponent();
            adjRelation = new Dictionary<string, string>();
            edge = new Dictionary<string, int>();
            drawingCount = new Dictionary<string, int>();
            this.AutoScroll = true;
        }
        public void GetInfo(Dictionary<string, List<string>> target)
        {
            log = target;
            foreach (string key in target[inputLogName])
            {
                if (key[0] == '#')
                {
                    string[] word = key.Split('_');
                    string firstWord = word[0].Replace("#", "");
                    if (edge.ContainsKey(firstWord))
                    {
                        edge[firstWord]++;
                    }
                    else
                    {
                        edge.Add(firstWord, 1);
                        drawingCount.Add(firstWord, 0);
                    }
                }
            }
        }



        public void InputClosure(Dictionary<string, List<string>> target)
        {
            closure = target;
        }

        private void BuildClosure(string closureName)
        {
            TextBox box = new TextBox();
            box.Width = ClOSUREWIDTH;
            box.Height = CLOSUREHEIGHT;
            if ((x + 2) * ClOSUREWIDTH > this.Size.Width)
            {
                x = 0.5;
                y += 2;

            }
            box.Location = new Point((int)(x * ClOSUREWIDTH), (int)(y * CLOSUREHEIGHT));
            box.Multiline = true;
            box.Name = closureName;
            box.Text = closureName;
            box.Font = new Font("微软雅黑", box.Font.Size, box.Font.Style | FontStyle.Bold);

            box.ScrollBars = ScrollBars.Vertical;
            box.ReadOnly = true;
            box.AppendText("\r\n" + RewriteRuleExpression(closure[closureName][0]));

            this.Controls.Add(box);
            x += 2;

        }
        private void FillClosure(string targetClosure)
        {
            TextBox t = (TextBox)this.Controls[targetClosure];
            t.Clear();
            t.AppendText(targetClosure + "\r\n");
            foreach (string s in closure[targetClosure])
                t.AppendText(RewriteRuleExpression(s) + "\r\n");
            t.Select(0, 0);
            t.ScrollToCaret();
        }
        private string RewriteRuleExpression(string originalRule)
        {
            string[] word = originalRule.Split('_');
            if (word[0] == "." + parser.Parsing.NULL_SYMBOL)
                word[0] = ".";
            return word[1] + "->" + word[0] + ", " + word[2];
        }
        private void AddLabel(Point position, string symbol)
        {

            Label label = new Label();
            //label.Height = LABELHEIGHT;
            //label.Width = LABELWIDTH;
            label.AutoSize = true;
            label.Text = symbol;
            label.BackColor = System.Drawing.Color.Transparent;
            label.Location = position;
            this.Controls.Add(label);

        }
        private void DrawEdge(string original, string symbol, string target)
        {
            Graphics g = this.CreateGraphics();
            Point lineStart = new Point();
            Point lineEnd = new Point();
            Point symbolPoint = new Point();
            lineStart.X = this.Controls[original].Location.X + ClOSUREWIDTH;
            lineStart.Y = this.Controls[original].Location.Y + (int)((double)drawingCount[original] / edge[original] * CLOSUREHEIGHT);
            lineEnd.X = lineStart.X + (int)(0.5 * ClOSUREWIDTH);
            lineEnd.Y = lineStart.Y;
            //g.DrawLine(new Pen(Color.Red, 3), lineStart, lineEnd);

            AddLabel(lineEnd, target);
            symbolPoint.X = lineStart.X + (int)(0.1 * ClOSUREWIDTH);
            symbolPoint.Y = lineStart.Y;
            AddLabel(symbolPoint, symbol);

            Label line = new Label();
            line.Location = lineStart;
            line.Height = LABELHEIGHT / 2;
            line.Text = "------------";
            line.AutoSize = true;
            line.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(line);
            drawingCount[original]++;

        }
        private void NextStep_Click(object sender, EventArgs e)
        {
            while (closureNum < log[inputLogName].Count())
            {
                string key = log[inputLogName][closureNum];
                if (key[0] == '#')
                {
                    string solvedKey = key.Replace("#", "");
                    string original = solvedKey.Split('_')[0];
                    string target = solvedKey.Split('_')[2];
                    string symbol = solvedKey.Split('_')[1];
                    if (display == 0) {
                        display = 1;
                        if (!this.Controls.ContainsKey(original))
                        {
                            BuildClosure(original);
                        }
                        break;
                    }
                    else if (display == 1) {
                       
                        FillClosure(original);
                        display = 2;
                        break;
                    }
                    else if (display == 2) {
                        display = 0;
                         if (!this.Controls.ContainsKey(target))
                        {
                            BuildClosure(target);
                            FillClosure(target);
                        }
                        DrawEdge(original, symbol, target);
                        closureNum++;
                        break;
                    }

                }
                else
                {
                    outputTextBox.AppendText(log[inputLogName][closureNum] + "\r\n");
                    closureNum++;

                }


            }
            if(closureNum >= log[inputLogName].Count()){
                NextStep.Enabled = false;
                FinishButon.Enabled = false;
            }
        }

        private void FinishButon_Click(object sender, EventArgs e)
        {
            while (closureNum < log[inputLogName].Count())
            {
                string key = log[inputLogName][closureNum];
                if (key[0] == '#')
                {
                    string solvedKey = key.Replace("#", "");
                    string original = solvedKey.Split('_')[0];
                    string target = solvedKey.Split('_')[2];
                    string symbol = solvedKey.Split('_')[1];
                    if (!this.Controls.ContainsKey(original))
                    {
                        BuildClosure(original);
                    }
                    FillClosure(original);
                    if (!this.Controls.ContainsKey(target))
                    {
                        BuildClosure(target);
                    }
                    //FillClosure(target);
                    DrawEdge(original, symbol, target);
                    closureNum++;
                    
                    

                }
                else
                {
                    outputTextBox.AppendText(log[inputLogName][closureNum] + "\r\n");
                    closureNum++;

                }
            }
            NextStep.Enabled = false;
            FinishButon.Enabled = false;
        }

        private void outputTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
