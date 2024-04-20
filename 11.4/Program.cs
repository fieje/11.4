using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the file name:");
        string fileName = Console.ReadLine();

        InputDataToFile(fileName);
        SortRecordsByStoreName(fileName);

        bool continueSearching = true;
        while (continueSearching)
        {
            Console.WriteLine("Enter the product name:");
            string product = Console.ReadLine();
            PrintProductInfo(fileName, product);

            Console.WriteLine("Do you want to search for another product? (yes/no)");
            string response = Console.ReadLine().ToLower();
            continueSearching = response == "yes";
        }
    }

    static void InputDataToFile(string fileName)
    {
        using (StreamWriter sw = new StreamWriter(fileName))
        {
            Console.WriteLine("Enter product data (product name, store name, price):");
            string input;
            do
            {
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
                {
                    string[] data = input.Split(',');
                    Price price;
                    price.productName = data[0].Trim();
                    price.storeName = data[1].Trim();
                    price.cost = double.Parse(data[2].Trim());
                    sw.WriteLine($"{price.productName},{price.storeName},{price.cost}");
                }
            } while (!string.IsNullOrEmpty(input));
        }
    }

    public static void PrintProductInfo(string fileName, string productName) 
    {
        bool found = false;
        foreach (string line in File.ReadAllLines(fileName))
        {
            string[] data = line.Split(',');
            if (data[0].Trim().Equals(productName, StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Product Name: {data[0].Trim()}");
                Console.WriteLine($"Store: {data[1].Trim()}");
                Console.WriteLine($"Price: {data[2].Trim()} UAH");
                found = true;
            }
        }

        if (!found)
            Console.WriteLine("No products found with that name.");
    }

    static void SortRecordsByStoreName(string fileName)
    {
        List<Price> prices = File.ReadAllLines(fileName)
                                 .Select(line => {
                                     string[] data = line.Split(',');
                                     return new Price
                                     {
                                         productName = data[0].Trim(),
                                         storeName = data[1].Trim(),
                                         cost = double.Parse(data[2].Trim())
                                     };
                                 })
                                 .OrderBy(p => p.storeName)
                                 .ToList();

        File.WriteAllLines(fileName, prices.Select(p => $"{p.productName},{p.storeName},{p.cost}"));
    }

    struct Price
    {
        public string productName;
        public string storeName;
        public double cost;
    }
}
