using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Lock all controls before processing
        /// </summary>
        private void LockUi()
        {
            StartBambu.IsEnabled = false;
            StartPrusa.IsEnabled = false;
            StartBambuDrop.IsEnabled = false;
            StartPrusaDrop.IsEnabled = false;
            StartBambuDrop.AllowDrop = false;
            StartPrusaDrop.AllowDrop = false;
        }

        /// <summary>
        /// Unlock all controls after finished / error
        /// </summary>
        private void UnlockUi()
        {
            StartBambu.IsEnabled = true;
            StartPrusa.IsEnabled = true;
            StartBambuDrop.IsEnabled = true;
            StartPrusaDrop.IsEnabled = true;
            StartBambuDrop.AllowDrop = true;
            StartPrusaDrop.AllowDrop = true;
        }

        private async void GeneratePrusa(object sender, RoutedEventArgs e)
        {
            LockUi();
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    FileName = "Document",
                    DefaultExt = ".hfp",
                    Filter = "Hueforge project (.hfp)|*.hfp",
                    DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory
                };

                bool? result = dialog.ShowDialog();
                if (result == true)
                {
                    string filename = dialog.FileName;
                    StatusLabel.Content = $"Status : Generating .3mf for {filename}";
                    await Task.Run(() =>
                    {
                        Tools.CreatePrusaPackage(filename);
                    });
                    StatusLabel.Content = $"Status : Completed .3mf generation for {filename}";
                }
                else
                {
                    StatusLabel.Content = "Status : Idle";
                }
            }
            catch (Exception ex)
            {
                StatusLabel.Content = $"Status ERROR(Prusa): {ex.Message}";
                MessageBox.Show($"Prusa 3MF generation error:\n{ex.ToString()}", "Conversion Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                UnlockUi();
            }
        }

        private async void GenerateBBL(object sender, RoutedEventArgs e)
        {
            LockUi();
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog
                {
                    FileName = "Document",
                    DefaultExt = ".hfp",
                    Filter = "Hueforge project (.hfp)|*.hfp",
                    DefaultDirectory = AppDomain.CurrentDomain.BaseDirectory
                };

                bool? result = dialog.ShowDialog();
                if (result == true)
                {
                    string filename = dialog.FileName;
                    StatusLabel.Content = $"Status : Generating .3mf for {filename}";
                    await Task.Run(() =>
                    {
                        Tools.CreatePackage(filename);
                    });
                    StatusLabel.Content = $"Status : Completed .3mf generation for {filename}";
                }
                else
                {
                    StatusLabel.Content = "Status : Idle";
                }
            }
            catch (Exception ex)
            {
                StatusLabel.Content = $"Status ERROR(Bambu): {ex.Message}";
                MessageBox.Show($"Bambu 3MF generation error:\n{ex.ToString()}", "Conversion Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                UnlockUi();
            }
        }

        private async void PrusaDrop_Drop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int projectsCount = 0;
            LockUi();
            try
            {
                foreach (string filename in fileList)
                {
                    if (filename.EndsWith(".hfp"))
                    {
                        StatusLabel.Content = $"Status : Generating .3mf for {filename}";
                        await Task.Run(() =>
                        {
                            Tools.CreatePrusaPackage(filename);
                        });
                        projectsCount++;
                        StatusLabel.Content = $"Status : Completed .3mf generation for {filename}";
                    }
                    else
                    {
                        StatusLabel.Content = $"Status : Skipped wrong file {filename}";
                    }
                }
                StatusLabel.Content = $"Status : Completed .3mf generation for {projectsCount} projects.";
            }
            catch (Exception ex)
            {
                StatusLabel.Content = $"Status ERROR(Prusa Drop): {ex.Message}";
                MessageBox.Show($"Prusa drag‑drop conversion error:\n{ex.ToString()}", "Conversion Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                UnlockUi();
            }
        }

        private async void BBLDrop_Drop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            int projectsCount = 0;
            LockUi();
            try
            {
                foreach (string filename in fileList)
                {
                    if (filename.EndsWith(".hfp"))
                    {
                        StatusLabel.Content = $"Status : Generating .3mf for {filename}";
                        await Task.Run(() =>
                        {
                            Tools.CreatePackage(filename);
                        });
                        projectsCount++;
                        StatusLabel.Content = $"Status : Completed .3mf generation for {filename}";
                    }
                    else
                    {
                        StatusLabel.Content = $"Status : Skipped wrong file {filename}";
                    }
                }
                StatusLabel.Content = $"Status : Completed .3mf generation for {projectsCount} projects.";
            }
            catch (Exception ex)
            {
                StatusLabel.Content = $"Status ERROR(Bambu Drop): {ex.Message}";
                MessageBox.Show($"Bambu drag‑drop conversion error:\n{ex.ToString()}", "Conversion Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                UnlockUi();
            }
        }
    }
}
