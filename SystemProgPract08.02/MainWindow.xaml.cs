using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
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
using System.Windows.Threading;

namespace SystemProgPract08._02
{
    public class MyProcess : Process
    {
        public int PDI { get; set; }
        public new string ProcessName { get; set; }
        public new DateTime? StartTime { get; set; }
        public new string MachineName { get; set; }
        public new TimeSpan TotalProcessorTime { get; set; }

        public ProcessPriorityClass Priority { get; set; }
        public long  MemorySize{get;set;}


        public new TimeSpan UserProcessorTime { get; set; }


        



    }
    public partial class MainWindow : Window
    {
         DispatcherTimer _timer = null;
        public MainWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer();
            _timer.Tick += _timer_Tick;
            comboBox.SelectedIndex = 0;
            _timer.Interval = new TimeSpan(0, 0, 1);
            _timer.Start();
            Load();

        }
      


       

        private void _timer_Tick(object sender, EventArgs e)
        {
            Load();
        
        }


        public void Load()
        {
           
            grid.Items.Clear();
            var procces = Process.GetProcesses();
            MyProcess proc = new MyProcess();
            foreach (var p in procces)
            {
                    proc = new MyProcess();
                try
                {
                    proc.PDI = p.Id;
                    proc.ProcessName = p.ProcessName;
                    proc.MemorySize=p.PrivateMemorySize64;
                    proc.TotalProcessorTime= p.TotalProcessorTime;
                    proc.UserProcessorTime= p.UserProcessorTime;
                    proc.Priority = p.PriorityClass;
                    proc.StartTime = p.StartTime;
 
                }
                catch {
             
                }
                finally
                {
                    if(proc.PDI!=0 )
                        grid.Items.Add(proc);
                }
            }

            

        }

     
 

        private void stopProcessButton_Click(object sender, RoutedEventArgs e)
        {
            if (grid.SelectedItem != null)
            {
                try
                {
                    Process p = Process.GetProcessById(((MyProcess)(grid.SelectedItem)).PDI);
                    p.Kill();
                    Load();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void startProcessButton_Click(object sender, RoutedEventArgs e)
        {
            string path= textBox.Text;
            if (String.IsNullOrWhiteSpace((path)))
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter= "Exe Files(.exe)|*.exe|All Files(*.*)|*.*";
              
                if (ofd.ShowDialog() == true)
                {
                    path = ofd.FileName;

                }
                else
                    return;
            }
            Process.Start(path);
            Load();
        }



        private void ComboBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }



        private void ComboBoxItem_Selected(object sender, RoutedEventArgs e)
        {
            bool p = int.TryParse(((ComboBoxItem)sender).Content.ToString(), out int sec);
            if (p)
            {
                _timer.Interval = new TimeSpan(0, 0, sec);
             
                _timer.Start();
            }
        }

        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var proc=((MyProcess)grid.SelectedItem);
            if (proc != null)
            {
                PDITextBlock.Text = proc.PDI.ToString();
                ProcessNameTextBlock.Text = proc.ProcessName;
                PriorityTextBlock.Text = proc.Priority.ToString();
                PrivateMemorySizeTextBlock.Text = proc.MemorySize.ToString();
                StartTimTextBlock.Text = proc.StartTime.ToString();
                TotalProcessorTimeTextBlock.Text = proc.TotalProcessorTime.ToString();
                UserProcessorTimeTextBlock.Text = proc.UserProcessorTime.ToString();
            }
        }
    }
}
