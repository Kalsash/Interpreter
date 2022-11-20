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
       }

%using ProgramTree;

%namespace SimpleParser

%token BEGIN END WHILE DO LOOP ASSIGN SEMICOLON PLUS MINUS	MULT DIV WRITE LPAREN RPAREN COLUMN
%token <iVal> INUM 
%token <dVal> RNUM
%token <bVal> BNUM
%token <sVal> ID
%token <oVal> FUN


%type <eVal> expr exprlist exprlistnull ident T F func 
%type <stVal> assign statement loop while write 
%type <blVal> stlist block

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
		| write   { $$ = $1; }
	;

ident 	: ID { $$ = new IdNode($1,@$); }	
		;
	
assign 	: ident ASSIGN expr { $$ = new AssignNode($1 as IdNode, $3,@$); }
		;

func    : FUN LPAREN exprlistnull RPAREN { $$ = new FuncNode($1, $3,@$); }
		;

exprlist: expr
		| exprlist COLUMN expr
		;

exprlistnull: exprlist
			| 
			;

expr	: expr PLUS T { $$ = new BinOpNode($1,$3,'+',@$); }
		| expr MINUS T { $$ = new BinOpNode($1,$3,'-',@$); }
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

while	: WHILE expr DO statement { $$ = new WhileNode($2, $4,@$); }
		;

write	: WRITE LPAREN expr RPAREN { $$ = new WriteNode($3,@$); }
		;
	
%%

