﻿using LibVLCSharp.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VLCPlayerLib
{
    public class VLCPlayer
    {

        LibVLC libVLC;
        Media media;
        MediaPlayer mp;

        public bool Init(out string outMessage, string url = "")
        {
            try
            {
                Core.Initialize();

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