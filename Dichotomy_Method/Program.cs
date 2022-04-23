using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace Dichotomy_Method
{
    internal class Program
    {
        //Функция 
        static double fx(double x) => Math.Cos(2 * x) - 3 * Math.Pow(x, 2) + 1;
        //static double fx(double x) => Pow(x, 3) + 4 * Pow(x, 2) - 1;
        //Производная функции
        static double f1x(double x) => -2 * Math.Sin(2 * x) - 6 * x;
        //static double f1x(double x) => 3 * Pow(x, 2) + 8 * x;
        static void Main(string[] args)
        {
            //Задается интервал и шаг, точность
            double a = -10, b = 10, h = 1, k = 0.001;
            double j;
            double c, funcC, funcB;
            double[] equation = new double[Convert.ToInt32((b - a) / h) + 1];
            Dictionary<double, double> DerEquation = new Dictionary<double, double>();
            int counter = 0;

            List<double> suspicious = new List<double>();

            //Поиск подозрительных отрезков изоляции
            for (double i = a; i <= b; i += h)
            {
                //исходное уравнение 
                double tmp = fx(i);
                equation[counter] = tmp;
                try
                {
                    if (equation[counter] > 0 && equation[counter - 1] < 0 || equation[counter] < 0 && equation[counter - 1] > 0)
                    {
                        suspicious.Add(i - 1);
                        suspicious.Add(i);
                    }
                }
                catch (IndexOutOfRangeException) { }

                counter++;
            }
            Console.Write("Подозрительные на отрезки озоляции: ");
            for (int i = 0; i < suspicious.Count; i += 2)
            {
                Console.Write("[{0,2};{1,2}] ", suspicious[i], suspicious[i + 1]);
            }
            Console.WriteLine();
            int counterPairs = suspicious.Count / 2;
            int iteration = 0;
            List<double> lastOfInterval = new List<double>();
            List<double> firstOfInterval = new List<double>();
            double jr = 0;

            //Определяются отрезки изоляции
            for (int i = 0; i < suspicious.Count; i += 2)
            {

                iteration = 0;
                int number = 0;
                try
                {
                    for (j = suspicious[i]; j <= suspicious[i + 1]; j += h / 10)
                    {

                        j = Round(j, 4);
                        jr = Math.Round(j, (h / 10).ToString().Count() - 1);

                        double tmp = f1x(j);

                        DerEquation.Add(jr, Math.Round(tmp, 13));
                        if (iteration == 0)
                        {
                            ;
                            firstOfInterval.Add(jr);
                        }
                        iteration++;
                        try
                        {
                            if (DerEquation[jr] == 0 && jr == 0)
                            {
                                DerEquation.Remove(jr);
                                iteration--;
                                firstOfInterval.Remove(jr);
                            }

                            if (((DerEquation[jr] > 0 && DerEquation[jr - h / 10] > 0) || (DerEquation[jr] < 0 && DerEquation[jr - h / 10] < 0)))
                            {
                                if (number != 1)
                                {
                                    Console.WriteLine("[{0,2};{1,2}] - отрезок изоляции", suspicious[i], suspicious[i + 1]);
                                    number = 1;
                                }

                            }
                            else
                            {
                                Console.WriteLine("Знак производной не сохраняется на отрезке [{0,2};{1,2}]. ", suspicious[i], suspicious[i + 1]);
                                suspicious.RemoveAt(i);
                                suspicious.RemoveAt(i);
                            }

                        }
                        catch (KeyNotFoundException) { }
                        catch (ArgumentOutOfRangeException) { }

                    }

                }
                catch (ArgumentOutOfRangeException) { }

                lastOfInterval.Add(DerEquation.Last().Key);


            }

            double length;
            //Метод дихотомии
            for (int i = 0; i < suspicious.Count; i += 2)
            {
                a = firstOfInterval[i / 2];
                b = lastOfInterval[i / 2];
                do
                {
                    c = (a + b) / 2;
                    //Подставляем исходную формулу для вычисления функций
                    funcC = fx(c);
                    funcB = fx(b);
                    length = b - a;

                    if (funcB * funcC < 0)
                        a = c;
                    else
                        b = c;

                } while (length > 2 * k);
                Console.WriteLine($"Корень {Math.Round(c, k.ToString().Length - 2)}");
            }

            Console.ReadLine();
        }
    }
}
