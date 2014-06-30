using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace CloseMT2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Process _process = null;
        private long _restartCount = 0;
        private String _fileName = null;

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Open1 = new OpenFileDialog();
            Open1.Filter = "可执行文件|*.exe";
            if (Open1.ShowDialog() == DialogResult.OK)
            {
                timer1.Stop();
                _fileName = Open1.FileName;
                _restartCount = 0;
                doScan();
            }
        }

        /// <summary>
        /// 扫描
        /// </summary>
        private void doScan()
        {
            label2.Text = System.IO.Path.GetFileName(_fileName);
            String proceeeName = System.IO.Path.GetFileNameWithoutExtension(_fileName);
            Process[] ps = Process.GetProcessesByName(proceeeName);
            if (ps.Length > 0)
            {
                //已经在运行了
                _process = ps[0];
                if (!checkBox1.Checked)
                {
                    //只是扫描是否存在
                } 
                else
                {
                    //需要重启
                    _process.Kill();
                    Process proxyProcess = new Process();
                    proxyProcess.StartInfo.FileName = _fileName;
                    proxyProcess.Start();
                    _restartCount++;
                }
                
            }
            else
            {
                Process proxyProcess = new Process();
                proxyProcess.StartInfo.FileName = _fileName;
                proxyProcess.Start();
                _restartCount++;
            }
            timer1.Tag = int.Parse(textBox1.Text);
            timer1.Start();
            label3.Text = String.Format("已重启{0}次，{1}分钟后再次扫描", _restartCount, (int)timer1.Tag);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int tmp = (int)timer1.Tag;
            tmp--;
            timer1.Tag = tmp;
            if (tmp == 0)
            {
                doScan();
            } 
            else
            {
                label3.Text = String.Format("已重启{0}次，{1}分钟后再次扫描", _restartCount, (int)timer1.Tag);
            }
        }
    }
}
