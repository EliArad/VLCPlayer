using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using VLCPlayerLib;

namespace VideoControl
{
    public interface IVideoControl
    {
        void NotifyPlay(string URL, bool isFile, int instance = 0);
        void NotifyTime(string time, long time_t, int instance = 0);
        void NotifyFullScreen(bool f, int instance = 0);
        void NotifyDuration(string time, long mvt, int instance = 0);
    }
    public partial class VideoControl : UserControl, IVideoControl
    {
        bool m_lastPlayedIsFile = true;
        Task m_task;
        bool m_playing = false;
        VLCPlayer m_vlc = new VLCPlayer();
            
        public VideoControl()
        {
            InitializeComponent();
          
            m_lastWidth = this.Width;
            m_lastHeight = this.Height;
 
            this.KeyDown += VideoControl_KeyDown;
            pCallback.Add(this);
        }

        private void VideoControl_KeyDown(object sender, KeyEventArgs e)
        {

            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;

            foreach (IVideoControl p in pCallback)
            {
                p.NotifyFullScreen(false);
            }
        }

        public void SetHandle()
        {
            m_vlc.SetHandle(this.Handle);
        }
        List<IVideoControl> pCallback = new List<IVideoControl>();
        public void SetInterface(IVideoControl p)
        {
            pCallback.Add(p);
        }

        public bool LastPlayedIsFile
        {
            set
            {
                m_lastPlayedIsFile = value;
            }
        }

        string m_url = string.Empty;
        public string URL
        {
            get
            {
                return m_url;
            }
            set
            {
                m_url = value;
            }
        }

        string m_fileName = string.Empty;
        public string FileName
        {
            get
            {
                return m_fileName;
            }
            set
            {
                m_fileName = value;
            }
        }

        private void loadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog1 = new OpenFileDialog
            {
                Title = "Browse Video Files",
                CheckFileExists = true,
                CheckPathExists = true,
                DefaultExt = "ts",
                Filter = "All Media Files|*.wav;*.aac;*.ts;*.wma;*.wmv;*.avi;*.mpg;*.mpeg;*.m1v;*.mp2;*.mp3;*.mpa;*.mpe;*.m3u;*.mp4;*.mov;*.3g2;*.3gp2;*.3gp;*.3gpp;*.m4a;*.cda;*.aif;*.aifc;*.aiff;*.mid;*.midi;*.rmi;*.mkv;*.WAV;*.AAC;*.WMA;*.WMV;*.AVI;*.MPG;*.MPEG;*.M1V;*.MP2;*.MP3;*.MPA;*.MPE;*.M3U;*.MP4;*.MOV;*.3G2;*.3GP2;*.3GP;*.3GPP;*.M4A;*.CDA;*.AIF;*.AIFC;*.AIFF;*.MID;*.MIDI;*.RMI;*.MKV",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                m_fileName = openFileDialog1.FileName;
                m_lastPlayedIsFile = true;
                Play(out string outMessage);
                
                if (pCallback != null)
                {
                    foreach (IVideoControl p in pCallback)
                    {
                        p.NotifyPlay(m_fileName, true);
                    }
                }
            }
        }

        int m_lastWidth;
        int m_lastHeight;
        bool m_fullScreen = false;
        private void fullScrenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_vlc.IsInit() == false)
                return;
            m_fullScreen = !m_fullScreen;
             
            foreach (IVideoControl p in pCallback)
            {
                p.NotifyFullScreen(m_fullScreen);
            }
        }

        void ShowDuration()
        {
            var t = new Thread(() =>
            {
                while (m_playing == true)
                {
                    long mvt = m_vlc.Length;
                    string time = TimeSpan.FromMilliseconds(mvt).ToString(@"hh\:mm\:ss");                    
                    if (mvt > 0)
                    {                        
                        foreach (IVideoControl p in pCallback)
                        {
                            p.NotifyDuration(time, mvt);
                        }

                        return;
                    }
                    Thread.Sleep(100);
                }
            });
            t.Start();

        }
        public bool Play(out string outMessage)
        {
             
            m_vlc.Stop();

            if (m_lastPlayedIsFile == false && string.IsNullOrEmpty(m_url) == false)
            {
                if (m_vlc.SetMedia(m_url, this.Handle, out outMessage) == false)
                    return false;
            } else 
            if (m_lastPlayedIsFile == true && string.IsNullOrEmpty(m_fileName) == false)
            {
                if (m_vlc.SetMedia(m_fileName, this.Handle, out outMessage) == false)
                    return false;
            } 
            else
            {
                outMessage = "not selected any file or url";
                return false;
            }
            if (m_vlc.State == VLCState.Error)
            {
                outMessage = "State error";
                return false;
            }

            
            m_playing = true;
            m_vlc.Play();
            ShowDuration();
            ShowTime();
            outMessage = string.Empty;
            return true;
        }


        async void ShowTime()
        {
            await Task.Run(() =>
            {
                if (m_task != null)
                    m_task.Wait();
            });
            
            m_task = new Task(ShowTimeTask);
            m_task.Start();

        }
        public bool Init(out string outMessage)
        {
            if (m_vlc.Init(out outMessage) == true)
            {                
                return true;
            }
            return false;
        }
        public void Pause()
        {
            m_vlc.Pause();
        }
        public long Time
        {
            set
            {
                m_vlc.Time = value;
            }
            get
            {
                return m_vlc.Time;
            }
        }
        public void Stop()
        {
            m_playing = false;
            m_task.Wait();
            m_vlc.Stop();
        }
        void ShowTimeTask()
        {
            while (m_playing == true)
            {
                long t = m_vlc.Time;
                string time = TimeSpan.FromMilliseconds(t).ToString(@"hh\:mm\:ss");
                foreach (IVideoControl p in pCallback)
                {
                    p.NotifyTime(time, t);
                }
                Thread.Sleep(1000);
            }
        }

        public void NotifyPlay(string URL, bool isFile, int instance = 0)
        {
            ShowDuration();
            ShowTime();
        }

        public void NotifyTime(string time, long time_t, int instance = 0)
        {

        }

        public void NotifyFullScreen(bool f, int instance = 0)
        {

            if (m_vlc.SetHandle(this.Handle) == false)
                return;
            this.Visible = true;
            this.BringToFront();
        }

        public void NotifyDuration(string time, long mvt, int instance = 0)
        {

        }

        private void streamURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectURL u = new SelectURL(m_url);
            if (u.ShowDialog() == DialogResult.OK)
            {
                m_url = u.URL;
                if (string.IsNullOrEmpty(m_url) == false)
                {
                    m_lastPlayedIsFile = false;
                    Play(out string outMessage);
                    if (pCallback != null)
                    {
                        foreach (IVideoControl p in pCallback)
                        {
                            p.NotifyPlay(m_url, false);
                        }
                    }
                }
            }
        }
        public void Close()
        {
            Stop();
        }
    }
}
