using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows;

namespace MemoApp
{
    public partial class MainWindow : Window
    {
        // 現在開いているファイル
        private string? FilePath;

        public MainWindow()
        {
            InitializeComponent();
        }

        // 測定結果をスクロール可能なウィンドウで表示
        private void ShowResultsWindow(string results, string title)
        {
            Window resultWindow = new Window();

            resultWindow.Title = title;
            resultWindow.Width = 450;
            resultWindow.Height = 600;
            resultWindow.WindowStartupLocation =
                WindowStartupLocation.CenterScreen;

            TextBox resultTextBox = new TextBox();

            resultTextBox.Text = results;
            resultTextBox.IsReadOnly = true;
            resultTextBox.AcceptsReturn = true;
            resultTextBox.VerticalScrollBarVisibility =
                ScrollBarVisibility.Auto;
            resultTextBox.HorizontalScrollBarVisibility =
                ScrollBarVisibility.Auto;
            resultTextBox.Margin = new Thickness(10);

            resultWindow.Content = resultTextBox;

            resultWindow.ShowDialog();
        }

        // ファイルを開く
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();

            dialog.Filter = "CSVファイル (*.csv)|*.csv";
            dialog.Title = "CSVファイルを開く";

            if (dialog.ShowDialog() == true)
            {
                FilePath = dialog.FileName;

                // 100+5回分の測定結果を保存する
                StringBuilder results = new StringBuilder();

                // 100+5回読み込みを繰り返す
                for (int i = 1; i <= 105; i++)
                {

                    // ここから読み込み時間の計測開始
                    Stopwatch sw = Stopwatch.StartNew();

                    string[] lines = File.ReadAllLines(FilePath);

                    if (lines.Length > 1)
                    {
                        MemoTextBox.Text = lines[1];
                    }

                    MemoTextBox.Text = string.Join(
    Environment.NewLine,
    lines);



                    // 読み込み処理終了
                    sw.Stop();
                    //

                    // 結果を保存
                    results.AppendLine(
                        $"{sw.Elapsed.TotalMilliseconds:F3}");

                }


                // 100+5回分をまとめて表示
                ShowResultsWindow(
                    results.ToString(),
                    "CSV 読み込み時間（100+5回）単位ms "
                );

            }
        }

        // 上書き保存
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                MessageBox.Show("先に「名前を付けて保存」をしてください。");
                return;
            }

            SaveCsv();

            MessageBox.Show("CSV形式で保存しました。");
        }

        // 名前を付けて保存
        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();

            dialog.Filter = "CSVファイル (*.csv)|*.csv";
            dialog.Title = "CSVファイルとして保存";

            if (dialog.ShowDialog() == true)
            {
                FilePath = dialog.FileName;

                SaveCsv();

                MessageBox.Show("CSV形式で保存しました。");
            }
        }

        // CSVとして保存
        private void SaveCsv()
        {
            string[] lines =
            {
                MemoTextBox.Text
            };

            // 100+5回分の測定結果を保存する
            StringBuilder results = new StringBuilder();

            // 100+5回保存を繰り返す
            for (int i = 1; i <= 105; i++)
            {

                // 保存処理の計測開始
                Stopwatch sw = Stopwatch.StartNew();
                //

                File.WriteAllLines(FilePath!, lines);


                // 保存処理終了
                sw.Stop();
                //

                // 結果を保存
                results.AppendLine(
                    $"{sw.Elapsed.TotalMilliseconds:F3}");

            }

            // 100+5回分をまとめて表示
            ShowResultsWindow(
                results.ToString(),
                "CSV 保存時間（100+5回）単位ms"
            );

        }
    }
}