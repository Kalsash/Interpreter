using ProgramTree;
using SimpleScanner;
using System;
using System.Collections.Generic;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace SimpleParser
{
    public enum Types { tint, tdouble, tvoid };
    public class Var
    {
         Types type;
         object value;
       public Var(Types type, object value)
        {
            this.type = type;
            this.value = value;
        }
        public Types Type
        {
            get => type;
            set => type = value;
        }
        public object Value
        {
            get => value;
            set => this.value = value;
        }
    }
    public static class SymbolTable // Таблица символов
    {
        public static Dictionary<string, Var> Vars = new Dictionary<string, Var>(); // таблица символов
        public static void NewVarDef(string name, Var v)
        {
            if (Vars.ContainsKey(name))
            {
                if (Vars[name].Type != v.Type)
                {

                    if (Vars[name].Type == Types.tint)
                    {
                        Dictionary<int, String> assign_dict = new Dictionary<int, String>(); // таблица символов
                        string FileName = @"..\..\a.txt";
                        string Text = File.ReadAllText(FileName);
                        Scanner scanner = new Scanner();
                        scanner.SetSource(Text, 0);
                        int tok = 0;
                        int k = 0;
                        do
                        {
                            tok = scanner.yylex();
                            
                            if (tok == (int)Tokens.ASSIGN)
                            {
                                assign_dict.Add(k,scanner.PosColumn());
                                k++;
                            }
                        } while (tok != (int)Tokens.EOF);
 
                        throw new SemanticException(assign_dict[3] + "Нельзя типу int присвоить тип double!");
                    }
                    else
                    {
                        Vars[name].Type = Types.tdouble;
                    }
                }
            }
            else Vars.Add(name, v);
        }
        public static void SetValue(string name, object ob)
        {
            if (Vars.ContainsKey(name))
            {
                Vars[name].Value = ob;
            }
        }
    }
    public class LexException : Exception
    {
        public LexException(string msg) : base(msg) { }
    }
    public class SyntaxException : Exception
    {
        public SyntaxException(string msg) : base(msg) { }
    }
    public class SemanticException : Exception
    {
        public SemanticException(string msg) : base(msg) { }
    }
    // Класс глобальных описаний и статических методов
    // для использования различными подсистемами парсера и сканера
    public static class ParserHelper
    {
    }
}