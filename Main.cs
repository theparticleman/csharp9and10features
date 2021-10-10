using System;

Console.WriteLine("This is a top-level statement");

// You used to have to do something like this
// using System;
//
// namespace MyProject
// {
//     public class Program
//     {
//         public void Main(string args)
//         {
//             Console.WriteLine("This is NOT using top-level statements");
//         }
//     }
// }

// You can only have one top-level file in a project.
// Having more than one or having a top-level file
// and a Main method results in a compiler error.