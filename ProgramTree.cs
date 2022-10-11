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
        public abstract void Eval(Visitor v);
    }

    public abstract class ExprNode : Node // базовый класс для всех выражений
    {
        public abstract double Execute();
    }

    public class IdNode : ExprNode
    {
        public string Name { get; set; }
        public double Value { get; set; }
        public IdNode(string name) { Name = name; }
        public override double Execute()
        {
            return Value;
        }
        public override void Eval(Visitor v)
        {
            //System.Console.WriteLine("Зашел в IdNode");
            v.VisitIdNode(this);
           
        }
    }

    public  class IntNumNode : ExprNode
    {
        public int Num { get; set; }
        public IntNumNode(int num) { Num = num; }

        public override double Execute()
        {
            return Num;
        }
        public override void Eval(Visitor v)
        {
            //System.Console.WriteLine("Зашел в IntNumNode");
            v.VisitIntNumNode(this);
           
        }
    }

    public class RealNumNode : ExprNode
    {
        public double Num { get; set; }
        public RealNumNode(double num) { Num = num; }

        public override double Execute()
        {
            return Num;
        }
        public override void Eval(Visitor v)
        {
            //System.Console.WriteLine("Зашел в IntNumNode");
            v.VisitRealNumNode(this);

        }
    }
    public class BinOpNode : ExprNode
    {
        public ExprNode Left { get; set; }
        public ExprNode Right { get; set; }
        public char Op { get; set; }
        public BinOpNode(ExprNode Left, ExprNode Right, char op)
        {
            this.Left = Left;
            this.Right = Right;
            this.Op = op;
        }
        public override double Execute()
        {
            switch (Op)
            {
                case '+':
                    return Left.Execute() + Right.Execute();
                case '-':
                    return Left.Execute() - Right.Execute();
                case '*':
                    return Left.Execute() * Right.Execute();
                case '/':
                    return Left.Execute() / Right.Execute();
            }
            return Left.Execute() + Right.Execute();
        }
        public override void Eval(Visitor v)
        {
            //System.Console.WriteLine("Зашел в BinOpNode");
            v.VisitBinOpNode(this);
        }
    }
    public abstract class StatementNode : Node // базовый класс для всех операторов
    {
    }

    public class AssignNode : StatementNode
    {

        public IdNode Id { get; set; }
        public ExprNode Expr { get; set; }
        public AssignType AssOp { get; set; }
        public AssignNode(IdNode id, ExprNode expr, AssignType assop = AssignType.Assign)
        {
            Id = id;
            Expr = expr;
            AssOp = assop;
        }
        public override void Eval(Visitor v)
        {
           // System.Console.WriteLine("Зашел в AssignNode");
            v.VisitAssignNode(this);
        }
    }

    public class CycleNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public CycleNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat = stat;
        }
        public override void Eval(Visitor v)
        {
           // System.Console.WriteLine("Зашел в CycleNode");
            v.VisitCycleNode(this);
        }

    }
    public class WhileNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public StatementNode Stat { get; set; }
        public WhileNode(ExprNode expr, StatementNode stat)
        {
            Expr = expr;
            Stat = stat;
        }
        public override void Eval(Visitor v)
        {
            System.Console.WriteLine("Зашел в WhileNode");
            v.VisitWhileNode(this);
        }

    }

    public class BlockNode : StatementNode
    {
        public List<StatementNode> StList = new List<StatementNode>();
        public BlockNode(StatementNode stat)
        {
            Add(stat);
        }
        public void Add(StatementNode stat)
        {
            StList.Add(stat);
        }
        public override void Eval(Visitor v)
        {
           // System.Console.WriteLine("Зашел в BlockNode");
            v.VisitBlockNode(this);
          
        }
    }

    public class WriteNode : StatementNode
    {
        public ExprNode Expr { get; set; }
        public WriteNode(ExprNode Expr)
        {
            this.Expr = Expr;
        }
        public override void Eval(Visitor v)
        {
            Console.WriteLine("Зашел в WriteNode");
            v.VisitWriteNode(this);
        }
    }

}