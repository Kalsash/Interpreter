﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ProgramTree;
using SimpleLang.Visitors;
using static System.Net.Mime.MediaTypeNames;
using SimpleParser;

namespace SimpleLang
{
    class Interpreter : Visitor<object>
    {
        public Dictionary<string, Var> Vars_Dict = SymbolTable.Vars;
        public int Loop_Counter = -1;
        public override object VisitIdNode(IdNode id)
        {
            if (Vars_Dict.ContainsKey(id.Name))
            {
                return Vars_Dict[id.Name].Value;
            }
            return 0;
        }
        public override object VisitIntNumNode(IntNumNode num)
        {
            return num.Num;
        }
        public override object VisitRealNumNode(RealNumNode num)
        {
            return num.Num;
        }
        public override object VisitBoolNumNode(BoolNumNode num)
        {
            return num.Num;
        }
        public override object VisitFuncNode(FuncNode f)
        {
  
                if (f.Name.StartsWith("@pi"))
                {
                    f.Val = new RunTimeValue(Math.PI);
                }
            if (f.Name.StartsWith("@sin"))
            {
                f.Val = new RunTimeValue(Math.Sqrt(double.Parse(f.ExprList.First().Eval(this).ToString())));
            }
            if (f.Name.StartsWith("@cos"))
            {
                f.Val = new RunTimeValue(Math.Sqrt(double.Parse(f.ExprList.First().Eval(this).ToString())));
            }
            if (f.Name.StartsWith("@tan"))
            {
                f.Val = new RunTimeValue(Math.Sqrt(double.Parse(f.ExprList.First().Eval(this).ToString())));
            }
            if (f.Name.StartsWith("@sqrt"))
            {
                f.Val = new RunTimeValue(Math.Sqrt(double.Parse(f.ExprList.First().Eval(this).ToString())));
            }

                if (f.Name.StartsWith("@max"))
                {
                double[] arr = new double[2];
                int k = 0;
                double x = 0;
                foreach (var ex in f.ExprList)
                {

                    x = double.Parse(ex.Eval(this).ToString());
                    arr[k] = x;
                    k++;
                }
                f.Val = new RunTimeValue(Math.Max(arr[0], arr[1]));
                }

            return f.Val.Value();
        }

        public override object VisitBinOpNode(BinOpNode binop)
        {
            var TypeChecker = new SemanticChecker();
          var val1 =  binop.Left.Eval(this);
           var t1 = binop.Left.Eval(TypeChecker);
           var val2 = binop.Right.Eval(this);
           var t2 = binop.Right.Eval(TypeChecker);
            if (t1 == SimpleParser.Types.tbool && t2 == SimpleParser.Types.tbool)
            {
                switch (binop.Op)
                {
                    case '&':
                        return bool.Parse(string.Format("{0}", val1)) && bool.Parse(string.Format("{0}", val2));
                    case '|':
                        return bool.Parse(string.Format("{0}", val1)) || bool.Parse(string.Format("{0}", val2));
                }
            }
            else
            {
                if (t1 == SimpleParser.Types.tint && t2 == SimpleParser.Types.tint)
                {
                    switch (binop.Op)
                    {
                        case '+':
                            //return (int)val1 + (int)val2;
                            return int.Parse(string.Format("{0}", val1)) +int.Parse(string.Format("{0}", val2));
                        case '-':
                            return int.Parse(string.Format("{0}", val1)) - int.Parse(string.Format("{0}", val2));
                        case '*':
                            return int.Parse(string.Format("{0}", val1)) * int.Parse(string.Format("{0}", val2));
                        case '/':
                            return int.Parse(string.Format("{0}", val1)) / int.Parse(string.Format("{0}", val2));
                        case '>':
                            return int.Parse(string.Format("{0}", val1)) > int.Parse(string.Format("{0}", val2));
                        case '<':
                            return int.Parse(string.Format("{0}", val1)) < int.Parse(string.Format("{0}", val2));
                        case '=':
                            return int.Parse(string.Format("{0}", val1)) == int.Parse(string.Format("{0}", val2));
                        case '!':
                            return int.Parse(string.Format("{0}", val1)) != int.Parse(string.Format("{0}", val2));
                    }
                }
                else
                {
                    switch (binop.Op)
                    {
                        case '+':
                            return double.Parse(string.Format("{0}", val1)) + double.Parse(string.Format("{0}", val2));
                        case '-':
                            return double.Parse(string.Format("{0}", val1)) - double.Parse(string.Format("{0}", val2));
                        case '*':
                            return double.Parse(string.Format("{0}", val1)) * double.Parse(string.Format("{0}", val2));
                        case '/':
                            return double.Parse(string.Format("{0}", val1)) / double.Parse(string.Format("{0}", val2));
                        case '>':
                            return double.Parse(string.Format("{0}", val1)) > double.Parse(string.Format("{0}", val2));
                        case '<':
                            return double.Parse(string.Format("{0}", val1)) < double.Parse(string.Format("{0}", val2));
                        case '=':
                            return double.Parse(string.Format("{0}", val1)) == double.Parse(string.Format("{0}", val2));
                        case '!':
                            return double.Parse(string.Format("{0}", val1)) != double.Parse(string.Format("{0}", val2));
                    }
                }
            }
           

      

            return 0;
        }

        public override object VisitAssignNode(AssignNode a)
        {
            a.Id.Eval(this);
            var val = a.Expr.Eval(this);
           SymbolTable.SetValue(a.Id.Name, val);
            return 0;
        }
        public override object VisitLoopNode(LoopNode l)
        {
            var val = l.Expr.Eval(this);
            if (Loop_Counter == -1)
            {
                Loop_Counter = int.Parse(string.Format("{0}", val));
            }
            if (Loop_Counter > 0)
            {
                Loop_Counter--;
                l.Stat.Eval(this);
                VisitLoopNode(l);
            }
            Loop_Counter = -1;
            return 0;
        }
        public override object VisitWhileNode(WhileNode w)
        {
            bool val = bool.Parse(w.Expr.Eval(this).ToString());
            if (val == true)
            {
                w.Stat.Eval(this);
                VisitWhileNode(w);
            }
            return 0;
        }
        public override object VisitIfNode(IfNode f)
        {
            bool val = bool.Parse(f.Expr.Eval(this).ToString());
            if (val == true)
            {
                f.Stat.Eval(this);
            }
            return 0;
        }
        public override object VisitBlockNode(BlockNode bl)
        {
            foreach (var st in bl.StList)
                st.Eval(this);
            return 0;
        }
        public override object VisitPrintNode(PrintNode p)
        {
            var val = p.Expr.Eval(this);
            Console.WriteLine(val);
            return val;
        }
    }
}
