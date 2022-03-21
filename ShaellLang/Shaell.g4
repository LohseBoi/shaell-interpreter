grammar Shaell;

IF: 'if';
THEN: 'then';
ELSEIF: 'elseif';
ELSE: 'else';
END: 'end';
WHILE: 'while';
DO: 'do';
FOREACH: 'foreach';
FOR: 'for';
SWITCH: 'switch';
ON: 'on';
IN: 'in';
CASE: 'case';
RETURN: 'return';
CONTINUE: 'continue';
BREAK: 'break';
FUNCTION: 'fn';
GLOBAL: 'global';
ASYNC: 'async';
DEFER: 'defer';
LPAREN: '(';
RPAREN: ')';
LCURL: '{';
RCURL: '}';
LSQUACKET: '[';
RSQUACKET: ']';
COLON: ':';
DEREF: '@';
DOLLAR: '$';
LNOT: '!';
BNOT: '~';
MULT: '*';
DIV: '/';
MOD: '%';
PLUS: '+';
MINUS: '-';
LSHIFT: '<<';
RSHIFT: '>>';
LT: '<';
GT: '>';
GEQ: '>=';
LEQ: '<=';
EQ: '==';
NEQ: '!=';
BAND: '&';
BXOR: '^';
BOR: '|';
LAND: '&&';
LOR: '||';
NULLCOAL: '??';
PIPE: '->';
ASSIGN: '=';
COMMA: ',';
PLUSEQ: '+=';
MINUSEQ: '-=';
MULTEQ: '*=';
DIVEQ: '/=';
BANDEQ: '&=';
BXOREQ: '^=';
BOREQ: '|=';
MODEQ: '%=';
RSHIFTEQ: '>>=';
LSHIFTEQ: '<<=';
FILEIDENTFIER: [a-zA-Z_.][a-zA-Z0-9_.$]*;
VARIDENTFIER: DOLLAR [a-zA-Z0-9_.$]*;
NUMBER: [0-9]+(.[0-9]+)?;
DQUOTE: '"';
SQUOTE: '\'';
FALSE: 'false';
TRUE: 'true';
WHITESPACE: (' ' | '\t' | '\n')+ -> skip;

/*
Lacks functions and comments
*/

prog: stmts;
stmts: stmt*;
stmt: ifstmt | forLoop | whileLoop | returnStatement | functionDefinition | expr;
expr:  
	identifier
	| LPAREN expr RPAREN 
	|<assoc=right> DEREF expr 
	|<assoc=right> LNOT expr 
	|<assoc=right> BNOT expr 
	|<assoc=right> MINUS expr 
	| expr COLON expr 
	| expr LSQUACKET expr RSQUACKET 
	| expr LPAREN innerArgList RPAREN
	| expr MULT expr
	| expr DIV expr
	| expr PLUS expr
	| expr LT expr
	| expr LEQ expr
	| expr GT expr
	| expr GEQ expr
	| expr EQ expr	
	| expr NEQ expr
	| expr LAND expr
	| expr LOR expr
	| expr PIPE expr
	|<assoc=right> expr ASSIGN expr;
innerArgList: (expr (COMMA expr)*)?;
identifier: FILEIDENTFIER | VARIDENTFIER;
ifstmt: IF expr THEN stmts (ELSE stmts)? END;
forLoop: FOR expr COMMA expr COMMA expr DO stmts END;
whileLoop: WHILE expr DO stmts END;
functionDefinition: FUNCTION VARIDENTFIER LPAREN innerArgList RPAREN stmts END;
returnStatement: RETURN expr;

