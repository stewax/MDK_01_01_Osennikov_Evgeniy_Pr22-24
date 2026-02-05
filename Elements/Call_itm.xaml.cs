using ClassModule;
using PhoneBook_Осенников.Pages;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhoneBook_Осенников.Elements
{
    /// <summary>
    /// Логика взаимодействия для Call_itm.xaml
    /// </summary>
    public partial class Call_itm : UserControl
    {
        Call call_loc;

        public Call_itm(Call _call)
        {
            InitializeComponent();
            call_loc = _call;
            if (_call.time_end != null)
            {
                User user_loc = MainWindow.connect.users.Find(x => x.id == _call.user_id);
                category_call_text.Content = user_loc.fio_user.ToString();

                string[] deted = _call.time_start.ToString().Split(' ');
                string[] deted1 = _call.time_end.ToString().Split(' ');

                string[] dete = (deted[0]).Split('.');
                string[] dete1 = (deted1[0]).Split('.');

                System.DateTime detedate = new DateTime(int.Parse(dete[2]),
                                                         int.Parse(dete[1]),
                                                         int.Parse(dete[0]),
                                                         int.Parse(deted[1].Split(':')[0]),
                                                         int.Parse(deted[1].Split(':')[1]), 0);

                System.DateTime detedate1 = new DateTime(int.Parse(dete1[2]),
                                                          int.Parse(dete1[1]),
                                                          int.Parse(dete1[0]),
                                                          int.Parse(deted1[1].Split(':')[0]),
                                                          int.Parse(deted1[1].Split(':')[1]), 0);

                System.TimeSpan detetime = detedate1.Subtract(detedate);

                time_call_text.Content = "Продолжительность: " + detetime.ToString();
                number_call_text.Content = "Номер телефона: " + user_loc.phone_num.ToString();
            }

            img_category_call.Source =
                (_call.category_call == 1) ?
                new BitmapImage(new Uri("/img/out.png", UriKind.RelativeOrAbsolute)) :
                new BitmapImage(new Uri("/img/in.png", UriKind.RelativeOrAbsolute));

            DoubleAnimation opacityAnimation = new DoubleAnimation();
            opacityAnimation.From = 0;
            opacityAnimation.To = 1;
            opacityAnimation.Duration = TimeSpan.FromSeconds(0.4);
            border.BeginAnimation(UIElement.OpacityProperty, opacityAnimation);
        }

        private void Click_redact(object sender, RoutedEventArgs e)
        {
            MainWindow.main.Anim_move(MainWindow.main.scroll_main, MainWindow.main.frame_main, MainWindow.main.frame_main, new Pages.PagesUser.Call_win(call_loc));
        }

        private void Click_remove(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.connect.LoadData(ClassConnection.Connection.tables.calls);

                string vs = "DELETE FROM [calls] WHERE [Код] = " + call_loc.id.ToString() + "";
                var pc = MainWindow.connect.QueryAccess(vs);
                if (pc != null)
                {
                    MessageBox.Show("Успешное удаление звонка", "Успешное", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow.connect.LoadData(ClassConnection.Connection.tables.calls);
                    MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main, null, null, Main.page_main.calls);
                }
                else MessageBox.Show("Запрос на удалении звонка не был обработан", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
