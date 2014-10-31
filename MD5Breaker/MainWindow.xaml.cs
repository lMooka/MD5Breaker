using MD5Breaker.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MD5Breaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Thread t;
        runner r;
        public static Queue<string> queue;
        public static string currentpw;
        public static string currenthash;

        public static MainWindow Instance { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            queue = new Queue<string>();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            r = new runner();
            t = new Thread(new ThreadStart(r.Run));

            //t.SetApartmentState(ApartmentState.STA);
            t.Start();
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
    }

    public class runner
    {
        public string currentString;
        MD5Decrypter dec;
        Window w;
        TextBox output;

        public runner()
        {
            DecrypterRange range = new DecrypterRange(MD5Decrypter.CharRange.Length);

            range.setStartRange(0, 0, 0);
            range.setStartRange(67, 67, 67, 67);

            dec = new MD5Decrypter("fa7f08233358e9b466effa1328168527", range); //kkkk
        }

        public void Run()
        {
            //StreamWriter sw = new StreamWriter("C:\\Users\\Guilherme\\Desktop\\hash.txt", false);
            
            try
            {
                while (true)
                {
                    dec.Crack();
                    //sw.WriteLine(MD5Decrypter.currentHashPassword + "," + MD5Decrypter.currentPassword + ";");
                }
            }
            catch (HashFoundException e)
            {
                MessageBox.Show("found: " + e.Message);
            }
            //catch (Exception e)
            //{
            //    MessageBox.Show("found: " + e.Message);
            //}
        }
    }
}
