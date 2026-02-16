using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ucpract1.DB;

namespace ucpract1
{
    /// <summary>
    /// Логика взаимодействия для AddEditClientWindow.xaml
    /// </summary>
    public partial class AddEditClientWindow : Window
    {
        private Zad1Context _db = new Zad1Context();
        private Client _client;

        public AddEditClientWindow()
        {
            InitializeComponent();

            if (Data.client == null)
            {
                this.Title = "Добавление клиента";
                btnSave.Content = "Добавить";
                _client = new Client();
            }
            else
            {
                this.Title = "Редактирование клиента";
                btnSave.Content = "Сохранить";

                _client = _db.Clients.Find(Data.client.Id);
            }

            this.DataContext = _client;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder errors = new StringBuilder();

                if (string.IsNullOrWhiteSpace(_client.FirstName))
                    errors.AppendLine("Введите фамилию");
                if (string.IsNullOrWhiteSpace(_client.MiddleName))
                    errors.AppendLine("Введите имя");
                if (string.IsNullOrWhiteSpace(_client.LastName))
                    errors.AppendLine("Введите отчество");
                if (string.IsNullOrWhiteSpace(_client.Phone))
                    errors.AppendLine("Введите телефон");

                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка ввода",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (Data.client == null)
                {
                    _db.Clients.Add(_client);
                }
                else
                {
                    _db.Entry(_client).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }

                _db.SaveChanges();

                Data.client = null;

                MessageBox.Show("Данные успешно сохранены!", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
