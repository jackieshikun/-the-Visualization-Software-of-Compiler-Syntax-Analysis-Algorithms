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
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LL1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            LL1Window window = new LL1Window();
            window.ShowDialog();
            
        }

        private void SLR1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            SLR1Window slr1 = new SLR1Window();
            slr1.ShowDialog();
            
        }

        private void LR1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            LR1Window lr1 = new LR1Window();
            lr1.ShowDialog();
        }

        private void LALR1_Click(object sender, EventArgs e)
        {
            this.Close();
            this.Dispose();
            LALR1Window lalr1 = new LALR1Window();
            lalr1.ShowDialog();
        }

        
    }
}
