using System;


namespace PrintOperations
{
    public class MyPrint
    {

        //class variable
        private string name;

        //a simple method just to print something
        public void Print() 
        {
            Console.WriteLine("print here");
        }

        //a method to return the private variable
        public string GetName()
        {
            return this.name;
        }

        //method to print the name
        public void PrintName() 
        { 
            Console.WriteLine($"my name is {this.name}");
        }

        //method that takes a parameter and print that
        public void Print( string name ) 
        {
            Console.WriteLine($"the name  as argument is {name}");

        }

        //a private method 
        private void PrivatePrint() 
        {
            Console.WriteLine("I am printing from private mode");
        }

        //PROPERTIES

        public string Name => name;

        public static string StaticName => " I am static property name";


    }
}