using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace MulticastChat
{
    public partial class ChatForm : Form
    {
        /// <summary>
        /// Флаг установки следующего потока
        /// </summary>
        private bool done = true;

        private SynchronizationContext _uiContext;

        /// <summary>
        /// Сокет клиента
        /// </summary>
        private UdpClient client;

        /// <summary>
        /// Групповой адрес рассылки
        /// </summary>
        private IPAddress groupAdress;
        private int localPort;
        private int remotePort;
        private int ttl;

        /// <summary>
        /// конечная точка
        /// </summary>
        private IPEndPoint endPoint;
        private UnicodeEncoding encoding = new UnicodeEncoding();

        /// <summary>
        /// Имя пользователя
        /// </summary>
        private string name;
        /// <summary>
        /// Сообщение пользователя
        /// </summary>
        private string message;



        public ChatForm()
        {
            InitializeComponent();
            var configuration = ConfigurationManager.AppSettings;
            groupAdress = IPAddress.Parse(configuration.Get("GlobalAddress"));
            localPort = int.Parse(configuration.Get("LocalPort"));
            remotePort = int.Parse(configuration.Get("RemotePort"));
            ttl = int.Parse(configuration.Get("TTL"));
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textName.Text))
            {
                MessageBox.Show(this, "Введите имя", "Warning MulticastChat", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            name = textName.Text;
            textName.ReadOnly = true;

            try
            {
                client = new UdpClient(localPort);
                client.JoinMulticastGroup(groupAdress, ttl);

                endPoint = new IPEndPoint(groupAdress, remotePort);

                Task.Factory.StartNew(Listen, TaskScheduler.FromCurrentSynchronizationContext());

                var data = encoding.GetBytes(name + " joined the chat");
                client.Send(data, data.Length, endPoint);

                Start.Enabled = false;
                Stop.Enabled = Send.Enabled = true;
            }
            catch (SocketException ex)
            {
                MessageBox.Show(this, ex.Message, "Error MulticastChat", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Listen(object sheduler)
        {
            done = false;
            try
            {
                while (!done)
                {
                    IPEndPoint ep = null;
                    var data = encoding.GetString(client.Receive(ref ep));


                    Task.Factory.StartNew(() =>
                    {
                        textMessages.Text = DateTime.Now.ToShortTimeString() + " " + data + "\r\n" +
                                            textMessages.Text;

                    }, CancellationToken.None, TaskCreationOptions.None, (TaskScheduler)sheduler).Wait();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error MulticastChat", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void Send_Click(object sender, EventArgs e)
        {
            try
            {
                var data = encoding.GetBytes(name + ": " + textMessage.Text);
                client.Send(data, data.Length, endPoint);
                textMessage.Clear();
                textMessage.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "Error MulticastChat", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Stop_Click(object sender, EventArgs e)
        {
            var data = encoding.GetBytes(name + " has left the chat");
            client.Send(data, data.Length, endPoint);

            client.DropMulticastGroup(groupAdress);
            client.Close();

            done = true;

            Start.Enabled = true;
            Stop.Enabled = Send.Enabled = false;
        }

    }
}
