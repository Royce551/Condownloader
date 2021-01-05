using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Condownloader
{
    public partial class LogViewer : Form
    {
        private LoggingManager logs;
        public LogViewer(LoggingManager logs)
        {
            InitializeComponent();
            this.logs = logs;
            logs.NewLogsRecieved += Logs_NewLogsRecieved;
        }
        private void Logs_NewLogsRecieved(object sender, string e)
        {
            listBox1.Items.Add(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }
    }
}
