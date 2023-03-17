using System;
using System.IO;
using System.Collections.Generic;
using SimpleScanner;
using SimpleParser;
using SimpleLang;
using System.Diagnostics;

namespace SimpleCompiler
{
    public class Value
    {
        public int i = 0;
        public double d;
        public bool b;
        unsafe public int* pi { get; set; }
        unsafe public double* pd { get; set; }
        unsafe public bool* pb { get; set; }
        public Value(int x)
        {
            i = x;
            unsafe { pi = &x; }
    
        }
        public Value(double x)
        {
            d = x;
            unsafe { pd = &x; }
        }
        public Value(bool x)
        {
            b = x;
            unsafe { pb = &x; }
        }

    }
    public class SimpleCompilerMain
    {


        public static void Main()
        {
            //Console.WriteLine("Begin");
            //Stopwatch stopwatch = new Stopwatch();

            //stopwatch.Start();
            //double s = 5.2;
            //int n = 0;
            //int i = 0;
            //bool flag = false;
            //double t = 0;
            //Value[] mem = new Value[5];
            //mem[0] = new Value(s);
            //mem[1] = new Value(n);
            //mem[2] = new Value(i);
            //mem[3] = new Value(flag);
            //mem[4] = new Value(t);

            //Optimiser op = new Optimiser(12);
            //op.AddCommands(new ThreeAddress(2));
            //op.AddCommands(new ThreeAddress(1));
            //op.AddCommands(new ThreeAddress(1));
            //op.AddCommands(new ThreeAddress(3));
            //op.AddCommands(new ThreeAddress(13));
            //op.AddCommands(new ThreeAddress(14));
            //op.AddCommands(new ThreeAddress(9));
            //op.AddCommands(new ThreeAddress(15));
            //op.AddCommands(new ThreeAddress(16));
            //op.AddCommands(new ThreeAddress(12));
            //op.AddCommands(new ThreeAddress(18));
            //op.AddCommands(new ThreeAddress(0));
            //unsafe
            //{
            //    op.Commands[0].pda = mem[0].pd; // s
            //    op.Commands[0].doubleVal = 0.0; // s = 0.0
            //    op.Commands[1].pia = mem[1].pi; // n
            //    op.Commands[1].intVal = 100000000; // n = 10000000
            //    op.Commands[2].pia = mem[2].pi;
            //    op.Commands[2].intVal = 1; // i = 1
            //    op.Commands[3].pba = mem[3].pb;
            //    op.Commands[3].boolVal = false; // b = false
            //    op.Commands[4].pba = op.Commands[3].pba;
            //    op.Commands[4].Goto = 11;  // if i >= n goto 
            //    op.Commands[5].pda = mem[4].pd;
            //    op.Commands[5].doubleVal = 1.0; // t = 1.0 / i
            //    op.Commands[5].pib = op.Commands[2].pia;
            //    op.Commands[6].pda = op.Commands[0].pda;
            //    op.Commands[6].pdb = op.Commands[0].pda;
            //    op.Commands[6].pdc = op.Commands[5].pda; // s = s + t
            //    op.Commands[7].pia = op.Commands[2].pia;
            //    op.Commands[7].pib = op.Commands[2].pia;
            //    op.Commands[7].intVal = 1; // i = i + 1
            //    op.Commands[8].pba = op.Commands[3].pba;
            //    op.Commands[8].pib = op.Commands[2].pia;
            //    op.Commands[8].pic = op.Commands[1].pia;
            //    op.Commands[9].Goto = 5;


            //}
            //op.RunCommands();

            //stopwatch.Stop();
            //Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds + " ms");
            //double temp = 0;
            //unsafe { temp = *mem[0].pd; };
            //Console.WriteLine("Value: " + temp);
            //Console.WriteLine("End");

            Console.WriteLine("Start");
            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            string FileName = @"..\..\a.txt";
            try
            {
                string Text = File.ReadAllText(FileName);

                Scanner scanner = new Scanner();
                scanner.SetSource(Text, 0);

                Parser parser = new Parser(scanner);

                var b = parser.Parse();
                if (!b)
                    Console.WriteLine("Ошибка");
                else
                {
                  //  Console.WriteLine("Синтаксическое дерево построено");
                    var TypeChecker = new SemanticChecker();
                    parser.root.Eval(TypeChecker);
                   // Console.WriteLine("Типы проверены!");
                    //var d = new Interpreter();
                    //parser.root.Eval(d);
                    var d = new VirtInterpreter();
                    parser.root.Eval(d);

                }
                stopwatch.Stop();
                Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds + " ms");
                Console.WriteLine(SymbolTable.mem[0].d);
                Console.WriteLine("Программа завершена");
            }

            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл {0} не найден", FileName);
            }
            catch (LexException e)
            {
                Console.WriteLine("Лексическая ошибка. " + e.Message);
            }
            catch (SyntaxException e)
            {
                Console.WriteLine("Синтаксическая ошибка. " + e.Message);
            }
            catch (SemanticException e)
            {
                Console.WriteLine("Семантическая ошибка. " + e.Message);
            }

            Console.ReadLine();
        }

    }
}
