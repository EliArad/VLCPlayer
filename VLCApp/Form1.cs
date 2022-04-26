using DBConnLib;
using InvokersLib;
using System;
using System.Windows.Forms;
using VideoControl;

namespace VLCApp
{
    public partial class Form1 : Form, IVideoControl
    {

        public Form1()
        {
            InitializeComponent();

            AppSettings.Instance.Load("VLCApp.json");

            if (videoControl1.Init(out string outMessage) == false)
            {
                MessageBox.Show(outMessage);
                return;
            }

            videoControl1.SetInterface(this);
            this.FormClosing += Form1_FormClosing;
            videoControl1.URL = AppSettings.Instance.Config.URL;
            videoControl1.FileName = AppSettings.Instance.Config.FileName;
            videoControl1.LastPlayedIsFile = AppSettings.Instance.Config.LastPlayedIsFile;


        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            videoControl1.Close();
            AppSettings.Instance.Save();
        }

        void Play()
        {            
            videoControl1.Play(out string outMessage);           
        } 

        private void btnStart_Click(object sender, EventArgs e)
        {
             Play();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Stop();
        }
        void Stop()
        {
            videoControl1.Stop();
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            videoControl1.Pause();
        }

        bool m_trackDown = false;
        private void trackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            m_trackDown = true;
        }

        private void trackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            m_trackDown = false;
        }

        private void trackBar1_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_trackDown == true)
            {
                videoControl1.Time = trackBar1.Value * 1000;
            }
        }

        public void NotifyPlay(string URL,  bool isFile, int instance = 0)
        {
            if (isFile == false)
                AppSettings.Instance.Config.URL = URL;
            else
                AppSettings.Instance.Config.FileName = URL;

            AppSettings.Instance.Config.LastPlayedIsFile = isFile;

        }

        struct WindowRect
        {
            public int Left;
            public int Top;
            public int Width;
            public int Height;
        }
        WindowRect m_winRect = new WindowRect();
        public void NotifyFullScreen(bool f, int instance = 0)
        {
            panel1.Visible = !f;
            trackBar1.Visible = !f;
            if (f == true)
            {
                m_winRect.Left = this.Left;
                m_winRect.Top = this.Top;
                m_winRect.Width = this.Width;
                m_winRect.Height = this.Height;

                this.WindowState = FormWindowState.Maximized;                
                this.Width = Screen.PrimaryScreen.Bounds.Width;
                this.Height = Screen.PrimaryScreen.Bounds.Height;
                videoControl1.Width = Screen.PrimaryScreen.Bounds.Width;
                videoControl1.Height = Screen.PrimaryScreen.Bounds.Height;
                this.FormBorderStyle = FormBorderStyle.None;                
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.Sizable;

                this.Left = m_winRect.Left;
                this.Top = m_winRect.Top;
                this.Width = m_winRect.Width;
                this.Height = m_winRect.Height;

                videoControl1.Width = this.Width;
                videoControl1.Height = this.trackBar1.Top;
            }
        }

        public void NotifyTime(string time, long time_t, int instance = 0)
        {
            
            INVOKERS.InvokeControlText(lblTime, time);
            if (m_trackDown == true)
                return;
            INVOKERS.InvokeTrackerValue(trackBar1, (int)(time_t / 1000));
        }

        public void NotifyDuration(string time, long mvt, int instance = 0)
        {
            INVOKERS.InvokeControlText(lblMovieDuration, time);
            mvt = mvt / 1000;
            INVOKERS.InvokeTrackerMaximum(trackBar1, (int)mvt);            
        }
    }
}
