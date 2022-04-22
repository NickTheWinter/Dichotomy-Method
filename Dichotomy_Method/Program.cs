using System;
using System.Collections.Generic;
using System.Linq;

namespace Dichotomy_Method
{
    internal class Program
    {
        //Функция 
        static double fx(double x) => Math.Cos(2 * x) - 3 * Math.Pow(x, 2) + 1;
        //Производная функции
        static double f1x(double x) => -2 * Math.Sin(2 * x) - 6 * x;
        static void Main(string[] args)
        {
            //Задается интервал и шаг, точность
            double a = -10, b = 10, h = 1, k = 0.001;

            double c, funcC, funcB;
            double[] equation = new double[Convert.ToInt32((b - a) / h) + 1];
            Dictionary<double, double> DerEquation = new Dictionary<double, double>();
            int counter = 0;

            List<double> suspicious = new List<double>();

            for (double i = a; i <= b; i += h)
            {
                //исходное уравнение 
                double tmp = fx(i);
                //double tmp = Math.Pow(i, 3) + i - 1;
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
            int counterPairs = suspicious.Count / 2;
            double jr;
            for (int i = 0; i < suspicious.Count; i += 2)
            {
                try
                {
                    for (double j = suspicious[i]; j < suspicious[i + 1]; j += h / 10)
                    {
                        jr = Math.Round(j, (h / 10).ToString().Count() - 1);
                        //Производная функции
                        double tmp = f1x(j);
                        //double tmp = 3 * Math.Pow(j, 2) + 1;
                        DerEquation.Add(jr, Math.Round(tmp, 13));
                        try
                        {
                            if (DerEquation[jr] == 0 && jr == 0)
                                DerEquation.Remove(jr);
                            if ((DerEquation[jr] > 0 && DerEquation[jr + 1] > 0) || (DerEquation[jr] < 0 && DerEquation[jr + 1] < 0))
                            {

                            }
                            else
                            {
                                suspicious.RemoveAt(i);
                                suspicious.RemoveAt(i);
                                Console.WriteLine($"Знак производной не сохраняется на отрезке [{i};{i + 1}]. ");
                            }


                        }
                        catch (KeyNotFoundException) { }
                        catch (ArgumentOutOfRangeException) { }

                    }

                }
                catch (ArgumentOutOfRangeException) { }


            }
            double length;
            for (int i = 0; i < suspicious.Count; i += 2)
            {
                a = DerEquation.ElementAt(Convert.ToInt32(i + (DerEquation.Count / counterPairs / 2.5) * i)).Key;
                b = DerEquation.ElementAt((DerEquation.Count / counterPairs - 1) + (i * DerEquation.Count / counterPairs) / 2).Key;
                do
                {
                    c = (a + b) / 2;
                    //Подставляем исходную формулу для вычисления функций
                    funcC = fx(c);
                    funcB = fx(b);
                    //funcC = Math.Pow(c, 3) + c - 1;
                    //funcB = Math.Pow(b, 3) + b - 1; 

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
