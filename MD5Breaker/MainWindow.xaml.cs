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
            DecrypterRange range = new DecrypterRange(MD5Decrypter.CharRange.Length);

            int max = Convert.ToInt32(txtb_max.Text);
            int min = Convert.ToInt32(txtb_min.Text);

            int i;
            int[] array = new int[min];

            for(i = 0; i < min; i++)
                array[i] = 0;
            
            range.setStartRange(array);
            array = new int[max];

             for(i = 0; i < max; i++)
                 array[i] = MD5Decrypter.CharRange.Length;

             range.setEndRange(array);

             r = new runner(txtb_findhash.Text, range);
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

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
    }

    public class runner
    {
        public string currentString;
        MD5Decrypter dec;


        public runner(string hash, DecrypterRange range)
        {
            dec = new MD5Decrypter(hash, range);
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
