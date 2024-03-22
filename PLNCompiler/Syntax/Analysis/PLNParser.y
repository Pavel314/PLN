%{
	public ProgramNode Root{get;private set;}
	public PLNParser(AbstractScanner<ValueType, Location> scanner) : base(scanner) { }
%}
%YYLTYPE Location
%output = PLNParser.cs
%parsertype PLNParser

%union {  
			public string stringValue;
			//......
			public DirectivesNode DirectivesValue;
			public DirectiveNode DirectiveValue;
			//......
			public ImportNode ImportValue;
			public ImportsNode ImportsValue;
			public UsingsNode UsingsValue;
			public UsingNode UsingValue;
			//......
			public TypeNameNode TypeNameValue;
			public TypeCollection TypeValueCollection; 
			public TypeNode TypeValue;
			public IdentiferNode IdentiferValue;
			public int IntValue;
			//......
			public BlockNode BlockValue;
			public StatementNode StatementValue;
			public LoopNode LoopValue;
			public VarDefineNode VarDefineValue;
			public IdentiferCollection IdentiferValueCollection;
			public FieldNode FieldValue;
			public MemberNode MemberValue;
			//.....
			public MethodInfo MethodValue;
			public FactArgumentNode FactArgumentValue;
			public ExpressionArgumentNode ExpressionArgumentValue;
			//.....
			public ConstNode ConstValue;
			public ExpressionNode ExpressionValue;
			public MethodArgumentsCollection MethodArgumentsValueCollection;
			public IndexerArgumentsCollection IndexerArgumentsValueCollection;
			
			//.....
       }

%using System.IO;
%using PLNCompiler.Syntax.SyntaxTree;

%namespace PLNCompiler.Syntax.Analysis
%start program

%token
 
IMPORT, USING,

BEGIN, END, SEMICOLON ";",
ASSIGN "=", ARRAY, IF, ELSE
WHILE,DO, LOOP, STEP, BREAK, CONTINUE, ALL, GOTO
PROCEDURE, FUNCTION, RETURNN,
AUTO, DOT ".", COLON ","

PLUS "+", MINUS "-", DIV "/", MUL "*",
EQUALLY "==", NOTEQUALLY "<>",
GREAT ">", LESS "<", GREATEQLS ">=", LESSEQLS "<=",
AND, OR, XOR, LEFTSHIFT "<<", RIGHTSHIFT ">>"
DIVTRUNC ":", MOD "%"
NEW, INVERSE,
LPAREN "(", RPAREN ")", SLPAREN "[", SRPAREN "]",

TRUE, FALSE, NULL,

REF,
TYPEOF,
SHIELD,SHARP "#"
IS, AS





%token <stringValue> INTNUM, FLOATNUM, ID, ALL, DISABLE_SYSTEM_LIBRARY,CONSOLE_APPLICATION, WINDOWS_APPLICATION, STRING2, STRING1, ONELINECOMMENT

%type <DirectivesValue> directiveSection directives
%type <DirectiveValue> directive directiveSpelling constDirective
//......
%type <ImportsValue> importSection imports
%type <ImportValue> import
%type <UsingsValue> usingSection usings using manyUsing
%type <UsingValue> usingSing
//......
%type <TypeValue> type arrayType
%type <TypeNameValue> genericTypeSing genericType
%type <TypeValueCollection> genericArgs leftGeneric relaxLeftGeneric
%type <IdentiferValue> identifer LabelName
%type <IntValue> dimension
//.....
%type <StatementValue> shortStatement loopSpecial simpleStatement fullIf statement goto assign varsDefine varDefineAssign
%type <LoopValue> loop
%type <BlockValue> block statements

//....
%type <ConstValue> intNumber
%type <ExpressionValue> constant factor mathExpression expression if
%type <MethodArgumentsValueCollection> methodArgs
%type <IndexerArgumentsValueCollection> indexsArgs indexs

%type <IdentiferValueCollection> namespace vars
%type <FieldValue> field genericField refField refFields
%type <MemberValue> member members method 
//....
%type <MethodValue> methodRight
%type <FactArgumentValue> methodArg 
%type <ExpressionArgumentValue> expressionArg


%left EQUALLY NOTEQUALLY GREAT LESS GREATEQLS LESSEQLS
%left PLUS MINUS OR XOR
%left MUL DIV DIVTRUNC MOD AND LSHIFT RSHIFT
//%left INVERSE ???
%left UNARYMINUS UNARYPLUS INVERSE NEW CAST

%%	

program				: directiveSection importSection usingSection block { Root = new ProgramNode($1,$2,$3,$4,@$); }
					;

//======================================================
//					DIRECTIVE SECTION
//======================================================

directiveSection	: directives { $$ = $1; }
					| empty { $$ = null; }
					;

directives			: directiveSpelling { $$ = new DirectivesNode($1,@$); }
					| directives directiveSpelling {$1.Directives.Add($2); $$ = $1;}
					;
					
directiveSpelling	: SHARP SLPAREN directive SRPAREN { $$ = $3; }
					;
					
directive			: constDirective { $$ = $1; }
					;
					
constDirective		: DISABLE_SYSTEM_LIBRARY { $$ = new ConstDirectiveNode($1,DirectiveKind.DisableSystemLibrary,@$); }
					| CONSOLE_APPLICATION { $$ = new ConstDirectiveNode($1,DirectiveKind.ConsoleApplication,@$); }
					| WINDOWS_APPLICATION { $$ = new ConstDirectiveNode($1,DirectiveKind.WindowApplication,@$); }
					;
					
//======================================================
//					IMPORT SECTION
//======================================================
					
importSection		: imports { $$ = $1; }
					| empty { $$ = null; }
					;
					
imports				: import { $$ = new ImportsNode($1); }
					| imports import { $1.Imports.Add($2); $$ = $1; }
					;
					
import				: IMPORT STRING2 { $$ = new ImportNode($2,@2); }
					; 

//======================================================
//					USING SECTION
//======================================================
					
usingSection		: usings { $$ = $1; }
					| empty { $$ = null; }
					;

usings				: using { $$ = $1; }
					| usings using { $1.Usings.AddRange($2.Usings); $$ = $1; }
					;
					
using				: USING manyUsing SEMICOLON { $$ = $2; }
					;

manyUsing			: usingSing { $$ = new UsingsNode($1); }
					| manyUsing COLON usingSing { $1.Usings.Add($3); $$ = $1; }
					;

usingSing			: namespace { $$ = new UsingNode($1,@$); }
					;

namespace			: identifer { $$ = new IdentiferCollection(); $$.Add($1); }
					| namespace DOT identifer { $$ = $1; $$.Add($3); }
					;
//======================================================
//					STATEMENT SECTION
//======================================================
					
block				: BEGIN statements END { $$ = $2; }
					| BEGIN empty END { $$ = new BlockNode(@$); }	
					;

statements			: statement { $$ = new BlockNode($1,@$); } 
					| statements statement { $1.Statements.Add($2); $$ = $1; }
					;

simpleStatement		: shortStatement SEMICOLON { $$ = $1; }
					| block { $$ = $1; }
					;
					
statement			: simpleStatement { $$ = $1; }
					| if fullIf ELSE statement { $$ = new IfElseNode($1,$2,$4,@$); }
					| if statement { $$ = new IfElseNode($1,$2,@$); }
					| loop { $$ = $1; }
					| LabelName DIVTRUNC statement { $$ = new LabelDefineNode($1,$3,@$); }
					| ONELINECOMMENT { $$ = new OneLineCommentNode($1,@$); }
					;

loop				: WHILE LPAREN expression RPAREN statement { $$ = new WhileLoopNode($3,$5,@$); }
					| DO statement WHILE LPAREN expression RPAREN { $$ = new DoWhileLoopNode($2,$5,@$); }
					| LOOP LPAREN shortStatement SEMICOLON expression  SEMICOLON shortStatement RPAREN statement { $$ = new BuiltInLoopNode($3,$5,$7,$9,@$); }
					;
					
					
fullIf				: simpleStatement { $$ = $1; }
					| if fullIf ELSE fullIf { $$ = new IfElseNode($1,$2,$4,@$); }
					;
					
if					: IF LPAREN expression RPAREN { $$ = $3; }
					;
					
shortStatement		: varsDefine {$$ = $1; }
					| assign {$$ = $1; }
					| members {$$ = $1; }
					| goto { $$ = $1; } 
					| loopSpecial { $$ = $1; }
					| empty { $$ = null; }
					;

loopSpecial			: BREAK { $$ = new BreakNode(true,@$); }
					| BREAK intNumber { $$ = new BreakNode($2,@$); }
					| BREAK ALL { $$ = new BreakNode(false,@$); }
					| CONTINUE { $$ = new ContinueNode(true, @$); } 
					| CONTINUE intNumber { $$ = new ContinueNode($2,@$); } 
					| CONTINUE ALL {$$ = new ContinueNode(false, @$);}
					;
					
goto				: GOTO LabelName { $$ = new GOTOLabelNode($2,@$); }
					;

LabelName			: identifer { $$ = $1; }
					| INTNUM { $$ = new IdentiferNode($1,@$); }
					;
					
varsDefine			: PLUS type vars { $$ = new VarDefineNode($2,$3,@$); }
					| varDefineAssign { $$ = $1; }
					;
  
varDefineAssign		: PLUS type identifer ASSIGN expression { $$ = new VarDefineAssignNode($2,$3,$5,@$); }
					| PLUS AUTO identifer ASSIGN expression { $$ = new VarDefineAssignAutoNode($3,$5,@$); }
					;
					
vars				: identifer { $$ = new IdentiferCollection(); $$.Add($1); }
					| vars COLON identifer { $$ = $1; $1.Add($3); }
					;
	
assign				: members ASSIGN expression { $$ = new AssignNode($1,$3,AssignKind.Assign,@$); }
					;
					
					
members				: member { $$ = $1; }
					| member DOT members { $$ = $1; $$.Child = $3; }
					;
					
member				: method { $$ = $1; }
					| field { $$ = $1; } 	
					;
					
field				: genericField { $$ = $1 ; }
					| identifer indexs { $$ = new FieldNode($1,null,$2,@$) ; }
					;
	
genericField		: identifer leftGeneric { $$ = new FieldNode($1,$2,null,@$) ; }
					;
	
method				: identifer relaxLeftGeneric methodRight { $$ = new MethodNode($1,$2,$3,@$) ; }
					| TYPEOF LPAREN type RPAREN { $$ = new TypeOfNode($3,@$); }
					;
				
//========================================
//			Statement.ACCUMULATION
//========================================
				
relaxLeftGeneric	: leftGeneric { $$ = $1; }
					| empty { $$ = null; }
					;	
	
leftGeneric			: SHIELD LESS genericArgs GREAT { $$ = $3; } 
					;
					
indexs				: SLPAREN indexsArgs SRPAREN { $$ = $2;}
					| empty { $$ = null; }
					;

indexsArgs			: expressionArg { $$ = new IndexerArgumentsCollection($1); }
					| indexsArgs COLON expressionArg { $1.Add($3); $$ = $1; }
					;
					
methodRight			: LPAREN methodArgs RPAREN indexs { $$ = new MethodInfo($2,$4) ; }
					| LPAREN RPAREN indexs{ $$ = new MethodInfo(null,$3) ; }
					;
					
methodArgs			: methodArg { $$  = new MethodArgumentsCollection($1); }
					| methodArgs COLON methodArg { $1.Add($3); $$ = $1; }
					;
					//: expression { $$ = new ExpressionCollection(); $$.Add($1); }
					//| methodArgs COLON expression{ $1.Add($3); $$ = $1; }
					
expressionArg		: expression { $$ = new ExpressionArgumentNode($1,@$); }
					;
					
methodArg			: expressionArg { $$ = $1; } 
					| REF refFields { $$ = new RefArgumentNode($2,@$); }
					;

refField			: identifer relaxLeftGeneric { $$ = new FieldNode($1,$2,null,@$) ; }
					;
					
refFields			: refField { $$ = $1; }
					| refField DOT refFields { $$ = $1; $$.Child = $3; }
					;
					
//========================================
//			Statement.EXPRESSION
//========================================
expression			: mathExpression { $$ = $1; }
					| expression AS type { $$ = new AsCastNode($1,$3,@$); }
					| expression IS type { $$ = new IsCastNode($1,$3,@$); }
					;

mathExpression		//Binary
					: mathExpression EQUALLY mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.Equally,@$); }
					| mathExpression NOTEQUALLY mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.NotEqually,@$); }
					| mathExpression GREAT mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.Great,@$); }
					| mathExpression LESS mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.Less,@$); }
					| mathExpression GREATEQLS mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.GreatEqls,@$); }
					| mathExpression LESSEQLS mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.LessEqls,@$); }
					| mathExpression PLUS mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.Plus,@$); }
					| mathExpression MINUS mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.Minus,@$); }
					| mathExpression MUL mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.Mul,@$); }
					| mathExpression DIV mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.Div,@$); }
					| mathExpression DIVTRUNC mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.DivTrunc,@$); }
					| mathExpression MOD mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.Mod,@$); }
					| mathExpression AND mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.And,@$); }
					| mathExpression OR mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.Or,@$); }
					| mathExpression XOR mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.Xor,@$); }
					| mathExpression LSHIFT mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.LShift,@$); }
					| mathExpression RSHIFT mathExpression { $$ = new BinaryNode($1,$3,BinaryOperation.RShift,@$); }
					//Unary
					| PLUS mathExpression %prec UNARYPLUS { $$ = new UnaryNode($2,UnaryOperation.Plus,@$); }
					| MINUS mathExpression %prec UNARYMINUS { $$ = new UnaryNode($2,UnaryOperation.Minus,@$); }
					| INVERSE mathExpression { $$ = new UnaryNode($2,UnaryOperation.Inverse,@$); }
					| NEW genericType methodRight  { $$ = new NewUnaryNode($2,$3,NewUnaryKind.Object,@$); }
					| NEW ARRAY LPAREN type COLON methodArgs RPAREN indexs {$$ = new NewUnaryNode($4,$6,$8,NewUnaryKind.Array,@$);}
					| SHIELD LPAREN type RPAREN mathExpression %prec CAST { $$ = new ExplicitCastNode($3,$5,@$); }				
					//Other	
					| factor { $$ = $1; }
					;
		
factor				: constant { $$ = $1; } 
					| constant DOT members { $$ = new MemberAccessNode($1,$3,@$); }
					| members { $$ = new MemberAccessNode(null,$1,@$); }		
					| LPAREN expression RPAREN { $$=$2; }
					| LPAREN expression RPAREN DOT members { $$ = new MemberAccessNode($2,$5,@$); }
					;
					
					
constant			: intNumber { $$ = $1; }
					| FLOATNUM { $$ = new ConstNode(new Constant(ConstKind.FloatNumber,$1),@$); }
					| STRING2 { $$ = new ParsableConstNode(ParsableConstant.CreatePLNString2($1),@$); }
					| STRING1 { $$ = new ParsableConstNode(ParsableConstant.CreatePLNString1($1),@$); }
					| TRUE { $$ = new ParsableConstNode(new ParsableConstant(true),@$); }
					| FALSE { $$ = new ParsableConstNode(new ParsableConstant(false),@$); }
					| NULL { $$ = new ParsableConstNode(ParsableConstant.CreateForNull(),@$); }
					;

intNumber			: INTNUM { $$ = new ConstNode(new Constant(ConstKind.IntNumber,$1),@$); }
					;
					
//======================================================
//						TYPE SECTION
//======================================================		
	
arrayType			: ARRAY type { $$ = new ArrayTypeNode($2,@$); }
					| ARRAY SLPAREN SRPAREN type { $$ = new ArrayTypeNode($4,@$); }
					| ARRAY SLPAREN dimension SRPAREN type{ $$ = new ArrayTypeNode($5,$3,@$); }
					;
					
dimension			: COLON { $$ = 2; }
					| dimension COLON { $$ = $1 + 1; }
					;
	
type				: genericType { $$ = $1; }
					| arrayType { $$ = $1 ; }
					;
					
genericType			: genericTypeSing { $$ = $1; }
					| genericTypeSing DOT genericType { $$ = $1; $$.Child = $3; }
					;
				
genericArgs			: type { $$ =  new TypeCollection(); $$.Add($1); }		
					| genericArgs COLON type {$1.Add($3);  $$=$1; } 
					;
					
genericTypeSing		: identifer LESS genericArgs GREAT { $$ = new TypeNameNode($1,$3,@$); }
					| identifer SHIELD LESS genericArgs GREAT { $$ = new TypeNameNode($1,$4,@$); }
					| identifer { $$ = new TypeNameNode($1,null,@$); }
					;	

identifer   		: ID { $$ = new IdentiferNode($1,@1); }	
					| ALL { $$ = new IdentiferNode($1,@1); }
					| DISABLE_SYSTEM_LIBRARY { $$ = new IdentiferNode($1,@1); }
					| CONSOLE_APPLICATION { $$ = new IdentiferNode($1,@1);  }
					| WINDOWS_APPLICATION { $$ = new IdentiferNode($1,@1);  }
					;	
					
empty				:
					;
				
%%
