
using Smol;

Runtime runtime = new Runtime();

runtime.RegisterCommand("exit", stack => Environment.Exit(0));
runtime.RegisterCommand("print", stack => Console.WriteLine(stack.Pop()));

while (true)
{
    Console.Write("> ");

    var line = Console.ReadLine();

    if(line == null)
    {
        Console.WriteLine("Invalid input.");
        continue;
    }

    var tokens = Lexer.Tokenize(line);

    runtime.Execute(tokens);
}