using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace Learn_01
{
    public partial class Form1 : Form
    {

        // Declare threads
        public Thread T1, T2, T3, T4, T5 ;

        List<Label> listLabel = new List<Label>();
        List<Label> listLabel01 = new List<Label>();
        List<Thread> listThread = new List<Thread>();

        List<double> listDbl = new List<double>();

        List<List<int>> listInt = new List<List<int>>();

        bool isNumeric(string val)
        {
            if (val.Split('.').Length > 2) { return false; }
            foreach (char c in val)
            {
                if (! char.IsDigit(c) && c != '.') { return false; }
            }
            return true;
        }
        
        public Form1()
        {
            InitializeComponent();
            //this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);

            


            btnAll.Click += new System.EventHandler(this.startAllThreads);

            listThread = new List<Thread>() {T1,T2,T3,T4,T5 };
            listLabel = new List<Label>() { label1, label2, label3, label4, label5 };
            listLabel01 = new List<Label>() { label6, label7, label8, label9, label10 };

            for (int i = 0; i < 5; i++)
            {
                listInt.Add(new List<int>() { i, Convert.ToInt32(Convert.ToString(i) + Convert.ToString(i)) });
            }
        }


        void test()
        {
            T2 = new Thread(worker01);
            //T3 = new Thread(worker02);
            T2.Start();
            T2.Join();
            //T3.Start();


            //T3.Join();

            //worker01();
            worker02();



            Console.WriteLine("Done");
            //for (int i = 0; i < 10; i++)
            //{
            //    //updateLable(label1, Convert.ToString(i));
                

            //}
        }

        void test2()
        {
            updateLable(label1, "runing");

            Thread.Sleep(2000);
        }

        void test(Label lbl, string val) {
            for (int i = 0; i < 10; i++)
            {
                updateLable(label1, Convert.ToString(i));
                Thread.Sleep(500);

            }
        }

        void updateLable(Label lbl, string val)
        {
            //if (InvokeRequired)
            //{
            //    Invoke((MethodInvoker)delegate { updateLable(lbl, val); });
            //}
            //else { lbl.Text = val; }

            Invoke((MethodInvoker)delegate { lbl.Text = val; });
        }


        void updateTb(TextBox tb, string val) { Invoke((MethodInvoker)delegate {  tb.Text = val; }); }

        private void textBox1_TextChanged(object sender, EventArgs e)
        { if (!isNumeric(textBox1.Text)) {  textBox1.Text = null; } }



        void writeLabel(Label lbl, int val)
        {
            for (int i = 0; i < 10; i++)
            {
                updateLable(lbl, Convert.ToString(val * 100 + i));
                Thread.Sleep(500);

            }
            
        }

        void do_conv(Label lbl)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();

            updateLable(lbl, "Started");

            List<float> pExcess = new List<float>();
            for (int i = 0; i < 1441; i++)
            {
                pExcess.Add((float)(i * .01));
            }

            List<float> uhg = new List<float>();
            for (int i = 0; i < 15000; i++)
            {
                uhg.Add((float)(i * .1));
            }

            List<float> conv = new List<float>();
            for (int i = 0; i < pExcess.Count + uhg.Count - 1; i++)
            {
                conv.Add(0);
            }
            float peak = 0;
            for (int i = 0; i < pExcess.Count; i++)
            {
                for (int j = 0; j < uhg.Count; j++)
                {
                    conv[i + j] += pExcess[i] * uhg[j];
                    if (peak < conv[i + j]) { peak = conv[i + j]; }
                }
            }

            updateLable(lbl, Convert.ToString(peak));
            //Console.WriteLine(peak);
            ////MessageBox.Show(Convert.ToString(peak));
            watch.Stop();
            updateTb(textBox1, Convert.ToString(watch.Elapsed.Seconds + "," + watch.Elapsed.Milliseconds));
            Console.WriteLine(watch.Elapsed.Seconds + "," + watch.Elapsed.Milliseconds);

        }


        void startAllThreads(object sender, EventArgs e)
        {
            //listLabel = new List<Label>() { label1, label2, label3, label4, label5 };
            //listLabel01 = new List<Label>() { label6, label7, label8, label9, label10 };
            //listInt.Clear();
            //for (int i = 0; i < 5; i++)
            //{
            //    listInt.Add(new List<int>() { i, Convert.ToInt32(Convert.ToString(i) + Convert.ToString(i)) });
            //}


            //worker01();
            //T1.Join();
            //worker02();

            T1 = new Thread(test);
            T1.Start();

        }

        void worker01()
        {
            //foreach (Label lbl in listLabel)
            //{
            //    ThreadPool.QueueUserWorkItem(WaitCallback => do_conv(lbl));
            //}

            //foreach (List<int> lst in listInt)
            //{
            //    ThreadPool.QueueUserWorkItem(WaitCallback => writeLabel(listLabel[lst[0]], lst[1]));
            //}
            //foreach (List<int> lst in listInt)
            //{
            //    listThread[lst[0]] = new Thread(delegate () { writeLabel(listLabel[lst[0]], lst[1]); });
            //    listThread[lst[0]].Start();
            //}


            using (CountdownEvent cdEvnet = new CountdownEvent(listInt.Count))
            {
                foreach (List<int> lst in listInt)
                {
                    ThreadPool.QueueUserWorkItem(WaitCallback => { updateLable(listLabel01[lst[0]], "doing"); writeLabel(listLabel[lst[0]], lst[1]); cdEvnet.Signal(); });
                    

                }
                cdEvnet.Wait();
            }

        }

        void worker02()
        {
            foreach (List<int> lst in listInt)
            {
                ThreadPool.QueueUserWorkItem(WaitCallback => updateLable(listLabel01[lst[0]], listLabel[lst[0]].Text));
            }

        }

    }
}
