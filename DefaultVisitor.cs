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
        public Var(type t, string val)
        {
            type = t;
            value = val;
        }
    }
    class DefaultVisitor: Visitor
    {
        public Dictionary<string, Var> Vars = new Dictionary<string, Var>(); // таблица символов
        public int Loop_Counter = -1;
        public string Value;
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
               Type = Vars[id.Name].type;           
               Value = Vars[id.Name].value;
            }
        }
        public override void VisitIntNumNode(IntNumNode num) 
        {
            Value = num.Num.ToString();
            Type = type.tint;
        }
        public override void VisitRealNumNode(RealNumNode num)
        {
            Value = num.Num.ToString();
            Type = type.tdouble;
        }

        public override void VisitBinOpNode(BinOpNode binop)
        {
            binop.Left.Eval(this);
            var t1 = Type;
            var val1 = Value;   
            binop.Right.Eval(this);
            var t2 = Type;
            var val2 = Value;
            if (t1 == type.tint && t2 == type.tint)
            {
                Type = type.tint;
                switch (binop.Op)
                {

                    case '+':
                        Value = (int.Parse(val1) + int.Parse(val2)).ToString();
                        break;
                    case '-':
                        Value = (int.Parse(val1) - int.Parse(val2)).ToString();
                        break;
                    case '*':
                        Value = (int.Parse(val1) * int.Parse(val2)).ToString();
                        break;
                    case '/':
                        Value = (int.Parse(val1) / int.Parse(val2)).ToString();
                        break;
                }
            }
            else
            {
                Type = type.tdouble;
                switch (binop.Op)
                {

                    case '+':
                        Value = (double.Parse(val1) + double.Parse(val2)).ToString();
                        break;
                    case '-':
                        Value = (double.Parse(val1) - double.Parse(val2)).ToString();
                        break;
                    case '*':
                        Value = (double.Parse(val1) * double.Parse(val2)).ToString();
                        break;
                    case '/':
                        Value = (double.Parse(val1) / double.Parse(val2)).ToString();
                        break;
                }
            }
              



        }
        public override void VisitAssignNode(AssignNode a)
        {
            a.Id.Eval(this);
            a.Expr.Eval(this);
            NewVarDef(a.Id.Name, new Var(Type, Value));
        }
        public override void VisitLoopNode(LoopNode l)
        {
            l.Expr.Eval(this);
            if (Loop_Counter == -1)
            {
                Loop_Counter = int.Parse(Value);
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
                Console.WriteLine("Переменная со значением " + Value + " типа int");
            }
            if (Type == type.tdouble)
            {
                Console.WriteLine("Переменная со значением " + Value + " типа double");
            }

        }
    }
}
