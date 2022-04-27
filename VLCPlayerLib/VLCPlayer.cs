using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCPlayerLib
{

    public interface IVLCPlayer
    {
        void NotifyTime(string time, long time_t, int instance = 0);
        void NotifyDuration(string time, long mvt, int instance = 0);
    }



    public class DebugMessagesEventArgs : EventArgs
    {
        public int severity { get; set; }
        public DateTime Dated { get; set; }
        public string message { get; set; }
    }
    public class VLCPlayer
    {

        LibVLC libVLC;
        Media media;
        MediaPlayer mp;

        public event EventHandler<DebugMessagesEventArgs> DebugMessages;
 

        int m_instance = 0;
        public VLCPlayer(int instance = 0)
        {
            m_instance = instance;

        }
        IVLCPlayer pCallback = null;
        public bool Init(IVLCPlayer p, out string outMessage, string url = "")
        {
            try
            {
                Core.Initialize();
                pCallback = p;

                DebugMessages?.Invoke(this, new DebugMessagesEventArgs
                {
                      Dated =  DateTime.Now,
                       message = "Initializing", 
                        severity = 0
                });

                libVLC = new LibVLC(enableDebugLogs: false);
                //mp.EnableHardwareDecoding = true;
                outMessage = string.Empty;
                if (string.IsNullOrEmpty(url) == false)
                {
                    return SetMedia(url, out outMessage);
                }
                return true;
            }
            catch (Exception err)
            {
                outMessage = err.Message;
                return false;
            }
        }
        public VLCState State
        {

            get
            {
                if (mp == null)
                    return VLCState.Error; 
                return mp.State;
            }
        }
            

        public bool SetMedia(string url, out string outMessage)
        {
            try
            {
                outMessage = string.Empty;
                media = new Media(libVLC, new Uri(url));
                mp = new MediaPlayer(media);
                mp.TimeChanged += Mp_TimeChanged;
                // Position changed is not good
                //mp.PositionChanged += Mp_PositionChanged;
                mp.LengthChanged += Mp_LengthChanged;
                return true;
            }
            catch (Exception err)
            {
                outMessage = err.Message;
                return false;
            }
        }
        public bool SetMedia(string url, IntPtr handle, out string outMessage)
        {
            try
            {
                outMessage = string.Empty;
                media = new Media(libVLC, new Uri(url));
                mp = new MediaPlayer(media);
                mp.TimeChanged += Mp_TimeChanged;
                mp.LengthChanged += Mp_LengthChanged;
                // Position changed is not good
                //mp.PositionChanged += Mp_PositionChanged;
                if (handle != IntPtr.Zero)
                {
                    mp.Hwnd = handle;
                }
                return true;
            }
            catch (Exception err)
            {
                outMessage = err.Message;
                return false;
            }
        }

        private void Mp_LengthChanged(object sender, MediaPlayerLengthChangedEventArgs e)
        {
        
            string time = TimeSpan.FromMilliseconds(e.Length).ToString(@"hh\:mm\:ss");
            pCallback.NotifyDuration(time, e.Length, m_instance);
        }

        private void Mp_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            Console.WriteLine(e.Position * 1000);
        }

        private void Mp_TimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            string time = TimeSpan.FromMilliseconds(e.Time).ToString(@"hh\:mm\:ss");
            pCallback.NotifyTime(time, e.Time, m_instance);
        }

        public bool EnableMouseInput
        {
            set
            {
                mp.EnableMouseInput = value;
            }
            
        }
        public bool IsInit()
        {
            return mp == null ? false : true;
        }
        IntPtr m_handle = IntPtr.Zero;
        public bool SetHandle(IntPtr Handle)
        {
            if (mp == null)
                return false;
            if (m_handle != IntPtr.Zero)
            {
                mp.Hwnd = Handle;
            }
            else
            {
                m_handle = Handle;
            }
            return true;
        }
        public void Stop()
        {
            if (mp != null)
            {
                mp.Stop();
            }
        }

        public void Pause()
        {
            if (mp != null)
            {
                mp.Pause();
            }
        }
        public void SetPositon(float pos)
        {
            if (mp != null)
            {
                mp.Position = pos;
            }
        }
        public long Length
        {
            get
            {
                if (mp != null)
                {
                    return mp.Length;
                }
                return 0;
            }
        }

        public long Time
        {
            get
            {
                if (mp != null)
                {
                    return mp.Time;
                }
                return 0;
            }
            set
            {
                if (mp != null)
                {
                    mp.Time = value;
                }
            }
        }

        public bool Play()
        {
            if (mp == null)
            {
                return false;
            }
              
            if (m_handle != IntPtr.Zero)
            {
                mp.Hwnd = m_handle;                
            }
            mp.Play();

            mp.EnableKeyInput = false;
            mp.EnableMouseInput = false;

            return true;
        }
         
    }
     
}
