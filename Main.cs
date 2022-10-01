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
        public class SymbolTable // Таблица символов
        {
            public  Dictionary<string, int> vars = new Dictionary<string, int>(); // таблица символов
            public  void NewVarDef(string name, int val)
            {
                if (vars.ContainsKey(name))
                    vars[name] = val;
                else vars.Add(name, val);
            }
        }
        public static void Main()
        {
            string FileName = @"..\..\a.txt";
            try
            {
                string Text = File.ReadAllText(FileName);

                Scanner scanner = new Scanner();
                scanner.SetSource(Text, 0);

                Parser parser = new Parser(scanner);

               // var Id_Dict = new Dictionary<string, int>();
                var b = parser.Parse();
                if (!b)
                    Console.WriteLine("Ошибка");
                else
                {
                    Console.WriteLine("Синтаксическое дерево построено");
                    //foreach (var st in parser.root.StList)
                    //Console.WriteLine(st);
                    //var n = new Nodes();
                    //parser.root.Nodes(n);
                    //foreach (var pair in n.Id_Dict)
                    //{
                    //    Console.WriteLine($"{pair.Key} = {pair.Value}");
                    //}
                    var dict = new SymbolTable();
                    dict = parser.root.Eval(dict);
                    foreach (var pair in dict.vars)
                    {
                        Console.WriteLine($"{pair.Key} = {pair.Value}");
                    }

                }
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

            Console.ReadLine();
        }

    }
}
