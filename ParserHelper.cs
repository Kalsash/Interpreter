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
        int index = 0;
       public Var(Types type, object value, int index)
        {
            this.type = type;
            this.value = value;
            this.index = index;
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
        public int Index
        {
            get => index;
            set => index = value;
        }
    }


    public static class SymbolTable // Таблица символов
    {

        public static  Value[]  mem = new Value[100];
        unsafe public static int MemSize = 0;
        unsafe public static int MemCounter = 0;
        unsafe public static int CommandsSize = 0;
        unsafe public static int CommandsCounter = 0;
        unsafe public static Dictionary<string, Var> Vars = new Dictionary<string, Var>(); // таблица символов
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
            else
            {
                Vars.Add(name, v);

                int c1 = 0;
                int c2 = 0;
                if (v.Type == Types.tint)
                {
                    if (MemSize == 0)
                    {
                        mem[MemSize++] = new Value(c1);
                    }
                    else
                        mem[MemSize++] = new Value(c2);
                }
                double a1 = 0.0;
                double a2 = 0.0;
                if (v.Type == Types.tdouble)
                {
                    if (MemSize == 0)
                    {
                        mem[MemSize++] = new Value(a1);
                    }
                    else
                    mem[MemSize++] = new Value(a2);
                }
                bool b1 = false;
                bool b2 = false;
                if (v.Type == Types.tbool)
                {
                    if (MemSize == 0)
                    {
                        mem[MemSize++] = new Value(b1);
                    }
                    else
                        mem[MemSize++] = new Value(b2);
                }
                if (v.Type == Types.tvoid)
                {
                    throw new SemanticException(string.Format("({0},{1}):" +
                             " Нельзя переменной присвоить тип void", line, col));
                }

            }
               
        }
  
        public static void SetValue(string name, object ob)
        {
            if (Vars.ContainsKey(name))
            {
                Vars[name].Value = ob;
            }
        }
    }




    public unsafe class Value
    {
        public int i = 0;
        public double d = 0.0;
        public bool b = false;

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