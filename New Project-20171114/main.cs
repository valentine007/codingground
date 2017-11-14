using System.IO;
using System;

class Program
{
    static void Main()
    {
        User tom = new UserBuilder().SetName("Tom").SetCompany("Microsoft").SetAge(23);
        Console.WriteLine("User Name: " + tom.Name.ToString());
        Console.WriteLine("User Company: " + tom.Company.ToString());
        Console.WriteLine("User Age: " + tom.Age.ToString());
}