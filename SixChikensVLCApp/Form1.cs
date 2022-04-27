using DBConnLib;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoControlLib;

namespace SixChikensVLCApp
{
    public partial class Form1 : Form, IVideoControl
    {

        List<VideoControl> m_videoControls = new List<VideoControl>();
        public Form1()
        {
            InitializeComponent();
            try
            {
                
                this.KeyPreview = true;
                AppSettings.Instance.Load("SixChikensVLCApp.json");

                VideoControlLib.VideoControl[] vc = { videoControl1,videoControl2,videoControl3,
                                                      videoControl4,videoControl5,videoControl6 };

                foreach (VideoControlLib.VideoControl c in vc)
                {
                    if (c.Init(out string outMessage) == false)
                    {
                        MessageBox.Show(outMessage);
                        return;
                    }
                }

                this.Resize += Form1_Resize;
                this.KeyDown += Form1_KeyDown;

                
                
                this.FormClosing += Form1_FormClosing;
                videoControl1.URL = AppSettings.Instance.Config.URL1;
                videoControl2.URL = AppSettings.Instance.Config.URL2;
                videoControl3.URL = AppSettings.Instance.Config.URL3;
                videoControl4.URL = AppSettings.Instance.Config.URL4;
                videoControl5.URL = AppSettings.Instance.Config.URL5;
                videoControl6.URL = AppSettings.Instance.Config.URL6;

                 
               
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        void ShowVideo()
        {

             
            VideoControlLib.VideoControl[] vc = { videoControl1,videoControl2,videoControl3,
                                                      videoControl4,videoControl5,videoControl6 };


            foreach (VideoControl c in vc)
            {
                c.SetInterface(this);
                m_videoControls.Add(c);
                c.LastPlayedIsFile = false;                
            }

                             
            foreach (VideoControl c in vc)
            {                        
                if (c.Play(out string outMessage) == false)
                {
                    MessageBox.Show(outMessage);
                    return;
                }
            }
            AutoArrange();                  
             
            
        }

        bool m_border = true;
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                m_border = !m_border;
                if (m_border == false)
                {
                    this.FormBorderStyle = FormBorderStyle.None;
                }
                else
                {
                    this.FormBorderStyle = FormBorderStyle.Sizable;
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            AutoArrange();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            VideoControlLib.VideoControl[] vc = { videoControl1,videoControl2,videoControl3,
                                                      videoControl4,videoControl5,videoControl6 };

            foreach (VideoControlLib.VideoControl c in vc)
            {
                c.Close();                
            }
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

         
         
        public void NotifyPlay(string URL,  bool isFile, int instance = 0)
        {
          

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
                
            }
        }

        public void NotifyTime(string time, long time_t, int instance = 0)
        {
            
          
        }

        public void NotifyDuration(string time, long mvt, int instance = 0)
        {
                   
        }
        bool m_primaryScreen = true;
        bool m_windowed = true;
        void AutoArrange()
        {

      
            try
            {
                int screenWidth = 0;
                int screenHeight = 0;

                if (m_windowed == false)
                {
                    if (m_primaryScreen == true)
                    {
                        screenWidth = Screen.AllScreens[0].Bounds.Width;
                        screenHeight = Screen.AllScreens[0].Bounds.Height;
                    }
                    else
                    {
                        screenWidth = Screen.AllScreens[1].Bounds.Width;
                        screenHeight = Screen.AllScreens[1].Bounds.Height;
                    }
                }
                else
                {
                    screenWidth = this.Width;
                    screenHeight = this.Height;
                }

                int vcount = m_videoControls.Count;
                int x = 0;
                int y = 0;
                int i = 0;

                int xwidth = 0;
                int yheight = 0;
                if (vcount == 1)
                {
                    xwidth = screenWidth;
                    yheight = screenHeight;
                    m_videoControls[i].Left = 0;
                    m_videoControls[i].Top = 0;
                    m_videoControls[i].Width = xwidth;
                    m_videoControls[i].Height = yheight;
                    UpdateBorders();
                    return;
                }
                else
                if (vcount == 2)
                {
                    xwidth = screenWidth / 2;
                    yheight = screenHeight;

                    for (i = 0; i < vcount / 2; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }
                    for (; i < vcount; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }
                    UpdateBorders();
                    return;
                }
                else
                if (vcount == 5 || vcount == 6 || vcount == 12)
                {
                    xwidth = screenWidth / 3;
                    yheight = screenHeight / 2;
                }
                else
                if (vcount == 7)
                {
                    xwidth = screenWidth / 4;
                    yheight = screenHeight / 2;
                }
                else
                if (vcount == 3 || vcount == 4)
                {
                    xwidth = screenWidth / 2;
                    yheight = screenHeight / 2;
                }
                else
                if (vcount == 8)
                {
                    xwidth = screenWidth / 4;
                    yheight = screenHeight / 2;
                }
                else
                if (vcount == 10)
                {
                    xwidth = screenWidth / 5;
                    yheight = screenHeight / 2;
                }
                else
                if (vcount == 9)
                {
                    xwidth = screenWidth / 3;
                    yheight = screenHeight / 3;

                    for (i = 0; i < vcount / 3; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }
                    y += yheight;
                    x = 0;
                    for (; i < 6; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }

                    y += yheight;
                    x = 0;
                    for (; i < vcount; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }
                    UpdateBorders();
                    return;
                }
                else
                if (vcount == 16)
                {
                    xwidth = screenWidth / 4;
                    yheight = screenHeight / 4;

                    for (i = 0; i < 4; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }
                    y += yheight;
                    x = 0;
                    for (; i < 8; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }

                    y += yheight;
                    x = 0;
                    for (; i < 12; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }

                    y += yheight;
                    x = 0;
                    for (; i < 16; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }
                    UpdateBorders();
                    return;
                }
                else
                if (vcount == 20)
                {
                    xwidth = screenWidth / 5;
                    yheight = screenHeight / 4;

                    for (i = 0; i < 5; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }
                    y += yheight;
                    x = 0;
                    for (; i < 10; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }

                    y += yheight;
                    x = 0;
                    for (; i < 15; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }

                    y += yheight;
                    x = 0;
                    for (; i < 20; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }
                    UpdateBorders();
                    return;
                }
                else
                if (vcount == 25)
                {
                    xwidth = screenWidth / 5;
                    yheight = screenHeight / 5;

                    for (i = 0; i < 5; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }
                    y += yheight;
                    x = 0;
                    for (; i < 10; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }

                    y += yheight;
                    x = 0;
                    for (; i < 15; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }

                    y += yheight;
                    x = 0;
                    for (; i < 20; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }

                    y += yheight;
                    x = 0;
                    for (; i < 25; i++)
                    {
                        m_videoControls[i].Left = x;
                        m_videoControls[i].Top = y;
                        m_videoControls[i].Width = xwidth;
                        m_videoControls[i].Height = yheight;
                        x += xwidth;
                    }
                    UpdateBorders();
                    return;
                }
                else
                {
                    MessageBox.Show("Not handled");
                    return;
                }
                x = 0;
                y = 0;
                for (i = 0; i < vcount / 2; i++)
                {
                    m_videoControls[i].Left = x;
                    m_videoControls[i].Top = y;
                    m_videoControls[i].Width = xwidth;
                    m_videoControls[i].Height = yheight;
                    x += xwidth;
                }
                y += yheight;
                x = 0;
                for (; i < vcount; i++)
                {
                    m_videoControls[i].Left = x;
                    m_videoControls[i].Top = y;
                    m_videoControls[i].Width = xwidth;
                    m_videoControls[i].Height = yheight;
                    x += xwidth;
                }
                UpdateBorders();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
        void UpdateBorders()
        {
            foreach (VideoControl c in m_videoControls)
            {
                c.ShowBorders(true);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Visible = false;
            Opacity = 0;
             
            ShowVideo();

            Visible = true;
            Opacity = 100;



        }
    }
}
