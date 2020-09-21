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

namespace CompMathPart3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Line axisX = new Line();
            axisX.Stroke = Brushes.DarkGray;
            axisX.X1 = 0;
            axisX.Y1 = GraphicPlace.Height * 0.5;
            axisX.X2 = GraphicPlace.Width;
            axisX.Y2 = GraphicPlace.Height * 0.5;
            GraphicPlace.Children.Add(axisX);

            double x = -6.28;
            double y = Math.Sin(x);
            while (x <= 6.28)
            {
                Line sine = new Line();
                sine.X1 = x * 40 + GraphicPlace.Width * 0.5;
                sine.Y1 = -1 * y * 40 + GraphicPlace.Height * 0.5;

                x += 0.01;
                y = Math.Sin(x);
                sine.X2 = x * 40 + GraphicPlace.Width * 0.5;
                sine.Y2 = -1 * y * 40 + GraphicPlace.Height * 0.5;
                sine.Stroke = Brushes.DarkRed;
                GraphicPlace.Children.Add(sine);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void LimitsTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            double reserveLimitValue;
            TextBox checkedTextBox = (TextBox)sender;

            try
            {
                reserveLimitValue = Double.Parse(checkedTextBox.Text.Replace(".", ","));
            }
            catch
            {
                checkedTextBox.Text = "0";
                MessageBox.Show("Значение для предела выставленно некорректно.\nОно будет сброшено.");
            }
        }

        private void Accuracy_LostFocus(object sender, RoutedEventArgs e)
        {
            double reserveAccuracyValue;
            TextBox checkedTextBox = (TextBox)sender;

            try
            {
                reserveAccuracyValue = Double.Parse(checkedTextBox.Text.Replace(".", ","));
            }
            catch
            {
                checkedTextBox.Text = "0.1";
                MessageBox.Show("Значение для погрешности выставленно некорректно.\nОно будет сброшено.");
            }
        }
    }
}
