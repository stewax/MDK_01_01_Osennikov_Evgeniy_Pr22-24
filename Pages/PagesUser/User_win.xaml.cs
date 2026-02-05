using ClassModule;
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

namespace PhoneBook_Осенников.Pages.PagesUser
{
    /// <summary>
    /// Логика взаимодействия для User_win.xaml
    /// </summary>
    public partial class User_win : Page
    {
        User user_loc;
        public User_win(User _user)
        {
            InitializeComponent();
            user_loc = _user;
            if (_user.fio_user != null)
            {
                fio_user.Text = _user.fio_user;
                phone_user.Text = _user.phone_num;
                addrec_user.Text = _user.passport_data;
            }
        }

        private void Click_User_Redact(object sender, RoutedEventArgs e)
        {
            if (!MainWindow.connect.ItsOnlyFIO(fio_user.Text))
            {
                MessageBox.Show("Вы не правильно напиcали ФИО");
                return;
            }
            if (!MainWindow.connect.ItsNumber(phone_user.Text))
            {
                MessageBox.Show("Вы не правильно написали номер телефона");
                return;
            }
            if (addrec_user.Text.Trim() == "")
            {
                MessageBox.Show("Вы не правильно написали номер пасспорта");
                return;
            }
            if (user_loc.fio_user == null)
            {
                int id = MainWindow.connect.SetLastId(ClassConnection.Connection.tables.users);
                string query = $"insert into [users]([Код], [phone_num], [FIO_user], [passport_data]) " +
                    $"values ({id.ToString()}, '{phone_user.Text}', '{fio_user.Text}', '{addrec_user.Text}')";

                var pc = MainWindow.connect.QueryAccess(query);
                if (pc != null)
                {
                    MainWindow.connect.LoadData(ClassConnection.Connection.tables.users);
                    MessageBox.Show("Успешное добавление клиента", "Успешное", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.users);
                }
                else
                {
                    MessageBox.Show("Запрос на добавление клмента не был обработан", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                string query = $"UPDATE [users] SET " +
                               $"[phone_num] = '{phone_user.Text}', " +
                               $"[FIO_user] = '{fio_user.Text}', " +
                               $"[passport_data] = '{addrec_user.Text}' " +
                               $"WHERE [Код] = {user_loc.id}";

                var pc = MainWindow.connect.QueryAccess(query);

                if (pc != null)
                {
                    MainWindow.connect.LoadData(ClassConnection.Connection.tables.users);
                    MessageBox.Show("Успешное изменение клиента", "Успешное", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.users);
                }
                else
                {
                    MessageBox.Show("Запрос на изменение клмента не был обработан", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void Click_Cancel_User_Redact(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
        }

        private void Click_Remove_User_Redact(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.connect.LoadData(ClassConnection.Connection.tables.users);
                Call userFind = MainWindow.connect.calls.Find(x => x.user_id == user_loc.id);
                if (userFind != null)
                {
                    var click = MessageBox.Show("У данного клиента есть звонки, все равно удалить его?", "Вопрос", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (click == MessageBoxResult.No)
                    {
                        return;
                    }
                }
                string vs1 = $"DELETE FROM [calls] WHERE [User_id] = '{user_loc.id.ToString()}'";
                var pc1 = MainWindow.connect.QueryAccess(vs1);

                string vs = "DELETE FROM [users] WHERE [Код] = " + user_loc.id.ToString() + "";
                var pc = MainWindow.connect.QueryAccess(vs);
                if (pc != null && pc1 != null)
                {
                    MessageBox.Show("Успешное удаление клиента", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.connect.LoadData(ClassConnection.Connection.tables.users);
                    MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.users);
                }
                else
                {
                    MessageBox.Show("Запрос на удаление клиента не был обработан", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
