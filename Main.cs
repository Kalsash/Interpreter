using System;
using System.IO;
using System.Collections.Generic;
using SimpleScanner;
using SimpleParser;
using SimpleLang;
using System.Diagnostics;

namespace SimpleCompiler
{
    public class SimpleCompilerMain
    {
        public static void Main()
        {
            Console.WriteLine("Begin");

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            ThreeAddress t1 = new ThreeAddress(2);
            ThreeAddress t2 = new ThreeAddress(1);
            ThreeAddress t3 = new ThreeAddress(1);
            ThreeAddress t4 = new ThreeAddress(3);
            ThreeAddress t5 = new ThreeAddress(13);
            ThreeAddress t6 = new ThreeAddress(14);
            ThreeAddress t7 = new ThreeAddress(9);
            ThreeAddress t8 = new ThreeAddress(15);
            ThreeAddress t9 = new ThreeAddress(16);
            ThreeAddress t10 = new ThreeAddress(12);
            ThreeAddress t11 = new ThreeAddress(0);

            double s;
            int n;
            int i;
            bool b;
            double t; 
            unsafe
            {
                t1.pda = &s;
                t1.doubleVal = 0.0; // s = 0.0
                t2.pia = &n;
                t2.intVal = 100000000; // n = 10000000
                t3.pia = &i;
                t3.intVal = 1; // i = 1
                t4.pba = &b;
                t4.boolVal = false; // b = false
               t5.pba = t4.pba;
                t5.Goto = 11;  // if i >= n goto 
               t6.pda = &t; 
                t6.doubleVal = 1.0; // t = 1.0 / i
                t6.pib = t3.pia;
                t7.pda = t1.pda;
                t7.pdb = t1.pda;
                t7.pdc = t6.pda; // s = s + t
                t8.pia = t3.pia;
                t8.pib = t3.pia;
                t8.intVal = 1; // i = i + 1
                t9.pba = t4.pba;
                t9.pib = t3.pia;
                t9.pic = t2.pia;
                t10.Goto = 5;

            }
            Optimiser op = new Optimiser(11);
            op.AddCommands(t1);
            op.AddCommands(t2);
            op.AddCommands(t3);
            op.AddCommands(t4);
            op.AddCommands(t5);
            op.AddCommands(t6);
            op.AddCommands(t7);
            op.AddCommands(t8);
            op.AddCommands(t9);
            op.AddCommands(t10);
            op.AddCommands(t11);

            op.RunCommands();

            stopwatch.Stop();
            Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds + " ms");


            Console.WriteLine("Value: " + s);
            Console.WriteLine("End");
                //Console.WriteLine(n);
                //Console.WriteLine(i);
                //Console.WriteLine(b);
                // Console.WriteLine(t);


            //string FileName = @"..\..\a.txt";
            //try
            //{
            //    string Text = File.ReadAllText(FileName);

            //    Scanner scanner = new Scanner();
            //    scanner.SetSource(Text, 0);

            //    Parser parser = new Parser(scanner);

            //    var b = parser.Parse();
            //    if (!b)
            //        Console.WriteLine("Ошибка");
            //    else
            //    {
            //        Console.WriteLine("Синтаксическое дерево построено");
            //        var TypeChecker = new SemanticChecker();
            //        parser.root.Eval(TypeChecker);
            //        Console.WriteLine("Типы проверены!");
            //        //var d = new Interpreter();
            //       // parser.root.Eval(d);
            //    }
            //    Console.WriteLine("Программа завершена");
            //}

            //catch (FileNotFoundException)
            //{
            //    Console.WriteLine("Файл {0} не найден", FileName);
            //}
            //catch (LexException e)
            //{
            //    Console.WriteLine("Лексическая ошибка. " + e.Message);
            //}
            //catch (SyntaxException e)
            //{
            //    Console.WriteLine("Синтаксическая ошибка. " + e.Message);
            //}
            //catch (SemanticException e)
            //{
            //    Console.WriteLine("Семантическая ошибка. " + e.Message);
            //}

            Console.ReadLine();
        }

    }
}
