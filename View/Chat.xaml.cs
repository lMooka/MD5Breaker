using Control;
using MD5Breaker.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace View
{
    public partial class Chat : UserControl
    {
        public delegate void UpdateMessage(string msg);

        public Chat()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            ViewController.Instance.OnMessageReceived += NewMessage;
        }

        private void NewMessage(string Message)
        {
            Dispatcher.Invoke((Action)delegate() { MessageArea.Text += Message + "\n"; });
            
        }

        private void Press_Enter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ModelController.Instance.SendMessage(txtb_msg.Text);
                //MessageArea.Text += txtb_msg.Text + "\n";
                txtb_msg.Text = "";
            }
        }
    }
}
