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

            Update_Graphics_Bisection();
            Update_Graphics_Newton();
        }

        private void Solve_Equation_Bisection(object sender, RoutedEventArgs e)
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


            Update_Graphics_Bisection();
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
                graph.X1 = x * 40 + GraphicPlaceBisection.Width * 0.5;
                graph.Y1 = -1 * y * 40 + GraphicPlaceBisection.Height * 0.5;

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
                graph.X2 = x * 40 + GraphicPlaceBisection.Width * 0.5;
                graph.Y2 = -1 * y * 40 + GraphicPlaceBisection.Height * 0.5;
                graph.Stroke = Brushes.LightPink;
                GraphicPlaceBisection.Children.Add(graph);
            }

            //добавляем корень на график

            if (solution.SolutionExistence) 
            {
                x = solution.EquationResult;
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
                Ellipse result = new Ellipse { Width = 6, Height = 6 };
                result.Fill = Brushes.LightGreen;
                result.Margin = new Thickness(x * 80, -y * 80, 0, 0);
                GraphicPlaceBisection.Children.Add(result);
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

        private void Update_Graphics_Bisection()
        {
            GraphicPlaceBisection.Children.Clear();

            //фон
            Rectangle background = new Rectangle();
            background.Fill = Brushes.DarkSlateGray;
            background.Width = GraphicPlaceBisection.Width - 7;
            background.Height = GraphicPlaceBisection.Height - 10;
            background.VerticalAlignment = VerticalAlignment.Top;
            background.RadiusX = 5;
            background.RadiusY = 5;
            GraphicPlaceBisection.Children.Add(background);
            //ось абсцисс
            Line axisX = new Line();
            axisX.Stroke = Brushes.LightGray;
            axisX.X1 = 0;
            axisX.Y1 = GraphicPlaceBisection.Height * 0.5;
            axisX.X2 = GraphicPlaceBisection.Width;
            axisX.Y2 = GraphicPlaceBisection.Height * 0.5;
            GraphicPlaceBisection.Children.Add(axisX);
            Line arrowXU = new Line();
            arrowXU.Stroke = Brushes.LightGray;
            arrowXU.X1 = GraphicPlaceBisection.Width - 10;
            arrowXU.Y1 = GraphicPlaceBisection.Height * 0.5 - 5;
            arrowXU.X2 = GraphicPlaceBisection.Width - 4;
            arrowXU.Y2 = GraphicPlaceBisection.Height * 0.5;
            GraphicPlaceBisection.Children.Add(arrowXU);
            Line arrowXL = new Line();
            arrowXL.Stroke = Brushes.LightGray;
            arrowXL.X1 = GraphicPlaceBisection.Width - 4;
            arrowXL.Y1 = GraphicPlaceBisection.Height * 0.5;
            arrowXL.X2 = GraphicPlaceBisection.Width - 10;
            arrowXL.Y2 = GraphicPlaceBisection.Height * 0.5 + 5;
            GraphicPlaceBisection.Children.Add(arrowXL);

            //ось ординат
            Line axisY = new Line();
            axisY.Stroke = Brushes.LightGray;
            axisY.X1 = GraphicPlaceBisection.Width / 2;
            axisY.Y1 = GraphicPlaceBisection.Height;
            axisY.X2 = GraphicPlaceBisection.Width / 2;
            axisY.Y2 = 0;
            GraphicPlaceBisection.Children.Add(axisY);
            Line arrowYL = new Line();
            arrowYL.Stroke = Brushes.LightGray;
            arrowYL.X1 = GraphicPlaceBisection.Width / 2 - 5;
            arrowYL.Y1 = 6;
            arrowYL.X2 = GraphicPlaceBisection.Width / 2;
            arrowYL.Y2 = 0;
            GraphicPlaceBisection.Children.Add(arrowYL);
            Line arrowYR = new Line();
            arrowYR.Stroke = Brushes.LightGray;
            arrowYR.X1 = GraphicPlaceBisection.Width / 2;
            arrowYR.Y1 = 0;
            arrowYR.X2 = GraphicPlaceBisection.Width / 2 + 5;
            arrowYR.Y2 = 6;
            GraphicPlaceBisection.Children.Add(arrowYR);

            //штрихи единичных отрезков
            for (int i = 1; i <= 7; i++)
            {
                Line hatchXR = new Line();
                hatchXR.Stroke = Brushes.LightGray;
                hatchXR.X1 = i * 40 + GraphicPlaceBisection.Width / 2;
                hatchXR.Y1 = GraphicPlaceBisection.Height / 2 - 3;
                hatchXR.X2 = i * 40 + GraphicPlaceBisection.Width / 2;
                hatchXR.Y2 = GraphicPlaceBisection.Height / 2 + 3;
                GraphicPlaceBisection.Children.Add(hatchXR);

                Line hatchXL = new Line();
                hatchXL.Stroke = Brushes.LightGray;
                hatchXL.X1 = -1 * i * 40 + GraphicPlaceBisection.Width / 2;
                hatchXL.Y1 = GraphicPlaceBisection.Height / 2 - 3;
                hatchXL.X2 = -1 * i * 40 + GraphicPlaceBisection.Width / 2;
                hatchXL.Y2 = GraphicPlaceBisection.Height / 2 + 3;
                GraphicPlaceBisection.Children.Add(hatchXL);

                Line hatchYU = new Line();
                hatchYU.Stroke = Brushes.LightGray;
                hatchYU.X1 = GraphicPlaceBisection.Width / 2 - 3;
                hatchYU.Y1 = -1 * i * 40 + GraphicPlaceBisection.Height / 2;
                hatchYU.X2 = GraphicPlaceBisection.Width / 2 + 3;
                hatchYU.Y2 = -1 * i * 40 + GraphicPlaceBisection.Height / 2; ;
                GraphicPlaceBisection.Children.Add(hatchYU);

                Line hatchYL = new Line();
                hatchYL.Stroke = Brushes.LightGray;
                hatchYL.X1 = GraphicPlaceBisection.Width / 2 - 3;
                hatchYL.Y1 = i * 40 + GraphicPlaceBisection.Height / 2;
                hatchYL.X2 = GraphicPlaceBisection.Width / 2 + 3;
                hatchYL.Y2 = i * 40 + GraphicPlaceBisection.Height / 2; ;
                GraphicPlaceBisection.Children.Add(hatchYL);
            }
        }

        private void Backhitch_Bisection(object sender, RoutedEventArgs e)
        {
            Update_Graphics_Bisection();

            EquationList.SelectedIndex = 0;
            UpperLimit.Text = "1";
            LowerLimit.Text = "0";
            Accuracy.Text = "0.1";
            OutputConsole.Text = "";
        }

        private void Solve_Equation_Newton(object sender, RoutedEventArgs e)
        {
            int methodIndex = 0;
            int equationIndex = EquationList2.SelectedIndex;
            double upperLimit = Double.Parse(UpperLimit.Text.Replace(".", ","));
            double lowerLimit = Double.Parse(LowerLimit.Text.Replace(".", ","));
            double accuracy = Math.Abs(Double.Parse(Accuracy.Text.Replace(".", ",")));
        }

        private void Update_Graphics_Newton()
        {
            GraphicPlaceNewton.Children.Clear();

            //фон
            Rectangle background = new Rectangle();
            background.Fill = Brushes.DarkSlateGray;
            background.VerticalAlignment = VerticalAlignment.Top;
            background.Width = GraphicPlaceNewton.Width - 7;
            background.Height = GraphicPlaceNewton.Height - 10;
            background.RadiusX = 5;
            background.RadiusY = 5;
            GraphicPlaceNewton.Children.Add(background);
            //ось абсцисс
            Line axisX = new Line();
            axisX.Stroke = Brushes.LightGray;
            axisX.X1 = 0;
            axisX.Y1 = GraphicPlaceNewton.Height * 0.5;
            axisX.X2 = GraphicPlaceNewton.Width;
            axisX.Y2 = GraphicPlaceNewton.Height * 0.5;
            GraphicPlaceNewton.Children.Add(axisX);
            Line arrowXU = new Line();
            arrowXU.Stroke = Brushes.LightGray;
            arrowXU.X1 = GraphicPlaceNewton.Width - 10;
            arrowXU.Y1 = GraphicPlaceNewton.Height * 0.5 - 5;
            arrowXU.X2 = GraphicPlaceNewton.Width - 4;
            arrowXU.Y2 = GraphicPlaceNewton.Height * 0.5;
            GraphicPlaceNewton.Children.Add(arrowXU);
            Line arrowXL = new Line();
            arrowXL.Stroke = Brushes.LightGray;
            arrowXL.X1 = GraphicPlaceNewton.Width - 4;
            arrowXL.Y1 = GraphicPlaceNewton.Height * 0.5;
            arrowXL.X2 = GraphicPlaceNewton.Width - 10;
            arrowXL.Y2 = GraphicPlaceNewton.Height * 0.5 + 5;
            GraphicPlaceNewton.Children.Add(arrowXL);

            //ось ординат
            Line axisY = new Line();
            axisY.Stroke = Brushes.LightGray;
            axisY.X1 = GraphicPlaceNewton.Width / 2;
            axisY.Y1 = GraphicPlaceNewton.Height;
            axisY.X2 = GraphicPlaceNewton.Width / 2;
            axisY.Y2 = 0;
            GraphicPlaceNewton.Children.Add(axisY);
            Line arrowYL = new Line();
            arrowYL.Stroke = Brushes.LightGray;
            arrowYL.X1 = GraphicPlaceNewton.Width / 2 - 5;
            arrowYL.Y1 = 6;
            arrowYL.X2 = GraphicPlaceNewton.Width / 2;
            arrowYL.Y2 = 0;
            GraphicPlaceNewton.Children.Add(arrowYL);
            Line arrowYR = new Line();
            arrowYR.Stroke = Brushes.LightGray;
            arrowYR.X1 = GraphicPlaceNewton.Width / 2;
            arrowYR.Y1 = 0;
            arrowYR.X2 = GraphicPlaceNewton.Width / 2 + 5;
            arrowYR.Y2 = 6;
            GraphicPlaceNewton.Children.Add(arrowYR);

            //штрихи единичных отрезков
            for (int i = 1; i <= 7; i++)
            {
                Line hatchXR = new Line();
                hatchXR.Stroke = Brushes.LightGray;
                hatchXR.X1 = i * 40 + GraphicPlaceNewton.Width / 2;
                hatchXR.Y1 = GraphicPlaceNewton.Height / 2 - 3;
                hatchXR.X2 = i * 40 + GraphicPlaceNewton.Width / 2;
                hatchXR.Y2 = GraphicPlaceNewton.Height / 2 + 3;
                GraphicPlaceNewton.Children.Add(hatchXR);

                Line hatchXL = new Line();
                hatchXL.Stroke = Brushes.LightGray;
                hatchXL.X1 = -1 * i * 40 + GraphicPlaceNewton.Width / 2;
                hatchXL.Y1 = GraphicPlaceNewton.Height / 2 - 3;
                hatchXL.X2 = -1 * i * 40 + GraphicPlaceNewton.Width / 2;
                hatchXL.Y2 = GraphicPlaceNewton.Height / 2 + 3;
                GraphicPlaceNewton.Children.Add(hatchXL);

                Line hatchYU = new Line();
                hatchYU.Stroke = Brushes.LightGray;
                hatchYU.X1 = GraphicPlaceNewton.Width / 2 - 3;
                hatchYU.Y1 = -1 * i * 40 + GraphicPlaceNewton.Height / 2;
                hatchYU.X2 = GraphicPlaceNewton.Width / 2 + 3;
                hatchYU.Y2 = -1 * i * 40 + GraphicPlaceNewton.Height / 2; ;
                GraphicPlaceNewton.Children.Add(hatchYU);

                Line hatchYL = new Line();
                hatchYL.Stroke = Brushes.LightGray;
                hatchYL.X1 = GraphicPlaceNewton.Width / 2 - 3;
                hatchYL.Y1 = i * 40 + GraphicPlaceNewton.Height / 2;
                hatchYL.X2 = GraphicPlaceNewton.Width / 2 + 3;
                hatchYL.Y2 = i * 40 + GraphicPlaceNewton.Height / 2; ;
                GraphicPlaceNewton.Children.Add(hatchYL);
            }
        }

        private void Backhitch_Newton(object sender, RoutedEventArgs e)
        {
            Update_Graphics_Newton();

            EquationList2.SelectedIndex = 0;
            StartApproximation.Text = "0";
            Accuracy2.Text = "0.1";
            OutputConsole2.Text = "";
        }

        private void StartApproximation_LostFocus(object sender, RoutedEventArgs e)
        {
            if(Double.Parse(StartApproximation.Text) < -8) StartApproximation.Text = "-8";
            if (Double.Parse(StartApproximation.Text) > 8) StartApproximation.Text = "8";
        }
    }
}
