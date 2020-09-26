using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            Show_Graphic_Bisection(-8, 8);

            Update_Graphics_Newton();
            Show_Graphic_Newton(-8, 8);

            Update_Graphics_Simple_Iter();
            Show_Graphics_SimpleIter_X1(-8, 8);
            Show_Graphics_SimpleIter_X2(-8, 8);
        }

        //Общие
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
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
                MessageBox.Show("Значение выставленно некорректно.\nОно будет сброшено.");
            }
        }

        //Для метода бисекции
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

            var solution = new NonlinearEquationMethods(methodIndex, equationIndex, upperLimit, lowerLimit, accuracy, 0);
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

            Show_Graphic_Bisection(lowerLimit, upperLimit);

            //добавляем корень на график
            double x;
            double y = 0;

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
                result.Fill = Brushes.Lime;
                result.Margin = new Thickness(x * 80, -y * 80, 0, 0);
                GraphicPlaceBisection.Children.Add(result);
            }
        }

        private void Update_Graphics_Bisection()
        {
            if (GraphicPlaceBisection != null)
            {
                GraphicPlaceBisection.Children.Clear();

                //фон
                Rectangle background = new Rectangle();
                background.Fill = Brushes.DarkSlateGray;
                background.Width = GraphicPlaceBisection.Width - 4;
                background.Height = GraphicPlaceBisection.Height - 8;
                background.VerticalAlignment = VerticalAlignment.Top;
                background.HorizontalAlignment = HorizontalAlignment.Left;
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

        }

        private void Show_Graphic_Bisection(double a, double b)
        {
            if (GraphicPlaceBisection != null)
            {
                int equationIndex = EquationList.SelectedIndex;
                double x = a;
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
                while (x <= b)
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
                    graph.Stroke = Brushes.HotPink;
                    graph.StrokeThickness = 3;
                    GraphicPlaceBisection.Children.Add(graph);
                }
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

        private void EquationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update_Graphics_Bisection();
            Show_Graphic_Bisection(-8, 8);
        }


        //Для метода Ньютона
        private void Solve_Equation_Newton(object sender, RoutedEventArgs e)
        {
            int methodIndex = 1;
            int equationIndex = EquationList2.SelectedIndex;
            double startApproximation = Double.Parse(StartApproximation.Text.Replace(".", ","));
            double accuracy = Math.Abs(Double.Parse(Accuracy.Text.Replace(".", ",")));

            var solution = new NonlinearEquationMethods(methodIndex, equationIndex, 0, 0, accuracy, startApproximation);

            if (solution.SolutionExistence)
            {
                if(solution.FastConvergence){
                    OutputConsole2.Text += "При данном начальном приближении обеспечена быстрая сходимость.\n";
                }

                OutputConsole2.Text += "Значение корня: " + solution.EquationResult + "\n" +
                    "Достигнутая погрешность: " + solution.ResultAccuracy + "\n" +
                    "Количество итераций: " + solution.CountOfIterations + "\n" +
                    "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";
            }
            else
            {
                OutputConsole2.Text += "При данном приближении нет гарнатии существования корня, задайте другой.\n" +
                    "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";
            }

            Update_Graphics_Newton();

            Show_Graphic_Newton(-8, 8);

            //добавляем корень на график
            double x;
            double y = 0;

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
                result.Fill = Brushes.Lime;
                result.Margin = new Thickness(x * 80, -y * 80, 0, 0);
                GraphicPlaceNewton.Children.Add(result);
            }
        }

        private void Update_Graphics_Newton()
        {
            if(GraphicPlaceNewton != null)
            {
                GraphicPlaceNewton.Children.Clear();

                //фон
                Rectangle background = new Rectangle();
                background.Fill = Brushes.DarkSlateGray;
                background.VerticalAlignment = VerticalAlignment.Top;
                background.HorizontalAlignment = HorizontalAlignment.Left;
                background.Width = GraphicPlaceNewton.Width - 4;
                background.Height = GraphicPlaceNewton.Height - 8;
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

        }

        private void Show_Graphic_Newton(double a, double b)
        {
            if (GraphicPlaceNewton != null)
            {
                int equationIndex = EquationList2.SelectedIndex;
                double x = a;
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
                while (x <= b)
                {
                    Line graph = new Line();
                    graph.X1 = x * 40 + GraphicPlaceNewton.Width * 0.5;
                    graph.Y1 = -1 * y * 40 + GraphicPlaceNewton.Height * 0.5;

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
                    graph.X2 = x * 40 + GraphicPlaceNewton.Width * 0.5;
                    graph.Y2 = -1 * y * 40 + GraphicPlaceNewton.Height * 0.5;
                    graph.Stroke = Brushes.HotPink;
                    graph.StrokeThickness = 3;
                    GraphicPlaceNewton.Children.Add(graph);
                }
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

        private void EquationList2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update_Graphics_Newton();
            Show_Graphic_Newton(-8, 8);
        }


        //Для метода простой итерации
        private void Solve_System(object sender, RoutedEventArgs e)
        {
            int equationIndexX1 = EquationListX1.SelectedIndex;
            int equationIndexX2 = EquationListX2.SelectedIndex;
            double startApproximationX1 = Double.Parse(StartApproximationX1.Text.Replace(".", ","));
            double startApproximationX2 = Double.Parse(StartApproximationX2.Text.Replace(".", ","));
            double accuracy = Double.Parse(Accuracy3.Text.Replace(".", ","));

            Update_Graphics_Simple_Iter();
            Show_Graphics_SimpleIter_X1(-8, 8);
            Show_Graphics_SimpleIter_X2(-8, 8);



            OutputConsole3.Text += "Индекс первого уравнения: " + equationIndexX1 + "\n";
            OutputConsole3.Text += "Индекс второго уравнения: " + equationIndexX2 + "\n";
            OutputConsole3.Text += "Приближение первого уравнения: " + startApproximationX1 + "\n";
            OutputConsole3.Text += "Приближение второго уравнения: " + startApproximationX2 + "\n";
            OutputConsole3.Text += "Погрешность решения: " + accuracy + "\n";
            OutputConsole3.Text += "~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n";
        }

        private void Update_Graphics_Simple_Iter()
        {
            if (GraphicPlaceSimpleIter != null)
            {
                GraphicPlaceSimpleIter.Children.Clear();

                //фон
                Rectangle background = new Rectangle();
                background.Fill = Brushes.DarkSlateGray;
                background.VerticalAlignment = VerticalAlignment.Top;
                background.HorizontalAlignment = HorizontalAlignment.Left;
                background.Width = GraphicPlaceSimpleIter.Width - 4;
                background.Height = GraphicPlaceSimpleIter.Height - 8;
                background.RadiusX = 5;
                background.RadiusY = 5;
                GraphicPlaceSimpleIter.Children.Add(background);
                //ось абсцисс
                Line axisX = new Line();
                axisX.Stroke = Brushes.LightGray;
                axisX.X1 = 0;
                axisX.Y1 = GraphicPlaceSimpleIter.Height * 0.5;
                axisX.X2 = GraphicPlaceSimpleIter.Width;
                axisX.Y2 = GraphicPlaceSimpleIter.Height * 0.5;
                GraphicPlaceSimpleIter.Children.Add(axisX);
                Line arrowXU = new Line();
                arrowXU.Stroke = Brushes.LightGray;
                arrowXU.X1 = GraphicPlaceSimpleIter.Width - 10;
                arrowXU.Y1 = GraphicPlaceSimpleIter.Height * 0.5 - 5;
                arrowXU.X2 = GraphicPlaceSimpleIter.Width - 4;
                arrowXU.Y2 = GraphicPlaceSimpleIter.Height * 0.5;
                GraphicPlaceSimpleIter.Children.Add(arrowXU);
                Line arrowXL = new Line();
                arrowXL.Stroke = Brushes.LightGray;
                arrowXL.X1 = GraphicPlaceSimpleIter.Width - 4;
                arrowXL.Y1 = GraphicPlaceSimpleIter.Height * 0.5;
                arrowXL.X2 = GraphicPlaceSimpleIter.Width - 10;
                arrowXL.Y2 = GraphicPlaceSimpleIter.Height * 0.5 + 5;
                GraphicPlaceSimpleIter.Children.Add(arrowXL);

                //ось ординат
                Line axisY = new Line();
                axisY.Stroke = Brushes.LightGray;
                axisY.X1 = GraphicPlaceSimpleIter.Width / 2;
                axisY.Y1 = GraphicPlaceSimpleIter.Height;
                axisY.X2 = GraphicPlaceSimpleIter.Width / 2;
                axisY.Y2 = 0;
                GraphicPlaceSimpleIter.Children.Add(axisY);
                Line arrowYL = new Line();
                arrowYL.Stroke = Brushes.LightGray;
                arrowYL.X1 = GraphicPlaceSimpleIter.Width / 2 - 5;
                arrowYL.Y1 = 6;
                arrowYL.X2 = GraphicPlaceSimpleIter.Width / 2;
                arrowYL.Y2 = 0;
                GraphicPlaceSimpleIter.Children.Add(arrowYL);
                Line arrowYR = new Line();
                arrowYR.Stroke = Brushes.LightGray;
                arrowYR.X1 = GraphicPlaceSimpleIter.Width / 2;
                arrowYR.Y1 = 0;
                arrowYR.X2 = GraphicPlaceSimpleIter.Width / 2 + 5;
                arrowYR.Y2 = 6;
                GraphicPlaceSimpleIter.Children.Add(arrowYR);

                //штрихи единичных отрезков
                for (int i = 1; i <= 7; i++)
                {
                    Line hatchXR = new Line();
                    hatchXR.Stroke = Brushes.LightGray;
                    hatchXR.X1 = i * 40 + GraphicPlaceSimpleIter.Width / 2;
                    hatchXR.Y1 = GraphicPlaceSimpleIter.Height / 2 - 3;
                    hatchXR.X2 = i * 40 + GraphicPlaceSimpleIter.Width / 2;
                    hatchXR.Y2 = GraphicPlaceSimpleIter.Height / 2 + 3;
                    GraphicPlaceSimpleIter.Children.Add(hatchXR);

                    Line hatchXL = new Line();
                    hatchXL.Stroke = Brushes.LightGray;
                    hatchXL.X1 = -1 * i * 40 + GraphicPlaceSimpleIter.Width / 2;
                    hatchXL.Y1 = GraphicPlaceSimpleIter.Height / 2 - 3;
                    hatchXL.X2 = -1 * i * 40 + GraphicPlaceSimpleIter.Width / 2;
                    hatchXL.Y2 = GraphicPlaceSimpleIter.Height / 2 + 3;
                    GraphicPlaceSimpleIter.Children.Add(hatchXL);

                    Line hatchYU = new Line();
                    hatchYU.Stroke = Brushes.LightGray;
                    hatchYU.X1 = GraphicPlaceSimpleIter.Width / 2 - 3;
                    hatchYU.Y1 = -1 * i * 40 + GraphicPlaceSimpleIter.Height / 2;
                    hatchYU.X2 = GraphicPlaceSimpleIter.Width / 2 + 3;
                    hatchYU.Y2 = -1 * i * 40 + GraphicPlaceSimpleIter.Height / 2; ;
                    GraphicPlaceSimpleIter.Children.Add(hatchYU);

                    Line hatchYL = new Line();
                    hatchYL.Stroke = Brushes.LightGray;
                    hatchYL.X1 = GraphicPlaceSimpleIter.Width / 2 - 3;
                    hatchYL.Y1 = i * 40 + GraphicPlaceSimpleIter.Height / 2;
                    hatchYL.X2 = GraphicPlaceSimpleIter.Width / 2 + 3;
                    hatchYL.Y2 = i * 40 + GraphicPlaceSimpleIter.Height / 2; ;
                    GraphicPlaceSimpleIter.Children.Add(hatchYL);
                }
            }
        }

        private void Show_Graphics_SimpleIter_X1(double a, double b)
        {
            if (GraphicPlaceSimpleIter != null)
            {
                int equationIndex = EquationListX1.SelectedIndex;
                double x = a;
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
                        y = 1.5 * Math.Sin(1.5 * x);
                        break;
                }
                while (x <= b)
                {
                    Line graph = new Line();
                    graph.X1 = x * 40 + GraphicPlaceSimpleIter.Width * 0.5;
                    graph.Y1 = -1 * y * 40 + GraphicPlaceSimpleIter.Height * 0.5;

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
                    graph.X2 = x * 40 + GraphicPlaceSimpleIter.Width * 0.5;
                    graph.Y2 = -1 * y * 40 + GraphicPlaceSimpleIter.Height * 0.5;
                    graph.Stroke = Brushes.HotPink;
                    graph.StrokeThickness = 3;
                    GraphicPlaceSimpleIter.Children.Add(graph);
                }
            }
        }

        private void Show_Graphics_SimpleIter_X2(double a, double b)
        {
            if (GraphicPlaceSimpleIter != null)
            {
                int equationIndex = EquationListX2.SelectedIndex;
                double x = a;
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
                        y = 1.5 * Math.Sin(1.5 * x);
                        break;
                }
                while (x <= b)
                {
                    Line graph = new Line();
                    graph.X1 = x * 40 + GraphicPlaceSimpleIter.Width * 0.5;
                    graph.Y1 = -1 * y * 40 + GraphicPlaceSimpleIter.Height * 0.5;

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
                    graph.X2 = x * 40 + GraphicPlaceSimpleIter.Width * 0.5;
                    graph.Y2 = -1 * y * 40 + GraphicPlaceSimpleIter.Height * 0.5;
                    graph.Stroke = Brushes.OrangeRed;
                    graph.StrokeThickness = 3;
                    GraphicPlaceSimpleIter.Children.Add(graph);
                }
            }
        }

        private void Backhitch_SimpleIter(object sender, RoutedEventArgs e)
        {
            EquationListX1.SelectedIndex = 0;
            EquationListX2.SelectedIndex = 0;
            StartApproximationX1.Text = "1";
            StartApproximationX2.Text = "1";
            Accuracy3.Text = "0.1";

            Update_Graphics_Simple_Iter();
        }

        private void EquationLists_SimpleIter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update_Graphics_Simple_Iter();
            Show_Graphics_SimpleIter_X1(-8, 8);
            Show_Graphics_SimpleIter_X2(-8, 8);
        }
    }
}
