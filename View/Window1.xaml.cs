using Control;
using MD5Breaker.Core;
using MD5Breaker.Networking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace View
{
    public partial class Window1 : Window
    {
        public static Queue<string> queue;
        public static string currentpw;
        public static string currenthash;

        public static Window1 Instance { get; private set; }

        public Window1()
        {
            InitializeComponent();
            Instance = this;
            queue = new Queue<string>();
            ViewController.Instance.OnHashFoundEvent += ShowHashFound;
        }

        private void ShowHashFound(string Message)
        {
            MessageBox.Show(Message);
        }



        private void Decrypt_Click(object sender, RoutedEventArgs e)
        {
            uint max = Convert.ToUInt32(txtb_max.Text);
            uint min = Convert.ToUInt32(txtb_min.Text);

            ModelController.Instance.CrackHash(txtb_findhash.Text, min, max);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            lbl_output.Text = currentpw;
        }

        private void Window_LayoutUpdated_1(object sender, EventArgs e)
        {
            lbl_output.Text = MD5Decrypter.currentPassword;
            lbl_hash.Text = MD5Decrypter.currentHashPassword;
        }

        // Conectar
        private void Conectar(object sender, RoutedEventArgs e)
        {
            try
            {
                string IP = txtb_IP.Text;
                int port = int.Parse(txtb_Port.Text);

                ModelController.Instance.Connect(IP, port);
            }
            catch (SystemException)
            {
                MessageBox.Show("Formato incorreto de IP ou Porta.");
            }
        }

        // escutar porta
        private void Listen(object sender, RoutedEventArgs e)
        {
            try
            {
                int port = int.Parse(txtb_ListenPort.Text);

                ModelController.Instance.Listen(port);
            }
            catch (SystemException)
            {
                MessageBox.Show("Formato incorreto de IP ou Porta.");
            }
        }

        private void Test_Click(object sender, RoutedEventArgs e)
        {
            ulong value = ulong.Parse(txtb_value.Text);
            uint r = Convert.ToUInt32(MD5Decrypter.CharRange.Length);
            DecrypterRange range = new DecrypterRange(new uint[] { 0, 0, 0, 0 }, new uint[] { r, r, r, r, r, r, r, r }, r);

            range.Plus(value);

            foreach (int i in range.currentRange)
            {
                txtb_text.Text += i + ",";
            }
            txtb_text.Text += "\n";
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DecrypterRange range = new DecrypterRange(Convert.ToUInt64(test_blockid.Text), Convert.ToUInt64(test_blocksize.Text), Convert.ToUInt32(test_charoffset.Text));

        }
    }



}
