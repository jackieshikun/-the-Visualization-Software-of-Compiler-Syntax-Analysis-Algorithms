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
    public partial class LL1Table : Form
    {
        int tableLine;
        string InputLogName = "LL1table";
        Dictionary<string, List<string>> log;
        public LL1Table()
        {
            InitializeComponent();
            
           
        }

        private void dataTable_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
            
            
        }
        public void InputLog(Dictionary<string, List<string>> temp) { 
            log = temp;
        }
        public void BuildTable(string [,] anaTable) {
            dataTable.Columns.Add("col0","非终结符");
            for (int i = 1; i < anaTable.GetLength(1); i++) {
                dataTable.Columns.Add("col"+i,anaTable[0,i]);
            }

            for (int row = 1; row < anaTable.GetLength(0); row++)
            {
                dataTable.Rows.Add();
                dataTable.Rows[row - 1].Cells[0].Value = anaTable[row, 0];
            }
            tableLine = 0;
        }

        private void Table_Load(object sender, EventArgs e)
        {

        }

        private void tableOutput_TextChanged(object sender, EventArgs e)
        {

        }

        private void NextStep_Click(object sender, EventArgs e)
        {
            if (tableLine < log[InputLogName].Count())
            {
                string line = log[InputLogName][tableLine];
                if (line[0] == '#')
                {
                    string[] word = line.Replace("#","").Split('_');
                    int row = Int32.Parse(word[0]);
                    int column = Int32.Parse(word[1]);
                    dataTable.Rows[row - 1].Cells[column].Value = word[2];
                }
                else {
                    tableOutput.AppendText(log[InputLogName][tableLine] + "\r\n");
                }
                tableLine++;
            }
            else {
                tableOutput.AppendText("结束\r\n");
                NextStep.Enabled = false;
                FinishButon.Enabled = false;
            }
        }

        private void FinishButon_Click(object sender, EventArgs e)
        {
            while (tableLine < log[InputLogName].Count()) {
                string line = log[InputLogName][tableLine];
                if (line[0] == '#')
                {
                    string[] word = line.Replace("#", "").Split('_');
                    int row = Int32.Parse(word[0]);
                    int column = Int32.Parse(word[1]);
                    dataTable.Rows[row - 1].Cells[column].Value = word[2];
                }
                else
                {
                    tableOutput.AppendText(log[InputLogName][tableLine] + "\r\n");
                }
                tableLine++;
            }
            NextStep.Enabled = false;
            FinishButon.Enabled = false;
        }
    }
}
