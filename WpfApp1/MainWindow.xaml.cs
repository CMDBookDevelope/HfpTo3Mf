using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void GeneratePrusa(object sender, RoutedEventArgs e)
        {
            SetUiBusy(true);
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.FileName = "Document";
                dialog.DefaultExt = ".hfp";
                dialog.Filter = "Hueforge project (.hfp)|*.hfp";
                dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

                bool? result = dialog.ShowDialog();
                if (result == true)
                {
                    string filename = dialog.FileName;
                    StatusLabel.Content = "Status : Generating .3mf for " + filename;
                    await Task.Run(() =>
                    {
                        Tools.CreatePrusaPackage(filename);
                    });
                    StatusLabel.Content = "Status : Completed .3mf generation for " + filename;
                }
                else
                    StatusLabel.Content = "Status : Idle";
            }
            catch (Exception ex)
            {
                StatusLabel.Content = $"Status : ERROR : {ex.Message}";
                MessageBox.Show($"转换异常：{ex.Message}\n{ex.StackTrace}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                SetUiBusy(false);
            }
        }

        private async void GenerateBBL(object sender, RoutedEventArgs e)
        {
            SetUiBusy(true);
            try
            {
                var dialog = new Microsoft.Win32.OpenFileDialog();
                dialog.FileName = "Document";
                dialog.DefaultExt = ".hfp";
                dialog.Filter = "Hueforge project (.hfp)|*.hfp";
                dialog.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;

                bool? result = dialog.ShowDialog();
                if (result == true)
                {
                    string filename = dialog.FileName;
                    StatusLabel.Content = "Status : Generating .3mf for " + filename;
                    await Task.Run(() =>
                    {
                        Tools.CreatePackage(filename);
                    });
                    StatusLabel.Content = "Status : Completed .3mf generation for " + filename;
                }
                else
                    StatusLabel.Content = "Status : Idle";
            }
            catch (Exception ex)
            {
                StatusLabel.Content = $"Status : ERROR : {ex.Message}";
                MessageBox.Show($"转换异常：{ex.Message}\n{ex.StackTrace}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                SetUiBusy(false);
            }
        }

        private async void PrusaDrop_Drop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            SetUiBusy(true);
            int projectsCount = 0;
            try
            {
                foreach (string filename in fileList)
                {
                    if (filename.EndsWith(".hfp"))
                    {
                        StatusLabel.Content = "Status : Generating .3mf for " + filename;
                        await Task.Run(() =>
                        {
                            Tools.CreatePrusaPackage(filename);
                        });
                        projectsCount++;
                        StatusLabel.Content = "Status : Completed .3mf generation for " + filename;
                    }
                    else
                    {
                        StatusLabel.Content = "Status : Skipped wrong file " + filename;
                    }
                }
                StatusLabel.Content = $"Status : Completed .3mf generation for {projectsCount} projects.";
            }
            catch (Exception ex)
            {
                StatusLabel.Content = $"Status : ERROR : {ex.Message}";
                MessageBox.Show($"转换异常：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                SetUiBusy(false);
            }
        }

        private async void BBLDrop_Drop(object sender, DragEventArgs e)
        {
            string[] fileList = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            SetUiBusy(true);
            int projectsCount = 0;
            try
            {
                foreach (string filename in fileList)
                {
                    if (filename.EndsWith(".hfp"))
                    {
                        StatusLabel.Content = "Status : Generating .3mf for " + filename;
                        await Task.Run(() =>
                        {
                            Tools.CreatePackage(filename);
                        });
                        projectsCount++;
                        StatusLabel.Content = "Status : Completed .3mf generation for " + filename;
                    }
                    else
                    {
                        StatusLabel.Content = "Status : Skipped wrong file " + filename;
                    }
                }
                StatusLabel.Content = $"Status : Completed .3mf generation for {projectsCount} projects.";
            }
            catch (Exception ex)
            {
                StatusLabel.Content = $"Status : ERROR : {ex.Message}";
                MessageBox.Show($"转换异常：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                SetUiBusy(false);
            }
        }

        /// <summary>统一设置界面忙碌状态，消除重复代码</summary>
        void SetUiBusy(bool isBusy)
        {
            StartBambu.IsEnabled = !isBusy;
            StartPrusa.IsEnabled = !isBusy;
            StartBambuDrop.IsEnabled = !isBusy;
            StartBambuDrop.AllowDrop = !isBusy;
            StartPrusaDrop.AllowDrop = !isBusy;
        }
    }
}
