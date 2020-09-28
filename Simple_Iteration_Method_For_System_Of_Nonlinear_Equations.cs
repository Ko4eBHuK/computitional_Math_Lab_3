using System;

namespace CompMathPart3
{
    class Simple_Iteration_Method_For_System_Of_Nonlinear_Equations
    {
        public double StartApproximationX { get; set; }
        public double StartApproximationY { get; set; }
        public double Accuracy { get; set; }
        public int CountOfIterations { get; set; }
        public double SystemResultX { get; set; }
        public double SystemResultY { get; set; }
        public double ResultAccuracyX { get; set; }
        public double ResultAccuracyY { get; set; }
        public bool SolutionExistence { get; set; }
        public bool ConvergenceСondition { get; set; }

        
        public Simple_Iteration_Method_For_System_Of_Nonlinear_Equations(double startApproximationX, double startApproximationY, double accuracy)
        {
            StartApproximationX = startApproximationX;
            StartApproximationY = startApproximationY;
            Accuracy = accuracy;

            Solve_The_System();
        }


        
        private void Solve_The_System()
        {
            double previousX = StartApproximationX;
            double previousY = StartApproximationY;
            double currentX = 0;
            double currentY = 0;
            CountOfIterations = 0;
            ResultAccuracyX = 1;
            ResultAccuracyY = 1;
            double determinant = GetDerivativeFiOneByX(previousX, previousY) * GetDerivativeFiTwoByY(previousX, previousY) - GetDerivativeFiOneByY(previousX, previousY) * GetDerivativeFiTwoByX(previousX, previousY);

            //проверка условий сходимости
            SolutionExistence = determinant != 0;
            ConvergenceСondition = (Math.Abs(GetDerivativeFiOneByX(previousX, previousY)) + Math.Abs(GetDerivativeFiTwoByX(previousX, previousY))) < 1 & (Math.Abs(GetDerivativeFiOneByY(previousX, previousY)) + Math.Abs(GetDerivativeFiTwoByY(previousX, previousY))) < 1;

            //итерационный процесс
            while (SolutionExistence & ( ResultAccuracyX > Accuracy | ResultAccuracyY > Accuracy ) & CountOfIterations < 1000)
            {
                CountOfIterations++;

                currentX = GetFiOne(previousX, previousY);
                currentY = GetFiTwo(previousX, previousY);

                ResultAccuracyX = Math.Abs(currentX - previousX);
                ResultAccuracyY = Math.Abs(currentY - previousY);

                determinant = GetDerivativeFiOneByX(currentX, currentY) * GetDerivativeFiTwoByY(currentX, currentY) - GetDerivativeFiOneByY(currentX, currentY) * GetDerivativeFiTwoByX(currentX, currentY);
                SolutionExistence = determinant != 0;

                previousX = currentX;
                previousY = currentY;
            }

            if (SolutionExistence)
            {
                SystemResultX = currentX;
                SystemResultY = currentY;
            }
        }

        private double GetFiOne(double x, double y)
        {
            return Math.Pow((y + 5) / 2, 0.5);
        }

        private double GetFiTwo(double x, double y)
        {
            return 0.1 * Math.Pow(x - 4, 2) - 4;
        }

        private double GetDerivativeFiOneByX(double x, double y)
        {
            return 0;
        }

        private double GetDerivativeFiOneByY(double x, double y)
        {
            return 1 / (4 * Math.Pow((y + 5) / 2, 0.5));
        }

        private double GetDerivativeFiTwoByX(double x, double y)
        {
            return 0.2 * (x - 4);
        }

        private double GetDerivativeFiTwoByY(double x, double y)
        {
            return 0;
        }
    }
}
