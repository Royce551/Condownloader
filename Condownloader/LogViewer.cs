using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Condownloader
{
    public partial class LogViewer : Form
    {
        private readonly LoggingManager logs;
        public LogViewer(LoggingManager logs)
        {
            InitializeComponent();
            this.logs = logs;
            logs.NewLogsRecieved += Logs_NewLogsRecieved;
            foreach (var log in logs.Logs) listBox1.Items.Add(log);
        }
        private void Logs_NewLogsRecieved(object sender, string e)
        {
            listBox1.Items.Add(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            using var dialog = new SaveFileDialog();
            dialog.Filter = "Text File|*.txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            await File.WriteAllLinesAsync(dialog.FileName, logs.Logs);
        }
    }
}
