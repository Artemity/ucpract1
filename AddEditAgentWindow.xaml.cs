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
using System.Windows.Shapes;
using ucpract1.DB;

namespace ucpract1
{
    /// <summary>
    /// Логика взаимодействия для AddEditAgentWindow.xaml
    /// </summary>
    public partial class AddEditAgentWindow : Window
    {
        private Zad1Context _db = new Zad1Context();
        private Agent _agent;

        public AddEditAgentWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Определяем режим работы на основе Data.agent
            if (Data.agent == null)
            {
                this.Title = "Добавление агента";
                btnSave.Content = "Добавить";
                _agent = new Agent();
            }
            else
            {
                this.Title = "Редактирование агента";
                btnSave.Content = "Сохранить";
                // Загружаем свежие данные из БД
                _agent = _db.Agents.Find(Data.agent.Id);

                if (_agent == null)
                {
                    MessageBox.Show("Агент не найден в базе данных", "Ошибка",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                    return;
                }
            }

            this.DataContext = _agent;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder errors = new StringBuilder();

                // Валидация через свойства объекта
                if (string.IsNullOrWhiteSpace(_agent.FirstName))
                    errors.AppendLine("Введите фамилию");

                if (string.IsNullOrWhiteSpace(_agent.MiddleName))
                    errors.AppendLine("Введите имя");

                if (string.IsNullOrWhiteSpace(_agent.LastName))
                    errors.AppendLine("Введите отчество");

                // Проверка комиссии
                if (!string.IsNullOrWhiteSpace(txtDealShare.Text))
                {
                    if (!decimal.TryParse(txtDealShare.Text, out decimal dealShare))
                    {
                        errors.AppendLine("Комиссия должна быть числом");
                    }
                }

                if (errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка ввода",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Добавление или обновление
                if (Data.agent == null) // Режим добавления
                {
                    _db.Agents.Add(_agent);
                }
                else // Режим редактирования
                {
                    _db.Entry(_agent).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }

                _db.SaveChanges();

                // Очищаем данные после успешного сохранения
                Data.agent = null;

                // Можно добавить информационное сообщение об успехе
                MessageBox.Show("Данные успешно сохранены!", "Успех",
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