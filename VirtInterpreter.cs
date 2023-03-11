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
            SymbolTable.IsVar = true;
           return new Value(Vars_Dict[id.Name].Index);
        }
        public override Value VisitIntNumNode(IntNumNode num)
        {
            SymbolTable.IsVar = false;
            return new Value(num.Num);
        }
        public override Value VisitRealNumNode(RealNumNode num)
        {
            SymbolTable.IsVar = false;
            return new Value(num.Num);
        }
        public override Value VisitBoolNumNode(BoolNumNode num)
        {
            SymbolTable.IsVar = false;
            return new Value(num.Num);
        }
        public override Value VisitAssignNode(AssignNode a)
        {

            var TypeChecker = new SemanticChecker();
            var tname = a.Id.Eval(TypeChecker);
            var tval = a.Expr.Eval(TypeChecker);
            var val = a.Expr.Eval(this);
            var val2 = a.Id.Eval(this);
            if (tname == Types.tint && tval == Types.tint)
            {
                op.AddCommands(new ThreeAddress(1));
                unsafe
                {
                    op.Commands[SymbolTable.CommandsCounter].pia = SymbolTable.mem[val2.i].pi; // n
                    op.Commands[SymbolTable.CommandsCounter++].intVal = val.i; //  n = 10000000
                }
            }
            if (tname== Types.tdouble && tval == Types.tdouble)
            {
                op.AddCommands(new ThreeAddress(2));
                unsafe {
                    op.Commands[SymbolTable.CommandsCounter].pda = SymbolTable.mem[val2.i].pd; // s
                    op.Commands[SymbolTable.CommandsCounter++].doubleVal = val.d; // s = 0.0

                }
            }
            if (tname == Types.tbool && tval == Types.tbool)
            {
                op.AddCommands(new ThreeAddress(3));
                unsafe
                {
                    op.Commands[SymbolTable.CommandsCounter].pba = SymbolTable.mem[val2.i].pb; // s
                    op.Commands[SymbolTable.CommandsCounter++].boolVal = val.b; // b = false

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
                    switch (binop.Op)
                    {
                        case '+':
                            
                            return new Value(val1.i + val2.i);
                        case '-':
                            return new Value(val1.i - val2.i);
                        case '*':
                            return new Value(val1.i * val2.i);
                        case '/':
                            return new Value(val1.i / val2.i);
                        case '>':
                            return new Value(val1.i > val2.i);
                        case '<':
                            return new Value(val1.i < val2.i);
                        case '=':
                            return new Value(val1.i == val2.i);
                        case '!':
                            return new Value(val1.i != val2.i);
                    }
                }
                if (t1 == SimpleParser.Types.tdouble && t2 == SimpleParser.Types.tdouble)
                    switch (binop.Op)
                {
                    case '+':
                        return new Value(val1.d + val2.d);
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
                if (t1 == SimpleParser.Types.tdouble && t2 == SimpleParser.Types.tint)
                    switch (binop.Op)
                    {
                        case '+':
                            return new Value(val1.d + val2.i);
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
                if (t1 == SimpleParser.Types.tint && t2 == SimpleParser.Types.tdouble)
                    switch (binop.Op)
                    {
                        case '+':
                            return new Value(val1.i + val2.d);
                        case '-':
                            return new Value(val1.i - val2.d);
                        case '*':
                            return new Value(val1.i * val2.d);
                        case '/':
                            return new Value(val1.i / val2.d);
                        case '>':
                            return new Value(val1.i > val2.d);
                        case '<':
                            return new Value(val1.i < val2.d);
                        case '=':
                            return new Value(val1.i == val2.d);
                        case '!':
                            return new Value(val1.i != val2.d);
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
            if (SymbolTable.IsVar)
            {
                if (tval == Types.tint)
                {
                    op.AddCommands(new ThreeAddress(20));
                }
                if (tval == Types.tdouble)
                {
                    op.AddCommands(new ThreeAddress(21));
                }
                if (tval == Types.tbool)
                {
                    op.AddCommands(new ThreeAddress(22));
                }
                op.Commands[SymbolTable.CommandsCounter++].intVal = val.i;

            }
            else
            {
                if (tval == Types.tint)
                {
                    op.AddCommands(new ThreeAddress(17));
                    op.Commands[SymbolTable.CommandsCounter++].intVal = val.i;
                }
                if (tval == Types.tdouble)
                {
                    op.AddCommands(new ThreeAddress(18));
                    op.Commands[SymbolTable.CommandsCounter++].doubleVal = val.d;
                }
                if (tval == Types.tbool)
                {
                    op.AddCommands(new ThreeAddress(19));
                    op.Commands[SymbolTable.CommandsCounter++].boolVal = val.b;
                }
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