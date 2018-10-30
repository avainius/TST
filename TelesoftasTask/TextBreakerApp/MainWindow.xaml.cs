using Microsoft.Win32;
using System.IO;
using System.Windows;
using TextBreaker.Interfaces;
using TextBreaker.Services;

namespace TextBreakerApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ITextHandler textHandlerService;
        public MainWindow()
        {
            InitializeComponent();
            textHandlerService = new TextHandlerService();
        }

        private void btnBrowseInput_Click(object sender, RoutedEventArgs e)
        {
            txtInputFilePath.Text = GetFileName();
        }

        private void btnBrowseOutput_Click(object sender, RoutedEventArgs e)
        {
            txtOutputFilePath.Text = GetFileName();
        }

        private string GetFileName()
        {
            var browser = new OpenFileDialog();
            if (browser.ShowDialog() == true)
            {
                return browser.FileName;
            }

            return null;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnBreak_Click(object sender, RoutedEventArgs e)
        {
            int maxLength;
            if (string.IsNullOrEmpty(txtInputFilePath.Text) || string.IsNullOrEmpty(txtLength.Text) || !int.TryParse(txtLength.Text, out maxLength))
            {
                lbInputResult.Items.Clear();
                lbInputResult.Items.Add("SELECT A FILE TO READ FROM AND ENTER MAX LINE LENGTH");
                return;
            }

            GetOriginalText();

            var result = textHandlerService.BreakText(txtInputFilePath.Text, maxLength, txtOutputFilePath.Text);
            lbOutputResult.Items.Clear();
            result.ForEach(x => lbOutputResult.Items.Add($"[L{x.Length}] {x}"));
        }

        private void GetOriginalText()
        {
            lbInputResult.Items.Clear();
            using(var stream = new StreamReader(txtInputFilePath.Text))
            {
                while (!stream.EndOfStream)
                {
                    var line = stream.ReadLine();
                    lbInputResult.Items.Add($"[L{line.Length}] {line}");
                }
            }
        }
    }
}
