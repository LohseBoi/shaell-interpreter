lexer grammar ShaellLexer;

options { superClass=ShaellLexerBase; }

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
ARGS: 'args';
LET: 'let';
LPAREN: '(';
RPAREN: ')';
LCURL: '{';
STRINGCLOSEBRACE: {this.IsInTemplateString()}? '}' -> popMode;
RCURL: '}';
LSQUACKET: '[';
RSQUACKET: ']';
COLON: ':';
DEREF: '@';
DOLLAR: '$';
LNOT: '!';
BNOT: '~';
MULT: '*';
POW: '**';
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
POWEQ: '**=';
RSHIFTEQ: '>>=';
LSHIFTEQ: '<<=';
FALSE: 'false';
TRUE: 'true';
NULL: 'null';
DQUOTE: '"' {this.IncreaseDepth();} -> pushMode(STRING_MODE);
IDENTIFIER: [a-zA-Z_.$][a-zA-Z0-9_.$]*;
NUMBER: [0-9]+('.'[0-9]+)?;
SQUOTE: '\'';
COMMENT : '#' ~('\n')* (('\r'? '\n') | EOF) -> skip;
MULTILINECOMMENT : '/*'(.)*? (MULTILINECOMMENT | .)*? '*/' -> skip;
WHITESPACE: (' ' | '\t' | '\r' | '\n')+ -> skip;
LAMBDA: '=>';

mode STRING_MODE;
INTERPOLATION: '${' -> pushMode(DEFAULT_MODE);
NEWLINE: '\\n';
END_STRING: '"' {this.DecreaseDepth();} -> popMode;
TEXT: ~('"'|'\n'|'$')+;