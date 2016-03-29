using System;

public abstract class Expr {}

public sealed class IntExpr : Expr {
    public IntExpr(Int32 val) {
        this.Val = val;
    }

    public override String ToString() => $"{Val}";

    public Int32 Val { get; }
}

public abstract class BinaryOp<E> : Expr {
    public BinaryOp(Expr left, Expr right) {
        this.Left = left;
        this.Right = right;
    }

    public Expr Left { get; }
    public Expr Right { get; }
}

public sealed class AddExpr : BinaryOp<AddExpr> {
    public AddExpr(Expr left, Expr right)
        : base(left, right) {}

    public override String ToString() => $"{this.Left} + {this.Right}";
}

public sealed class MultExpr : BinaryOp<MultExpr> {
    public MultExpr(Expr left, Expr right)
        : base(left, right) {}

    public override String ToString() {
        String ret = "";

        if (this.Left is AddExpr) {
            ret += $"({this.Left})";
        } else {
            ret += $"{this.Left}";
        }

        ret += " * ";

        if (this.Right is AddExpr) {
            ret += $"({this.Right})";
        } else {
            ret += $"{this.Right}";
        }

        return ret;
    }
}