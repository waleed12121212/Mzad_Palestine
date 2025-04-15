using System;
using System.Linq;
using System.Reflection;
using Mzad_Palestine_Core.Models;

class Program
{
    static void Main()
    {
        var types = typeof(Auction).Assembly.GetTypes()
            .Where(t => t.IsClass && t.Namespace == "Mzad_Palestine_Core.Models");

        foreach (var type in types)
        {
            Console.WriteLine($"Class: {type.Name}");
            foreach (var prop in type.GetProperties())
            {
                Console.WriteLine($"\tProperty: {prop.Name} ({prop.PropertyType.Name})");
            }
        }
    }
}
