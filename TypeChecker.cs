using ProgramTree;
using SimpleLang.Visitors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleLang
{
    public enum type { tint, tdouble, tnope };
    class TypeChecker:Visitor<type>
    {
        public Dictionary<string, type> Types = new Dictionary<string, type>(); // таблица символов
        public void NewVarDef(string name, type t)
        {
            if (Types.ContainsKey(name))
            {
                if (Types[name] != t)
                {
                  
                    if (Types[name] == type.tint)
                    {
                        throw new Exception("Нельзя типу int присвоить double!");
                    }
                    else
                    {
                        Types[name] = type.tdouble;
                    }
                }
            }
            else Types.Add(name, t);
        }
        public override type VisitIdNode(IdNode id)
        {
            if (Types.ContainsKey(id.Name))
                return Types[id.Name];
            return type.tnope;
        }
        public override type VisitIntNumNode(IntNumNode num)
        {
            return type.tint;
        }
        public override type VisitRealNumNode(RealNumNode num)
        {
           
            return type.tdouble;
        }
        public override type VisitBinOpNode(BinOpNode binop)
        {
           var t1 = binop.Left.Eval(this);
           var t2 = binop.Right.Eval(this);
            if (t1 == t2)
            {
                return t1;
            }
            else
            {
                return type.tdouble;
            }
        }
        public override type VisitAssignNode(AssignNode a)
        {
            NewVarDef(a.Id.Name, a.Expr.Eval(this));
            return type.tnope;
        }
        public override type VisitLoopNode(LoopNode l)
        {
            return type.tnope;
        }
        public override type VisitWhileNode(WhileNode w)
        {
            return type.tnope;
        }
        public override type VisitBlockNode(BlockNode bl)
        {
            foreach (var st in bl.StList)
                st.Eval(this);
            return type.tnope;
        }
        public override type VisitWriteNode(WriteNode w)
        {
            return type.tnope;
        }
    }
}
