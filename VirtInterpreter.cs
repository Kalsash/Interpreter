
using ProgramTree;
using SimpleLang.Visitors;
using SimpleParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLang
{
    internal class VirtInterpreter : Visitor<Value>
    {
        Value[] mem = new Value[++SymbolTable.CommandsSize];
        public Dictionary<string, Var> Vars_Dict = SymbolTable.Vars;
        Optimiser op = new Optimiser(SymbolTable.CommandsSize);
        Value v = new Value(0);
        bool IsNum = false;

        public override Value VisitIdNode(IdNode id)
        {
            return SymbolTable.mem[Vars_Dict[id.Name].Index];
        }
        public override Value VisitIntNumNode(IntNumNode num)
        {
            IsNum = true;
            return new Value(num.Num);
        }
        public override Value VisitRealNumNode(RealNumNode num)
        {
            IsNum = true;
            return new Value(num.Num);
        }
        public override Value VisitBoolNumNode(BoolNumNode num)
        {
            IsNum = true;
            return new Value(num.Num);
        }
        public override Value VisitAssignNode(AssignNode a)
        {
            SymbolTable.IsRun = true;
            var TypeChecker = new SemanticChecker();
            var tname = a.Id.Eval(TypeChecker);
            var tval = a.Expr.Eval(TypeChecker);
            var idVal = a.Id.Eval(this);
            v = idVal;
            var ExprVal = a.Expr.Eval(this);
            IsNum = false;
            if (IsNum)
            {
                if (tname == Types.tint)
                {
                    unsafe
                    {

                        op.AddCommands(new ThreeAddress(1, idVal.pi, ExprVal.i, "aa","__"));
                        SymbolTable.CommandsCounter++;
                    }
                }
                if (tname == Types.tdouble)
                {
                    if (tval == Types.tdouble)
                    {
                        unsafe
                        {
                            op.AddCommands(new ThreeAddress(2, idVal.pd, ExprVal.d, "aa", "__"));
                            SymbolTable.CommandsCounter++;
                        }
                    }
                    else
                        unsafe
                        {
                            op.AddCommands(new ThreeAddress(56, idVal.pd, ExprVal.i, "aa", "__"));
                            SymbolTable.CommandsCounter++;
                        }
                }
                if (tname == Types.tbool)
                {
                    unsafe
                    {
                        op.AddCommands(new ThreeAddress(3, idVal.pb, ExprVal.b, "aa", "__"));
                        SymbolTable.CommandsCounter++;
                    }
                }
                IsNum = false;
            }
            else
            {
                if (tname == Types.tint && tval == Types.tint)
                {
                    unsafe
                    {

                        if (idVal.pi == ExprVal.pi)
                        {
                            SymbolTable.CommandsSize--;
                            op.Size--;
                            return new Value(0);
                        }
                        else
                        {
                            op.AddCommands(new ThreeAddress(4, idVal.pi, ExprVal.pi, "aa", "__"));
                            SymbolTable.CommandsCounter++;
                        }
                    }

                }
                if (tname == Types.tdouble && tval == Types.tdouble)
                {

                    unsafe
                    {
                        if (idVal.pd == ExprVal.pd)
                        {
                            SymbolTable.CommandsSize--;
                            op.Size--;
                            return new Value(0);
                        }
                        else
                        {
                            op.AddCommands(new ThreeAddress(5, idVal.pd, ExprVal.pd, "aa", "__"));
                            SymbolTable.CommandsCounter++;
                        }

                    }


                }
                if (tname == Types.tbool && tval == Types.tbool)
                {
                    unsafe { op.AddCommands(new ThreeAddress(6, idVal.pb, ExprVal.pb, "aa", "__")); }
                    SymbolTable.CommandsCounter++;
                }

                if (op.c == SymbolTable.CommandsSize - 1)
                {
                    op.AddCommands(new ThreeAddress(0));
                    op.RunCommands();
                }
            }
            return new Value(0);
        }

        public override Value VisitBinOpNode(BinOpNode binop)
        {

            var TypeChecker = new SemanticChecker();
            var val1 = binop.Left.Eval(this);
            var c1 = IsNum;
            var t1 = binop.Left.Eval(TypeChecker);
           var val2 = binop.Right.Eval(this);
            var c2 = IsNum;
            var t2 = binop.Right.Eval(TypeChecker);
            IsNum = false;
            var ti = new Value(0);
            var td = new Value(0.0);
            var tb = new Value(false);

            string operation = "__";
            string assign = "aa";
            c1 = false;
            c2 = false;
            switch (binop.Op)
            {
                case '+':
                    operation = "op";
                    assign = "ap";
                    break;
                case '-':
                    operation = "on";
                    assign = "an";
                    break;
                case '*':
                    operation = "om";
                    assign = "am";
                    break;
                case '/':
                    operation = "od";
                    assign = "ad";
                    break;
                case '<':
                    operation = "ol";
                    break;
                case '>':
                    operation = "ob";
                    break;
                case '=':
                    operation = "oe";
                    break;
                case '!':
                    operation = "ow";
                    break;
                case '|':
                    operation = "oo";
                    break;
                case '&':
                    operation = "oa";
                    break;
                default:
                    break;
            }
            if (c1 == true)
            {
                if (t1 == SimpleParser.Types.tdouble && t2 == SimpleParser.Types.tint)
                {
                    switch (binop.Op)
                    {
                        case '/':
                            unsafe
                            {
                                op.AddCommands(new ThreeAddress(14, td.pd, val1.d, val2.pi,"aa",operation));
                            }
                            SymbolTable.CommandsCounter++;
                            return td;

                    }
                }
            }

            if (c2 == true)
            {
                if (t1 == SimpleParser.Types.tint && t2 == SimpleParser.Types.tint)
                {
                            unsafe
                            {
                                if (val1.pi == v.pi)
                                {
                                    op.AddCommands(new ThreeAddress(54, val1.pi, val2.i, assign, operation));
                                    SymbolTable.CommandsCounter++;
                                    return val1;
                                }
                                op.AddCommands(new ThreeAddress(15, ti.pi, val1.pi, val2.i, "aa", operation));
                        SymbolTable.CommandsCounter++;
                                return ti;
                            }

                }
            }
            if (t1 == SimpleParser.Types.tbool && t2 == SimpleParser.Types.tbool)
            {
                        unsafe
                        {
                            op.AddCommands(new ThreeAddress(57, tb.pb, val1.pb, val2.pb, "aa", operation));
                }
                        SymbolTable.CommandsCounter++;
                        return tb;
            }
            else
            {
                if (t1 == SimpleParser.Types.tint && t2 == SimpleParser.Types.tint)
                {
                    if (assign != "aa")
                    {
                        unsafe
                        {
                            if (val1.pi == v.pi)
                            {
                                op.AddCommands(new ThreeAddress(50, val1.pi, val2.pi, assign, operation));
                                SymbolTable.CommandsCounter++;
                                return val1;
                            }
                            op.AddCommands(new ThreeAddress(8, ti.pi, val1.pi, val2.pi, "aa", operation));
                            SymbolTable.CommandsCounter++;
                            return ti;
                        }
                    }
                    else
                    {
                        unsafe { op.AddCommands(new ThreeAddress(20, tb.pb, val1.pi, val2.pi, assign, operation));
                        }
                        SymbolTable.CommandsCounter++;
                        return tb;
                    }
                }
                if (t1 == SimpleParser.Types.tdouble && t2 == SimpleParser.Types.tdouble)
                {
                    if (assign != "aa")
                    {
                        unsafe
                        {
                            if (val1.pd == v.pd)
                            {
                                op.AddCommands(new ThreeAddress(51, val1.pd, val2.pd, assign, operation));
                                SymbolTable.CommandsCounter++;
                                return val1;
                            }
                            if (val2.pd == v.pd)
                            {
                                op.AddCommands(new ThreeAddress(151, val2.pd, val1.pd, assign, operation));
                                SymbolTable.CommandsCounter++;
                                return val2;
                            }
                            op.AddCommands(new ThreeAddress(9, td.pd, val1.pd, val2.pd, "aa", operation));
                            SymbolTable.CommandsCounter++;
                            return td;
                        }
                    }
                    else
                    {
                        unsafe { op.AddCommands(new ThreeAddress(34, tb.pb, val1.pd, val2.pd, assign, operation));
                        }
                        SymbolTable.CommandsCounter++;
                        return tb;
                    }
                }

                if (t1 == SimpleParser.Types.tdouble && t2 == SimpleParser.Types.tint)
                {
                    if (assign != "aa")
                    {
                        unsafe
                        {
                            if (val1.pd == v.pd)
                            {
                                op.AddCommands(new ThreeAddress(65, val1.pd, val2.pi, assign, operation));
                                SymbolTable.CommandsCounter++;
                                return val1;
                            }
                            if (val2.pd == v.pd)
                            {
                                op.AddCommands(new ThreeAddress(165, val2.pd, val1.pi, assign, operation));
                                SymbolTable.CommandsCounter++;
                                return val2;
                            }

                            op.AddCommands(new ThreeAddress(10, td.pd, val1.pd, val2.pi, "aa", operation));
                            SymbolTable.CommandsCounter++;
                            return td;
                        }
                    }
                    else
                    {
                            unsafe { op.AddCommands(new ThreeAddress(40, tb.pb, val1.pd, val2.pi, assign, operation));
                        }
                        SymbolTable.CommandsCounter++;
                        return tb;
                    }
                }

                if (t1 == SimpleParser.Types.tint && t2 == SimpleParser.Types.tdouble)
                {
                    if (assign != "aa")
                    {
                            unsafe 
                        {
                            if (val1.pd == v.pd)
                            {
                                op.AddCommands(new ThreeAddress(265, val1.pi, val2.pd, assign, operation));
                                SymbolTable.CommandsCounter++;
                                return val1;
                            }
                            if (val2.pd == v.pd)
                            {
                                op.AddCommands(new ThreeAddress(365, val2.pi, val1.pd, assign, operation));
                                SymbolTable.CommandsCounter++;
                                return val2;
                            }
                            op.AddCommands(new ThreeAddress(10, td.pd, val2.pd, val1.pi, "aa", operation));
                            SymbolTable.CommandsCounter++;
                            return td;
                        }

                    }
                    else
                    {
                        unsafe { op.AddCommands(new ThreeAddress(47, tb.pb, val1.pi, val2.pd, assign, operation));
                        }
                        SymbolTable.CommandsCounter++;
                        return tb;
                    }
                }

            }



            return new Value(0);
        }
        public override Value VisitIfNode(IfNode f)
        {

            var val = f.Expr.Eval(this);
            int first = SymbolTable.CommandsCounter;
            unsafe { op.AddCommands(new ThreeAddress(22, val.pb)); }
            SymbolTable.CommandsCounter++;
            f.Stat.Eval(this);
            op.Commands[first].Goto = SymbolTable.CommandsCounter;
            if (op.c == SymbolTable.CommandsSize - 1)
            {
                op.AddCommands(new ThreeAddress(0));
                op.RunCommands();
            }
            return new Value(0);
        }

        public override Value VisitWhileNode(WhileNode w)
        {
            var val = w.Expr.Eval(this);
            int first = SymbolTable.CommandsCounter;
            unsafe { op.AddCommands(new ThreeAddress(22, val.pb)); }
            SymbolTable.CommandsCounter++;
            w.Stat.Eval(this);
            op.Commands[first].Goto = SymbolTable.CommandsCounter;
            op.AddCommands(new ThreeAddress(23));
            op.Commands[SymbolTable.CommandsCounter++].Goto = first - 1;
            if (op.c == SymbolTable.CommandsSize - 1)
            {
                op.AddCommands(new ThreeAddress(0));
                op.RunCommands();
                //op.PrintCommands();
            }
            return new Value(0);
        }


        public override Value VisitBlockNode(BlockNode bl)
        {
            foreach (var st in bl.StList)
                st.Eval(this);
            return new Value(0);
        }
        unsafe public override Value VisitPrintNode(PrintNode p)
        {
            var TypeChecker = new SemanticChecker();
            var tval = p.Expr.Eval(TypeChecker);
            var val = p.Expr.Eval(this);
            if (tval == Types.tint)
            {
                op.AddCommands(new ThreeAddress(17, val.pi));
                op.Commands[SymbolTable.CommandsCounter].Tok = Toks.printint;

            }
            if (tval == Types.tdouble)
            {
                op.AddCommands(new ThreeAddress(18, val.pd));
                op.Commands[SymbolTable.CommandsCounter].Tok = Toks.printdouble;
            }
            if (tval == Types.tbool)
            {
                op.AddCommands(new ThreeAddress(19, val.pb));
                op.Commands[SymbolTable.CommandsCounter].Tok = Toks.printbool;
            }
            if (SymbolTable.CommandsCounter >= 2)
            {
                if (op.Commands[SymbolTable.CommandsCounter - 2].Tok == Toks.iff ||
                    op.Commands[SymbolTable.CommandsCounter - 2].Tok == Toks.got)
                {
                    return new Value(0);
                }
            }
            if (op.c == SymbolTable.CommandsSize - 1)
            {
                op.AddCommands(new ThreeAddress(0));
                op.RunCommands();
                //op.PrintCommands();
            }
            return new Value(0);
        }

    }
}