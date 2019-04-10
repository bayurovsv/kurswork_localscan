using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.Serialization;
using System.Diagnostics;

namespace WindowsFormsApp4
{
    public partial class Form1 : Form
    {
        public int StartPort;
        public int EndPort;
        public int timeO;
        public int step = 1;
        public Form1()
        {
            InitializeComponent();
            listBox1.Items.Clear();
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            timeO = Convert.ToInt32(numericUpDown3.Value);
            try
            {
                AutoResetEvent waiter = new AutoResetEvent(false);
                byte[] buffer = Encoding.ASCII.GetBytes("тестируем Ping");
                string host = System.Net.Dns.GetHostName();
                textBox1.Text = host;
                List<string> serversList = new List<string>();
                IPAddress ip = Dns.GetHostByName(host).AddressList[0];
                textBox2.Text = ip.ToString();
                string[] ipParts = ip.ToString().Split('.');
                string[] ips = new string[254];
                for (int i = 1; i < 255; i++)
                {
                    ips[i - 1] = ipParts[0] + '.' + ipParts[1] + '.' + ipParts[2] + '.' + i.ToString();
                }
                foreach (string ipToScan in ips)
                {
                    serversList.Add(ipToScan);
                }
                foreach (string server in serversList)
                {
                    Ping ping = new Ping();
                    ping.PingCompleted += new PingCompletedEventHandler(p_PingCompleted);
                    PingOptions options = new PingOptions(64, true);
                    ping.SendAsync(IPAddress.Parse(server), timeO, buffer, options, waiter);
                }
                listBox1.Items.Clear();
            }
            catch (Exception)
            {
            }
        }
        private void p_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Reply.Status == IPStatus.Success)
            {
                listBox1.Items.Add(e.Reply.Address.ToString());
            }
            ((AutoResetEvent)e.UserState).Set();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                trrr_vse();
                if (richTextBox2.Text == "")
                    MessageBox.Show("Свободных портов не найдено!");
            }
            else
            {
                trrr();
                if (richTextBox1.Text == "")
                    MessageBox.Show("Свободных портов не найдено!");
            }
        }
        public void trrr_vse()
        {
            richTextBox2.Text = null;
            StartPort = Convert.ToInt32(numericUpDown1.Value);
            EndPort = Convert.ToInt32(numericUpDown2.Value);
            progressBar1.Minimum = StartPort;
            progressBar1.Maximum = EndPort*listBox1.Items.Count+ listBox1.Items.Count;
            progressBar1.Value = StartPort;
            for (int CurrPort = StartPort; CurrPort <= EndPort; CurrPort++)
            {
                TcpClient TcpScan = new TcpClient();
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    try
                    {
                        TcpScan.Connect(Convert.ToString(listBox1.Items[i]), CurrPort);
                        richTextBox2.Text += "IP-Adress" + Convert.ToString(listBox1.Items[i]) + ": " + "Port " + Convert.ToString(CurrPort) + " Открыт\r\n" + Environment.NewLine;
                    }
                    catch
                    {
                    }
                    progressBar1.Value += step;
                }
            }
            progressBar1.Value = StartPort;
        }
        public void trrr()
        {
            richTextBox1.Text = null;
            StartPort = Convert.ToInt32(numericUpDown1.Value);
            EndPort = Convert.ToInt32(numericUpDown2.Value);
            progressBar1.Minimum = StartPort;
            progressBar1.Maximum = EndPort+1;
            progressBar1.Value = StartPort;
            for (int CurrPort = StartPort; CurrPort <= EndPort; CurrPort++)
            { 
                TcpClient TcpScan = new TcpClient();
                try
                {
                    TcpScan.Connect(textBox2.Text, CurrPort);
                    richTextBox1.Text += "Port " + Convert.ToString(CurrPort) + " Открыт\r\n" + Environment.NewLine;
                }
                catch
                {          
                }
                progressBar1.Value += step;        
            }  
            progressBar1.Value = StartPort;
        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
        }
    }
}
