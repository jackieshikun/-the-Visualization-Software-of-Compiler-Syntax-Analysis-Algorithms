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
    public partial class LALR1Window : Form
    {
        public LALR1 backEnd;
        private string inputLogName = "LALR1table";
        private bool isSyntaxValid;
        public LALR1Window()
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
            if (fileName.ShowDialog() == DialogResult.OK)
            {
                //MessageBox.Show("OK");
                string path = fileName.FileName.ToString();
                backEnd = new LALR1(path);
                Dictionary<string, List<string>> rule = backEnd.GetRuleSet();
                RuleDisplay.Clear();
                if (rule == null)
                    MessageBox.Show("LALR1运行失败，输入的规则式有问题");
                else
                {
                    foreach (string key in rule.Keys)
                    {
                        foreach (string showRule in rule[key])
                        {
                            RuleDisplay.AppendText(key + "->" + showRule + "\r\n");
                        }
                    }
                    backEnd.SolveFirstSet();
                    backEnd.SolveFollowSet();
                    backEnd.BuildLR1ForView();
                    backEnd.ConvertLR1ToLALR1();
                    isSyntaxValid = backEnd.BuildTable();
                    MessageBox.Show("输入的规则式合法");
                    GetLR0.Enabled = true;
                }


            }
        }

        private void backing_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            MainWindow m = new MainWindow();
            m.ShowDialog();
        }

        private void GetTable_Click(object sender, EventArgs e)
        {
            LALR1Table t = new LALR1Table();

            t.LogName(inputLogName);
            t.InputGotoAndActionTable(backEnd.GetGotoTable(), backEnd.GetActionTable());
            t.BuildTable();
            t.InputLog(backEnd.FetchOutput());

            t.Show();
            if (isSyntaxValid)
                AnalyzeInput.Enabled = true;
            else
                MessageBox.Show("不是LALR1文法，无法使用Analyze按钮");
        }

        private void AnalyzeInput_Click(object sender, EventArgs e)
        {
            LALR1Analyze ana = new LALR1Analyze();
            ana.InputAlorithm(backEnd);
            ana.Show();
        }

        private void GetLR0_Click(object sender, EventArgs e)
        {
            LALR1Automaton auto = new LALR1Automaton();
            //backEnd.fir

            auto.InputClosure(backEnd.closure,backEnd.combinedClosure);
            auto.GetInfo(backEnd.FetchOutput());
            auto.BuildAllClosure();
            auto.Show();
            GetTable.Enabled = true;
        }
    }
}
