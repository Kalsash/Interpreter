using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ProgramTree;
using SimpleLang.Visitors;

namespace SimpleLang
{
    public enum type { tint, tdouble };
    class DefaultVisitor: Visitor
    {
        public Dictionary<string, double> Vars = new Dictionary<string, double>(); // таблица символов
        public double Loop_Counter = -1;
        public double Value;
        public void NewVarDef(string name, double val)
        {
            if (Vars.ContainsKey(name))
                Vars[name] = val;
            else Vars.Add(name, val);
        }
        public override void VisitIdNode(IdNode id)
        {
            if (Vars.ContainsKey(id.Name))
            {
                Value = Vars[id.Name];
            }
        }
        public override void VisitIntNumNode(IntNumNode num) 
        {
            Value = num.Num;
        }
        public override void VisitRealNumNode(RealNumNode num)
        {
            Value = num.Num;
        }

        public override void VisitBinOpNode(BinOpNode binop)
        {
            binop.Left.Eval(this);
            double val1 = Value;
            binop.Right.Eval(this);
            double val2 = Value;
            switch (binop.Op)
            {
           
                case '+':
                    Value = val1 + val2;
                    break;
                case '-':
                    Value = val1 + val2;
                    break;
                case '*':
                    Value = val1 + val2;
                    break;
                case '/':
                    Value = val1 + val2;
                    break;
            }
        }
        public override void VisitAssignNode(AssignNode a)
        {
            // для каких-то визиторов порядок может быть обратный - вначале обойти выражение, потом - идентификатор
            a.Id.Eval(this);
            a.Expr.Eval(this);
            NewVarDef(a.Id.Name, Value);
        }
        public override void VisitLoopNode(LoopNode l)
        {
            l.Expr.Eval(this);
            {

            }
            if (Loop_Counter == -1)
            {
                Loop_Counter = Value;
            }
            if (Loop_Counter > 0)
            {
                Loop_Counter--;
                l.Stat.Eval(this);
                VisitLoopNode(l);
            }
            Loop_Counter = -1;
        }
        public override void VisitWhileNode(WhileNode w)
        {
            //w.Expr.Eval(this);
            //{

            //}
            //if (Loop_Counter == -1)
            //{
            //    Loop_Counter = value;
            //}
            //if (Loop_Counter > 0)
            //{
            //    Loop_Counter--;
            //    w.Stat.Eval(this);
            //    VisitWhileNode(w);
            //}
            //Loop_Counter = -1;
            Console.WriteLine("Цикл на стадии разработки!");

        }
        public override void VisitBlockNode(BlockNode bl)
        {
            foreach (var st in bl.StList)
                st.Eval(this);
        }
        public override void VisitWriteNode(WriteNode w)
        {
            w.Expr.Eval(this); 
            Console.WriteLine(Value);
        }
    }
}
