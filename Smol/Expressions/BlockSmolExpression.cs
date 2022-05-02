namespace Smol.Expressions
{
    public class BlockSmolExpression : SmolExpression
    {
        public SmolExpression[] Expressions { get; private set; }

        public BlockSmolExpression(SmolExpression[] expressions)
        {
            Expressions = expressions;
        }

        public override void Execute(SmolContext context)
        {
            SmolContext r = new SmolContext(context);
            
            foreach (var expression in Expressions)
            {
                expression.Execute(r);
            }

            if (r.StackHeight() > 1) throw new ApplicationException("Inner block cannot have a stackheight of more than 1");

            if(r.StackHeight() == 1)
            {
                context.PushValue(r.PopValue());
            }
        }

        public override string ToString()
        {
            return $"({string.Join(" ", Expressions.Select(x => x.ToString()))})";
        }
    }
}
