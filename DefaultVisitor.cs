using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ProgramTree;
using SimpleLang.Visitors;
using static System.Net.Mime.MediaTypeNames;

namespace SimpleLang
{
    public enum type { tint, tdouble };
    struct Var
    {
        public type type;
        public string value;
    }
    class DefaultVisitor: Visitor
    {
        public Dictionary<string, Var> Vars = new Dictionary<string, Var>(); // таблица символов
        public double Loop_Counter = -1;
        public double dValue;
        public int iValue;
        public type Type;
        public void NewVarDef(string name, Var v)
        {
            if (Vars.ContainsKey(name))
                Vars[name] = v;
            else Vars.Add(name, v);
        }
        public override void VisitIdNode(IdNode id)
        {
            if (Vars.ContainsKey(id.Name))
            {
                if (Vars[id.Name].type == type.tint)
                {
                    iValue = Int32.Parse(Vars[id.Name].value);
                    Type = type.tint;
                }
                if (Vars[id.Name].type == type.tdouble)
                {
                    dValue = double.Parse(Vars[id.Name].value);
                    Type = type.tdouble;
                }

            }
        }
        public override void VisitIntNumNode(IntNumNode num) 
        {
            iValue = num.Num;
            Type = type.tint;
        }
        public override void VisitRealNumNode(RealNumNode num)
        {
            dValue = num.Num;
            Type = type.tdouble;
        }

        public override void VisitBinOpNode(BinOpNode binop)
        {
           // type t1 = type.tdouble;
            //type t2 = type.tdouble;
            binop.Left.Eval(this);
            //if (Type == type.tint)
            //{
            //    t1 = type.tint;
            //    int val1 = iValue;
            //}
            //else
            //if (Type == type.tdouble)
            //{
            //    double val1 = iValue;
            //}
            binop.Right.Eval(this);
            //if (Type == type.tint)
            //{
            //    int val2 = iValue;
            //}
            //else
            //if (Type == type.tdouble)
            //{
            //    double val2 = iValue;
            //}
            //switch (binop.Op)
            //{
           
            //    case '+':
            //        dValue = val1 + val2;
            //        break;
            //    case '-':
            //        Value = val1 - val2;
            //        break;
            //    case '*':
            //        Value = val1 * val2;
            //        break;
            //    case '/':
            //        Value = val1 / val2;
            //        break;
            //}
        }
        public override void VisitAssignNode(AssignNode a)
        {
            // для каких-то визиторов порядок может быть обратный - вначале обойти выражение, потом - идентификатор
            a.Id.Eval(this);
            a.Expr.Eval(this);
            Var v = new Var();
            if (Type == type.tint)
            {
                v.type = type.tint;
                v.value = iValue.ToString();
            }
            if (Type == type.tdouble)
            {
                v.type = type.tdouble;
                v.value = dValue.ToString();
            }
            NewVarDef(a.Id.Name, v);
        }
        public override void VisitLoopNode(LoopNode l)
        {
            //l.Expr.Eval(this);
            //{

            //}
            //if (Loop_Counter == -1)
            //{
            //    Loop_Counter = Value;
            //}
            //if (Loop_Counter > 0)
            //{
            //    Loop_Counter--;
            //    l.Stat.Eval(this);
            //    VisitLoopNode(l);
            //}
            //Loop_Counter = -1;
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
            if (Type == type.tint)
            {
                Console.WriteLine("Переменная со значением " + iValue + " типа int");
            }
            if (Type == type.tdouble)
            {
                Console.WriteLine("Переменная со значением " + dValue + " типа double");
            }

        }
    }
}
