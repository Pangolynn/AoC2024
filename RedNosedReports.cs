using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Input is many reports, 1 report per line
// Each report is a list of #s containing levels separated by spaces
// x reports, y levels
class RedNosedReports
{
    private string fileName = "day2input.txt";
    
    public RedNosedReports()
    {
        if (!File.Exists(fileName))
        {
            Console.WriteLine("File not found.");
        }
    }
    
    public List<List<int>> GetReports()
    {
        List<List<int>> reports = new List<List<int>>();
        
        // read each line
        foreach (string line in File.ReadLines(fileName))
        {
            // create a report list
            List<int> report = new List<int>();
            // split the line on spaces, remove empties
            foreach (string x in line.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
                if (int.TryParse(x, out int level))
                {
                    // add each level to the report
                    report.Add(level);
                }
                else
                {
                    Console.WriteLine($"Skipping invalid number: {x}");
                }
            }
            reports.Add(report);
        }

        return reports;
    }
    
    // is the report safe? 
    // all levels increasing || all levels decreasing
    // && adjacent lvls differ by at least 1 and at most 3
    public bool CheckReportSafety(List<int> report)
    {
        bool increasing = false;
        bool decreasing = false;

        
        for (int i = 0; i < report.Count() - 1; i++)
        {
            if ((report[i] - report[i + 1]) <= -1 && (report[i] - report[i + 1]) >= -3)
            {
                // if we were decreasing and now increasing, unsafe
                if (decreasing)
                {
                    // attempt to dampen the levels
                    if (testDampenLevels(new List<int>(report), i))
                    {
                        return true;
                    }
                    return false;
                }

                increasing = true;
            }
            else if ((report[i] - report[i + 1]) >= 1 && (report[i] - report[i + 1]) <= 3)
            {
                // if we were increasing and now decreasing
                if (increasing) 
                {
                    // attempt to dampen the levels
                    if (testDampenLevels(new List<int>(report), i))
                    {
                        return true;
                    }
                    return false;
                } 

                decreasing = true;
            }
            else 
            {
                // neither increasing or decreasing OR increasing and decreasing > 3
                if (testDampenLevels(new List<int>(report), i))
                {
                    return true;
                }
                return false;
            }
        }

        return true;
    }

    // Check if the report is safe if we remove one of the concerning levels
    private bool testDampenLevels(List<int> report, int failedLevelIndex)
    {
        // make a copy of the report without the first concerned level
        List<int> reportCopy1 = report.ToList();
        reportCopy1.RemoveAt(failedLevelIndex);
        // check if the report works without this level
        if (CheckDampenedReportSafety(reportCopy1))
        {
            return true;
        }
        
        // We don't know which level could be the problem, check both situations
        // make a copy of the report without the 2nd concerned level
        List<int> reportCopy2 = report.ToList();
        reportCopy2.RemoveAt(failedLevelIndex + 1);
        // check if the report works without this level
        if (CheckDampenedReportSafety(reportCopy2))
        {
            return true;
        }
        
        // if the report still does not work without either of these levels, fail
        return false;
    }
    
    // TODO: Find a way to maybe make this just a recursive call to CheckReportSafety
    //       issues with infinitely shrinking the list until it works
    //       maybe a flag in the class and not the function
    // This is just CheckReportSafety minus the dampening, because we only 
    // call this once a failure has a occured and are within testDampenLevels
    public bool CheckDampenedReportSafety(List<int> report)
    {
        bool increasing = false;
        bool decreasing = false;
        
        for (int i = 0; i < report.Count() - 1; i++)
        {
            if ((report[i] - report[i + 1]) <= -1 && (report[i] - report[i + 1]) >= -3)
            {
                // if we were decreasing and now increasing, unsafe
                if (decreasing)
                {
                    return false;
                }

                increasing = true;
            }
            else if ((report[i] - report[i + 1]) >= 1 && (report[i] - report[i + 1]) <= 3)
            {
                // if we were increasing and now decreasing
                if (increasing) 
                {
                    return false;
                } 

                decreasing = true;
            }
            else 
            {
                // neither increasing nor decreasing OR increasing and decreasing > 3
                return false;
            }
        }

        return true;
    }
}
