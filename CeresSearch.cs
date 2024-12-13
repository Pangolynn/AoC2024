using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventOfCode2024;

class CeresSearch
{
    private string _fileName = "day4input.txt";
    private string xmas = @"XMAS|SAMX";
    private int horizontalMatches = 0;
    private int verticalMatches = 0;
    private int primaryMatches = 0;
    private int secondaryMatches = 0;
    
    
    public CeresSearch() {
        GetInput();

    }
    private void GetInput() {
        try
        {
            string[] lines = File.ReadAllLines(_fileName);

            int rows = lines.Length;
            int cols = lines[0].Length;
            
            string[] verticals = new string[cols];
            for (int i = 0; i < cols; i++)
            {
                verticals[i] = "";
            }
            
            Regex pattern = new Regex(xmas);
            foreach (string line in lines)
            {
                // find each match in the horizontal direction
                int horizontalCount = CountOverlappingMatches(line, pattern);
                horizontalMatches += horizontalCount;
                Console.WriteLine($"Horizontal: {line}, Matches: {horizontalMatches}");
                // set up the array for verticals
                for (int i = 0; i < cols; i++)
                {
                    verticals[i] += line[i];
                }
            }

            // Find each match in the vertical direction
            foreach (string vertical in verticals)
            {
                int verticalCount = CountOverlappingMatches(vertical, pattern);
                verticalMatches += verticalCount;
                Console.WriteLine($"Vertical: {vertical}, Matches: {verticalMatches}");
            }
            
            // Check diagonals from top-left to bottom-right
            Console.WriteLine("Top-left to Bottom-right Diagonals:");
            for (int startRow = 0; startRow < rows; startRow++)
            {
                // get diagonal starting at (startRow, 0)
                string diagonal = GetDiagonal(lines, startRow, 0, 1, 1);
                int diagonalCount = CountOverlappingMatches(diagonal, pattern);
                primaryMatches += diagonalCount;
                Console.WriteLine($"Diagonal starting at ({startRow}, 0): {diagonal}, Matches: {primaryMatches}");
            }
            for (int startCol = 1; startCol < cols; startCol++)
            {
                // get diagonal starting at (0, startCol)
                string diagonal = GetDiagonal(lines, 0, startCol, 1, 1);
                int diagonalCount = CountOverlappingMatches(diagonal, pattern);
                primaryMatches += diagonalCount;
                Console.WriteLine($"Diagonal starting at (0, {startCol}): {diagonal}, Matches: {primaryMatches}");
            }

            // diagonals from top right to bottom left
            Console.WriteLine("\nTop-right to Bottom-left Diagonals:");
            for (int startRow = 0; startRow < rows; startRow++)
            {
                // Get diagonal starting at (startRow, cols-1)
                string diagonal = GetDiagonal(lines, startRow, cols - 1, 1, -1);
                int diagonalCount = CountOverlappingMatches(diagonal, pattern);
                secondaryMatches += diagonalCount;
                Console.WriteLine($"Diagonal starting at ({startRow}, {cols - 1}): {diagonal}, Matches: {secondaryMatches}");
            }
            for (int startCol = cols - 2; startCol >= 0; startCol--)
            {
                // Get diagonal starting at (0, startCol)
                string diagonal = GetDiagonal(lines, 0, startCol, 1, -1);
                int diagonalCount = CountOverlappingMatches(diagonal, pattern);
                secondaryMatches += diagonalCount;
                Console.WriteLine($"Diagonal starting at (0, {startCol}): {diagonal}, Matches: {secondaryMatches}");
            }

           
            Console.WriteLine($"Matches: {CalculateMatches()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
            
    }

    // return each diagonal as a string given a startRow and startCol
    // and rowInc and colInc increments
    static string GetDiagonal(string[] lines, int startRow, int startCol, int rowInc, int colInc)
    {
        int rows = lines.Length;
        int cols = lines[0].Length;
        string diagonal = "";

        int row = startRow;
        int col = startCol;

        // Get characters within bounds
        while (row >= 0 && row < rows && col >= 0 && col < cols)
        {
            diagonal += lines[row][col];
            row += rowInc;
            col += colInc;
        }

        return diagonal;
    }
    
    // get the count of overlapping regex matches, given a string and regex pattern
    private int CountOverlappingMatches(string input, Regex pattern)
    {
        int count = 0;
        int startIndex = 0;

        // Search for the next match starting from the last match position
        while (startIndex < input.Length)
        {
            Match match = pattern.Match(input, startIndex);
            if (match.Success)
            {
                count++;
                // Move startIndex forward by one to allow overlapping matches
                startIndex = match.Index + 1;
            }
            else
            {
                break; // No more matches found
            }
        }

        return count;
    }
    

    private int CalculateMatches()
    {
        return horizontalMatches + verticalMatches + primaryMatches + secondaryMatches;
    }
}