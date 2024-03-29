%using SimpleParser;
%using QUT.Gppg;
%using System.Linq;

%namespace SimpleScanner

Alpha 	[a-zA-Z_]
Digit   [0-9] 
AlphaDigit {Alpha}|{Digit}
INTNUM  {Digit}+
REALNUM {INTNUM}\.{INTNUM}
BOOLNUM true|false
ID {Alpha}{AlphaDigit}*
FUN [@]{ID}

%%
{INTNUM} { 
  yylval.iVal = int.Parse(yytext); 
  return (int)Tokens.INUM; 
}

{REALNUM} { 
  yylval.dVal = double.Parse(yytext,new System.Globalization.CultureInfo("en-US")); 
  return (int)Tokens.RNUM;
}
{BOOLNUM} { 
  yylval.bVal = bool.Parse(yytext); 
  return (int)Tokens.BNUM; 
}

{ID}  { 
  int res = ScannerHelper.GetIDToken(yytext);
  if (res == (int)Tokens.ID)
	yylval.sVal = yytext;
  return res;
}
{FUN} { 
   yylval.oVal = yytext; 
return (int)Tokens.FUN;
}

":=" { return (int)Tokens.ASSIGN; }
";" { return (int)Tokens.SEMICOLON; }
"+" { return (int)Tokens.PLUS; }
"-" { return (int)Tokens.MINUS; }
"*" { return (int)Tokens.MULT; }
"/" { return (int)Tokens.DIV; }
"(" { return (int)Tokens.LPAREN; }
")" { return (int)Tokens.RPAREN; }
"," { return (int)Tokens.COLUMN; }
">" { return (int)Tokens.MORE; }
"<" { return (int)Tokens.LESS; }
"=" { return (int)Tokens.EQ; }
"!" { return (int)Tokens.NEQ; }
"&" { return (int)Tokens.AND; }
"|" { return (int)Tokens.OR; }


[^ \r\n] {
	LexError();
}

%{
  yylloc = new LexLocation(tokLin, tokCol, tokELin, tokECol);
%}

%%

public override void yyerror(string format, params object[] args) // ��������� �������������� ������
{
  var ww = args.Skip(1).Cast<string>().ToArray();
  string errorMsg = string.Format("({0},{1}): ��������� {2}, � ��������� {3}", yyline, yycol, args[0], string.Join(" ��� ", ww));
  throw new SyntaxException(errorMsg);
}

public void LexError()
{
  string errorMsg = string.Format("({0},{1}): ����������� ������ {2}", yyline, yycol, yytext);
  throw new LexException(errorMsg);
}

class ScannerHelper 
{
  private static Dictionary<string,int> keywords;

  static ScannerHelper() 
  {
    keywords = new Dictionary<string,int>();
    keywords.Add("begin",(int)Tokens.BEGIN);
    keywords.Add("end",(int)Tokens.END);
    keywords.Add("loop",(int)Tokens.LOOP);
	keywords.Add("while",(int)Tokens.WHILE);
	keywords.Add("if",(int)Tokens.IF);
	keywords.Add("print",(int)Tokens.PRINT);
  }
  public static int GetIDToken(string s)
  {
	if (keywords.ContainsKey(s.ToLower()))
	  return keywords[s];
	else
      return (int)Tokens.ID;
  }
  
}
