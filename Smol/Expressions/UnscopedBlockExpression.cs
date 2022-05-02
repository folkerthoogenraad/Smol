namespace Smol.Expressions
{
    public class UnscopedBlockExpression : SmolExpression
    {
        public SmolExpression[] Expressions { get; private set; }

        public UnscopedBlockExpression(SmolExpression[] expressions)
        {
            Expressions = expressions;
        }

        public override void Execute(SmolContext context)
        {
            foreach (var expression in Expressions)
            {
                context.Execute(expression);
            }
        }

        public override string ToString()
        {
            return $"{string.Join(" ", Expressions.Select(x => x.ToString()))}";
        }
    }
}
