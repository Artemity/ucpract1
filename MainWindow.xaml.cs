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
using ucpract1.DB;

namespace ucpract1
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDBInDataGrid();
        }

        void LoadDBInDataGrid()
        {
            using (Zad1Context _db = new Zad1Context())
            {
                DataGrid1.ItemsSource = _db.Agents.ToList();
                DataGrid2.ItemsSource = _db.Clients.ToList();
            }
        }

        private void tbFind_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbFind.Text.ToLower();

            using (Zad1Context _db = new Zad1Context())
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    DataGrid1.ItemsSource = _db.Agents.ToList();
                }
                else
                {
                    var filteredAgents = _db.Agents
                        .Where(a => a.FirstName.ToLower().Contains(searchText))
                        .ToList();

                    DataGrid1.ItemsSource = filteredAgents;
                }
            }
        }

        private void tbFind1_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbFind1.Text.ToLower();

            using (Zad1Context _db = new Zad1Context())
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    DataGrid2.ItemsSource = _db.Clients.ToList();
                }
                else
                {
                    var filteredClients = _db.Clients
                        .Where(c =>
                            (c.FirstName != null && c.FirstName.ToLower().Contains(searchText)) ||
                            (c.Phone != null && c.Phone.ToLower().Contains(searchText)) ||
                            (c.Email != null && c.Email.ToLower().Contains(searchText)))
                        .ToList();
                    DataGrid2.ItemsSource = filteredClients;
                }
                DataGrid2.Items.Refresh();
            }
        }

        private void btnAdd1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Data.client = null;
                AddEditClientWindow addWindow = new AddEditClientWindow();
                addWindow.Owner = this;
                addWindow.ShowDialog();
                LoadDBInDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы добавления: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEdit1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGrid2.SelectedItem == null)
                {
                    MessageBox.Show("Выберите запись для редактирования!", "Предупреждение",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Data.client = (Client)DataGrid2.SelectedItem;

                AddEditClientWindow editWindow = new AddEditClientWindow();
                editWindow.Owner = this;
                editWindow.ShowDialog();
                LoadDBInDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы редактирования: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete1_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (DataGrid2.SelectedItem == null)
                    {
                        MessageBox.Show("Выберите запись для удаления!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    Client row = (Client)DataGrid2.SelectedItem;

                    if (row != null)
                    {
                        using (Zad1Context _db = new Zad1Context())
                        {
                            _db.Clients.Attach(row);
                            _db.Clients.Remove(row);
                            _db.SaveChanges();
                        }
                        LoadDBInDataGrid();

                        MessageBox.Show("Запись успешно удалена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                DataGrid1.Focus();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Data.agent = null;
                AddEditAgentWindow addWindow = new AddEditAgentWindow();
                addWindow.Owner = this;
                addWindow.ShowDialog();
                LoadDBInDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы добавления: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (DataGrid1.SelectedItem == null)
                {
                    MessageBox.Show("Выберите запись для редактирования!", "Предупреждение",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Data.agent = (Agent)DataGrid1.SelectedItem;

                AddEditAgentWindow editWindow = new AddEditAgentWindow();
                editWindow.Owner = this;
                editWindow.ShowDialog();
                LoadDBInDataGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии формы редактирования: {ex.Message}",
                    "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            result = MessageBox.Show("Удалить запись?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    if (DataGrid1.SelectedItem == null)
                    {
                        MessageBox.Show("Выберите запись для удаления!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    Agent row = (Agent)DataGrid1.SelectedItem;

                    if (row != null)
                    {
                        using (Zad1Context _db = new Zad1Context())
                        {
                            _db.Agents.Attach(row);
                            _db.Agents.Remove(row);
                            _db.SaveChanges();
                        }
                        LoadDBInDataGrid();

                        MessageBox.Show("Запись успешно удалена!", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                DataGrid1.Focus();
            }
        }

        private void btnInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Задание 1 по учебной практике\nстудента группы ИСП-41 Лотакова Артемия", "Информация");
        }
    }
}