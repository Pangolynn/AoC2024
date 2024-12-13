// Corrupted input of functions that multiply #s
// mul(x,y)
// x & y are 1-3 digit #s
// ex: mul(44,46) = 2024
// invalid characters in the corrupted input should be ignored
// Corrupted Sequences ex:
// mul(4*, mul(6,9!, ?(12,34), or mul ( 2 , 4 ) do nothing.
// Add up the results of all actual mul instructions
using System.Text.RegularExpressions;

class MullItOver
{
    private string _fileName = "day3input.txt";
    private string _corruptedInput = "";
    // mul is matched easily, then use \ to escape the ( match
    // then we group the number matches ([0-9]{1,3})
    // then a comma, then the 2nd number match ([0-9]{1,3})
    // then the last ) needs to be escaped with \
    // @ is used to denote a verbatim string literal
    // so we don't have to escape our backspaces in the string
    private string _mulPattern = @"mul\(([0-9]{1,3}),([0-9]{1,3})\)";
    private int _sum;
    
    // part 2
    private string _doPattern = @"do\(\)";
    private string _dontPattern = @"don't\(\)";
    private string _dontDoPattern = @"don't\(\).*?do\(\)";

    public MullItOver()
    {
        if(!File.Exists(_fileName))
        {
            Console.WriteLine("File not found");
        }

        GetInput();
        PurifyInput(_corruptedInput);

        // foreach (string line in _corruptedInput)
        // {
        //     PurifyInput(line);
        // }
        
    }

    private void GetInput()
    {
        try
        {
            _corruptedInput = File.ReadAllText(_fileName);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private void PurifyInput(string input)
    {   
        Console.WriteLine("Purifying Input");
        // match values in the form mul(#,#)
        Regex regex = new Regex(_mulPattern);

        Regex dontDoRegex = new Regex(_dontDoPattern);
        
        MatchCollection dontDoMatches = dontDoRegex.Matches(input);
        int lastIndex = 0;

        
        string beforeDont = "";
        string betweenDontDo = "";
        foreach(Match match in dontDoMatches)
        {
            beforeDont += input.Substring(lastIndex, match.Index - lastIndex);
            betweenDontDo += match.Value;
            lastIndex = match.Index + match.Length;
            Console.WriteLine("Before Dont: " + beforeDont + "\n");
            Console.WriteLine("Between Dont Do: " + betweenDontDo + "\n");
        }

        string afterDontDo = input.Substring(lastIndex);

        string newInput = beforeDont + afterDontDo;
        Console.WriteLine("After dont do: " + afterDontDo);

        MatchCollection matches = regex.Matches(newInput);
        
        foreach (Match match in matches)
        {
            
            // grab the 1st and 2nd index based group matches
            // which are the numbers X Y
            if (int.TryParse(match.Groups[1].Value, out int x) &&
                int.TryParse(match.Groups[2].Value, out int y))
            {
                _sum += x * y;
            }
        }
    
    }

    public int GetSum()
    {
        return _sum;
    }

    
    

}