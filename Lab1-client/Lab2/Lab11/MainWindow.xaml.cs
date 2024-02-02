using Lab1.Services;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DataService _service = new DataService();

        public MainWindow()
        {
            InitializeComponent();
            LoadData();
        }

        public async void LoadData()
        {
            var data = await _service.GetDataFromServerAsync("https://localhost:7112/api/data/read");
            MyData.ItemsSource = data;
        }

        private void OpenFormWindow(object sender, RoutedEventArgs e)
        {
            FormWindow formWindow = new FormWindow();
            formWindow.DataAdded += OnDataAdded; // Подписываемся на событие
            formWindow.Show();
        }

        private void OnDataAdded(object sender, EventArgs e)
        {
            LoadData(); // Обновляем данные после добавления новых данных
        }

    }
}
