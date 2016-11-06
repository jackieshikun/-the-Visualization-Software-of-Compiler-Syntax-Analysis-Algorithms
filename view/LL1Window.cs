using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ConsoleApplication1.parser;

namespace ConsoleApplication1.view
{
    public partial class LL1Window : Form
    {
        //private Log a;
        private LL1 backEnd;
        private bool isSyntaxValid;
        //private LogOutput log;
        public LL1Window()
        {
            InitializeComponent();
            
            
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileName = new OpenFileDialog();
            
            fileName.InitialDirectory = Application.StartupPath;
            fileName.Filter = "txt files (*.txt)|*.txt";
            fileName.FilterIndex = 1;
            fileName.RestoreDirectory = true;
            if (fileName.ShowDialog() == DialogResult.OK) {

                string path = fileName.FileName.ToString();
                //string Name = path.Substring(path.LastIndexOf)
                backEnd = new LL1(path);
                Dictionary<string, List<string>> rule = backEnd.GetRuleSet();
                RuleDisplay.Clear();
                if (rule == null)
                    MessageBox.Show("LL1运行失败，输入的规则式有问题");
                else {
                    foreach (string key in rule.Keys) {
                        foreach (string showRule in rule[key]) {
                            RuleDisplay.AppendText(key + "->" + showRule + "\r\n");
                        }
                    }
                    MessageBox.Show("输入的规则式合法");
                    backEnd.SolveFirstSet();
                    backEnd.SolveFollowSet();
                    isSyntaxValid = backEnd.BuildTable();
                    GetFirstAndFollow.Enabled = true;
                }
                
            }
           
            
            
        }

        private void GetFirstAndFollow_Click(object sender, EventArgs e)
        {
            
            FirstAndFollow print = new FirstAndFollow();
            print.InputList(backEnd.FetchOutput());
            //print.InputFirstSet(backEnd.GetFirstSet());
            //print.InputFollowSet(backEnd.GetFollowSet());
            print.Show();
            GetTable.Enabled = true;
            //print.PrintFirstAndFollow(backEnd.GetFirstSet(),backEnd.GetFollowSet());
        }

        private void GetTable_Click(object sender, EventArgs e)
        {
            LL1Table t = new LL1Table();
            
            t.BuildTable(backEnd.GetAnaTable());
            t.InputLog(backEnd.FetchOutput());
            t.Show();
            if (isSyntaxValid == true)
                AnalyzeInput.Enabled = true;
            else
                MessageBox.Show("不是LL1型文法，不能使用Analyze按钮");
        }

        private void AnalyzeInput_Click(object sender, EventArgs e)
        {
            LL1AnalyzeTable anaT = new LL1AnalyzeTable();
            anaT.InputAlorithm(backEnd);
            anaT.Show();
        }

        private void RuleDisplay_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void backing_Click(object sender, EventArgs e)
        {
            
            this.Close();
            this.Dispose();
            MainWindow m = new MainWindow();
            m.ShowDialog();
            
        }

       

        

        
    }
}
