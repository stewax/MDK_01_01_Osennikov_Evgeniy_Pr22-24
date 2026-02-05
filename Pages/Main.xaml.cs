using System;
using System.Collections.Generic;
using ClassConnection;
using ClassModule;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PhoneBook_Осенников.Elements;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace PhoneBook_Осенников.Pages
{
    /// <summary>
    /// Логика взаимодействия для Main.xaml
    /// </summary>
    public partial class Main : Page
    {
        private List<Call> allCalls = new List<Call>();

        private void LoadAllCalls()
        {
            MainWindow.connect.LoadData(ClassConnection.Connection.tables.calls);
            allCalls = new List<Call>(MainWindow.connect.calls);
        }

        private void ShowCalls(List<Call> callsToShow)
        {
            parent.Children.Clear();
            foreach (var call in callsToShow)
            {
                parent.Children.Add(new Elements.Call_itm(call));
            }
            parent.Children.Add(new Elements.Add_itm(new Pages.PagesUser.Call_win(new Call())));
        }
        //
        public enum page_main
        {
            users, calls, none
        };
        public static page_main page_select;

        public Main()
        {
            InitializeComponent();
            page_select = page_main.none;
        }
        private void Click_Phone(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible)
            {
                MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            }
            if (page_select != page_main.users)
            {
                page_select = page_main.users;


                DoubleAnimation opacityAnimation = new DoubleAnimation();
                opacityAnimation.From = 1;
                opacityAnimation.To = 0;
                opacityAnimation.Duration = TimeSpan.FromSeconds(0.2);
                opacityAnimation.Completed += delegate
                {
                    parent.Children.Clear();
                    DoubleAnimation opacityAnimation2 = new DoubleAnimation();
                    opacityAnimation2.From = 0;
                    opacityAnimation2.To = 1;
                    opacityAnimation2.Duration = TimeSpan.FromSeconds(0.2);
                    opacityAnimation2.Completed += delegate
                    {
                        Dispatcher.InvokeAsync(async () =>
                        {
                            MainWindow.connect.LoadData(ClassConnection.Connection.tables.users);

                            foreach (User user_item in MainWindow.connect.users)
                            {
                                if (page_select == page_main.users)
                                {
                                    parent.Children.Add(new Elements.User_itm(user_item));
                                    await Task.Delay(90);
                                }
                            }

                            if (page_select == page_main.users)
                            {
                                var ff = new Pages.PagesUser.User_win(new User());
                                parent.Children.Add(new Elements.Add_itm(ff));
                            }
                        });
                    };
                    parent.BeginAnimation(StackPanel.OpacityProperty, opacityAnimation2);
                };
                parent.BeginAnimation(StackPanel.OpacityProperty, opacityAnimation);
            }
        }

        private void Click_History(object sender, RoutedEventArgs e)
        {
            if (frame_main.Visibility == Visibility.Visible)
            {
                MainWindow.main.Anim_move(MainWindow.main.frame_main, MainWindow.main.scroll_main);
            }
            if (page_select != page_main.calls)
            {
                page_select = page_main.calls;
                DoubleAnimation opacityAnimation = new DoubleAnimation();
                opacityAnimation.From = 1;
                opacityAnimation.To = 0;
                opacityAnimation.Duration = TimeSpan.FromSeconds(0.2);
                opacityAnimation.Completed += delegate
                {
                    parent.Children.Clear();
                    DoubleAnimation opacityAnimation2 = new DoubleAnimation();
                    opacityAnimation2.From = 0;
                    opacityAnimation2.To = 1;
                    opacityAnimation2.Duration = TimeSpan.FromSeconds(0.2);
                    opacityAnimation2.Completed += delegate
                    {
                        Dispatcher.InvokeAsync(async () =>
                        {
                            MainWindow.connect.LoadData(ClassConnection.Connection.tables.calls);
                            foreach (Call call_itm in MainWindow.connect.calls)
                            {
                                if (page_select == page_main.calls)
                                {
                                    parent.Children.Add(new Elements.Call_itm(call_itm));
                                    await Task.Delay(90);
                                }
                            }
                            if (page_select == page_main.calls)
                            {
                                var ff = new Pages.PagesUser.Call_win(new ClassModule.Call());
                                parent.Children.Add(new Elements.Add_itm(ff));
                            }
                        });
                    };
                    parent.BeginAnimation(StackPanel.OpacityProperty, opacityAnimation2);
                };
                parent.BeginAnimation(StackPanel.OpacityProperty, opacityAnimation);

                LoadAllCalls();
                ShowCalls(allCalls);
            }
        }
        public void Anim_move(Control control1, Control control2, Frame frame_main = null, Page pages = null, page_main page_restart = page_main.none)
        {
            if (page_restart != page_main.none)
            {
                if (page_restart == page_main.users)
                {
                    page_select = page_main.none;
                    Click_Phone(new object(), new RoutedEventArgs());
                }
                else if (page_restart == page_main.calls)
                {
                    page_select = page_main.none;
                    Click_History(new object(), new RoutedEventArgs());
                }
            }
            else
            {
                DoubleAnimation opacityAnimation = new DoubleAnimation();
                opacityAnimation.From = 1;
                opacityAnimation.To = 0;
                opacityAnimation.Duration = TimeSpan.FromSeconds(0.3);
                opacityAnimation.Completed += delegate
                {
                    if (pages != null)
                    {
                        frame_main.Navigate(pages);
                        //if (control1 == frame_main && control2 == frame_main)
                        // if (MainWindow.actualUser.role != "admin")
                        // {
                        //     parent.Children.Clear();
                        // }
                    }

                    control1.Visibility = Visibility.Hidden;
                    control2.Visibility = Visibility.Visible;

                    DoubleAnimation opacityAnimation2 = new DoubleAnimation();
                    opacityAnimation2.From = 0;
                    opacityAnimation2.To = 1;
                    opacityAnimation2.Duration = TimeSpan.FromSeconds(0.4);

                    control2.BeginAnimation(ScrollViewer.OpacityProperty, opacityAnimation2);
                };

                control1.BeginAnimation(ScrollViewer.OpacityProperty, opacityAnimation);
            }
        }

        private void Click_filter(object sender, RoutedEventArgs e)
        {
            // Проверяем, что даты выбраны
            if (date_start_call.SelectedDate == null || date_end_call.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату начала и дату окончания", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Получаем выбранные даты
            DateTime startDate = date_start_call.SelectedDate.Value.Date; // Начало дня
            DateTime endDate = date_end_call.SelectedDate.Value.Date; // Начало дня

            // Добавляем 1 день к конечной дате, чтобы включить весь последний день
            endDate = endDate.AddDays(1).AddTicks(-1);

            // Проверяем корректность диапазона
            if (startDate > endDate)
            {
                MessageBox.Show("Дата начала не может быть больше даты окончания", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Очищаем текущий список
            parent.Children.Clear();

            // Загружаем все звонки
            MainWindow.connect.LoadData(Connection.tables.calls);

            foreach (Call call in MainWindow.connect.calls)
            {
                try
                {
                    if (!string.IsNullOrEmpty(call.time_start))
                    {
                        string[] dateTimeParts = call.time_start.Split(' ');

                        if (dateTimeParts.Length >= 1)
                        {
                            string dateStr = dateTimeParts[0];

                            DateTime callDate;
                            bool dateParsed = false;

                            if (DateTime.TryParseExact(dateStr, "dd.MM.yyyy",
                                System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None, out callDate))
                            {
                                dateParsed = true;
                            }

                            if (dateParsed)
                            {
                                if (callDate.Date >= startDate.Date && callDate.Date <= endDate.Date)
                                {
                                    parent.Children.Add(new Elements.Call_itm(call));
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при обработке звонка ID {call.id}: {ex.Message}");
                }
            }
        }
    }
}
