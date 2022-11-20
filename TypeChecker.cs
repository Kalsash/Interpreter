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
    
    class TypeChecker:Visitor<SimpleParser.Types>
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
        public override SimpleParser.Types VisitFuncNode(FuncNode num)
        {
            return SimpleParser.Types.tdouble;
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
            if (l.Expr.Eval(this) == SimpleParser.Types.tvoid)
            {
                throw new SemanticException(string.Format("({0},{1}):" +
                              " Неизвестное имя переменной ", l.lx.StartLine, l.lx.StartColumn + 5));
            }
            return SimpleParser.Types.tvoid;
        }
        public override SimpleParser.Types VisitWhileNode(WhileNode w)
        {
            return SimpleParser.Types.tvoid;
        }
        public override SimpleParser.Types VisitBlockNode(BlockNode bl)
        {
            foreach (var st in bl.StList)
                st.Eval(this);
            return SimpleParser.Types.tvoid;
        }
        public override SimpleParser.Types VisitWriteNode(WriteNode w)
        {
            var val = w.Expr.Eval(this);
            if (val == Types.tvoid)
            {
                throw new SemanticException(string.Format("({0},{1}):" +
                              " Неизвестное имя переменной ", w.lx.StartLine, w.lx.StartColumn+6));
            }
            return SimpleParser.Types.tvoid;
        }
    }
}
