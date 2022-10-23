using System;
using System.Collections.Generic;

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
                        throw new Exception("Нельзя типу int присвоить double!");
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
    // Класс глобальных описаний и статических методов
    // для использования различными подсистемами парсера и сканера
    public static class ParserHelper
    {
    }
}