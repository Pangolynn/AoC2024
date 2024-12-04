using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


// Advent of Code 2024: https://adventofcode.com/2024/day/1
// Day 1: Historian Hysteria
class Program
{
    static void Main(string[] args)
    {
        // -- Start Day 1 --
        string filePath = "input.txt";
        
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File not found.");
            return;
        }
        
        (List<int> col1, List<int> col2) = ReadColumnsFromFile(filePath);
        
        if (col1.Count != col2.Count)
        {
            Console.WriteLine("The number of items in the list are different.");
            return;
        }
        
        Console.WriteLine($"Total distance: {CalculateDistance(col1, col2)}");
        Console.WriteLine($"Total similarity score: {CalculateSimilarityScore(col1, col2)}");
        
        // -- End Day 1 --
        
        // -- Start Day 2 --

        RedNosedReports reports = new RedNosedReports();
        List<List<int>> listOfReports = reports.GetReports();
        int numberOfSafeReports = 0;
        foreach (List<int> report in listOfReports)
        {
            if (reports.CheckReportSafety(report))
            {
                numberOfSafeReports++;
            }
        }
        Console.WriteLine($"Total number of safe reports: {numberOfSafeReports}");
        
        // -- End Day 2 --
        
        // -- Start Day 3 --
        MullItOver m1 = new MullItOver();
        Console.WriteLine($"The sum of all muls is : {m1.GetSum()}");
        // -- End Day 3
    }

    static (List<int>, List<int>) ReadColumnsFromFile(string filePath)
    {
        List<int> col1 = new List<int>();
        List<int> col2 = new List<int>();

        foreach (string line in File.ReadLines(filePath))
        {
            string[] parts = line.Split("   ");

            if (parts.Length >= 2 &&
                int.TryParse(parts[0], out int num1) &&
                int.TryParse(parts[1], out int num2))
            {
                col1.Add(num1);
                col2.Add(num2);
            }
            else
            {
                Console.WriteLine($"Skipping invalid line: {line}");
            }
        }
        return (col1, col2);
    }

    static int CalculateDistance(List<int> col1, List<int> col2)
    {
        // Match the left and right locations in ascending order
        col1.Sort();
        col2.Sort();

        int distance = 0;

        // find the distance between each left/right pair and add 
        // each distance to find the total distance between the lists
        for (int i = 0; i < col1.Count; i++)
        {
            distance += (Math.Abs(col1[i] - col2[i]));
        }
        
        return distance;
    }
    
    
    
    // total similarity score =
    //      addition of all left numbers * number of times found in right
    static int CalculateSimilarityScore(List<int> col1, List<int> col2)
    {
        int similarityScore = 0;
        for (int i = 0; i < col1.Count; i++)
        {
            // how often do left locations appear in right locations
            int count = col2.Count(x => x == col1[i]);
            similarityScore += (col1[i] * count);
        }

        return similarityScore;
    }
}