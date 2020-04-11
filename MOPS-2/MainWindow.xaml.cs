﻿using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Threading;
using System.IO;
using Un4seen.Bass;
using System.Drawing;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace MOPS
{
    public struct Palette
    {
        public string name;
        public Brush brush;

        public Palette(string f1, Brush f2)
        {
            name = f1; brush = f2;
        }
    }

    public class rdata
    {
        public string Name { get; set; }
        public int Ind { get; set; }
    }

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random rnd = new Random();

        Audio Player = new Audio();

        public DispatcherTimer Timer = new DispatcherTimer(DispatcherPriority.Send);
        public DispatcherTimer AnimTimer = new DispatcherTimer(DispatcherPriority.Render);
        double Correction;
        public double beat_length = 0;
        public double buildup_beat_len = 0;

        public static Palette[] hues =
        {
            new Palette("Black", (SolidColorBrush)new BrushConverter().ConvertFromString("#000000")),
            new Palette("Brick", (SolidColorBrush)new BrushConverter().ConvertFromString("#550000")),
            new Palette("Crimson", (SolidColorBrush)new BrushConverter().ConvertFromString("#aa0000")),
            new Palette("Red", (SolidColorBrush)new BrushConverter().ConvertFromString("#ff0000")),
            new Palette("Turtle", (SolidColorBrush)new BrushConverter().ConvertFromString("#005500")),
            new Palette("Sludge", (SolidColorBrush)new BrushConverter().ConvertFromString("#555500")),
            new Palette("Brown", (SolidColorBrush)new BrushConverter().ConvertFromString("#aa5500")),
            new Palette("Orange", (SolidColorBrush)new BrushConverter().ConvertFromString("#ff5500")),
            new Palette("Green", (SolidColorBrush)new BrushConverter().ConvertFromString("#00aa00")),
            new Palette("Grass", (SolidColorBrush)new BrushConverter().ConvertFromString("#55aa00")),
            new Palette("Maize", (SolidColorBrush)new BrushConverter().ConvertFromString("#aaaa00")),
            new Palette("Citrus", (SolidColorBrush)new BrushConverter().ConvertFromString("#ffaa00")),
            new Palette("Lime", (SolidColorBrush)new BrushConverter().ConvertFromString("#00ff00")),
            new Palette("Leaf", (SolidColorBrush)new BrushConverter().ConvertFromString("#55ff00")),
            new Palette("Chartreuse", (SolidColorBrush)new BrushConverter().ConvertFromString("#aaff00")),
            new Palette("Yellow", (SolidColorBrush)new BrushConverter().ConvertFromString("#ffff00")),
            new Palette("Midnight", (SolidColorBrush)new BrushConverter().ConvertFromString("#000055")),
            new Palette("Plum", (SolidColorBrush)new BrushConverter().ConvertFromString("#550055")),
            new Palette("Pomegranate", (SolidColorBrush)new BrushConverter().ConvertFromString("#aa0055")),
            new Palette("Rose", (SolidColorBrush)new BrushConverter().ConvertFromString("#ff0055")),
            new Palette("Swamp", (SolidColorBrush)new BrushConverter().ConvertFromString("#005555")),
            new Palette("Dust", (SolidColorBrush)new BrushConverter().ConvertFromString("#555555")),
            new Palette("Dirt", (SolidColorBrush)new BrushConverter().ConvertFromString("#aa5555")),
            new Palette("Blossom", (SolidColorBrush)new BrushConverter().ConvertFromString("#ff5555")),
            new Palette("Sea", (SolidColorBrush)new BrushConverter().ConvertFromString("#00aa55")),
            new Palette("Ill", (SolidColorBrush)new BrushConverter().ConvertFromString("#55aa55")),
            new Palette("Haze", (SolidColorBrush)new BrushConverter().ConvertFromString("#aaaa55")),
            new Palette("Peach", (SolidColorBrush)new BrushConverter().ConvertFromString("#ffaa55")),
            new Palette("Spring", (SolidColorBrush)new BrushConverter().ConvertFromString("#00ff55")),
            new Palette("Mantis", (SolidColorBrush)new BrushConverter().ConvertFromString("#55ff55")),
            new Palette("Brilliant", (SolidColorBrush)new BrushConverter().ConvertFromString("#aaff55")),
            new Palette("Canary", (SolidColorBrush)new BrushConverter().ConvertFromString("#ffff55")),
            new Palette("Navy", (SolidColorBrush)new BrushConverter().ConvertFromString("#0000aa")),
            new Palette("Grape", (SolidColorBrush)new BrushConverter().ConvertFromString("#5500aa")),
            new Palette("Mauve", (SolidColorBrush)new BrushConverter().ConvertFromString("#aa00aa")),
            new Palette("Purple", (SolidColorBrush)new BrushConverter().ConvertFromString("#ff00aa")),
            new Palette("Cornflower", (SolidColorBrush)new BrushConverter().ConvertFromString("#0055aa")),
            new Palette("Deep", (SolidColorBrush)new BrushConverter().ConvertFromString("#5555aa")),
            new Palette("Lilac", (SolidColorBrush)new BrushConverter().ConvertFromString("#aa55aa")),
            new Palette("Lavender", (SolidColorBrush)new BrushConverter().ConvertFromString("#ff55aa")),
            new Palette("Aqua", (SolidColorBrush)new BrushConverter().ConvertFromString("#00aaaa")),
            new Palette("Steel", (SolidColorBrush)new BrushConverter().ConvertFromString("#55aaaa")),
            new Palette("Grey", (SolidColorBrush)new BrushConverter().ConvertFromString("#aaaaaa")),
            new Palette("Pink", (SolidColorBrush)new BrushConverter().ConvertFromString("#ffaaaa")),
            new Palette("Bay", (SolidColorBrush)new BrushConverter().ConvertFromString("#00ffaa")),
            new Palette("Marina", (SolidColorBrush)new BrushConverter().ConvertFromString("#55ffaa")),
            new Palette("Tornado", (SolidColorBrush)new BrushConverter().ConvertFromString("#aaffaa")),
            new Palette("Saltine", (SolidColorBrush)new BrushConverter().ConvertFromString("#ffffaa")),
            new Palette("Blue", (SolidColorBrush)new BrushConverter().ConvertFromString("#0000ff")),
            new Palette("Twilight", (SolidColorBrush)new BrushConverter().ConvertFromString("#5500ff")),
            new Palette("Orchid", (SolidColorBrush)new BrushConverter().ConvertFromString("#aa00ff")),
            new Palette("Magenta", (SolidColorBrush)new BrushConverter().ConvertFromString("#ff00ff")),
            new Palette("Azure", (SolidColorBrush)new BrushConverter().ConvertFromString("#0055ff")),
            new Palette("Liberty", (SolidColorBrush)new BrushConverter().ConvertFromString("#5555ff")),
            new Palette("Royalty", (SolidColorBrush)new BrushConverter().ConvertFromString("#aa55ff")),
            new Palette("Thistle", (SolidColorBrush)new BrushConverter().ConvertFromString("#ff55ff")),
            new Palette("Ocean", (SolidColorBrush)new BrushConverter().ConvertFromString("#00aaff")),
            new Palette("Sky", (SolidColorBrush)new BrushConverter().ConvertFromString("#55aaff")),
            new Palette("Periwinkle", (SolidColorBrush)new BrushConverter().ConvertFromString("#aaaaff")),
            new Palette("Carnation", (SolidColorBrush)new BrushConverter().ConvertFromString("#ffaaff")),
            new Palette("Cyan", (SolidColorBrush)new BrushConverter().ConvertFromString("#00ffff")),
            new Palette("Turquoise", (SolidColorBrush)new BrushConverter().ConvertFromString("#55ffff")),
            new Palette("Powder", (SolidColorBrush)new BrushConverter().ConvertFromString("#aaffff")),
            new Palette("White", (SolidColorBrush)new BrushConverter().ConvertFromString("#ffffff"))
        };

        public static Settings set = new Settings();

        public bool muted = false;
        public bool full_auto_mode = true;
        public bool buildup_enabled = true;
        public int muted_volume;

        public int current_song = 0;
        public int current_image = 55;
        private int anim_ind = 0;

        private string loop_rhythm;
        private string build_rhythm;
        public int rhythm_pos = 0;
        public int b_rhythm_pos = 0;

        public static ObservableCollection<rdata> enabled_songs = new ObservableCollection<rdata>();
        public static ObservableCollection<rdata> enabled_images = new ObservableCollection<rdata>();

        public MainWindow()
        {
            InitializeComponent();
            RPManager.SupremeReader("Packs/Defaults_v5.0.zip");
            for (int i = 0; i < RPManager.allSongs.Length; i++) enabled_songs.Add(new rdata() { Name = RPManager.allSongs[i].title, Ind = i });
            songs_listbox.ItemsSource = enabled_songs;

            for (int i = 0; i < RPManager.allPics.Length; i++) enabled_images.Add(new rdata() { Name = RPManager.allPics[i].name, Ind = i });
            images_listbox.ItemsSource = enabled_images;
            ImageChange(current_image);

            Timer.Tick += new EventHandler(Timer_Tick);
            AnimTimer.Tick += new EventHandler(AnimTimer_Tick);
            
            Player.SetReference(this);
            set.SetReference(this);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            set.Owner = this;
            timeline_color_change();
            Settings.rp_names.Add(new setdata() { Name = RPManager.ResPacks[0].name, State = true });
            set.stat_update();

            songs_listbox.SelectedIndex = current_song;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (e.ChangedButton == MouseButton.Left) timeline_noblur();
            if (e.ChangedButton == MouseButton.Right)
            {
                if (set.IsVisible) set.Hide();
                else set.Show();
            }
        }

        //
        //Key Controls
        //

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (muted) toggle_mute();
            if (e.Delta < 0 & Player.Volume != 0)
            {
                Player.Volume -= 10;
                volume_label.Content = Player.Volume;
                Player.SetVolumeToStream(Player.Channel, Player.Volume);
            }
            if (e.Delta > 0 & Player.Volume != 100)
            {
                if (muted) toggle_mute();
                Player.Volume += 10;
                volume_label.Content = Player.Volume;
                Player.SetVolumeToStream(Player.Channel, Player.Volume);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.M:
                    toggle_mute();
                    break;
                case Key.Up:
                    next_song();
                    break;
                case Key.Down:
                    prev_song();
                    break;
                case Key.Right:
                    next_image();
                    break;
                case Key.Left:
                    prev_image();
                    break;
                case Key.Space:
                    timeline_noblur();
                    break;
            }
        }

        private void next_song()
        {
            if (songs_listbox.SelectedIndex == enabled_songs.Count - 1) songs_listbox.SelectedIndex = 0;
            else songs_listbox.SelectedIndex += 1;
        }
        private void prev_song()
        {
            if (songs_listbox.SelectedIndex == 0) songs_listbox.SelectedIndex = songs_listbox.Items.Count - 1;
            else songs_listbox.SelectedIndex -= 1;
        }

        private void toggle_mute()
        {
            if (!muted)
            {
                muted_volume = Player.Volume;
                volume_label.Content = 0;
                Player.Volume = 0;
                Player.SetVolumeToStream(Player.Channel, Player.Volume);
                muted = true;
            }
            else
            {
                Player.Volume = muted_volume;
                volume_label.Content = Player.Volume;
                Player.SetVolumeToStream(Player.Channel, Player.Volume);
                muted = false;
            }
        }

        private void next_image()
        {
            if (images_listbox.SelectedIndex == images_listbox.Items.Count - 1) images_listbox.SelectedIndex = 0;
            else images_listbox.SelectedIndex += 1;
            full_auto_mode = false;
        }
        private void prev_image()
        {
            if (images_listbox.SelectedIndex == 0) images_listbox.SelectedIndex = images_listbox.Items.Count - 1;
            else images_listbox.SelectedIndex -= 1;
            full_auto_mode = false;
        }



        //
        // Timeline Effects Controls
        //

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeLine_Move();

            if (rhythm_pos >= 0)
            {
                Correction = beat_length * rhythm_pos - Player.GetPosOfStream(Player.Stream_L);
                if (Correction > 0 & Correction < beat_length * 1.5) Timer.Interval = TimeSpan.FromSeconds(Correction);
                else //Jumping forward to compensate lag
                {
                    if (rhythm_pos != 0 & Correction < 0) Timer.Interval = TimeSpan.FromMilliseconds(0.0001);
                }
            }
            else
            {
                b_rhythm_pos += 1;
                Correction = buildup_beat_len * (build_rhythm.Length + rhythm_pos) - Player.GetPosOfStream(Player.Stream_B);
                if (Correction > 0) Timer.Interval = TimeSpan.FromSeconds(Correction);
                else Timer.Interval = TimeSpan.FromMilliseconds(0.0001);
            }
        }

        private void TimelineLenghtFill()
        {
            if (timeline_label.Content.ToString().Length < 150) timeline_label.Content = timeline_label.Content.ToString() + loop_rhythm;
        }

        private void TimeLine_Move()
        {
            beat(timeline_label.Content.ToString()[2]);
            timeline_label.Content = timeline_label.Content.ToString().Remove(2, 1);
            TimelineLenghtFill();
            rhythm_pos += 1;
            if (rhythm_pos == loop_rhythm.Length) rhythm_pos = 0;
        }

        private void beat(char c)
        {
            switch (c)
            {
                case 'o':
                    timeline_o();
                    break;
                case 'x':
                    timeline_x();
                    break;
                case '-':
                    timeline_noblur();
                    break;
                case '‑'://thank to tylup for that one
                    timeline_noblur();
                    break;
                case ':':
                    timeline_color_change();
                    break;
                case '*':
                    timeline_image_change();
                    break;
            }
        }

        // Vertical blur (snare)
        private void timeline_x()
        {
            timeline_noblur();
        }
        // Horizontal blur (bass)
        private void timeline_o()
        {
            timeline_noblur();
        }

        // For '-' in the timeline
        private void timeline_noblur()
        {
            timeline_color_change();
            timeline_image_change();
        }
        // '+'
        private void timeline_blackout()
        {

        }
        // '¤'
        private void timeline_whiteout()
        {

        }
        // '|'
        private void timeline_blackout_short()
        {

        }
        // ':'
        public void timeline_color_change()
        {
            int index;
            while (true)
            {
                index = rnd.Next(0, hues.Length - 1);
                if (Background != hues[index].brush) break;
            }
            Background = hues[index].brush;
            color_label.Content = hues[index].name.ToUpper();
        }
        // '*'
        private void timeline_image_change()
        {
            if (full_auto_mode)
            {
                int i;
                if (enabled_images.Count != 0)
                {
                    while (true)
                    {
                        i = rnd.Next(0, enabled_images.Count - 1);
                        if (images_listbox.SelectedIndex != i) break;
                    }
                }
                else i = -1;
                ImageChange(i);
            }
        }
        // 'X' Vertical blur only
        private void timeline_blur_vert()
        {

        }

        // 'O' Vertical blur only
        private void timeline_blur_hor()
        {

        }

        // ')' Trippy cirle in
        private void timeline_circle_in()
        {

        }

        // '(' Trippy cirle out
        private void timeline_circle_out()
        {

        }

        // '~' Fade color
        private void timeline_fade()
        {

        }

        // '=' Fade and change image
        private void timeline_fade_image()
        {

        }

        // 'i' Invert all colours
        private void timeline_invert()
        {

        }

        // 'I' Invert & change image
        private void timeline_invert_w_image()
        {

        }

        // 's' Horizontal slice
        private void timeline_slice_hor()
        {

        }
        // 'S' Horizontal slice & change image
        private void timeline_slice_hor_w_im()
        {

        }
        // 'v' Vertical slice
        private void timeline_slice_ver()
        {

        }
        // 'V' Vertical slice & change image
        private void timeline_slice_ver_w_im()
        {

        }
        // '#' Double slice
        private void timeline_slice_double()
        {

        }
        // '@' Double slice and change image
        private void timeline_scice_double_w_im()
        {

        }


        //
        // Settings Window Position Controls
        //
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (WindowState != WindowState.Maximized)
            {
                set.Top = this.Top + (this.Height / 2) - (set.Height / 2);
                set.Left = this.Left + (this.Width / 2) - (set.Width / 2);
            }
            if (Width / Height > 2) image.Stretch = Stretch.Uniform;
            else image.Stretch = Stretch.UniformToFill;
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                set.Top = this.Top + (this.Height / 2) - (set.Height / 2);
                set.Left = this.Left + (this.Width / 2) - (set.Width / 2);
            }
            if (this.WindowState == WindowState.Maximized)
            {
                set.Top = (SystemParameters.MaximizedPrimaryScreenHeight / 2) - (set.Height / 2);
                set.Left = (SystemParameters.MaximizedPrimaryScreenWidth / 2) - (set.Width / 2);
            }
        }
        private void Window_LocationChanged(object sender, EventArgs e)
        {
            set.Top = this.Top + (this.Height / 2) - (set.Height / 2);
            set.Left = this.Left + (this.Width / 2) - (set.Width / 2);
        }


        private void next_song_be_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            next_song();
        }
        private void songs_be_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (songs_listbox.Visibility == Visibility.Hidden)
            {
                songs_listbox.Visibility = Visibility.Visible;
                images_listbox.Visibility = Visibility.Hidden;
            }
            else songs_listbox.Visibility = Visibility.Hidden;
        }

        private void prev_song_be_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            prev_song();
        }

        private void songs_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (songs_listbox.SelectedIndex != -1)
            {
                int i = enabled_songs[songs_listbox.SelectedIndex].Ind;
                Player.loop_mem = RPManager.allSongs[i].buffer;
                loop_rhythm = RPManager.allSongs[i].rhythm;
                song_label.Content = RPManager.allSongs[i].title.ToUpper();
                timeline_label.Content = RPManager.allSongs[i].rhythm;
                current_song = songs_listbox.SelectedIndex;

                rhythm_pos = 0;
                b_rhythm_pos = 0;
                if (RPManager.allSongs[i].buildup_buffer != null & buildup_enabled)
                {
                    Player.build_mem = RPManager.allSongs[i].buildup_buffer;
                    Player.Play_With_Buildup();
                    build_rhythm = RPManager.allSongs[i].buildup_rhythm;
                    int expected_size = Convert.ToInt32(Math.Round(Audio.GetTimeOfStream(Player.Stream_B) / (Audio.GetTimeOfStream(Player.Stream_L) / loop_rhythm.Length)));
                    if (build_rhythm == null)
                    {
                        build_rhythm = new string('.', expected_size);
                    }
                    else if (build_rhythm.Length < expected_size)
                    {
                        build_rhythm += new string('.', expected_size - build_rhythm.Length - 1);
                    }
                    timeline_label.Content = build_rhythm + timeline_label.Content;
                    rhythm_pos -= expected_size;
                }
                else Player.Play_Without_Buildup();

                timeline_label.Content = ">>" + timeline_label.Content;

                beat_length = Audio.GetTimeOfStream(Player.Stream_L) / loop_rhythm.Length;
                
                TimelineLenghtFill();
                Timer.Interval = TimeSpan.FromTicks(Convert.ToInt64(beat_length * 1000 * 10000));
                if (RPManager.allSongs[i].buildup_buffer != null & buildup_enabled)
                {
                    Timer.Interval = TimeSpan.FromSeconds(buildup_beat_len);
                    buildup_beat_len = Audio.GetTimeOfStream(Player.Stream_B) / build_rhythm.Length;
                }

                Player.Play();
                TimeLine_Move();
                Timer.Start();
            }
            else
            {
                if (enabled_songs.Count == 0)
                {
                    StopSong();
                }
            }
        }
        public void StopSong()
        {
            Timer.Stop();
            Player.Stop();
            song_label.Content = "NONE";
            beat_length = 0;
            timeline_label.Content = ">>.";
        }

        private void prev_image_be_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            prev_image();
        }

        private void next_image_be_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            next_image();
        }

        private void full_auto_be_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            full_auto_mode = true;
        }


        private void Images_listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            full_auto_mode = false;
            ImageChange(enabled_images[images_listbox.SelectedIndex].Ind);
        }

        private void ImageChange(int index)
        {
            current_image = index;
            if (index != -1)
            {
                image.Source = RPManager.allPics[index].pic;
                if (RPManager.allPics[index].animation == null)
                {
                    AnimTimer.Stop();
                    character_label.Content = RPManager.allPics[index].fullname.ToUpper();
                    switch (RPManager.allPics[index].align)
                    {
                        case "left":
                            image.HorizontalAlignment = HorizontalAlignment.Left;
                            break;
                        case "center":
                            image.HorizontalAlignment = HorizontalAlignment.Center;
                            break;
                        case "right":
                            image.HorizontalAlignment = HorizontalAlignment.Right;
                            break;
                    }
                }
                else
                {
                    anim_ind = 1;
                    AnimTimer.Interval = TimeSpan.FromMilliseconds(RPManager.allPics[current_image].frameDuration);
                    AnimTimer.Start();
                    switch (RPManager.allPics[index].align)
                    {
                        case "left":
                            image.HorizontalAlignment = HorizontalAlignment.Left;
                            break;
                        case "center":
                            image.HorizontalAlignment = HorizontalAlignment.Center;
                            break;
                        case "right":
                            image.HorizontalAlignment = HorizontalAlignment.Right;
                            break;
                    }
                }
            }
            else
            {
                if (enabled_images.Count == 0)
                {
                    image.Source = null;
                    character_label.Content = "NONE";
                }
            }
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            image.Source = RPManager.allPics[current_image].animation[anim_ind];
            if (RPManager.allPics[current_image].animation.Length == anim_ind + 1) anim_ind = 0;
            else anim_ind++;
        }

        private void Images_be_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (images_listbox.Visibility == Visibility.Hidden)
            {
                images_listbox.Visibility = Visibility.Visible;
                songs_listbox.Visibility = Visibility.Hidden;
            }
            else images_listbox.Visibility = Visibility.Hidden;
        }

        private void Images_listbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private void Songs_listbox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

    }
}
