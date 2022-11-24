// This code was generated by the Gardens Point Parser Generator
// Copyright (c) Wayne Kelly, QUT 2005-2010
// (see accompanying GPPGcopyright.rtf)

// GPPG version 1.3.6
// Machine:  LASTHEROPC
// DateTime: 24.11.2022 11:32:34
// UserName: LastHero
// Input file <SimpleYacc.y>

// options: no-lines gplex

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using QUT.Gppg;
using ProgramTree;

namespace SimpleParser
{
public enum Tokens {
    error=1,EOF=2,BEGIN=3,END=4,WHILE=5,DO=6,
    LOOP=7,IF=8,ASSIGN=9,SEMICOLON=10,PLUS=11,MINUS=12,
    MULT=13,DIV=14,WRITE=15,LPAREN=16,RPAREN=17,COLUMN=18,
    MORE=19,LESS=20,EQ=21,NEQ=22,AND=23,OR=24,
    INUM=25,RNUM=26,BNUM=27,ID=28,FUN=29};

public struct ValueType
{ 
			public int iVal;
			public double dVal; 
			public bool bVal;
			public string sVal;
			public object oVal;
			public Node nVal;
			public ExprNode eVal;
			public StatementNode stVal;
			public BlockNode blVal;
			public RunTimeValue rtv;
       }
// Abstract base class for GPLEX scanners
public abstract class ScanBase : AbstractScanner<ValueType,LexLocation> {
  private LexLocation __yylloc = new LexLocation();
  public override LexLocation yylloc { get { return __yylloc; } set { __yylloc = value; } }
  protected virtual bool yywrap() { return true; }
}

public class Parser: ShiftReduceParser<ValueType, LexLocation>
{
  // Verbatim content from SimpleYacc.y
// ��� ���������� ����������� � ����� GPPGParser, �������������� ����� ������, ������������ �������� gppg
    public BlockNode root; // �������� ���� ��������������� ������ 
    public Parser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
  // End verbatim content from SimpleYacc.y

#pragma warning disable 649
  private static Dictionary<int, string> aliasses;
#pragma warning restore 649
  private static Rule[] rules = new Rule[44];
  private static State[] states = new State[79];
  private static string[] nonTerms = new string[] {
      "expr", "exprlist", "exprlistnull", "ident", "Q", "S", "P", "T", "F", "func", 
      "assign", "statement", "loop", "while", "if", "write", "stlist", "block", 
      "progr", "$accept", };

  static Parser() {
    states[0] = new State(new int[]{3,4},new int[]{-19,1,-18,3});
    states[1] = new State(new int[]{2,2});
    states[2] = new State(-1);
    states[3] = new State(-2);
    states[4] = new State(new int[]{28,24,3,4,7,57,5,61,8,68,15,74},new int[]{-17,5,-12,78,-11,9,-4,10,-18,55,-13,56,-14,60,-15,67,-16,73});
    states[5] = new State(new int[]{4,6,10,7});
    states[6] = new State(-39);
    states[7] = new State(new int[]{28,24,3,4,7,57,5,61,8,68,15,74},new int[]{-12,8,-11,9,-4,10,-18,55,-13,56,-14,60,-15,67,-16,73});
    states[8] = new State(-4);
    states[9] = new State(-5);
    states[10] = new State(new int[]{9,11});
    states[11] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-1,12,-5,49,-6,50,-7,51,-8,52,-9,53,-4,23,-10,28});
    states[12] = new State(new int[]{23,13,24,36,4,-12,10,-12});
    states[13] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-5,14,-6,50,-7,51,-8,52,-9,53,-4,23,-10,28});
    states[14] = new State(new int[]{21,15,22,38,23,-18,24,-18,4,-18,10,-18,18,-18,17,-18,28,-18,3,-18,7,-18,5,-18,8,-18,15,-18});
    states[15] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-6,16,-7,51,-8,52,-9,53,-4,23,-10,28});
    states[16] = new State(new int[]{19,17,20,40,21,-21,22,-21,23,-21,24,-21,4,-21,10,-21,18,-21,17,-21,28,-21,3,-21,7,-21,5,-21,8,-21,15,-21});
    states[17] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-7,18,-8,52,-9,53,-4,23,-10,28});
    states[18] = new State(new int[]{11,19,12,42,19,-24,20,-24,21,-24,22,-24,23,-24,24,-24,4,-24,10,-24,18,-24,17,-24,28,-24,3,-24,7,-24,5,-24,8,-24,15,-24});
    states[19] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-8,20,-9,53,-4,23,-10,28});
    states[20] = new State(new int[]{13,21,14,44,11,-27,12,-27,19,-27,20,-27,21,-27,22,-27,23,-27,24,-27,4,-27,10,-27,18,-27,17,-27,28,-27,3,-27,7,-27,5,-27,8,-27,15,-27});
    states[21] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-9,22,-4,23,-10,28});
    states[22] = new State(-30);
    states[23] = new State(-33);
    states[24] = new State(-11);
    states[25] = new State(-34);
    states[26] = new State(-35);
    states[27] = new State(-36);
    states[28] = new State(-37);
    states[29] = new State(new int[]{16,30});
    states[30] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46,17,-17},new int[]{-3,31,-2,33,-1,54,-5,49,-6,50,-7,51,-8,52,-9,53,-4,23,-10,28});
    states[31] = new State(new int[]{17,32});
    states[32] = new State(-13);
    states[33] = new State(new int[]{18,34,17,-16});
    states[34] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-1,35,-5,49,-6,50,-7,51,-8,52,-9,53,-4,23,-10,28});
    states[35] = new State(new int[]{23,13,24,36,18,-15,17,-15});
    states[36] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-5,37,-6,50,-7,51,-8,52,-9,53,-4,23,-10,28});
    states[37] = new State(new int[]{21,15,22,38,23,-19,24,-19,4,-19,10,-19,18,-19,17,-19,28,-19,3,-19,7,-19,5,-19,8,-19,15,-19});
    states[38] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-6,39,-7,51,-8,52,-9,53,-4,23,-10,28});
    states[39] = new State(new int[]{19,17,20,40,21,-22,22,-22,23,-22,24,-22,4,-22,10,-22,18,-22,17,-22,28,-22,3,-22,7,-22,5,-22,8,-22,15,-22});
    states[40] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-7,41,-8,52,-9,53,-4,23,-10,28});
    states[41] = new State(new int[]{11,19,12,42,19,-25,20,-25,21,-25,22,-25,23,-25,24,-25,4,-25,10,-25,18,-25,17,-25,28,-25,3,-25,7,-25,5,-25,8,-25,15,-25});
    states[42] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-8,43,-9,53,-4,23,-10,28});
    states[43] = new State(new int[]{13,21,14,44,11,-28,12,-28,19,-28,20,-28,21,-28,22,-28,23,-28,24,-28,4,-28,10,-28,18,-28,17,-28,28,-28,3,-28,7,-28,5,-28,8,-28,15,-28});
    states[44] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-9,45,-4,23,-10,28});
    states[45] = new State(-31);
    states[46] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-1,47,-5,49,-6,50,-7,51,-8,52,-9,53,-4,23,-10,28});
    states[47] = new State(new int[]{17,48,23,13,24,36});
    states[48] = new State(-38);
    states[49] = new State(new int[]{21,15,22,38,23,-20,24,-20,4,-20,10,-20,18,-20,17,-20,28,-20,3,-20,7,-20,5,-20,8,-20,15,-20});
    states[50] = new State(new int[]{19,17,20,40,21,-23,22,-23,23,-23,24,-23,4,-23,10,-23,18,-23,17,-23,28,-23,3,-23,7,-23,5,-23,8,-23,15,-23});
    states[51] = new State(new int[]{11,19,12,42,19,-26,20,-26,21,-26,22,-26,23,-26,24,-26,4,-26,10,-26,18,-26,17,-26,28,-26,3,-26,7,-26,5,-26,8,-26,15,-26});
    states[52] = new State(new int[]{13,21,14,44,11,-29,12,-29,19,-29,20,-29,21,-29,22,-29,23,-29,24,-29,4,-29,10,-29,18,-29,17,-29,28,-29,3,-29,7,-29,5,-29,8,-29,15,-29});
    states[53] = new State(-32);
    states[54] = new State(new int[]{23,13,24,36,18,-14,17,-14});
    states[55] = new State(-6);
    states[56] = new State(-7);
    states[57] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-1,58,-5,49,-6,50,-7,51,-8,52,-9,53,-4,23,-10,28});
    states[58] = new State(new int[]{23,13,24,36,28,24,3,4,7,57,5,61,8,68,15,74},new int[]{-12,59,-11,9,-4,10,-18,55,-13,56,-14,60,-15,67,-16,73});
    states[59] = new State(-40);
    states[60] = new State(-8);
    states[61] = new State(new int[]{16,62});
    states[62] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-1,63,-5,49,-6,50,-7,51,-8,52,-9,53,-4,23,-10,28});
    states[63] = new State(new int[]{17,64,23,13,24,36});
    states[64] = new State(new int[]{6,65});
    states[65] = new State(new int[]{28,24,3,4,7,57,5,61,8,68,15,74},new int[]{-12,66,-11,9,-4,10,-18,55,-13,56,-14,60,-15,67,-16,73});
    states[66] = new State(-41);
    states[67] = new State(-9);
    states[68] = new State(new int[]{16,69});
    states[69] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-1,70,-5,49,-6,50,-7,51,-8,52,-9,53,-4,23,-10,28});
    states[70] = new State(new int[]{17,71,23,13,24,36});
    states[71] = new State(new int[]{28,24,3,4,7,57,5,61,8,68,15,74},new int[]{-12,72,-11,9,-4,10,-18,55,-13,56,-14,60,-15,67,-16,73});
    states[72] = new State(-42);
    states[73] = new State(-10);
    states[74] = new State(new int[]{16,75});
    states[75] = new State(new int[]{28,24,25,25,26,26,27,27,29,29,16,46},new int[]{-1,76,-5,49,-6,50,-7,51,-8,52,-9,53,-4,23,-10,28});
    states[76] = new State(new int[]{17,77,23,13,24,36});
    states[77] = new State(-43);
    states[78] = new State(-3);

    rules[1] = new Rule(-20, new int[]{-19,2});
    rules[2] = new Rule(-19, new int[]{-18});
    rules[3] = new Rule(-17, new int[]{-12});
    rules[4] = new Rule(-17, new int[]{-17,10,-12});
    rules[5] = new Rule(-12, new int[]{-11});
    rules[6] = new Rule(-12, new int[]{-18});
    rules[7] = new Rule(-12, new int[]{-13});
    rules[8] = new Rule(-12, new int[]{-14});
    rules[9] = new Rule(-12, new int[]{-15});
    rules[10] = new Rule(-12, new int[]{-16});
    rules[11] = new Rule(-4, new int[]{28});
    rules[12] = new Rule(-11, new int[]{-4,9,-1});
    rules[13] = new Rule(-10, new int[]{29,16,-3,17});
    rules[14] = new Rule(-2, new int[]{-1});
    rules[15] = new Rule(-2, new int[]{-2,18,-1});
    rules[16] = new Rule(-3, new int[]{-2});
    rules[17] = new Rule(-3, new int[]{});
    rules[18] = new Rule(-1, new int[]{-1,23,-5});
    rules[19] = new Rule(-1, new int[]{-1,24,-5});
    rules[20] = new Rule(-1, new int[]{-5});
    rules[21] = new Rule(-5, new int[]{-5,21,-6});
    rules[22] = new Rule(-5, new int[]{-5,22,-6});
    rules[23] = new Rule(-5, new int[]{-6});
    rules[24] = new Rule(-6, new int[]{-6,19,-7});
    rules[25] = new Rule(-6, new int[]{-6,20,-7});
    rules[26] = new Rule(-6, new int[]{-7});
    rules[27] = new Rule(-7, new int[]{-7,11,-8});
    rules[28] = new Rule(-7, new int[]{-7,12,-8});
    rules[29] = new Rule(-7, new int[]{-8});
    rules[30] = new Rule(-8, new int[]{-8,13,-9});
    rules[31] = new Rule(-8, new int[]{-8,14,-9});
    rules[32] = new Rule(-8, new int[]{-9});
    rules[33] = new Rule(-9, new int[]{-4});
    rules[34] = new Rule(-9, new int[]{25});
    rules[35] = new Rule(-9, new int[]{26});
    rules[36] = new Rule(-9, new int[]{27});
    rules[37] = new Rule(-9, new int[]{-10});
    rules[38] = new Rule(-9, new int[]{16,-1,17});
    rules[39] = new Rule(-18, new int[]{3,-17,4});
    rules[40] = new Rule(-13, new int[]{7,-1,-12});
    rules[41] = new Rule(-14, new int[]{5,16,-1,17,6,-12});
    rules[42] = new Rule(-15, new int[]{8,16,-1,17,-12});
    rules[43] = new Rule(-16, new int[]{15,16,-1,17});
  }

  protected override void Initialize() {
    this.InitSpecialTokens((int)Tokens.error, (int)Tokens.EOF);
    this.InitStates(states);
    this.InitRules(rules);
    this.InitNonTerminals(nonTerms);
  }

  protected override void DoAction(int action)
  {
    switch (action)
    {
      case 2: // progr -> block
{ root = ValueStack[ValueStack.Depth-1].blVal; }
        break;
      case 3: // stlist -> statement
{ 
				CurrentSemanticValue.blVal = new BlockNode(ValueStack[ValueStack.Depth-1].stVal,CurrentLocationSpan); 
			}
        break;
      case 4: // stlist -> stlist, SEMICOLON, statement
{ 
				ValueStack[ValueStack.Depth-3].blVal.Add(ValueStack[ValueStack.Depth-1].stVal); 
				CurrentSemanticValue.blVal = ValueStack[ValueStack.Depth-3].blVal; 
			}
        break;
      case 5: // statement -> assign
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].stVal; }
        break;
      case 6: // statement -> block
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].blVal; }
        break;
      case 7: // statement -> loop
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].stVal; }
        break;
      case 8: // statement -> while
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].stVal; }
        break;
      case 9: // statement -> if
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].stVal; }
        break;
      case 10: // statement -> write
{ CurrentSemanticValue.stVal = ValueStack[ValueStack.Depth-1].stVal; }
        break;
      case 11: // ident -> ID
{ CurrentSemanticValue.eVal = new IdNode(ValueStack[ValueStack.Depth-1].sVal,CurrentLocationSpan); }
        break;
      case 12: // assign -> ident, ASSIGN, expr
{ CurrentSemanticValue.stVal = new AssignNode(ValueStack[ValueStack.Depth-3].eVal as IdNode, ValueStack[ValueStack.Depth-1].eVal,CurrentLocationSpan); }
        break;
      case 13: // func -> FUN, LPAREN, exprlistnull, RPAREN
{ CurrentSemanticValue.eVal = new FuncNode(ValueStack[ValueStack.Depth-4].oVal, ValueStack[ValueStack.Depth-2].eVal,CurrentLocationSpan); }
        break;
      case 18: // expr -> expr, AND, Q
{ CurrentSemanticValue.eVal = new BinOpNode(ValueStack[ValueStack.Depth-3].eVal,ValueStack[ValueStack.Depth-1].eVal,'&',CurrentLocationSpan); }
        break;
      case 19: // expr -> expr, OR, Q
{ CurrentSemanticValue.eVal = new BinOpNode(ValueStack[ValueStack.Depth-3].eVal,ValueStack[ValueStack.Depth-1].eVal,'|',CurrentLocationSpan); }
        break;
      case 20: // expr -> Q
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 21: // Q -> Q, EQ, S
{ CurrentSemanticValue.eVal = new BinOpNode(ValueStack[ValueStack.Depth-3].eVal,ValueStack[ValueStack.Depth-1].eVal,'=',CurrentLocationSpan); }
        break;
      case 22: // Q -> Q, NEQ, S
{ CurrentSemanticValue.eVal = new BinOpNode(ValueStack[ValueStack.Depth-3].eVal,ValueStack[ValueStack.Depth-1].eVal,'!',CurrentLocationSpan); }
        break;
      case 23: // Q -> S
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 24: // S -> S, MORE, P
{ CurrentSemanticValue.eVal = new BinOpNode(ValueStack[ValueStack.Depth-3].eVal,ValueStack[ValueStack.Depth-1].eVal,'>',CurrentLocationSpan); }
        break;
      case 25: // S -> S, LESS, P
{ CurrentSemanticValue.eVal = new BinOpNode(ValueStack[ValueStack.Depth-3].eVal,ValueStack[ValueStack.Depth-1].eVal,'<',CurrentLocationSpan); }
        break;
      case 26: // S -> P
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 27: // P -> P, PLUS, T
{ CurrentSemanticValue.eVal = new BinOpNode(ValueStack[ValueStack.Depth-3].eVal,ValueStack[ValueStack.Depth-1].eVal,'+',CurrentLocationSpan); }
        break;
      case 28: // P -> P, MINUS, T
{ CurrentSemanticValue.eVal = new BinOpNode(ValueStack[ValueStack.Depth-3].eVal,ValueStack[ValueStack.Depth-1].eVal,'-',CurrentLocationSpan); }
        break;
      case 29: // P -> T
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 30: // T -> T, MULT, F
{ CurrentSemanticValue.eVal = new BinOpNode(ValueStack[ValueStack.Depth-3].eVal,ValueStack[ValueStack.Depth-1].eVal,'*',CurrentLocationSpan); }
        break;
      case 31: // T -> T, DIV, F
{ CurrentSemanticValue.eVal = new BinOpNode(ValueStack[ValueStack.Depth-3].eVal,ValueStack[ValueStack.Depth-1].eVal,'/',CurrentLocationSpan); }
        break;
      case 32: // T -> F
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal; }
        break;
      case 33: // F -> ident
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal as IdNode; }
        break;
      case 34: // F -> INUM
{ CurrentSemanticValue.eVal = new IntNumNode(ValueStack[ValueStack.Depth-1].iVal,CurrentLocationSpan); }
        break;
      case 35: // F -> RNUM
{ CurrentSemanticValue.eVal = new RealNumNode(ValueStack[ValueStack.Depth-1].dVal,CurrentLocationSpan); }
        break;
      case 36: // F -> BNUM
{ CurrentSemanticValue.eVal = new BoolNumNode(ValueStack[ValueStack.Depth-1].bVal,CurrentLocationSpan); }
        break;
      case 37: // F -> func
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-1].eVal as FuncNode; }
        break;
      case 38: // F -> LPAREN, expr, RPAREN
{ CurrentSemanticValue.eVal = ValueStack[ValueStack.Depth-2].eVal; }
        break;
      case 39: // block -> BEGIN, stlist, END
{ CurrentSemanticValue.blVal = ValueStack[ValueStack.Depth-2].blVal; }
        break;
      case 40: // loop -> LOOP, expr, statement
{ CurrentSemanticValue.stVal = new LoopNode(ValueStack[ValueStack.Depth-2].eVal, ValueStack[ValueStack.Depth-1].stVal,CurrentLocationSpan);}
        break;
      case 41: // while -> WHILE, LPAREN, expr, RPAREN, DO, statement
{ CurrentSemanticValue.stVal = new WhileNode(ValueStack[ValueStack.Depth-4].eVal, ValueStack[ValueStack.Depth-1].stVal,CurrentLocationSpan); }
        break;
      case 42: // if -> IF, LPAREN, expr, RPAREN, statement
{ CurrentSemanticValue.stVal = new IfNode(ValueStack[ValueStack.Depth-3].eVal, ValueStack[ValueStack.Depth-1].stVal,CurrentLocationSpan);}
        break;
      case 43: // write -> WRITE, LPAREN, expr, RPAREN
{ CurrentSemanticValue.stVal = new WriteNode(ValueStack[ValueStack.Depth-2].eVal,CurrentLocationSpan); }
        break;
    }
  }

  protected override string TerminalToString(int terminal)
  {
    if (aliasses != null && aliasses.ContainsKey(terminal))
        return aliasses[terminal];
    else if (((Tokens)terminal).ToString() != terminal.ToString(CultureInfo.InvariantCulture))
        return ((Tokens)terminal).ToString();
    else
        return CharToString((char)terminal);
  }


}
}
