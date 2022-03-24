using System;
using System.Collections.Generic;
using System.Linq;

namespace ShaellLang;

public class ScopeContext
{
    private Dictionary<string, RefValue> _values;

    public ScopeContext()
    {
        _values = new Dictionary<string, RefValue>();
    }
    
    public RefValue GetValue(string key)
    {
        bool found = _values.TryGetValue(key, out RefValue foundValue);

        return found ? foundValue : null;
    }

    public RefValue SetValue(string key, IValue val)
    {
        RefValue rv = new RefValue(val);
        _values[key] = rv;
        return rv;
    }
}

public class ScopeManager
{
    private List<ScopeContext> _scopes;

    public ScopeManager()
    {
        _scopes = new List<ScopeContext>();
    }

    public ScopeManager CopyScopes()
    {
        ScopeManager rv = new ScopeManager();
        foreach (var scope in _scopes)
        {
            rv.PushScope(scope);
        }

        return rv;
    }
    
    public RefValue GetValue(string key)
    {
        foreach (var scope in _scopes)
        {
            var rv = scope.GetValue(key);
            if (rv != null)
            {
                return rv;
            }
        }

        return null;
    }

    public RefValue SetValue(string key, IValue val)
    {
        return _scopes.Last().SetValue(key, val);
    }

    public void PushScope(ScopeContext scopeContext)
    {
        _scopes.Add(scopeContext);
    }

    public void PopScope()
    {
        if (_scopes.Count > 0)
        {
            _scopes.RemoveAt(_scopes.Count - 1);
        }

    }
}

public class ExecutionVisitor : ShaellBaseVisitor<IValue>
{
    private ScopeManager _scopeManager;
    private ScopeContext _globalScope;
    private bool _shouldReturn;
    public ExecutionVisitor()
    {
        _globalScope = new ScopeContext();
        _scopeManager = new ScopeManager();
        _scopeManager.PushScope(_globalScope);
        _shouldReturn = false;
    }
    
    public ExecutionVisitor(ScopeContext globalScope, ScopeManager scopeManager)
    {
        _globalScope = globalScope;
        _scopeManager = scopeManager;
        _shouldReturn = false;
    }

    public void SetGlobal(string key, IValue val)
    {
        _globalScope.SetValue(key, val);
    }
    
    public override IValue VisitProg(ShaellParser.ProgContext context)
    {
        VisitStmts(context.stmts());
        return null;
    }
    
    public override IValue VisitStmts(ShaellParser.StmtsContext context)
    {
        _scopeManager.PushScope(new ScopeContext());
        foreach (var stmt in context.stmt())
        {
            var rv = VisitStmt(stmt);
            if (_shouldReturn)
            {
                _shouldReturn = true;
                return rv;
            }
        }
        return null;
    }

    public override IValue VisitStmt(ShaellParser.StmtContext context)
    {
        if (context.children.Count == 1)
        {
            return Visit(context.children[0]);
        }
        else
        {
            throw new Exception("No no no");
        }
    }

    public override IValue VisitIfStmt(ShaellParser.IfStmtContext context)
    {
        var stmts = context.stmts();
        
        if (Visit(context.expr()).ToBool())
        {
            return VisitStmts(stmts[0]);
        }
        else if (stmts.Length > 1)
        {
            return VisitStmts(stmts[1]);
        }

        return null;
    }

    public override IValue VisitWhileLoop(ShaellParser.WhileLoopContext context)
    {
        while (Visit(context.expr()).ToBool())
        {
            var rv = VisitStmts(context.stmts());
            if (_shouldReturn)
            {
                return rv;
            }
        }

        return null;
    }

    public override IValue VisitReturnStatement(ShaellParser.ReturnStatementContext context)
    {
        _shouldReturn = true;
        return Visit(context.expr());
    }

    public override IValue VisitFunctionDefinition(ShaellParser.FunctionDefinitionContext context)
    {
        var formalArgIdentifiers = new List<string>();
        foreach (var formalArg in context.innerFormalArgList().VARIDENTFIER())
        {
            formalArgIdentifiers.Add(formalArg.GetText());
        }
        
        _scopeManager.SetValue(
            context.VARIDENTFIER().GetText(),
            new UserFunc(
                _globalScope, 
                context.stmts(), 
                _scopeManager.CopyScopes(), 
                formalArgIdentifiers
                )
        );
        
        return null;
    }

    public override IValue VisitExpr(ShaellParser.ExprContext context)
    {
        throw new Exception("nejnejnej");
    }
    
    public override IValue VisitAssignExpr(ShaellParser.AssignExprContext context)
    {
        var lhs = Visit(context.expr(0));
        
        if (lhs is not RefValue)
        {
            throw new Exception("Tried to assign to non ref");
        }

        RefValue refLhs = lhs as RefValue;

        var rhs = Visit(context.expr(1));
        if (rhs is RefValue)
        {
            rhs = (rhs as RefValue).Get();
        }
        
        refLhs.Set(rhs);

        return refLhs.Get();
    }

    public override IValue VisitAddExpr(ShaellParser.AddExprContext context)
    {
        var lhs = Visit(context.expr(0));
        var rhs = Visit(context.expr(1));

        return lhs.ToNumber() + rhs.ToNumber();
    }

    public override IValue VisitVarIdentifier(ShaellParser.VarIdentifierContext context)
    {
        var val = _scopeManager.GetValue(context.VARIDENTFIER().GetText());
        if (val == null)
        {
            return _scopeManager.SetValue(context.VARIDENTFIER().GetText(), new SNull());
        }
        return val;
    }

    public override IValue VisitNumberExpr(ShaellParser.NumberExprContext context)
    {
        return new Number(int.Parse(context.NUMBER().GetText()));
    }

    public override IValue VisitFunctionCallExpr(ShaellParser.FunctionCallExprContext context)
    {

        var lhs = Visit(context.expr()).ToFunction();
        
        var args = new List<IValue>();
        foreach (var expr in context.innerArgList().expr())
        {
            var val = Visit(expr);
            if (val is RefValue refVal)
            {
                val = refVal.Get();
            }

            args.Add(val);
        }

        return lhs.Call(args);
    }

    public override IValue VisitStringLiteralExpr(ShaellParser.StringLiteralExprContext context)
    {
        return new SString(context.STRINGLITERAL().GetText()[1..^1]);
    }
}