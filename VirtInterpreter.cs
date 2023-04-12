
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

        public override Value VisitIdNode(IdNode id)
        {
            return SymbolTable.mem[Vars_Dict[id.Name].Index];
        }
        public override Value VisitIntNumNode(IntNumNode num)
        {
            //unsafe { op.AddCommands(new ThreeAddress(1, v.pi, num.Num)); }
            //SymbolTable.CommandsCounter++;
            return new Value(num.Num);
            //return null;
        }
        public override Value VisitRealNumNode(RealNumNode num)
        {
            //unsafe { op.AddCommands(new ThreeAddress(2, v.pd, num.Num)); }
            //SymbolTable.CommandsCounter++;
             return new Value(num.Num);
            //return null;
        }
        public override Value VisitBoolNumNode(BoolNumNode num)
        {
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
            if (ExprVal != null)
                {
                if (tname == Types.tint && tval == Types.tint)
                {
                    unsafe { op.AddCommands(new ThreeAddress(4, idVal.pi, ExprVal.pi)); }
                    SymbolTable.CommandsCounter++;
                }
                if (tname == Types.tdouble && tval == Types.tdouble)
                {
                    unsafe { op.AddCommands(new ThreeAddress(5, idVal.pd, ExprVal.pd)); }
                    SymbolTable.CommandsCounter++;
                }
                if (tname == Types.tbool && tval == Types.tbool)
                {
                    unsafe { op.AddCommands(new ThreeAddress(6, idVal.pb, ExprVal.pb)); }
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
            var t1 = binop.Left.Eval(TypeChecker);
           var val2 = binop.Right.Eval(this);
            var t2 = binop.Right.Eval(TypeChecker);

            if (t1 == SimpleParser.Types.tbool && t2 == SimpleParser.Types.tbool)
            {
                switch (binop.Op)
                {
                    case '&':
                        return new Value(val1.b && val2.b);
                    case '|':
                        return new Value(val1.b || val2.b);
                }
            }
            else
            {
                if (t1 == SimpleParser.Types.tint && t2 == SimpleParser.Types.tint)
                {
                    var ti = new Value(0);
                    var tb = new Value(false);
                    switch (binop.Op)
                    {

                        case '+':
                            unsafe
                            {
                                if (val1.pi == v.pi)
                                {
                                    op.AddCommands(new ThreeAddress(50, val1.pi, val2.pi));
                                    SymbolTable.CommandsCounter++;
                                    //return val1;
                                    return null;
                                }
                            }
                            unsafe { op.AddCommands(new ThreeAddress(8, ti.pi, val1.pi, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            return ti;
                        case '-':
                            unsafe { op.AddCommands(new ThreeAddress(24, ti.pi, val1.pi, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            return ti;
                        case '*':
                            unsafe { op.AddCommands(new ThreeAddress(25, ti.pi, val1.pi, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            return ti;
                        case '/':
                            unsafe { op.AddCommands(new ThreeAddress(26, ti.pi, val1.pi, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            return ti;
                        case '>':
                            unsafe { op.AddCommands(new ThreeAddress(27, tb.pb, val1.pi, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            return tb;
                        case '<':
                            //unsafe { op.AddCommands(new ThreeAddress(20, tb.pb, val1.pi, val2.pi)); }
                            unsafe { op.AddCommands(new ThreeAddress(20, v.pb, val1.pi, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            //return tb;
                            return null;
                        case '=':
                            unsafe { op.AddCommands(new ThreeAddress(28, tb.pb, val1.pi, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            return tb;
                        case '!':
                            unsafe { op.AddCommands(new ThreeAddress(29, tb.pb, val1.pi, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            return tb;
                    }
                }
                if (t1 == SimpleParser.Types.tdouble && t2 == SimpleParser.Types.tdouble)
                {
                    var td = new Value(0.0);
                    var tb = new Value(false);
                    switch (binop.Op)
                    {
                        case '+':
                            unsafe
                            {

                                if (val1.pd == v.pd)
                                {
                                    op.AddCommands(new ThreeAddress(51, val1.pd, val2.pd));
                                    SymbolTable.CommandsCounter++;
                                    //return val1;
                                    return null;
                                }
                            }
                            unsafe { op.AddCommands(new ThreeAddress(9, td.pd, val1.pd, val2.pd)); }
                            SymbolTable.CommandsCounter++;
                            return td;
                        case '-':
                            unsafe { op.AddCommands(new ThreeAddress(30, td.pd, val1.pd, val2.pd)); }
                            SymbolTable.CommandsCounter++;
                            return td;
                        case '*':
                            unsafe { op.AddCommands(new ThreeAddress(31, td.pd, val1.pd, val2.pd)); }
                            SymbolTable.CommandsCounter++;
                            return td;
                        case '/':
                            unsafe { op.AddCommands(new ThreeAddress(32, td.pd, val1.pd, val2.pd)); }
                            SymbolTable.CommandsCounter++;
                            return td;
                        case '>':
                            unsafe { op.AddCommands(new ThreeAddress(33, tb.pb, val1.pd, val2.pd)); }
                            SymbolTable.CommandsCounter++;
                            return tb;
                        case '<':
                            unsafe { op.AddCommands(new ThreeAddress(34, tb.pb, val1.pd, val2.pd)); }
                            SymbolTable.CommandsCounter++;
                            return tb;
                        case '=':
                            unsafe { op.AddCommands(new ThreeAddress(35, tb.pb, val1.pd, val2.pd)); }
                            SymbolTable.CommandsCounter++;
                            return tb;
                        case '!':
                            unsafe { op.AddCommands(new ThreeAddress(36, tb.pb, val1.pd, val2.pd)); }
                            SymbolTable.CommandsCounter++;
                            return tb;
                    }
                }

                if (t1 == SimpleParser.Types.tdouble && t2 == SimpleParser.Types.tint)
                {
                    var td2 = new Value(0.0);
                    var tb = new Value(false);
                    switch (binop.Op)
                    {
                        case '+':
                            unsafe { op.AddCommands(new ThreeAddress(10, td2.pd, val1.pd, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            return td2;
                        case '-':
                            unsafe { op.AddCommands(new ThreeAddress(37, td2.pd, val1.pd, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            return td2;
                        case '*':
                            unsafe { op.AddCommands(new ThreeAddress(38, td2.pd, val1.pd, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            return td2;
                        case '/':
                            // unsafe { op.AddCommands(new ThreeAddress(21, td2.pd, val1.pd, val2.pi)); }
                            unsafe { op.AddCommands(new ThreeAddress(21, v.pd, val1.pd, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            //return td2;
                            return null;
                        case '>':
                            unsafe { op.AddCommands(new ThreeAddress(39, tb.pb, val1.pd, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            return tb;
                        case '<':
                            unsafe { op.AddCommands(new ThreeAddress(40, tb.pb, val1.pd, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            return tb;
                        case '=':
                            unsafe { op.AddCommands(new ThreeAddress(41, tb.pb, val1.pd, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            return tb;
                        case '!':
                            unsafe { op.AddCommands(new ThreeAddress(42, tb.pb, val1.pd, val2.pi)); }
                            SymbolTable.CommandsCounter++;
                            return tb;
                    }
                }

                if (t1 == SimpleParser.Types.tint && t2 == SimpleParser.Types.tdouble)
                {
                    var td3 = new Value(0.0);
                    var tb = new Value(false);
                    switch (binop.Op)
                    {
                        case '+':
                            unsafe { op.AddCommands(new ThreeAddress(10, td3.pd, val2.pd, val1.pi)); }
                            SymbolTable.CommandsCounter++;
                            return td3;
                        case '-':
                            unsafe { op.AddCommands(new ThreeAddress(43, td3.pd, val1.pi, val2.pd)); }
                            SymbolTable.CommandsCounter++;
                            return td3;
                        case '*':
                            unsafe { op.AddCommands(new ThreeAddress(44, td3.pd, val1.pi, val2.pd)); }
                            SymbolTable.CommandsCounter++;
                            return td3;
                        case '/':
                            unsafe { op.AddCommands(new ThreeAddress(45, td3.pd, val1.pi, val2.pd)); }
                            SymbolTable.CommandsCounter++;
                            return td3;
                        case '>':
                            unsafe { op.AddCommands(new ThreeAddress(46, tb.pb, val1.pi, val2.pd)); }
                            SymbolTable.CommandsCounter++;
                            return tb;
                        case '<':
                            unsafe { op.AddCommands(new ThreeAddress(47, tb.pb, val1.pi, val2.pd)); }
                            SymbolTable.CommandsCounter++;
                            return tb;
                        case '=':
                            unsafe { op.AddCommands(new ThreeAddress(48, tb.pb, val1.pi, val2.pd)); }
                            SymbolTable.CommandsCounter++;
                            return tb;
                        case '!':
                            unsafe { op.AddCommands(new ThreeAddress(49, tb.pb, val1.pi, val2.pd)); }
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
                op.AddCommands(new ThreeAddress(17));
                op.Commands[SymbolTable.CommandsCounter++].pia = val.pi;
            }
            if (tval == Types.tdouble)
            {
                op.AddCommands(new ThreeAddress(18));
                op.Commands[SymbolTable.CommandsCounter++].pda = val.pd;
            }
            if (tval == Types.tbool)
            {
                op.AddCommands(new ThreeAddress(19));
                op.Commands[SymbolTable.CommandsCounter++].pba = val.pb;
            }
            if (SymbolTable.CommandsCounter >= 2)
            {
                if (op.Commands[SymbolTable.CommandsCounter - 2].NumberOfCommand == 22 ||
                    op.Commands[SymbolTable.CommandsCounter - 2].NumberOfCommand == 23)
                {
                    return new Value(0);
                }
            }
            if (op.c == SymbolTable.CommandsSize - 1)
            {
                op.AddCommands(new ThreeAddress(0));
                //op.RunCommands();
                op.PrintCommands();
            }
            return new Value(0);
        }

    }
}