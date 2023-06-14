using System;
using System.Reflection;


namespace MyReflection
{
    class Program
    {
        static void Main(string[] args)
        {
            //assembly path
            var assembly = Assembly.LoadFrom(@"C:\Users\Mahsa\Desktop\MyReflection\MyReflection\bin\Debug\net6.0\MyReflection.dll");

            //get diffrent types defined inside of this assembly
            foreach (var type in assembly.GetTypes())
            {
                //Print the type name for each type
                Console.WriteLine($"The type is {type.Name}");
                var instance = Activator.CreateInstance(type);

                // print whatever foelds, properties and methos we have

                //fields
                foreach (var field in type.GetFields(BindingFlags.NonPublic
                    | BindingFlags.Instance
                    | BindingFlags.DeclaredOnly))
                {

                    Console.WriteLine($"Our field is: {field.Name}");
                    //set the value of nonpublic field (cause we can do this usingreflection)
                    field.SetValue(instance, "Luna");

                }

                //methods
                foreach (var method in type.GetMethods(BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.DeclaredOnly)
                    .Where(m => !m.IsSpecialName))
                {
                    Console.WriteLine($"Our method is {method.Name}");
                    //if it has parameter
                    if (method.GetParameters().Length > 0)
                    {
                        method.Invoke(instance, new[] { "Ellie" });
                    }
                    //if it returns something
                    else if (method.ReturnType.Name != "Void")
                    {
                        var returnValue = method.Invoke(instance, null);
                        Console.WriteLine($"returned value from method is {returnValue}");
                    }
                    else
                    {
                        method.Invoke(instance, null);
                    }

                }

                //properties

                foreach (var property in type.GetProperties())
                {
                    Console.WriteLine($"Our property is {property.Name}");
                    var propertyValue = property.GetValue(instance);
                    Console.WriteLine($"property value is {propertyValue}");
                }



            }

        }

    }
}