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
        public override SimpleParser.Types VisitBinOpNode(BinOpNode binop)
        {
           var t1 = binop.Left.Eval(this);
           var t2 = binop.Right.Eval(this);
            if (t1 == t2)
            {
                return t1;
            }
            else
            {
                return SimpleParser.Types.tdouble;
            }
        }
        public override SimpleParser.Types VisitAssignNode(AssignNode a)
        {
            Var v = new Var(a.Expr.Eval(this), 0);
            SymbolTable.NewVarDef(a.Id.Name, v);
            return SimpleParser.Types.tvoid;
        }
        public override SimpleParser.Types VisitLoopNode(LoopNode l)
        {
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
            return SimpleParser.Types.tvoid;
        }
    }
}
