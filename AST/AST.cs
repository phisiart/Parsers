using System;

public abstract class Expr {}

public sealed class IntExpr : Expr {
    public IntExpr(Int32 val) {
        this.Val = val;
    }

    public override String ToString() => $"{Val}";

    public Int32 Val { get; }
}

public abstract class BinaryOp : Expr {
    public BinaryOp(Expr left, Expr right) {
        this.Left = left;
        this.Right = right;
    }

    public sealed override String ToString() {
        String ret = "";

        if ((this.Left as BinaryOp)?.Precedence < this.Precedence) {
            ret += $"({this.Left})";
        } else {
            ret += $"{this.Left}";
        }

        ret += $" {this.Op} ";

        if ((this.Right as BinaryOp)?.Precedence < this.Precedence) {
            ret += $"({this.Right})";
        } else {
            ret += $"{this.Right}";
        }

        return ret;
    }

    public Expr Left { get; }
    public Expr Right { get; }

    public abstract Int32 Precedence { get; }
    public abstract String Op { get; }
}

public abstract class AdditiveExpr : BinaryOp {
    public AdditiveExpr(Expr left, Expr right)
        : base(left, right) {}

    public sealed override Int32 Precedence => 0;
}

public sealed class AddExpr : AdditiveExpr {
    public AddExpr(Expr left, Expr right)
        : base(left, right) {}

    public override String Op => "+";
}

public sealed class SubExpr : AdditiveExpr {
    public SubExpr(Expr left, Expr right)
        : base(left, right) {}

    public override String Op => "-";

}

public abstract class MultiplicativeExpr : BinaryOp {
    public MultiplicativeExpr(Expr left, Expr right)
        : base(left, right) {}

    public sealed override Int32 Precedence => 1;

}

public sealed class MultExpr : MultiplicativeExpr {
    public MultExpr(Expr left, Expr right)
        : base(left, right) {}

    public override String Op => "*";
}

public sealed class DivExpr : MultiplicativeExpr {
    public DivExpr(Expr left, Expr right)
        : base(left, right) {}

    public override String Op => "/";
}
