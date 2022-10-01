using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ProgramTree;
using SimpleLang.Visitors;

namespace SimpleLang
{
    class DefaultVisitor: Visitor
    {
        public Dictionary<string, int> vars = new Dictionary<string, int>(); // таблица символов
        public void NewVarDef(string name, int val)
        {
            if (vars.ContainsKey(name))
                vars[name] = val;
            else vars.Add(name, val);
        }
        public override void VisitIdNode(IdNode id)
        {
            if (vars.ContainsKey(id.Name))
            {
                id.Value = vars[id.Name];
            }
        }
        public override void VisitBinOpNode(BinOpNode binop)
        {
            binop.Left.Eval(this);
            binop.Right.Eval(this);
        }
        public override void VisitAssignNode(AssignNode a)
        {
            // для каких-то визиторов порядок может быть обратный - вначале обойти выражение, потом - идентификатор
            a.Id.Eval(this);
            a.Expr.Eval(this);
            NewVarDef(a.Id.Name, a.Expr.Execute());

        }
        public override void VisitCycleNode(CycleNode c)
        {
            c.Expr.Eval(this);
            c.Stat.Eval(this);
        }
        public override void VisitWhileNode(WhileNode w)
        {
            w.Expr.Eval(this);
            w.Stat.Eval(this);
        }
        public override void VisitBlockNode(BlockNode bl)
        {
            foreach (var st in bl.StList)
                st.Eval(this);
        }
        public override void VisitWriteNode(WriteNode w)
        {
            w.Expr.Eval(this);
            Console.WriteLine(w.Expr.Execute()); 
        }
    }
}
