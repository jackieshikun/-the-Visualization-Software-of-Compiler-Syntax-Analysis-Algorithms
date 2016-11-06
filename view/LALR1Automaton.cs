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
    public partial class LALR1Automaton : Form
    {
        int closureNum = 0;
        int ClOSUREWIDTH = 120;
        int CLOSUREHEIGHT = 90;
        //int LABELHEIGHT = 10;
        bool isBackColorChanged = false;
        int lalr1Number = 0;
        string lalr1Name = "LALR1Convert";
        string inputLogName = "LR1LR0";
        Dictionary<string, List<string>> log;
        Dictionary<string, List<string>> closure;
        Dictionary<string, List<string>> combinedClosure;
        Dictionary<string, string> adjRelation;
        Dictionary<string, int> edge;//一个closure总共有几条边
        Dictionary<string, int> drawingCount;//记录画到了closure的第几条边
        //Dictionary<string, int> offsetX;//记录Label的偏移X
        //Dictionary<string, int> offsetY;//记录Label的偏移Y
        double x = 0.5;
        double y = 1;
        double reScheduleX = 0.5;
        double reScheduleY = 1;
        bool isClosureRescheduled = false;
        public LALR1Automaton()
        {
            InitializeComponent();
            adjRelation = new Dictionary<string, string>();
            edge = new Dictionary<string, int>();
            drawingCount = new Dictionary<string, int>();
            //offsetX = new Dictionary<string, int>();
            //offsetY = new Dictionary<string, int>();
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
        private void BuildClosure(string closureName)
        {
            TextBox box = new TextBox();
            box.Width = ClOSUREWIDTH;
            box.Height = CLOSUREHEIGHT;
            if ((x + 2.5) * ClOSUREWIDTH > this.Size.Width)
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
            //this.pan.Controls.Add(box);
            x += 2.5;

        }
        private void ReArrangeClosure(){
            //this.StartPosition = FormStartPosition.CenterScreen;
            if (!isClosureRescheduled)
            {
                for (int i = 0; i < this.Controls.Count; i++)
                {
                    if (this.Controls[i] is TextBox && this.Controls[i].Name != "outputTextBox")
                    {
                        if ((reScheduleX + 2.5) * ClOSUREWIDTH > this.Size.Width)
                        {
                            reScheduleX = 0.5;
                            reScheduleY += 2;

                        }
                        Point p = new Point((int)(reScheduleX * ClOSUREWIDTH), (int)(reScheduleY * CLOSUREHEIGHT));
                        if (this.Controls[i].Location != p)
                        {
                            this.Controls[i].Location = p;
                        }
                        reScheduleX += 2.5;
                    }else if(this.Controls[i] is Label){
                        
                    }
                }
                isClosureRescheduled = true;
            }
        }
        private void ReBuildClosure(string closureName, string delClosureName,string context)
        {
            TextBox box = new TextBox();
            box.Width = ClOSUREWIDTH;
            box.Height = CLOSUREHEIGHT;
            box.Location = this.Controls[delClosureName].Location;
            box.Multiline = true;
            box.Name = closureName;
            box.Text = closureName;
            box.Font = new Font("微软雅黑", box.Font.Size, box.Font.Style | FontStyle.Bold);

            box.ScrollBars = ScrollBars.Vertical;
            box.ReadOnly = true;
            //box.AppendText("\r\n" + RewriteRuleExpression(closure[closureName][0]));
            box.AppendText("\r\n" + RewriteRuleExpression(context));
            
            this.Controls.RemoveByKey(delClosureName);
            this.Controls.Add(box);
            //this.Controls.RemoveByKey(delClosureName);
            //this.Controls.Add(box);

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
        private void AddLabel(Point position, string symbol, string name)
        {

            Label label = new Label();
            //label.Height = LABELHEIGHT;
            //label.Width = LABELWIDTH;
            label.Name = name;//用于在controls中找控件
            label.AutoSize = true;
            label.Text = symbol;
            label.BackColor = System.Drawing.Color.Transparent;
            label.Location = position;
            this.Controls.Add(label);
            //this.Controls.Add(label);

        }
        private void DrawEdge(string original, string symbol, string target, string edgeName)
        {
            //Graphics g = this.CreateGraphics();
            Point lineStart = new Point();
            Point lineEnd = new Point();
            Point symbolPoint = new Point();
            //lineStart.X = this.Controls[original].Location.X + ClOSUREWIDTH;
            //lineStart.Y = this.Controls[original].Location.Y + (int)((double)drawingCount[original] / edge[original] * CLOSUREHEIGHT);
            lineStart.X = this.Controls[original].Location.X + ClOSUREWIDTH;
            lineStart.Y = this.Controls[original].Location.Y + (int)((double)drawingCount[original] / edge[original] * CLOSUREHEIGHT);
            lineEnd.X = lineStart.X + (int)(0.5 * ClOSUREWIDTH);
            lineEnd.Y = lineStart.Y;
            symbolPoint.X = lineStart.X + (int)(0.1 * ClOSUREWIDTH);
            symbolPoint.Y = lineStart.Y;
            //g.DrawLine(new Pen(Color.Red, 3), lineStart, lineEnd);

            AddLabel(lineEnd, target, edgeName + "_target_" + target);
            //offsetX.Add(edgeName + "_target_" + target,(int)(1.5 * ClOSUREWIDTH));
            //offsetY.Add(edgeName + "_target_" + target,(int)((double)drawingCount[original] / edge[original] * CLOSUREHEIGHT));
            
            
            AddLabel(symbolPoint, symbol, edgeName + "_symbol_" + target);
            //offsetX.Add(edgeName + "_symbol_" + target, (int)(1.1 * ClOSUREWIDTH));
            //offsetY.Add(edgeName + "_symbol_" + target, (int)((double)drawingCount[original] / edge[original] * CLOSUREHEIGHT));
            
            AddLabel(lineStart, "------------",edgeName + "_line_" + target);
            //offsetX.Add(edgeName + "_line_" + target, ClOSUREWIDTH);
            //offsetY.Add(edgeName + "_line_" + target, (int)((double)drawingCount[original] / edge[original] * CLOSUREHEIGHT));
            /*
            Label line = new Label();
            line.Location = lineStart;
            line.Height = LABELHEIGHT / 2;
            line.Text = "------------";
            line.AutoSize = true;
            line.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(line);
             * */
            drawingCount[original]++;

        }

        private void DelLabel(string firstName){
            //用foreach的话会删不干净，指针会乱飞
            for (int i = this.Controls.Count - 1; i >= 0; i--) {
                if (this.Controls[i] is Label) { 
                    string fName = this.Controls[i].Name.Split('_')[0];
                    if (fName == firstName)
                    {
                        this.Controls.RemoveAt(i);
                    }
                }
            }
        }
        private void Redirect(string oldName, string newName) {
            for (int i = this.Controls.Count-1; i >= 0; i--) {
                if (this.Controls[i] is Label) {
                    string[] word = this.Controls[i].Name.Split('_');
                    string firstName = word[0];
                    string type = word[1];
                    string target = word[2];
                    if (type == "target" && target == oldName) {
                        this.Controls[i].Name = firstName + "_" + type + "_" + newName;
                        this.Controls[i].Text = newName;
                    }
                }
            }
        }

        public void BuildAllClosure() {
            //log[inputLogName].OrderByDescending();
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
                    DrawEdge(original, symbol, target, original);
                    if (!this.Controls.ContainsKey(target))
                    {
                        BuildClosure(target);
                    }
                    //FillClosure(target);
                    closureNum++;



                }
                else
                {
                    //outputTextBox.AppendText(log[inputLogName][closureNum] + "\r\n");
                    closureNum++;

                }
            }
        
        }
        public void InputClosure(Dictionary<string, List<string>> target, Dictionary<string, List<string>> combinedTarget)
        {
            closure = target;
            combinedClosure = combinedTarget;
        }

        private void NextStep_Click(object sender, EventArgs e)
        {

            if (lalr1Number < log[lalr1Name].Count())
            {
                string info = log[lalr1Name][lalr1Number];
                if (info[0] == '#')
                {
                    info = info.Replace("#", "");
                    string[] word = info.Split('_');
                    string start = word[0];
                    string target = word[1];
                    string final = word[2];
                    string context = word[3] + "_" + word[4] + "_" + word[5];
                    if (isBackColorChanged)
                    {
                        if (this.Controls.ContainsKey(start))
                        {
                            //this.Controls.RemoveByKey(start);
                            ReBuildClosure(final, start, context);
                        }
                        if (this.Controls.ContainsKey(target))
                            this.Controls.RemoveByKey(target);
                        DelLabel(target);
                        Redirect(start, final);
                        Redirect(target, final);
                        //this.Controls.Add(start);
                        lalr1Number++;
                    }
                    else
                    {
                        if (this.Controls.ContainsKey(start))
                            this.Controls[start].BackColor = Color.Red;
                        if (this.Controls.ContainsKey(target))
                            this.Controls[target].BackColor = Color.Red;
                    }
                    isBackColorChanged = !isBackColorChanged;

                    //DrawRedLine(start, target);
                    //DrawEdge(start, null, target);
                    //this.Controls.RemoveByKey(start);
                    //this.Controls.RemoveByKey(target);
                }
                else
                {
                    outputTextBox.AppendText(info + "\r\n");
                    lalr1Number++;
                }



            }
            else {
                //ReArrangeClosure();
                NextStep.Enabled = false;
                FinishButon.Enabled = false;
            }
            
        }

        private void FinishButon_Click(object sender, EventArgs e)
        {
            while (lalr1Number < log[lalr1Name].Count())
            {
                string info = log[lalr1Name][lalr1Number];
                if (info[0] == '#')
                {
                    info = info.Replace("#", "");
                    string[] word = info.Split('_');
                    string start = word[0];
                    string target = word[1];
                    string final = word[2];
                    string context = word[3] + "_" + word[4] + "_" + word[5];
                    if (isBackColorChanged)
                    {
                        if (this.Controls.ContainsKey(start))
                        {
                            //this.Controls.RemoveByKey(start);
                            ReBuildClosure(final, start, context);
                        }
                        if (this.Controls.ContainsKey(target))
                            this.Controls.RemoveByKey(target);
                        DelLabel(target);
                        Redirect(start, final);
                        Redirect(target, final);
                        //this.Controls.Add(start);
                        lalr1Number++;
                    }
                    else
                    {
                        if (this.Controls.ContainsKey(start))
                            this.Controls[start].BackColor = Color.Red;
                        if (this.Controls.ContainsKey(target))
                            this.Controls[target].BackColor = Color.Red;
                    }
                    isBackColorChanged = !isBackColorChanged;
                }
                else
                {
                    outputTextBox.AppendText(info + "\r\n");
                    lalr1Number++;
                }
            }
            NextStep.Enabled = false;
            FinishButon.Enabled = false;
        }
    }
}
