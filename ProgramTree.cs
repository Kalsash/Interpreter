using QUT.Gppg;
using SimpleLang;
using SimpleLang.Visitors;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Xml.Linq;
using static SimpleCompiler.SimpleCompilerMain;

namespace ProgramTree
{
    public enum AssignType { Assign, AssignPlus, AssignMinus, AssignMult, AssignDivide };

    public abstract class Node // базовый класс для всех узлов    
    {
       public LexLocation lx;
        public abstract T Eval<T>(Visitor<T> v);
    }

    public abstract class ExprNode : Node // базовый класс для всех выражений
    {
    }

    public class IdNode : ExprNode
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public IdNode(string name, LexLocation lx = null) { this.lx = lx; Name = name; }
        public override T Eval<T>(Visitor<T> v)
        {
           return v.VisitIdNode(this);       
        }
    }

    public  class IntNumNode : ExprNode
    {
        public int Num { get; set; }
        public IntNumNode(int num, LexLocation lx = null) { this.lx = lx; Num = num; }

        public override T Eval<T>(Visitor<T> v)
        {
            return v.VisitIntNumNode(this); 
        }
    }

    public class RealNumNode : ExprNode
    {
        public double Num { get; set; }
        public RealNumNode(double num,LexLocation lx = null) {
            this.lx = lx; Num = num;  }


        public override T Eval<T>(Visitor<T> v)
        {
            return v.VisitRealNumNode(this);
        }
    }
    public class FuncNode : ExprNode
    {
        public double Val { get; set; }
        public FuncNode(object name, LexLocation lx = null) 
        { 
            this.lx = lx;
            var s = name.ToString();
            Val = 0;
            if (s.StartsWith("@pi"))
            {
                Val = Math.PI;
            }
            if (s.StartsWith("@e"))
            {
                Val = Math.E;
            }
            if (s.StartsWith("@sqrt"))
            {
                int k = s.IndexOf(')');
                var ss = s.Substring(6, k - 6);
                Val = Math.Sqrt(int.Parse(ss));
            }
            if (s.StartsWith("@sin"))
            {
                int k = s.IndexOf(')');
                var ss = s.Substring(5, k-5);
                Val = Math.Sin(int.Parse(ss));
            }
            if (s.StartsWith("@cos"))
            {
                int k = s.IndexOf(')');
                var ss = s.Substring(5, k - 5);
                Val = Math.Cos(int.Parse(ss));
            }
            if (s.StartsWith("@tan"))
            {
                int k = s.IndexOf(')');
                var ss = s.Substring(5, k - 5);
                Val = Math.Tan(int.Parse(ss));
            }

        }

        public override T Eval<T>(Visitor<T> v)
        {
            return v.VisitFuncNode(this);
        }
    }
    public class BinOpNode : ExprNode
    {
        public ExprNode Left { get; set; }
        public ExprNode Right { get; set; }
        public char Op { get; set; }
        public BinOpNode(ExprNode Left, ExprNode Right, char op, LexLocation lx = null)
        {
            this.Left = Left;
            this.Right = Right;
            this.Op = op;
            this.lx = lx;
        }
        public override T Eval<T>(Visitor<T> v)
        {
            return v.VisitBinOpNode(this);
        }
    }
    public abstract class StatementNode : Node // базовый класс для всех операторов
    {
    }

    public class AssignNode : StatementNode
    {
        public int AssignCounter = 0; 
        public IdNode Id { get; set; }
        public ExprNode Expr { get; set; }
        public AssignType AssOp { get; set; }
        public AssignNode(IdNode id, ExprNode expr, LexLocation lx, AssignType assop = AssignType.Assign)
        {
            Id = id;
            Expr = expr;
            AssOp = assop;
            this.lx = lx;
            AssignCounter++;
        }
        public override T Eval<T>(Visitor<T> v)
        {
            return v.VisitAssignNode(this);
        }
    }

    public class LoopNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public LoopNode(ExprNode expr, StatementNode stat, LexLocation lx = null)
        {
            Expr = expr;
            Stat = stat;
            this.lx = lx;
        }
        public override T Eval<T>(Visitor<T> v)
        {
            return v.VisitLoopNode(this);
        }

    }
    public class WhileNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public WhileNode(ExprNode expr, StatementNode stat, LexLocation lx = null)
        {
            Expr = expr;
            Stat = stat;
            this.lx = lx;
        }
        public override T Eval<T>(Visitor<T> v)
        {
            System.Console.WriteLine("Зашел в WhileNode");
            return v.VisitWhileNode(this);
        }

    }

    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();
        public BlockNode(StatementNode stat, LexLocation lx = null)
        {
            this.lx = lx;
            Add(stat);
        }
        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }
        public override T Eval<T>(Visitor<T> v)
        {
            return v.VisitBlockNode(this);      
        }
    }

    public class WriteNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public WriteNode(ExprNode Expr, LexLocation lx = null)
        {
            this.Expr = Expr;
            this.lx = lx;
        }
        public override T Eval<T>(Visitor<T> v)
        {
            return v.VisitWriteNode(this);
         
        }
    }

}