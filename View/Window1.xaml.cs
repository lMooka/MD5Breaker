﻿using Control;
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
        Thread t;
        runner r;

        public static Queue<string> queue;
        public static string currentpw;
        public static string currenthash;

        public static Window1 Instance { get; private set; }

        public Window1()
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

            for (i = 0; i < min; i++)
                array[i] = 0;

            range.setStartRange(array);
            array = new int[max];

            for (i = 0; i < max; i++)
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

        // Conectar
        private void Conectar(object sender, RoutedEventArgs e)
        {
           try
           {
               string IP = txtb_IP.Text;
               int port = int.Parse(txtb_Port.Text);

               ModelController.Instance.Connect(IP, port);
           }
            catch(SystemException ex)
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
            catch (SystemException ex)
            {
                MessageBox.Show("Formato incorreto de IP ou Porta.");
            }
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
            try
            {
                while (true)
                {
                    dec.Crack();
                }
            }
            catch (HashFoundException e)
            {
                MessageBox.Show("found: " + e.Message);
            }
        }
    }

}
