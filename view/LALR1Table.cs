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
    public partial class LALR1Table : Form
    {
        string inputLogName = "LALR1table";
        Dictionary<string, List<string>> log;
        string[,] action;
        string[,] goTo;
        int tableLine;
        public LALR1Table()
        {
            InitializeComponent();
        }

        public void LogName(string name)
        {
            inputLogName = name;
        }
        public void InputGotoAndActionTable(string[,] goToTable, string[,] actionTable)
        {
            goTo = goToTable;
            action = actionTable;
        }
        public void InputLog(Dictionary<string, List<string>> temp)
        {
            log = temp;
        }
        public void BuildTable()
        {
            dataTable.Columns.Add("col0", "非终结符");
            int i;
            for (i = 1; i < action.GetLength(1); i++)
            {
                dataTable.Columns.Add("col" + i, action[0, i]);
            }
            for (i = i + 1; i - action.GetLength(1) < goTo.GetLength(1); i++)
            {
                dataTable.Columns.Add("col" + i, goTo[0, i - action.GetLength(1)]);
            }

            for (int row = 1; row < action.GetLength(0); row++)
            {
                dataTable.Rows.Add();
                dataTable.Rows[row - 1].Cells[0].Value = action[row, 0];
            }
            tableLine = 0;
        }
        private void NextStep_Click(object sender, EventArgs e)
        {
            if (tableLine < log[inputLogName].Count())
            {
                string line = log[inputLogName][tableLine];
                if (line[0] == '#')
                {
                    string[] word = line.Replace("#", "").Split('_');
                    string whichTable = word[0];
                    int row = Int32.Parse(word[1]);
                    int column = Int32.Parse(word[2]);
                    if (whichTable == "action")
                    {
                        dataTable.Rows[row - 1].Cells[column].Value = word[3];
                    }
                    else if (whichTable == "goto")
                    {
                        dataTable.Rows[row - 1].Cells[column + action.GetLength(1) - 1].Value = word[3];
                    }
                    else
                    {
                        tableOutput.AppendText("不可能！！！" + "\r\n");
                    }


                }
                else
                {
                    //string [] word = log[inputLogName][tableLine].Split('_');
                    //tableOutput.AppendText("根据Automaton,发现" + word[0] + "遇到" + word[1] + "转移到" + word[2] + "\r\n");
                    tableOutput.AppendText(log[inputLogName][tableLine]);
                }
                tableLine++;
            }
            else
            {
                tableOutput.AppendText("结束\r\n");
                NextStep.Enabled = false;
                FinishButton.Enabled = false;
            }
        }

        private void FinishButton_Click(object sender, EventArgs e)
        {
            while (tableLine < log[inputLogName].Count())
            {
                string line = log[inputLogName][tableLine];
                if (line[0] == '#')
                {
                    string[] word = line.Replace("#", "").Split('_');
                    string whichTable = word[0];
                    int row = Int32.Parse(word[1]);
                    int column = Int32.Parse(word[2]);
                    if (whichTable == "action")
                    {
                        dataTable.Rows[row - 1].Cells[column].Value = word[3];
                    }
                    else if (whichTable == "goto")
                    {
                        dataTable.Rows[row - 1].Cells[column + action.GetLength(1) - 1].Value = word[3];
                    }
                    else
                    {
                        tableOutput.AppendText("不可能！！！" + "\r\n");
                    }


                }
                else
                {
                    //string [] word = log[inputLogName][tableLine].Split('_');
                    //tableOutput.AppendText("根据Automaton,发现" + word[0] + "遇到" + word[1] + "转移到" + word[2] + "\r\n");
                    tableOutput.AppendText(log[inputLogName][tableLine]);
                }
                tableLine++;
            }
            NextStep.Enabled = false;
            FinishButton.Enabled = false;
        }
    }
}
