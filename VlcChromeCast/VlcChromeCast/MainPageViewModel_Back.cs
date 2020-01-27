using System;
using System.ComponentModel;
using LibVLCSharp.Shared;

namespace VlcChromeCast
{
    public class MainPageViewModel_Back : INotifyPropertyChanged
    {
        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of <see cref="MainPageViewModel_Back"/> class.
        /// </summary>
        public MainPageViewModel_Back()
        {
        }

        private LibVLC _libVLC;
        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.LibVLC"/> instance.
        /// </summary>
        public LibVLC LibVLC
        {
            get => _libVLC;
            private set => Set(nameof(LibVLC), ref _libVLC, value);
        }

        private MediaPlayer _mediaPlayer;
        /// <summary>
        /// Gets the <see cref="LibVLCSharp.Shared.MediaPlayer"/> instance.
        /// </summary>
        public MediaPlayer MediaPlayer
        {
            get => _mediaPlayer;
            private set => Set(nameof(MediaPlayer), ref _mediaPlayer, value);
        }

        /// <summary>
        /// Initialize LibVLC and playback when page appears
        /// </summary>
        public void OnAppearing(long time)
        {
            Core.Initialize();

            LibVLC = new LibVLC(new string[] { "--sout-keep" });


            var media = new Media(LibVLC,
                "https://player.vimeo.com/external/381866012.hd.mp4?s=ade26e2856bcd5f50d09acd9cd2449e57467514b&profile_id=174",
                FromType.FromLocation);

            //media.AddOption("casting_passthrough");
            //media.AddOption("--sout-chromecast-audio-passthrough");
            //media.AddOption("--sout-keep");


            MediaPlayer = new MediaPlayer(media) { EnableHardwareDecoding = true, Time = time };
            MediaPlayer.Play();
        }

        private void Set<T>(string propertyName, ref T field, T value)
        {
            if (field == null && value != null || field != null && !field.Equals(value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
