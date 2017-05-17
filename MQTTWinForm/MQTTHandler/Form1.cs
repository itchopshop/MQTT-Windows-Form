using System;
using System.Net;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;



namespace MQTTHandler
{
    public partial class Form1 : Form
    {
        MqttClient client;// = new MqttClient(IPAddress.Parse(texBox1.Text));
        //MqttClient clientSub;
        delegate void SetTextCallback(string text);
        

        public Form1()
        {
            InitializeComponent();


        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            try
            {
                client = new MqttClient(textBox1.Text);
                client.Connect(textBox2.Text);
                listBox1.Items.Add("* Client connected");
                client.Subscribe(new string[] { textBox3.Text }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
                listBox1.Items.Add("** Subscribing to: " + textBox3.Text);

                client.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(EventPublished);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                client.Publish(textBox3.Text, Encoding.UTF8.GetBytes(textBox4.Text), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE,true);
                listBox1.Items.Add("*** Publishing on: " + textBox3.Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                client.Disconnect();
                listBox1.Items.Add("* Client disconnected");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        //private void MainForm_Load(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        //IPAddress HostIP;
        //        //HostIP = IPAddress.Parse(textBox1.Text);
        //        //clientSub = new MqttClient(HostIP);
        //        client.MqttMsgPublishReceived += new MqttClient.MqttMsgPublishEventHandler(EventPublished);

                
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.ToString());
        //    }
        //}

        private void EventPublished(Object sender, MqttMsgPublishEventArgs e)
        {
            try
            {
                SetText("*** Received Message");
                SetText("*** Topic: " + e.Topic);
                SetText("*** Message: " + Encoding.UTF8.GetString(e.Message));
                SetText("");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void SetText(string text)
        {
            if (listBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                Invoke(d, new object[] { text });
            }
            else
            {
                listBox1.Items.Add(text);
            }
        }

    }
}
