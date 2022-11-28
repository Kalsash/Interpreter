%{
// Эти объявления добавляются в класс GPPGParser, представляющий собой парсер, генерируемый системой gppg
    public BlockNode root; // Корневой узел синтаксического дерева 
    public Parser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
%}

%output = SimpleYacc.cs

%union { 
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
			public ExprList exl;
       }

%using ProgramTree;

%namespace SimpleParser

%token BEGIN END WHILE LOOP IF ASSIGN SEMICOLON PLUS MINUS MULT DIV PRINT LPAREN RPAREN COLUMN MORE LESS EQ NEQ AND OR 
%token <iVal> INUM 
%token <dVal> RNUM
%token <bVal> BNUM
%token <sVal> ID
%token <oVal> FUN


%type <eVal> expr exprlist exprlistnull ident Q S P T F func 
%type <stVal> assign statement loop while if print 
%type <blVal> stlist block
%type <exl> exprlist exprlistnull

%%

progr   : block { root = $1; }
		;

stlist	: statement 
			{ 
				$$ = new BlockNode($1,@$); 
			}
		| stlist SEMICOLON statement 
			{ 
				$1.Add($3); 
				$$ = $1; 
			}
		;

statement: assign { $$ = $1; }
		| block   { $$ = $1; }
		| loop   { $$ = $1; }
		| while   { $$ = $1; }
		| if   { $$ = $1; }
		| print   { $$ = $1; }
	;

ident 	: ID { $$ = new IdNode($1,@$); }	
		;
	
assign 	: ident ASSIGN expr { $$ = new AssignNode($1 as IdNode, $3,@$); }
		;

func    : FUN LPAREN exprlistnull RPAREN { $$ = new FuncNode($1, $3,@$); }
		;

exprlist: expr { $$ = new ExprList(); $$.Add($1);}
		| exprlist COLUMN expr{ $$ = $1; $1.Add($3); }		
		;

exprlistnull: exprlist
			| 
			;

expr	: expr AND Q { $$ = new BinOpNode($1,$3,'&',@$); }
		| expr OR  Q { $$ = new BinOpNode($1,$3,'|',@$); }
		| Q { $$ = $1; }
		;

Q    	: Q EQ S { $$ = new BinOpNode($1,$3,'=',@$); }
		| Q NEQ S { $$ = new BinOpNode($1,$3,'!',@$); }
		| S { $$ = $1; }
		;

S	    : S MORE P { $$ = new BinOpNode($1,$3,'>',@$); }
		| S LESS P { $$ = new BinOpNode($1,$3,'<',@$); }
		| P { $$ = $1; }
		;

P       : P PLUS T { $$ = new BinOpNode($1,$3,'+',@$); }
		| P MINUS T { $$ = new BinOpNode($1,$3,'-',@$); }
		| T { $$ = $1; }
		;

T 		: T MULT F { $$ = new BinOpNode($1,$3,'*',@$); }
		| T DIV F { $$ = new BinOpNode($1,$3,'/',@$); }
		| F { $$ = $1; }
		;
		
F 		: ident  { $$ = $1 as IdNode; }
		| INUM { $$ = new IntNumNode($1,@$); }
		| RNUM { $$ = new RealNumNode($1,@$); }
		| BNUM { $$ = new BoolNumNode($1,@$); }
		| func  { $$ = $1 as FuncNode; }
		| LPAREN expr RPAREN { $$ = $2; }
		;

block	: BEGIN stlist END { $$ = $2; }
		;

loop	: LOOP expr statement { $$ = new LoopNode($2, $3,@$);}
		;

while	: WHILE LPAREN expr RPAREN statement { $$ = new WhileNode($3, $5,@$); }
		;

if		: IF LPAREN expr RPAREN statement { $$ = new IfNode($3, $5,@$);}
		;

print	: PRINT LPAREN expr RPAREN { $$ = new PrintNode($3,@$); }
		;
	
%%

