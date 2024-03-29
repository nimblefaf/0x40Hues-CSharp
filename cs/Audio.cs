﻿using System;
using System.Runtime.InteropServices;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Mix;


namespace HuesSharp
{
    public class Audio
    {

        public static string err;
        public static bool check;

        /// <summary>
        /// Frequency
        /// </summary>
        private static readonly int HZ = 44100;
        /// <summary>
        /// Init state
        /// </summary>
        public static bool InitDefaultDevice;
        /// <summary>
        /// Channel
        /// </summary>
        public int Stream_Loop;
        /// <summary>
        /// Volume
        /// </summary>
        public int Volume = 50;

        public int Channel;
        public int Stream_Buildup;

        long loop_len;
        public double build_len_seconds;



        GCHandle handle_Buildup = new GCHandle();
        GCHandle handle_Loop = new GCHandle();

        public byte[] loop_mem;
        public byte[] build_mem;



        private static bool InitBass(int hz)
        {
            if (!InitDefaultDevice)
                InitDefaultDevice = Bass.BASS_Init(-1, hz, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            return InitDefaultDevice;
        }

        public void Play_With_Buildup()
        {
            Stop();
            if (InitBass(HZ))
            {
                Channel = BassMix.BASS_Mixer_StreamCreate(HZ, 2, BASSFlag.BASS_MIXER_END);

                handle_Buildup = GCHandle.Alloc(build_mem, GCHandleType.Pinned);
                handle_Loop = GCHandle.Alloc(loop_mem, GCHandleType.Pinned);

                Stream_Buildup = Bass.BASS_StreamCreateFile(handle_Buildup.AddrOfPinnedObject(), 0, build_mem.LongLength, BASSFlag.BASS_STREAM_DECODE);
                //build_len_bytes = Bass.BASS_ChannelGetLength(Stream_Buildup, BASSMode.BASS_POS_BYTE);
                build_len_seconds = GetTimeOfStream(Stream_Buildup);

                Stream_Loop = Bass.BASS_StreamCreateFile(handle_Loop.AddrOfPinnedObject(), 0, loop_mem.LongLength, BASSFlag.BASS_STREAM_DECODE);
                loop_len = Bass.BASS_ChannelGetLength(Stream_Loop);

                BassMix.BASS_Mixer_StreamAddChannel(Channel, Stream_Buildup, BASSFlag.BASS_DEFAULT);
                BassMix.BASS_Mixer_StreamAddChannelEx(Channel, Stream_Loop, BASSFlag.BASS_MIXER_NORAMPIN, Bass.BASS_ChannelSeconds2Bytes(Channel, build_len_seconds), 0);

                _loopSync = BassMix.BASS_Mixer_ChannelSetSync(Stream_Loop, BASSSync.BASS_SYNC_POS | BASSSync.BASS_SYNC_MIXTIME, loop_len, _loopSyncCallback, new IntPtr(1));
            }
        }

        private int _loopSync = 0;
        private SYNCPROC _loopSyncCallback;
        public Audio()
        {
            _loopSyncCallback = new SYNCPROC(EndSync);
        }
        private void EndSync(int syncHandle, int channel, int data, IntPtr user)
        {
            BassMix.BASS_Mixer_ChannelSetPosition(Stream_Loop, user.ToInt64());
        }

        public void Play_Without_Buildup()
        {
            Stop();
            if (InitBass(HZ))
            {
                Stream_Loop = Bass.BASS_SampleLoad(loop_mem, 0, loop_mem.Length, 1, BASSFlag.BASS_SAMPLE_LOOP);
                Channel = Bass.BASS_SampleGetChannel(Stream_Loop, BASSFlag.BASS_SAMPLE_LOOP);
            }
        }

        public void Play()
        {
            Bass.BASS_ChannelSetAttribute(Channel, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100f);
            Bass.BASS_ChannelPlay(Channel, false);
        }


        /// <summary>
        /// Length of channel in seconds
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static double GetTimeOfStream(int stream)
        {
            long TimeBytes = Bass.BASS_ChannelGetLength(stream);
            double Time = Bass.BASS_ChannelBytes2Seconds(stream, TimeBytes);
            //return (int)Time;
            return Time;
        }

        /// <summary>
        /// Current position in seconds
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public double GetPosOfStream(int stream)
        {
            long pos;
            if (handle_Loop.IsAllocated) pos = BassMix.BASS_Mixer_ChannelGetPosition(stream, BASSMode.BASS_POS_BYTE);
            else pos = Bass.BASS_ChannelGetPosition(stream);
            return Bass.BASS_ChannelBytes2Seconds(stream, pos);
        }

        /// <summary>
        /// Stop
        /// </summary>
        public void Stop()
        {
            Bass.BASS_ChannelStop(Channel);
            if (handle_Loop.IsAllocated) handle_Loop.Free();
            if (handle_Buildup.IsAllocated) handle_Buildup.Free();
            Bass.BASS_StreamFree(Channel);
            Bass.BASS_SampleFree(Channel);

            Bass.BASS_StreamFree(Stream_Buildup);
            Bass.BASS_StreamFree(Stream_Loop);

            ////Uncomment this in case of unsolvable memory leak.
            ////The whole BASS module will be freed, creating a small pause before new song is played
            //Bass.BASS_Free();
            //InitDefaultDevice = false;
        }


        // Will be used for Editor tab
        public static void SetPosOfScroll(int stream, int pos)
        {
            Bass.BASS_ChannelSetPosition(stream, (double)pos);
        }

        /// <summary>
        /// Setting volume
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="vol"></param>
        public void SetVolumeToStream(int stream, int vol)
        {
            Volume = vol;
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);
            Bass.BASS_ChannelPlay(stream, false);
        }

    }
}
