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
using System.Windows.Threading;
using LibMas;

namespace практос_13
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool[] B;
        int[,] A;
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            
        }
        /// <summary>
        /// Таймер включается при загрузке окна
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Windows_Louded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 1, 0);
            timer.IsEnabled = true;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            tblTime.Text = now.ToString("HH:mm");
            tblData.Text = now.ToString("dd:MM:yyyy");
            timer.Start();
        }
        /// <summary>
        /// закрывает программу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /// <summary>
        /// Очистка всех полей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClean_Click(object sender, RoutedEventArgs e)
        {
            if (dataGridA.ItemsSource != null || datagridB.ItemsSource != null)
            {
                dataGridA.ItemsSource = null;
                A = null;
                datagridB.ItemsSource = null;
                B = null;
                tbrange.Clear();
            }
            else MessageBox.Show("Таблица пуста", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        /// <summary>
        /// Информация о задании
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInf_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("По массиву A(5,6)получить массив В(6), присвоив его j-элементу значение true, если все " +
                "элементы j - столбца массива А нулевые, и значение false иначе.", "Задание", MessageBoxButton.OK, MessageBoxImage.Question);
        }
        /// <summary>
        /// Информация о разработчике
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAutor_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Андрианов Алексей, Вариант 14", "Автор", MessageBoxButton.OK, MessageBoxImage.Question);
        }
        /// <summary>
        /// метод для заполнения матрицы
        /// </summary>
        /// <param name="A">матрица</param>
        /// <param name="range">диапозон</param>
        /// <returns></returns>
        private int[,] Fill(int[,] A, int range)
        {
            Random rand = new Random();
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    A[i, j] = rand.Next(range + 1);
                }
            }
            return A;
        }
        /// <summary>
        /// метод ждя заполнения матрицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFill_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(tbrange.Text, out int Range) && Range > 0)
            {
                A = new int[5, 6];

                Fill(A, Range);
                dataGridA.ItemsSource = VisualArray.ToDataTableA(A).DefaultView;
            }
            else MessageBox.Show("Введите правильное значение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        /// <summary>
        /// метод для проверки j-элементов, устанавливающий true если все j-элементы нулевые и false если нет
        /// </summary>
        /// <param name="B">булевый массив</param>
        /// <returns></returns>
        private bool[] Cals(bool[] B)
        {
            for (int j = 0; j < A.GetLength(1); j++)
            {
                bool elementB = true;
                for (int i = 0; i < A.GetLength(0); i++)
                {
                    if (A[i, j] != 0)
                    {
                        elementB = false;
                        break;
                    }
                }
                B[j] = elementB;
            }
            return B;
        }
        /// <summary>
        /// метод для проверки j-элементов
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCalc_Click(object sender, RoutedEventArgs e)
        {
            if (A != null && cellEditEnding == cellEditEndingDone)
            {
                B = new bool[A.GetLength(1)];
                Cals(B);

                datagridB.ItemsSource = VisualArray.ToDataTableB(B).DefaultView;
            }
            else MessageBox.Show("Введите правильное значение", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        /// <summary>
        /// флаги для проверки коректной работы ручного изменения 
        /// </summary>
        bool cellEditEnding = false;
        bool cellEditEndingDone = false;
        /// <summary>
        /// метод для ручного изменения значений матрицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            cellEditEnding = true;
            int indexColumn = e.Column.DisplayIndex;
            if (Int32.TryParse(((TextBox)e.EditingElement).Text, out int result))
            {
                cellEditEndingDone = true;
                int indexRow = e.Row.GetIndex();

                A[indexRow, indexColumn] = result;
            }

        }
        /// <summary>
        /// метод для открытия файла с сохраненой матрицей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            A = ArrayEditor.Open();
            dataGridA.ItemsSource = VisualArray.ToDataTableA(A).DefaultView;
        }
        /// <summary>
        /// метод для сохранения матрицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ArrayEditor.Save(A);
        }
    }
}
