using Core;

using Microsoft.WindowsAPICodePack.Dialogs;

using System;
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

namespace Demo
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private Video video;

        public MainWindow()
        {
            InitializeComponent();
            video = new Video();
            video.UpdateImage += new Action<BitmapImage, BitmapImage>(UpdateImage);
            video.UpdateFrames += new Action<int>(UpdateFrames);
            PauseBtn.Visibility = Visibility.Collapsed;
            PlayBtn.IsEnabled = false;
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.Multiselect = false;
                dialog.IsFolderPicker = false;
                dialog.Filters.Add(new CommonFileDialogFilter($"AVI", ".AVI"));

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    video.Load(dialog.FileName);
                    VideoSlider.Value = 0;
                    VideoSlider.Maximum = video.MaxFrames();
                    PlayBtn.IsEnabled = true;
                }
            }
        }

        private void UpdateImage(BitmapImage image, BitmapImage processingImage)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                SourceImage.Source = image;
                ProcessingImage.Source = processingImage;
            }));
        }

        private void UpdateFrames(int posFrames)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                VideoSlider.Value = posFrames;
            }));
        }

        private void TestOpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            using (CommonOpenFileDialog dialog = new CommonOpenFileDialog())
            {
                dialog.Multiselect = true;
                dialog.IsFolderPicker = false;
                dialog.Filters.Add(new CommonFileDialogFilter($"AVI", ".AVI"));

                if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    BackgroundModel model = new BackgroundModel(640, 480);
                    model.CreateBackground(dialog.FileNames.ToList());
                }
            }
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!video.IsPlay())
            {
                Task.Run(() =>
                {
                    video.PlayVideo();
                });
            }

            video.Play();
            PauseBtn.Visibility = Visibility.Visible;
        }

        private void PauseBtn_Click(object sender, RoutedEventArgs e)
        {
            video.Pause();
            PauseBtn.Visibility = Visibility.Collapsed;
        }

        private void PreBtn_Click(object sender, RoutedEventArgs e)
        {
            video.Previous();
        }

        private void NextBtn_Click(object sender, RoutedEventArgs e)
        {
            video.Next();
        }

        private void VideoSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            video.SetFrames((int)VideoSlider.Value);
        }
    }
}
