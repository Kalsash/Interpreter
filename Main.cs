using System;
using System.IO;
using System.Collections.Generic;
using SimpleScanner;
using SimpleParser;
using SimpleLang;

namespace SimpleCompiler
{
    public class SimpleCompilerMain
    {
        public static void Main()
        {
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
