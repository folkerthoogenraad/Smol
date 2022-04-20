# Smol
Smol is a programming language and tool for programmers to quickly process non-trivial data. It is similar to PowerShell or Bash, but has more focus on interopability.

```smol
$ "Hello World" print
```

----------------------------------------------------------------------
## Installation
For the installation you need the following tools:
 - .NET 6+
 - git

```bash
$ git clone https://github.com/folkerthoogenraad/Smol
$ cd ./Smol
$ dotnet build
```

Now in the `Smol/bin` folder there is an executable for running Smol.

----------------------------------------------------------------------
## Getting started
To start the program, navigate to the bin directory and run `./Smol` to start the program. 

As said, smol is a stack machine based interpreted language. 
```smol
$ "Hello World" print

Hello World
```
In this case, first `"Hello World"` is pushed onto the stack, then the `print` function is called. This pops the top from the stack and prints the result.

```smol
$ "a,b,c,d" split -on "," print

["a" "b" "c" "d"]
```

First, the string `"a,b,c,d"` is pushed onto the stack. This is followed by the `split` operator. The split operator requires one extra parameter, which is the `-on` that is specified. In this case `","` is provided, meaning it will split on each `,` character.

### Example use
Imagine a json file with people, with a first and last name:
```json
{
    "people": [
        {
            "name": "Liam",
            "age": 26
        },
        {
            "name": "Noah",
            "age": 24
        },
        {
            "name": "Emma",
            "age": 35
        },
        {
            "name": "Charlotte",
            "age": 44
        }
    ]
}
```
First, we can read the file, parse the json and store it in a variable (called `$data` in this case). 
```smol
"file.json" file_read json_parse -> $data
```
We push the filename onto the stack, let the `file_read` function read the file and push it to the stack (as string). The `json_parse` then converts it into a smol object and the `->` operator stores the top of the stack in a variable, in this case called `$data`.

And you want to view all names that contain the letter `o`.

```smol
$ $data .people {.name "o" contains} filter print

[("Noah" -> $name 24 -> $age this) ("Charlotte" -> $name 44 -> $age this)]
```
For this, we push `$data` to the stack, use the `.people` to lookup the people array. Then we declare a new lamda in the `{}` and push the lamda onto the stack. Then the `filter` function pops the lamda and the array from the stack and filters the array by executing the lamda on each element. 

The output is ofcourse workable, but this might not be easy to see with a bigger list of people. It would be nice if we can change the way it outputs.

For that we can use the `each` function. This executes a lamda for each element in an array. It creates a copy of the array with the processed data.

```smol
$ $data .people {.name "o" contains} filter {.name} each print

["Noah" "Charlotte"]
```

Thats a lot better, but we can do better. Using the `join` command we can join an array into one long string. The join function can take one extra parameter `-with`, a string that will be inserted between each element. In this case we use the `\n` as newline.

```smol
$ $data .people {.name "o" contains} filter {.name} each join -with "\n" print

Noah
Charlotte
```

Ofcourse, if we want to save this information to an `filtered.json` we can also do that:

```smol
# Filter the data and store it in the $data .people part.
$data .people {.name "o" contains} filter -> $data .people

# Output the data to output.json
"filtered.json" $data json_serialize file_write
```

----------------------------------------------------------------------
### Arrays
To create an array, you use the `[]` array notation. This converts the stack within the brackets into an array:
```smol
$ [0 1 2 3]
[0 1 2 3]

$ [4 5 add]
[9]
```

----------------------------------------------------------------------
### Variables, scopes and objects
Smol has support for using variables

```smol
$ "Hello World" -> $variable
```
Pushes `"Hello World"` onto the stack, and then stores the top of the stack into $variable.

Now, you can use the variable whenever you want:
```smol
$ $variable print

Hello World
```
This will trim the spaces off of the variable and print it out.

To view all current variables, you can push the `this` (context) onto the stack to print it:
```smol
$ this print

("Hello World" -> $variable this)
```

Using brackets, you can create a new scope with new variables. Using brackets will still be executed immediatly!

```smol
$ ("Hello Other World" -> $variable2)
$ $variable2 print

Unknown variable $variable2
```
The $variable2 doesn't exist! This is because the `()` imply a different scope, so different variables can be used in this space. To visualise, we can print the this pointer (just like before) within the scope:

```smol
$ ("Hello Other World" -> $variable2 this print)

("Hello Other World" -> $variable2 this)
```

Notice how `$variable` is not present here. Its not part of this scopes this pointer. However, it can be accessed:

```smol
$ ($variable -> $variable2 this print)

("Hello World" -> $variable2 this)
```

The bracketed expression cannot use the stack outside of itself. However, when the bracketed expression is done the leftover stack is pushed onto the stack of the outer scope. However, this cannot be more than _one_.
```smol
$ "Hello" (print)
Stack empty.

$ ("Hello") print
Hello

$ ("Hello" "Hello") print
Inner block cannot have a stackheight of more than 1
```

With this observation we can create __objects__ by leveraging the this, we've seen before: 
```smol
("Foo" -> $name this)
```
This pushes `"Foo"` onto the stack, stores it in `$name` and puts the `this` onto the stack. This then will be picked up by the outer scope. However, currently we discard this information.

```smol
("Foo" -> $name this) -> $foo
```

Now, we have stored the foo object into a variable named `$foo`. 

The smallest object that you can make is an empty object:
```smol
(this)
```

There is a shorthand function for this too:
```smol
new
```

Which does exactly the same thing.

----------------------------------------------------------------------
### Lambdas
Smol has support for lambda functions. They are pieces of executable smol code.
```smol
$ {"Hello" print}
```
Running this will push the new lamda onto the stack, but will then stop because the lambda itself is not executed. 
```smol
$ {"Hello" print} invoke
Hello
```

The scope of a lamda is not defined beforehand. The executer can decide the scope where to execute. The `invoke` function, by default, executes the lamda in the current scope, meaning all variables can be modified by the lamda. For the `invoke` function you can specify a parameter `-on` in which scope to execute.

```smol
$ {"Hello" -> $variable} invoke -on $data
```

So, if you invoke it on a `new` object it is equivalent to use either:
```smol
$ {"Hello" -> $variable} invoke -on new

$ ("Hello" -> $variable)
```

----------------------------------------------------------------------
## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## Roadmap
Planned features are:
 - Typesafety. It might be possible to make this language typesafe too. This will need static analysis too.

## License
[MIT](https://choosealicense.com/licenses/mit/)