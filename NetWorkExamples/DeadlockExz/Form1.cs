using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeadlockExz
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            Foo().Wait();
            label1.Text = Guid.NewGuid().ToString();
        }

        private async Task Foo()
        {
            await Bar().ConfigureAwait(false);
        }

        private async Task Bar()
        {
            await Task.Delay(2000);
        }
    }
}
