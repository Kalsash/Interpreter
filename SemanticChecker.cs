using ProgramTree;
using SimpleLang.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleParser;

namespace SimpleLang
{
    
    class SemanticChecker:Visitor<SimpleParser.Types>
    {
        Dictionary<string, Var> Vars_Dict = SymbolTable.Vars;
        public override SimpleParser.Types VisitIdNode(IdNode id)
        {
            if (Vars_Dict.ContainsKey(id.Name))
                return Vars_Dict[id.Name].Type;
            return SimpleParser.Types.tvoid;
        }
        public override SimpleParser.Types VisitIntNumNode(IntNumNode num)
        {
            return SimpleParser.Types.tint;
        }
        public override SimpleParser.Types VisitRealNumNode(RealNumNode num)
        {          
            return SimpleParser.Types.tdouble;
        }
        public override SimpleParser.Types VisitBoolNumNode(BoolNumNode num)
        {
            return SimpleParser.Types.tbool;
        }
        public override SimpleParser.Types VisitFuncNode(FuncNode f)
        {

            switch (f.Name)
            {
                case "@pi":
                    if (f.EList.ExList.Count != 0)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
          " Неверное количество параметров функции {2} ", f.lx.StartLine, f.lx.StartColumn + 5, f.Name));
                    }
                    return Types.tdouble;
                case "@e":
                    if (f.EList.ExList.Count != 0)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
          " Неверное количество параметров функции {2} ", f.lx.StartLine, f.lx.StartColumn + 5, f.Name));
                    }
                    return Types.tdouble;
                case "@sin":
                    if (f.EList.ExList.Count != 1)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
          " Неверное количество параметров функции {2} ", f.lx.StartLine, f.lx.StartColumn + 5, f.Name));
                    }
                    if (f.EList.ExList.First().Eval(this) != Types.tdouble && f.EList.ExList.First().Eval(this) != Types.tint)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
          " Неверный аргумент функции {2} ", f.lx.StartLine, f.lx.StartColumn + 5, f.Name));
                    }
                    return Types.tdouble;
                case "@cos":
                    if (f.EList.ExList.Count != 1)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
          " Неверное количество параметров функции {2} ", f.lx.StartLine, f.lx.StartColumn + 5, f.Name));
                    }
                    if (f.EList.ExList.First().Eval(this) != Types.tdouble && f.EList.ExList.First().Eval(this) != Types.tint)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
          " Неверный аргумент функции {2} ", f.lx.StartLine, f.lx.StartColumn + 5, f.Name));
                    }
                    return Types.tdouble;
                case "@tan":
                    if (f.EList.ExList.Count != 1)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
          " Неверное количество параметров функции {2} ", f.lx.StartLine, f.lx.StartColumn + 5, f.Name));
                    }
                    if (f.EList.ExList.First().Eval(this) != Types.tdouble && f.EList.ExList.First().Eval(this) != Types.tint)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
          " Неверный аргумент функции {2} ", f.lx.StartLine, f.lx.StartColumn + 5, f.Name));
                    }
                    return Types.tdouble;
                case "@sqrt":
                    if (f.EList.ExList.Count != 1)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
          " Неверное количество параметров функции {2} ", f.lx.StartLine, f.lx.StartColumn + 5, f.Name));
                    }
                    if (f.EList.ExList.First().Eval(this) != Types.tdouble && f.EList.ExList.First().Eval(this) != Types.tint)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
          " Неверный аргумент функции {2} ", f.lx.StartLine, f.lx.StartColumn + 5, f.Name));
                    }
                    f.Val = new RunTimeValue(Math.Sqrt(double.Parse(f.EList.ExList.First().Eval(this).ToString())));
                    return f.Val.tt;
                case "@max":
                    if (f.EList.ExList.Count != 2)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
          " Неверное количество параметров функции {2} ", f.lx.StartLine, f.lx.StartColumn + 5, f.Name));
                    }
                    //double[] arr = new double[2];
                    //int k = 0;
                    //double x = 0;
                    //foreach (var ex in f.EList.ExList)
                    //{

                    //    x = double.Parse(ex.Eval(this).ToString());
                    //    arr[k] = x;
                    //    k++;
                    //}
                    //f.Val = new RunTimeValue(Math.Max(arr[0], arr[1]));
                    //return f.Val.tt;
                    return Types.tdouble;
                case "@min":
                    if (f.EList.ExList.Count != 2)
                    {
                        throw new SemanticException(string.Format("({0},{1}):" +
          " Неверное количество параметров функции {2} ", f.lx.StartLine, f.lx.StartColumn + 5, f.Name));
                    }
                    double[] arr2 = new double[2];
                    int k2 = 0;
                    double x2 = 0;
                    foreach (var ex in f.EList.ExList)
                    {

                        x2 = double.Parse(ex.Eval(this).ToString());
                        arr2[k2] = x2;
                        k2++;
                    }
                    f.Val = new RunTimeValue(Math.Min(arr2[0], arr2[1]));
                    return f.Val.tt;


                default:
                    throw new SemanticException(string.Format("({0},{1}):" +
                              " Неизвестное имя функции {2} ", f.lx.StartLine, f.lx.StartColumn + 5, f.Name));
            }
            
        }
        public override SimpleParser.Types VisitBinOpNode(BinOpNode binop)
        {
           var t1 = binop.Left.Eval(this);
           var t2 = binop.Right.Eval(this);
            if (t1 == Types.tvoid || t2 == Types.tvoid)
            {
                throw new SemanticException(string.Format("({0},{1}):" +
              " Неизвестное имя переменной ", binop.lx.StartLine, binop.lx.EndColumn-1));
            }

            if (t1 == t2)
            {
                if (binop.Op == '>' || binop.Op == '<' || binop.Op == '=')
                {
                    return Types.tbool;
                }
                return t1;
            }
            else
            {

                if (t1 == Types.tbool || t2 == Types.tbool)
                {
                    throw new SemanticException(string.Format("({0},{1}):" +
                  " Для типов {2} и {3} операция '{4}' не применима",
                  binop.lx.StartLine, binop.lx.EndColumn - 1, t1, t2, binop.Op));
                }
                if (binop.Op == '>' || binop.Op == '<' || binop.Op == '=')
                {
                    return Types.tbool;
                }
                return SimpleParser.Types.tdouble;
            }
        }
        public override SimpleParser.Types VisitAssignNode(AssignNode a)
        {
            if (a.Expr.Eval(this) == SimpleParser.Types.tvoid)
            {
                throw new SemanticException(string.Format("({0},{1}):" +
              " Переменной присвоено неверное значение ", a.lx.StartLine, a.lx.StartColumn + 2));
            }
            Var v = new Var(a.Expr.Eval(this), 0);
            SymbolTable.NewVarDef(a.Id.Name, v,a.lx.StartLine,a.lx.EndColumn-1);
            return SimpleParser.Types.tvoid;
        }
        public override SimpleParser.Types VisitLoopNode(LoopNode l)
        {
            if (l.Expr.Eval(this) != SimpleParser.Types.tint)
            {
                throw new SemanticException(string.Format("({0},{1}):" +
                              "Loop должен принимать выражения типа int!", l.lx.StartLine, l.lx.StartColumn + 5));
            }
            return SimpleParser.Types.tvoid;
        }
        public override SimpleParser.Types VisitIfNode(IfNode f)
        {
            if (f.Expr.Eval(this) != SimpleParser.Types.tbool)
            {
                throw new SemanticException(string.Format("({0},{1}):" +
                              "If должен принимать логические высказывания!", f.lx.StartLine, f.lx.StartColumn + 5));
            }
            return SimpleParser.Types.tvoid;
        }
        public override SimpleParser.Types VisitWhileNode(WhileNode w)
        {
            if (w.Expr.Eval(this) != SimpleParser.Types.tbool)
            {
                throw new SemanticException(string.Format("({0},{1}):" +
                              "While должен принимать логические высказывания!", w.lx.StartLine, w.lx.StartColumn + 5));
            }
            return SimpleParser.Types.tvoid;
        }
        public override SimpleParser.Types VisitBlockNode(BlockNode bl)
        {
            foreach (var st in bl.StList)
                st.Eval(this);
            return SimpleParser.Types.tvoid;
        }
        public override SimpleParser.Types VisitPrintNode(PrintNode p)
        {
            var val = p.Expr.Eval(this);
            if (val == Types.tvoid)
            {
                throw new SemanticException(string.Format("({0},{1}):" +
                              " Неизвестное имя переменной ", p.lx.StartLine, p.lx.StartColumn+6));
            }
            return SimpleParser.Types.tvoid;
        }
    }
}
