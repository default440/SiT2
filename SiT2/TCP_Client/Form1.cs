using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCP_Client
{
    public partial class Form1 : Form
    {
        static int port = 3004;
        static string address = "127.0.0.1";
        
        IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Parse(address), port);
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        DateTime dateTime = DateTime.Now;
        string theme = "";

        public Form1()
        {
            InitializeComponent();

            monthCalendar1.SetDate(dateTime);
            textBox1.Text = theme;
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            dateTime = monthCalendar1.SelectionStart;
            label3.Text = dateTime.ToString() + "  --  " + theme;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            theme = textBox1.Text;
            label3.Text = dateTime.ToString() + "  --  " + theme;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (theme != null)
            {
                socket.Connect(ipEndPoint);

                byte[] byte_dateTime = Encoding.ASCII.GetBytes(dateTime.Ticks.ToString());
                byte[] byte_theme = Encoding.ASCII.GetBytes(theme);

                socket.Send(byte_dateTime);
                socket.Send(BitConverter.GetBytes(byte_theme.Length));
                socket.Send(byte_theme);

                socket.Close();
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
