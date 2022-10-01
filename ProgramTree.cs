using SimpleLang;
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
        public abstract SymbolTable Eval(SymbolTable dict);
        // public abstract void Nodes(Nodes n);
    }

    public abstract class ExprNode : Node // базовый класс для всех выражений
    {
        public abstract int Execute();
    }

    public class IdNode : ExprNode
    {
        public string Name { get; set; }
        public int Value { get; set; }
        public IdNode(string name) { Name = name; }
        public override int Execute()
        {
            return Value;
        }
        public override SymbolTable Eval(SymbolTable dict)
        {
            System.Console.WriteLine("Зашел в IdNode");
            if (dict.vars.ContainsKey(Name))
            {
                Value = dict.vars[Name];
            }
            return dict;
        }
        //public override void Nodes(Nodes n)
        //{
        //    n.GoToIdNode(this);
        //}
    }

    public  class IntNumNode : ExprNode
    {
        public int Num { get; set; }
        public IntNumNode(int num) { Num = num; }

        public override int Execute()
        {
            return Num;
        }
        public override SymbolTable Eval(SymbolTable dict)
        {
            System.Console.WriteLine("Зашел в IntNumNode");
            return dict;
        }
        //public override void Nodes(Nodes n)
        //{
        //    n.GoToIntNumNode(this);
        //}

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
        public override int Execute()
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
        public override SymbolTable Eval(SymbolTable dict)
        {
            System.Console.WriteLine("Зашел в BinOpNode");
            Left.Eval(dict);
            Right.Eval(dict);
            return dict;
        }
        //public override void Visit(Visitor v)
        //{
        //    v.VisitBinOpNode(this);
        //}
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
        public override SymbolTable Eval(SymbolTable dict)
        {
            System.Console.WriteLine("Зашел в AssignNode");
            Id.Eval(dict);
            Expr.Eval(dict);
            dict.NewVarDef(Id.Name, Expr.Execute());
            return dict;
        }
        //public override void Nodes(Nodes n)
        //{
        //    n.GoToAssignNode(this);      
        //}
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
        public override SymbolTable Eval(SymbolTable dict)
        {
            System.Console.WriteLine("Зашел в CycleNode");
            Expr.Eval(dict);
            Stat.Eval(dict);
            return dict;
        }
        //public override void Nodes(Nodes n)
        //{
        //    n.GoToCycleNode(this);
        //}
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
        public override SymbolTable Eval(SymbolTable dict)
        {
            System.Console.WriteLine("Зашел в WhileNode");
            Expr.Eval(dict);
            Stat.Eval(dict);
            return dict;
        }
        //public override void Nodes(Nodes n)
        //{
        //    n.GoToWhileNode(this);
        //}
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
        public override SymbolTable Eval(SymbolTable dict)
        {
            System.Console.WriteLine("Зашел в BlockNode");
            foreach (var st in StList)
                st.Eval(dict);
            return dict;
        }

        //public override void Nodes(Nodes n)
        //{
        //    n.GoToBlockNode(this);

        //}
    }

}