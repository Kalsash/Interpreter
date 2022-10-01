%{
// Эти объявления добавляются в класс GPPGParser, представляющий собой парсер, генерируемый системой gppg
    public BlockNode root; // Корневой узел синтаксического дерева 
    public Parser(AbstractScanner<ValueType, LexLocation> scanner) : base(scanner) { }
%}

%output = SimpleYacc.cs

%union { 
			public double dVal; 
			public int iVal; 
			public string sVal; 
			public Node nVal;
			public ExprNode eVal;
			public StatementNode stVal;
			public BlockNode blVal;
       }

%using ProgramTree;

%namespace SimpleParser

%token BEGIN END WHILE DO CYCLE ASSIGN SEMICOLON PLUS MINUS	MULT DIV LPAREN RPAREN COLUMN
%token <iVal> INUM 
%token <dVal> RNUM 
%token <sVal> ID

%type <eVal> expr ident T F
%type <stVal> assign statement cycle while
%type <blVal> stlist block

%%

progr   : block { root = $1; }
		;

stlist	: statement 
			{ 
				$$ = new BlockNode($1); 
			}
		| stlist SEMICOLON statement 
			{ 
				$1.Add($3); 
				$$ = $1; 
			}
		;

statement: assign { $$ = $1; }
		| block   { $$ = $1; }
		| cycle   { $$ = $1; }
		| while   { $$ = $1; }
	;

ident 	: ID { $$ = new IdNode($1); }	
		;
	
assign 	: ident ASSIGN expr { $$ = new AssignNode($1 as IdNode, $3); }
		;

expr	: expr PLUS T { $$ = new BinOpNode($1,$3,'+'); }
		| expr MINUS T { $$ = new BinOpNode($1,$3,'-'); }
		| T { $$ = $1; }
		;
		
T 		: T MULT F { $$ = new BinOpNode($1,$3,'*'); }
		| T DIV F { $$ = new BinOpNode($1,$3,'/'); }
		| F { $$ = $1; }
		;
		
F 		: ident  { $$ = $1 as IdNode; }
		| INUM { $$ = new IntNumNode($1); }
		| LPAREN expr RPAREN { $$ = $2; }
		;

block	: BEGIN stlist END { $$ = $2; }
		;

cycle	: CYCLE expr statement { $$ = new CycleNode($2, $3); }
		;
while	: WHILE expr DO statement { $$ = new WhileNode($2, $4); }
		;
	
%%

