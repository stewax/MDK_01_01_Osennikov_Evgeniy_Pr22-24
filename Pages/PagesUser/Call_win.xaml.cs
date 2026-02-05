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
    /// Логика взаимодействия для Call_win.xaml
    /// </summary>
    public partial class Call_win : Page
    {
        Call call_itm;
        public Call_win(Call _call)
        {
            InitializeComponent();
            call_itm = _call;
            if (_call.time_start != null)
            {
                string[] dateTimeStart = _call.time_start.Split(' ');
                string[] dateStart = dateTimeStart[0].Split('.');
                date_start_call.SelectedDate = new DateTime(int.Parse(dateStart[2]), int.Parse(dateStart[1]), int.Parse(dateStart[0]));
                time_start.Text = dateTimeStart[1];

                string[] dateTimeFinish = _call.time_end.Split(' ');
                string[] dateFinish = dateTimeFinish[0].Split('.');
                date_end_call.SelectedDate = new DateTime(int.Parse(dateFinish[2]), int.Parse(dateFinish[1]), int.Parse(dateFinish[0]));
                time_finish.Text = dateTimeFinish[1];
            }
            else
            {
                time_start.Text = "00:00";
                time_finish.Text = "00:00";
            }

            ComboBoxItem combItm = new ComboBoxItem();
            combItm.Tag = 1;
            combItm.Content = "Исходящий";
            if (_call.category_call == 1) combItm.IsSelected = true;
            call_category_text.Items.Add(combItm);

            ComboBoxItem combItm1 = new ComboBoxItem();
            combItm1.Tag = 2;
            combItm1.Content = "Входящий";
            if (_call.category_call == 2) combItm1.IsSelected = true;
            call_category_text.Items.Add(combItm1);

            MainWindow.connect.LoadData(ClassConnection.Connection.tables.users);
            foreach (User itm in MainWindow.connect.users)
            {
                ComboBoxItem combUser = new ComboBoxItem();
                combUser.Tag = itm.id;
                combUser.Content = itm.fio_user;
                if (_call.user_id == itm.id) combUser.IsSelected = true;

                user_select.Items.Add(combUser);
            }
        }

        private void Click_Call_Redact(object sender, RoutedEventArgs e)
        {
            if (!CheckTime(time_start.Text))
            {
                MessageBox.Show("Укажите время начала");
                return;
            }

            if (!CheckTime(time_finish.Text))
            {
                MessageBox.Show("Укажите время конца");
                return;
            }

            if (date_start_call.SelectedDate != null && date_end_call.SelectedDate != null)
            {
                System.DateTime dateStart = (System.DateTime)date_start_call.SelectedDate;
                System.DateTime dateFinish = (System.DateTime)date_end_call.SelectedDate;
                System.TimeSpan dateDiff = dateFinish.Subtract(dateStart);
                if (!dateDiff.ToString().Contains("-"))
                {
                    User id_temp_user;
                    if (user_select.SelectedItem != null)
                        id_temp_user = MainWindow.connect.users.Find(x => x.id == Convert.ToInt32(((ComboBoxItem)user_select.SelectedItem).Tag));
                    else
                    {
                        MessageBox.Show("Не выбран пользователь", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    int id_calls_categ;
                    if (call_category_text.SelectedItem != null)
                        id_calls_categ = Convert.ToInt32(((ComboBoxItem)call_category_text.SelectedItem).Tag);
                    else
                    {
                        MessageBox.Show("Не выбрана категория", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (call_itm.time_end == null)
                    {
                        int id = MainWindow.connect.SetLastId(ClassConnection.Connection.tables.calls);

                        //                    string query = $"INSERT INTO [calls]([Код], [user_id], [category_call], [date], [time_start], [time_end]) VALUES ({id.ToString()}, " +
                        //$"{id_temp_user.Id.ToString()}, {id_calls_categ.ToString()}, '{date_start_call.SelectedDate.Value.ToString().Split(' ')[0]}" +
                        //$"', '{time_start.Text}', '{time_finish.Text}')";

                        string query = $"INSERT INTO [calls] ([Код], [user_id], [category_call], [date], [time_start], [time_end]) VALUES ({id.ToString()}, " +
$"{id_temp_user.id.ToString()}, {id_calls_categ.ToString()}, '{dateStart.ToString().Split(' ')[0]}', " +
$"'{date_start_call.SelectedDate.Value.ToString().Split(' ')[0]} {time_start.Text}', " +
$"'{date_end_call.SelectedDate.Value.ToString().Split(' ')[0]} {time_finish.Text}')";

                        var pc = MainWindow.connect.QueryAccess(query);
                        if (pc != null)
                        {
                            MainWindow.connect.SetLastId(ClassConnection.Connection.tables.calls);
                            //MainWindow.messageBox.Show_msg("Успех", "Успешное добавление данных", "pack://application:,,,/img/icon_success.png");
                            MessageBox.Show("Успешное добавление", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.calls);
                        }
                        else MessageBox.Show("Дополнительные данные не были добавлены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        //string query = "UPDATE [calls] SET [user_id] = " + id_temp_user.Id.ToString() + ", " +
                        //    "[category_call] = " + id_calls_categ.ToString() + ", " +
                        //    "[date_call] = '" + date_start_call.SelectedDate.Value.ToString().Split(' ')[0] + "', " +
                        //    "[time_start] = '" + time_start.Text + "', " +
                        //    "[time_end] = '" + time_finish.Text + "' WHERE [id] = " + call_itm.Id.ToString() + "";

                        string query = $"UPDATE [calls] SET [user_id] = '{id_temp_user.id.ToString()}', " +
 $"[category_call]='{id_calls_categ.ToString()}', " +
 $"[date]='{date_start_call.SelectedDate.Value.ToString().Split(' ')[0]}', " +
 $"[time_start]='{date_start_call.SelectedDate.Value.ToString().Split(' ')[0]} {time_start.Text}', " +
 $"[time_end]='{date_end_call.SelectedDate.Value.ToString().Split(' ')[0]} {time_finish.Text}' WHERE [Код] = {call_itm.id}";
                        var pc = MainWindow.connect.QueryAccess(query);
                        if (pc != null)
                        {
                            MainWindow.connect.SetLastId(ClassConnection.Connection.tables.calls);
                            //MainWindow.messageBox.Show_msg("Успех", "Успешное изменение данных", "pack://application:,,,/img/icon_success.png");
                            MessageBox.Show("Успешное изменение", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                            MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.calls);
                        }
                        else MessageBox.Show("Изменения не были внесены", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                else MessageBox.Show("Дата старта больше чем дата конца");
            }
            else MessageBox.Show("Вы не указали дату");
        }

        private void Click_Cancel_Call_Redact(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
        }

        private void Click_Remove_Call_Redact(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.connect.SetLastId(ClassConnection.Connection.tables.calls);
                string vs = "DELETE FROM [calls] WHERE [Код] = " + call_itm.id.ToString() + "";
                var pc = MainWindow.connect.QueryAccess(vs);
                if (pc != null)
                {
                    MessageBox.Show("Успeшное удаление звонка", "Успeшное", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.connect.SetLastId(ClassConnection.Connection.tables.calls);
                    MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.calls);
                }
                else MessageBox.Show("Запрос на удаление звонка не был обработан", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
        public bool CheckTime(string str)
        {
            string[] str1 = str.Split(':');
            if (str1.Length == 2)
            {
                if (str1[0].Trim() != "" && str1[1].Trim() != "")
                {
                    if (int.Parse(str1[0]) >= 0 && int.Parse(str1[0]) <= 23)
                    {
                        if (int.Parse(str1[1]) >= 0 && int.Parse(str1[1]) <= 59)
                        {
                            return true;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
            else return false;
        }
    }
}