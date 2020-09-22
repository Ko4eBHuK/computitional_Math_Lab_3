using System;

namespace CompMathPart3
{
    
    class NonlinearEquationMethods {

        public int MethodIndex { get; set; }
        public int EquationIndex { get; set; }
        public double RightSectionValue { get; set; }
        public double LteftSectionValue { get; set; }
        public double Accuracy { get; set; }
        public int CountOfIterations { get; set; }
        public double EquationResult { get; set; }
        public double ResultAccuracy { get; set; }
        public bool SolutionExistence { get; set; }

        public NonlinearEquationMethods(int methodIndex, int equationIndex, double upperLimit, double lowerLimit, double accuracy) 
        {
            MethodIndex = methodIndex;
            EquationIndex = equationIndex;
            RightSectionValue = upperLimit;
            LteftSectionValue = lowerLimit;
            Accuracy = accuracy;

            if (MethodIndex == 0)
            {
                BisectionMethod();
            }
            else if (MethodIndex == 1)
            {
                EquationResult = 5;
            }
        }

        private void BisectionMethod()
        {
            double leftFunctionValue = 0;
            double rightFunctionValue = 0;
            double iterationResult = 0;
            double previousIterationResult = 0;
            double leftValue = LteftSectionValue;
            double rightValue = RightSectionValue;
            double functionValueInPoint = 0;
            CountOfIterations = 0;
            ResultAccuracy = 5;
            SolutionExistence = true;

            //нулевой шаг и проверки
            switch (EquationIndex)
            {
                case 0:
                    leftFunctionValue = Math.Pow(leftValue, 5) + 4;
                    rightFunctionValue = Math.Pow(rightValue, 5) + 4;
                    break;
                case 1:
                    leftFunctionValue = 3 * Math.Cos(3 * leftValue);
                    rightFunctionValue = 3 * Math.Cos(3 * rightValue);
                    break;
                case 2:
                    leftFunctionValue = 1.5 * Math.Sin(1.5 * leftValue) - 3 * Math.Cos(3 * leftValue);
                    rightFunctionValue = 1.5 * Math.Sin(1.5 * rightValue) - 3 * Math.Cos(3 * rightValue);
                    break;
            }
            if (leftFunctionValue * rightFunctionValue < 0)
            {
                previousIterationResult = (leftValue + rightValue) / 2;
                switch (EquationIndex)
                {
                    case 0:
                        functionValueInPoint = Math.Pow(previousIterationResult, 5) + 4;
                        break;
                    case 1:
                        functionValueInPoint = 3 * Math.Cos(3 * previousIterationResult);
                        break;
                    case 2:
                        functionValueInPoint = 1.5 * Math.Sin(1.5 * previousIterationResult) - 3 * Math.Cos(3 * previousIterationResult);
                        break;
                }
                if(functionValueInPoint == 0)
                {
                    EquationResult = previousIterationResult;
                    return;
                } 
            }
            else if (leftFunctionValue * rightFunctionValue == 0)
            {
                if (leftFunctionValue == 0) EquationResult = leftValue;
                if (rightFunctionValue == 0) EquationResult = rightValue;
                return;
            }
            else SolutionExistence = false;
            iterationResult = previousIterationResult;

            //шаги с первого по энный, когда разность знаков соблюдена
            while (ResultAccuracy > Accuracy & CountOfIterations < 10000001 & SolutionExistence)
            {
                previousIterationResult = iterationResult;

                switch (EquationIndex)
                {
                    case 0:
                        leftFunctionValue = Math.Pow(leftValue, 5) + 4;
                        rightFunctionValue = Math.Pow(rightValue, 5) + 4;
                        functionValueInPoint = Math.Pow(previousIterationResult, 5) + 4;
                        break;
                    case 1:
                        leftFunctionValue = 3 * Math.Cos(3 * leftValue);
                        rightFunctionValue = 3 * Math.Cos(3 * rightValue);
                        functionValueInPoint = 3 * Math.Cos(3 * previousIterationResult);
                        break;
                    case 2:
                        leftFunctionValue = 1.5 * Math.Sin(1.5 * leftValue) - 3 * Math.Cos(3 * leftValue);
                        rightFunctionValue = 1.5 * Math.Sin(1.5 * rightValue) - 3 * Math.Cos(3 * rightValue);
                        functionValueInPoint = 1.5 * Math.Sin(1.5 * previousIterationResult) - 3 * Math.Cos(3 * previousIterationResult);
                        break;
                }
                
                if(leftFunctionValue * functionValueInPoint < 0)
                {
                    rightValue = previousIterationResult;
                } 
                else if (rightFunctionValue * functionValueInPoint < 0)
                {
                    leftValue = previousIterationResult;
                }
                else if (functionValueInPoint == 0)
                {
                    EquationResult = previousIterationResult;
                    return;
                }

                iterationResult = (leftValue + rightValue) / 2;

                ResultAccuracy = Math.Abs(previousIterationResult - iterationResult);
                CountOfIterations++;
            }

            if(SolutionExistence & CountOfIterations < 10000000)
            {
                EquationResult = iterationResult;
            }
            else
            {
                SolutionExistence = false;
            }

            
        }

        private void NewtonMethod()
        {

        }
        
    }
}