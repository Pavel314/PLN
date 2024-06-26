%using QUT.Gppg;
%using System;
%using PLNCompiler;
%using PLNCompiler.Compile;
%using PLNCompiler.Compile.Errors;

%namespace PLNCompiler.Syntax.Analysis
%scannertype PLNScanner


Alpha [А-Яа-яёЁA-Za-z_]
Digit  [0-9]
AlphaDigit {Alpha}|{Digit}
Shield `
INTNUM  {Digit}+
FLOATNUM {INTNUM}\.{INTNUM}

ID {Alpha}{AlphaDigit}* 
SHIELDID {Shield}{ID}
STRING2 \"[^"]*\"
STRING1 \'[^']*\'


OneLineComment  \/\/[^\r\n]*

%%
{INTNUM} { 
  yylval.stringValue = yytext;
  return (int)Tokens.INTNUM;
}
{FLOATNUM} { 
  yylval.stringValue = yytext;
  return (int)Tokens.FLOATNUM;
}

{ID} { 
 int res = PLNKeywords.GetToken(yytext);
//  if (res == (int)Tokens.ID)
	yylval.stringValue = yytext;
  return res;
}

{SHIELDID} {
	yylval.stringValue=yytext.Substring(1);
	return (int)Tokens.ID;
}

{STRING2} {
	yylval.stringValue=yytext;
	return (int)Tokens.STRING2;
}
{STRING1} {
	yylval.stringValue=yytext;
	return (int)Tokens.STRING1;
}

{OneLineComment} {
	yylval.stringValue=yytext;
	return (int)Tokens.ONELINECOMMENT;
}

[{]		{return (int)Tokens.BEGIN;}
[}]		{return (int)Tokens.END;}
"#"		{return (int)Tokens.SHARP;}
"%"		{return (int)Tokens.MOD;}
":"		{return (int)Tokens.DIVTRUNC;}
"=="	{return (int)Tokens.EQUALLY;}
"="		{return (int)Tokens.ASSIGN;}
"<~"	{return (int)Tokens.LSHIFT;}
"~>"	{return (int)Tokens.RSHIFT;}
";"		{return (int)Tokens.SEMICOLON;}
"("  	{return (int)Tokens.LPAREN;}
")"  	{return (int)Tokens.RPAREN;}
"["  	{return (int)Tokens.SLPAREN;}
"]"  	{return (int)Tokens.SRPAREN;}
"+"  	{return (int)Tokens.PLUS;}
"-"  	{return (int)Tokens.MINUS;}
"/"  	{return (int)Tokens.DIV;}
"*"  	{return (int)Tokens.MUL;}
","  	{return (int)Tokens.COLON;}
"."  	{return (int)Tokens.DOT;}
"<>"	{return (int)Tokens.NOTEQUALLY;}
">=" 	{return (int)Tokens.GREATEQLS;}
"<=" 	{return (int)Tokens.LESSEQLS;}
">"  	{return (int)Tokens.GREAT;}
"<"  	{return (int)Tokens.LESS;}
{Shield}  	{return (int)Tokens.SHIELD;}


[^ \t\r\n] {
if	(!OnScanError(new ScanErrorEventArgs(yytext,yylloc))) return (int)Tokens.EOF;
	return (int)Tokens.error;
}

%{
  yylloc = new Location(tokLin, tokCol, tokELin, tokECol);
%}
%%
public event ScanErrorCallback ScanError;

private bool OnScanError(ScanErrorEventArgs e)
{
	if (!ReferenceEquals(ScanError,null))
	return	ScanError(this,e);
	return false;
}

  public override void yyerror(string format, params object[] args) 
  {
	  throw SyntaxErrors.UnknownConstruction.CreateForYYERROR(format,args,yylloc);
  }
  

