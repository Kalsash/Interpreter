﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using ProgramTree;
using SimpleLang.Visitors;
using static System.Net.Mime.MediaTypeNames;

namespace SimpleLang
{
    class Interpreter : Visitor<object>
    {
        public Dictionary<string, object> Vars = new Dictionary<string, object>(); // таблица символов
        public int Loop_Counter = -1;
        public void NewVarDef(string name, object ob)
        {
            if (Vars.ContainsKey(name))
            {
                Vars[name] = ob;
            }
            else
            {
                Vars.Add(name, ob);
            }

        }
        public override object VisitIdNode(IdNode id)
        {
            if (Vars.ContainsKey(id.Name))
            {
                return Vars[id.Name];
            }
            return 0;
        }
        public override object VisitIntNumNode(IntNumNode num)
        {
            return num.Num;
        }
        public override object VisitRealNumNode(RealNumNode num)
        {
            return num.Num;
        }

        public override object VisitBinOpNode(BinOpNode binop)
        {
            var TypeChecker = new TypeChecker();
          var val1 =  binop.Left.Eval(this);
           var t1 = binop.Left.Eval(TypeChecker);
           var val2 = binop.Right.Eval(this);
           var t2 = binop.Right.Eval(TypeChecker);

            if (t1 == type.tint && t2 == type.tint)
            {
                switch (binop.Op)
                {
                    case '+':
                        return int.Parse(string.Format("{0}", val1)) + int.Parse(string.Format("{0}", val2));
                    case '-':
                        return int.Parse(string.Format("{0}", val1)) - int.Parse(string.Format("{0}", val2));
                    case '*':
                        return int.Parse(string.Format("{0}", val1)) * int.Parse(string.Format("{0}", val2));
                    case '/':
                        return int.Parse(string.Format("{0}", val1)) / int.Parse(string.Format("{0}", val2));
                }
            }
            else
            {
                switch (binop.Op)
                {
                    case '+':
                        return double.Parse(string.Format("{0}", val1)) + double.Parse(string.Format("{0}", val2));
                    case '-':
                        return double.Parse(string.Format("{0}", val1)) - double.Parse(string.Format("{0}", val2));
                    case '*':
                        return double.Parse(string.Format("{0}", val1)) * double.Parse(string.Format("{0}", val2));
                    case '/':
                        return double.Parse(string.Format("{0}", val1)) / double.Parse(string.Format("{0}", val2));
                }
            }

            return 0;
        }

        public override object VisitAssignNode(AssignNode a)
        {
            a.Id.Eval(this);
            var val = a.Expr.Eval(this);
            NewVarDef(a.Id.Name, val);
            return 0;
        }
        public override object VisitLoopNode(LoopNode l)
        {
            var val = l.Expr.Eval(this);
            if (Loop_Counter == -1)
            {
                Loop_Counter = int.Parse(string.Format("{0}", val));
            }
            if (Loop_Counter > 0)
            {
                Loop_Counter--;
                l.Stat.Eval(this);
                VisitLoopNode(l);
            }
            Loop_Counter = -1;
            return 0;
        }
        public override object VisitWhileNode(WhileNode w)
        {
            Console.WriteLine("Цикл на стадии разработки!");
            return 0;
        }
        public override object VisitBlockNode(BlockNode bl)
        {
            foreach (var st in bl.StList)
                st.Eval(this);
            return 0;
        }
        public override object VisitWriteNode(WriteNode w)
        {
            var val = w.Expr.Eval(this);
            Console.WriteLine(val);
            return val;
        }
    }
}