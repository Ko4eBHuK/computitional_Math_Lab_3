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

            Update_Graphics();
        }

        private void Solve_Equation(object sender, RoutedEventArgs e)
        {
            int methodIndex = 0;
            int equationIndex = EquationList.SelectedIndex;
            double upperLimit = Double.Parse(UpperLimit.Text.Replace(".", ","));
            double lowerLimit = Double.Parse(LowerLimit.Text.Replace(".", ","));
            double accuracy = Math.Abs(Double.Parse(Accuracy.Text.Replace(".", ",")));

            if (upperLimit < lowerLimit)
            {
                upperLimit = lowerLimit;
                lowerLimit = Double.Parse(UpperLimit.Text.Replace(".", ","));
            }

            if (upperLimit > 8)
            {
                upperLimit = 8;
                UpperLimit.Text = upperLimit.ToString();
            }
            if (upperLimit < -8)
            {
                upperLimit = -8;
                UpperLimit.Text = upperLimit.ToString();
            }
            if (lowerLimit > 8)
            {
                lowerLimit = 8;
                LowerLimit.Text = lowerLimit.ToString();
            }
            if (lowerLimit < -8)
            {
                lowerLimit = -8;
                LowerLimit.Text = lowerLimit.ToString();
            }

            var solution = new NonlinearEquationMethods(methodIndex, equationIndex, upperLimit, lowerLimit, accuracy);
            if (solution.SolutionExistence)
            {
                OutputConsole.Text += "Корень уравнения на данном отрезке существует.\n" +
                    "Значение корня: " + solution.EquationResult + "\n" +
                    "Достигнутая погрешность: " + solution.ResultAccuracy + "\n" +
                    "Количество итераций: " + solution.CountOfIterations + "\n" +
                    "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";
            }
            else
            {
                OutputConsole.Text += "На данном отрезке нет гарнатии существования корня, задайте другой.\n" +
                    "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";
            }

            //OutputConsole.Text = solution.MethodIndex + " " + solution.EquationIndex + " " + solution.Accuracy + " " + solution.RightSectionValue + " " + solution.LteftSectionValue + "\n";

            Update_Graphics();
            double x = lowerLimit;
            double y = 0;
            switch (equationIndex)
            {
                case 0:
                    y = Math.Pow(x, 5) + 4;
                    break;
                case 1:
                    y = 3 * Math.Cos(3 * x);
                    break;
                case 2:
                    y = 1.5 * Math.Sin(1.5 * x) - 3 * Math.Cos(3 * x);
                    break;
            }
            while (x <= upperLimit)
            {
                Line graph = new Line();
                graph.X1 = x * 40 + GraphicPlace.Width * 0.5;
                graph.Y1 = -1 * y * 40 + GraphicPlace.Height * 0.5;

                x += 0.01;
                switch (equationIndex)
                {
                    case 0:
                        y = Math.Pow(x, 5) + 4;
                        break;
                    case 1:
                        y = 3 * Math.Cos(3 * x);
                        break;
                    case 2:
                        y = 1.5 * Math.Sin(1.5 * x) - 3 * Math.Cos(3 * x);
                        break;
                }
                graph.X2 = x * 40 + GraphicPlace.Width * 0.5;
                graph.Y2 = -1 * y * 40 + GraphicPlace.Height * 0.5;
                graph.Stroke = Brushes.DarkRed;
                GraphicPlace.Children.Add(graph);
            }
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

        private void Update_Graphics()
        {
            GraphicPlace.Children.Clear();
            //ось абсцисс
            Line axisX = new Line();
            axisX.Stroke = Brushes.DarkGray;
            axisX.X1 = 0;
            axisX.Y1 = GraphicPlace.Height * 0.5;
            axisX.X2 = GraphicPlace.Width;
            axisX.Y2 = GraphicPlace.Height * 0.5;
            GraphicPlace.Children.Add(axisX);
            Line arrowXU = new Line();
            arrowXU.Stroke = Brushes.DarkGray;
            arrowXU.X1 = GraphicPlace.Width - 10;
            arrowXU.Y1 = GraphicPlace.Height * 0.5 - 5;
            arrowXU.X2 = GraphicPlace.Width - 4;
            arrowXU.Y2 = GraphicPlace.Height * 0.5;
            GraphicPlace.Children.Add(arrowXU);
            Line arrowXL = new Line();
            arrowXL.Stroke = Brushes.DarkGray;
            arrowXL.X1 = GraphicPlace.Width - 4;
            arrowXL.Y1 = GraphicPlace.Height * 0.5;
            arrowXL.X2 = GraphicPlace.Width - 10;
            arrowXL.Y2 = GraphicPlace.Height * 0.5 + 5;
            GraphicPlace.Children.Add(arrowXL);

            //ось ординат
            Line axisY = new Line();
            axisY.Stroke = Brushes.DarkGray;
            axisY.X1 = GraphicPlace.Width / 2;
            axisY.Y1 = GraphicPlace.Height;
            axisY.X2 = GraphicPlace.Width / 2;
            axisY.Y2 = 0;
            GraphicPlace.Children.Add(axisY);
            Line arrowYL = new Line();
            arrowYL.Stroke = Brushes.DarkGray;
            arrowYL.X1 = GraphicPlace.Width / 2 - 5;
            arrowYL.Y1 = 6;
            arrowYL.X2 = GraphicPlace.Width / 2;
            arrowYL.Y2 = 0;
            GraphicPlace.Children.Add(arrowYL);
            Line arrowYR = new Line();
            arrowYR.Stroke = Brushes.DarkGray;
            arrowYR.X1 = GraphicPlace.Width / 2;
            arrowYR.Y1 = 0;
            arrowYR.X2 = GraphicPlace.Width / 2 + 5;
            arrowYR.Y2 = 6;
            GraphicPlace.Children.Add(arrowYR);

            //штрихи единичных отрезков
            for (int i = 1; i <= 7; i++)
            {
                Line hatchXR = new Line();
                hatchXR.Stroke = Brushes.DarkGray;
                hatchXR.X1 = i * 40 + GraphicPlace.Width / 2;
                hatchXR.Y1 = GraphicPlace.Height / 2 - 3;
                hatchXR.X2 = i * 40 + GraphicPlace.Width / 2;
                hatchXR.Y2 = GraphicPlace.Height / 2 + 3;
                GraphicPlace.Children.Add(hatchXR);

                Line hatchXL = new Line();
                hatchXL.Stroke = Brushes.DarkGray;
                hatchXL.X1 = -1 * i * 40 + GraphicPlace.Width / 2;
                hatchXL.Y1 = GraphicPlace.Height / 2 - 3;
                hatchXL.X2 = -1 * i * 40 + GraphicPlace.Width / 2;
                hatchXL.Y2 = GraphicPlace.Height / 2 + 3;
                GraphicPlace.Children.Add(hatchXL);

                Line hatchYU = new Line();
                hatchYU.Stroke = Brushes.DarkGray;
                hatchYU.X1 = GraphicPlace.Width / 2 - 3;
                hatchYU.Y1 = -1 * i * 40 + GraphicPlace.Height / 2;
                hatchYU.X2 = GraphicPlace.Width / 2 + 3;
                hatchYU.Y2 = -1 * i * 40 + GraphicPlace.Height / 2; ;
                GraphicPlace.Children.Add(hatchYU);

                Line hatchYL = new Line();
                hatchYL.Stroke = Brushes.DarkGray;
                hatchYL.X1 = GraphicPlace.Width / 2 - 3;
                hatchYL.Y1 = i * 40 + GraphicPlace.Height / 2;
                hatchYL.X2 = GraphicPlace.Width / 2 + 3;
                hatchYL.Y2 = i * 40 + GraphicPlace.Height / 2; ;
                GraphicPlace.Children.Add(hatchYL);
            }
        }

        private void Backhitch(object sender, RoutedEventArgs e)
        {
            Update_Graphics();

            MethodList.SelectedIndex = 0;
            EquationList.SelectedIndex = 0;
            UpperLimit.Text = "1";
            LowerLimit.Text = "0";
            Accuracy.Text = "0.1";
            OutputConsole.Text = "";
        }

    }
}
