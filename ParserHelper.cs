using ProgramTree;
using SimpleScanner;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Principal;
using static System.Net.Mime.MediaTypeNames;

namespace SimpleParser
{
    public class ExprList
    {
        public List<ExprNode> ExList = new List<ExprNode>();
        public ExprList()
        {
        }
        public ExprList(ExprNode Exp)
        {
            ExList.Add(Exp);
        }
        public void Add(ExprNode e)
        {
            ExList.Add(e);
        }
    }
    public enum Types { tint, tdouble, tbool, tvoid };
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
        public static void NewVarDef(string name, Var v, int line, int col)
        {
            if (Vars.ContainsKey(name))
            {
                if (Vars[name].Type != v.Type)
                {
                    if (v.Type == Types.tbool)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
                           " Нельзя типу {2} присвоить тип tbool!", line, col, Vars[name].Type));
                    }
                    if (v.Type == Types.tvoid)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
                           " Нельзя типу {2} присвоить тип tvoid!", line, col, Vars[name].Type));
                    }
                    if (Vars[name].Type == Types.tint)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
                            " Нельзя типу int присвоить тип double!", line, col));
                    }
                    Vars[name].Type = Types.tdouble;

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

    public class RunTimeValue
    {
        public int i;
        public double d;
        public bool b;
        public Types tt = Types.tvoid;
        public RunTimeValue(int x)
        {
           i = x;
            tt = Types.tint;
        }
        public RunTimeValue(double x)
        {
            d = x;
            tt = Types.tdouble;
        }
        public RunTimeValue(bool x)
        {
            b = x;
            tt = Types.tbool;
        }

        public object Value()
        {
            if (tt == Types.tint)
            {
                return i;
            }
            if (tt == Types.tdouble)
            {
                return d;
            }
            if (tt == Types.tbool)
            {
                return b;
            }
            return 0;
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