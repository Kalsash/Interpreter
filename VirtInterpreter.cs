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
        
        public override Value VisitIdNode(IdNode id)
        {
           return SymbolTable.mem[Vars_Dict[id.Name].Index];
        }
        public override Value VisitIntNumNode(IntNumNode num)
        {
            return new Value(num.Num);
        }
        public override Value VisitRealNumNode(RealNumNode num)
        {
            return new Value(num.Num);
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
            var val = a.Expr.Eval(this);
            var val2 = a.Id.Eval(this);
            if (tname == Types.tint && tval == Types.tint)
            {
                op.AddCommands(new ThreeAddress(4));
                unsafe
                {
                    op.Commands[SymbolTable.CommandsCounter].pia = val2.pi; // n
                    op.Commands[SymbolTable.CommandsCounter++].pib = val.pi; //  n = 10000000
                }
            }
            if (tname== Types.tdouble && tval == Types.tdouble)
            {
                op.AddCommands(new ThreeAddress(5));
                unsafe {
                    op.Commands[SymbolTable.CommandsCounter].pda = val2.pd; // s
                    op.Commands[SymbolTable.CommandsCounter++].pdb = val.pd; // s = 0.0

                }
            }
            if (tname == Types.tbool && tval == Types.tbool)
            {
                op.AddCommands(new ThreeAddress(6));
                unsafe
                {
                    op.Commands[SymbolTable.CommandsCounter].pba = val2.pb; // s
                    op.Commands[SymbolTable.CommandsCounter++].pbb = val.pb; // b = false

                }
            }

            if (op.c == SymbolTable.CommandsSize-1)
            {
                op.AddCommands(new ThreeAddress(0));
                op.RunCommands();
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
                            op.AddCommands(new ThreeAddress(8));
                            unsafe
                            {

                                op.Commands[SymbolTable.CommandsCounter].pib = val1.pi; // n
                                op.Commands[SymbolTable.CommandsCounter].pic = val2.pi; // n
                                op.Commands[SymbolTable.CommandsCounter++].pia = ti.pi; //  n = 10000000
                            }
                            return ti;
                        case '-':
                            return new Value(val1.i - val2.i);
                        case '*':
                            return new Value(val1.i * val2.i);
                        case '/':
                            return new Value(val1.i / val2.i);
                        case '>':
                            return new Value(val1.i > val2.i);
                        case '<':
                            op.AddCommands(new ThreeAddress(20));
                            unsafe
                            {

                                op.Commands[SymbolTable.CommandsCounter].pib = val1.pi; // n
                                op.Commands[SymbolTable.CommandsCounter].pic = val2.pi; // n
                                op.Commands[SymbolTable.CommandsCounter++].pba = tb.pb; //  n = 10000000
                            }
                            return tb;
                        case '=':
                            return new Value(val1.i == val2.i);
                        case '!':
                            return new Value(val1.i != val2.i);
                    }
                }
                if (t1 == SimpleParser.Types.tdouble && t2 == SimpleParser.Types.tdouble)
                {
                    var td = new Value(0.0);
                    switch (binop.Op)
                    {
                        case '+':
                            op.AddCommands(new ThreeAddress(9));
                            unsafe
                            {

                                op.Commands[SymbolTable.CommandsCounter].pdb = val1.pd; // n
                                op.Commands[SymbolTable.CommandsCounter].pdc = val2.pd; // n
                                op.Commands[SymbolTable.CommandsCounter++].pda = td.pd; //  n = 10000000
                            }

                            return td;
                        case '-':
                            return new Value(val1.d - val2.d);
                        case '*':
                            return new Value(val1.d * val2.d);
                        case '/':
                            return new Value(val1.d / val2.d);
                        case '>':
                            return new Value(val1.d > val2.d);
                        case '<':
                            return new Value(val1.d < val2.d);
                        case '=':
                            return new Value(val1.d == val2.d);
                        case '!':
                            return new Value(val1.d != val2.d);
                    }
                }

                if (t1 == SimpleParser.Types.tdouble && t2 == SimpleParser.Types.tint)
                {
                    var td2 = new Value(0.0);
                    switch (binop.Op)
                    {
                        case '+':
                            op.AddCommands(new ThreeAddress(10));
                            unsafe
                            {

                                op.Commands[SymbolTable.CommandsCounter].pdb = val1.pd; // n
                                op.Commands[SymbolTable.CommandsCounter].pic = val2.pi; // n
                                op.Commands[SymbolTable.CommandsCounter++].pda = td2.pd; //  n = 10000000
                            }
                            return td2;
                        case '-':
                            return new Value(val1.d - val2.i);
                        case '*':
                            return new Value(val1.d * val2.i);
                        case '/':
                            op.AddCommands(new ThreeAddress(21));
                            unsafe
                            {

                                op.Commands[SymbolTable.CommandsCounter].pdb = val1.pd; // n
                                op.Commands[SymbolTable.CommandsCounter].pic = val2.pi; // n
                                op.Commands[SymbolTable.CommandsCounter++].pda = td2.pd; //  n = 10000000
                            }
                            return td2;
                        case '>':
                            return new Value(val1.d > val2.i);
                        case '<':
                            return new Value(val1.d < val2.i);
                        case '=':
                            return new Value(val1.d == val2.i);
                        case '!':
                            return new Value(val1.d != val2.i);
                    }
                }

                if (t1 == SimpleParser.Types.tint && t2 == SimpleParser.Types.tdouble)
                {
                    var td3 = new Value(0.0);
                    switch (binop.Op)
                    {
                        case '+':
                            op.AddCommands(new ThreeAddress(10));
                            unsafe
                            {

                                op.Commands[SymbolTable.CommandsCounter].pic = val1.pi; // n
                                op.Commands[SymbolTable.CommandsCounter].pdb = val2.pd; // n
                                op.Commands[SymbolTable.CommandsCounter++].pda = td3.pd; //  n = 10000000
                            }
                            return td3;
                        case '-':
                            return new Value(val1.d - val2.i);
                        case '*':
                            return new Value(val1.d * val2.i);
                        case '/':
                            return new Value(val1.d / val2.i);
                        case '>':
                            return new Value(val1.d > val2.i);
                        case '<':
                            return new Value(val1.d < val2.i);
                        case '=':
                            return new Value(val1.d == val2.i);
                        case '!':
                            return new Value(val1.d != val2.i);
                    }
                }

            }



            return new Value(0);
        }
        public override Value VisitIfNode(IfNode f)
        {
            var val = f.Expr.Eval(this);
            if (val.b == true)
            {
                f.Stat.Eval(this);
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

            if (op.c == SymbolTable.CommandsSize-1)
            {
                op.AddCommands(new ThreeAddress(0));
                op.RunCommands();
            }
            return new Value(0);
        }

    }
}