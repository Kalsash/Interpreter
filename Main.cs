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
                    Console.WriteLine("Синтаксическое дерево построено");
                    var TypeChecker = new SemanticChecker();
                    parser.root.Eval(TypeChecker);
                    Console.WriteLine("Типы проверены!");
                    //var d = new Interpreter();
                   // parser.root.Eval(d);
                    var d = new VirtInterpreter();
                    parser.root.Eval(d);


                }

                //Optimiser op = new Optimiser(9);
                //double s;
                //int i;
                //bool c;
                //unsafe
                //{
                //    op.AddCommands(new ThreeAddress(2, &s, 0.0)); // s := 0.0
                //    op.AddCommands(new ThreeAddress(1, &i, 1)); // i := 1;
                //    op.AddCommands(new ThreeAddress(52, &c, &i, 100000000)); //c:= i <  100000000;
                //    op.AddCommands(new ThreeAddress(22, &c, 7)); //if c == false goto 8;
                //    op.AddCommands(new ThreeAddress(53, &s, 1.0,&i));//s += 1.0 / i;
                //    op.AddCommands(new ThreeAddress(54, &i, 1)); // i += 1;
                //    op.AddCommands(new ThreeAddress(52, &c, &i, 100000000)); // c := i <  100000000;
                //    op.AddCommands(new ThreeAddress(23, 2)); // goto 3;
                //    op.AddCommands(new ThreeAddress(0)); // exit
                //}
                //op.RunCommands();
                //stopwatch.Stop();
                Console.WriteLine("Time: " + stopwatch.ElapsedMilliseconds + " ms");
              //  Console.WriteLine(s);
                Console.WriteLine(SymbolTable.mem[0].d);
                // Console.WriteLine(SymbolTable.StrCommands);
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
