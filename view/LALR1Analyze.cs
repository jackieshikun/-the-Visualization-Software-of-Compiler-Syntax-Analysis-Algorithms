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
    public partial class LALR1Analyze : Form
    {
        private LALR1 alogrithm;
        private Dictionary<string, List<string>> log;
        private string logKey = "LALR1Analyze";
        private int outputLine = 0;
        public LALR1Analyze()
        {
            InitializeComponent();
            log = new Dictionary<string, List<string>>();
        }

        public void InputAlorithm(LALR1 al)
        {
            alogrithm = al;
        }
        private void FromKeyBorad_Click(object sender, EventArgs e)
        {
            string line = inputTextBox.Text;
            //Console.WriteLine("line :" + line);
            alogrithm.Analyze(line);
            MessageBox.Show("运行完成");
            stackOutput.Clear();
            InputOutput.Clear();
            LogOutput.Clear();
            log = alogrithm.FetchOutput();
            outputLine = 0;
            FinishButton.Enabled = true;
            NextStep.Enabled = true;
        }


        private void FinishButton_Click(object sender, EventArgs e)
        {
            while (outputLine < log[logKey].Count())
            {
                string output = log[logKey][outputLine];
                if (output[0] == '#')
                {
                    string[] word = output.Replace("#", "").Split('_');
                    stackOutput.AppendText(word[0] + "\r\n");
                    InputOutput.AppendText(word[1] + "\r\n");
                }
                else
                {
                    LogOutput.AppendText(log[logKey][outputLine] + "\r\n");
                }
                outputLine++;
            }
            NextStep.Enabled = false;
            FinishButton.Enabled = false;
        }

        private void NextStep_Click_1(object sender, EventArgs e)
        {
            if (outputLine < log[logKey].Count())
            {
                string output = log[logKey][outputLine];
                if (output[0] == '#')
                {
                    string[] word = output.Replace("#", "").Split('_');
                    stackOutput.AppendText(word[0] + "\r\n");
                    InputOutput.AppendText(word[1] + "\r\n");
                }
                else
                {
                    LogOutput.AppendText(log[logKey][outputLine] + "\r\n");
                }
                outputLine++;
            }
            else
            {
                LogOutput.AppendText("结束\r\n");
                NextStep.Enabled = false;
                FinishButton.Enabled = false;
            }
        }
    }
}
