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
    private string _fileName = "test.txt";
    private List<string> _corruptedInput = new();
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

    public MullItOver()
    {
        if(!File.Exists(_fileName))
        {
            Console.WriteLine("File not found");
        }

        GetInput();
        foreach (string line in _corruptedInput)
        {
            PurifyInput(line);
        }
        
    }

    private void GetInput()
    {
        foreach (string line in File.ReadLines(_fileName))
        {
            _corruptedInput.Add(line);
        }
    }

    private void PurifyInput(string input)
    {
        Regex regex = new Regex(_mulPattern);
        Regex doRegex = new Regex(_doPattern);
        Regex dontRegex = new Regex(_dontPattern);
        // match values in the form mul(#,#)
        MatchCollection matches = regex.Matches(input);
        MatchCollection doMatches = doRegex.Matches(input);
        MatchCollection dontMatches = dontRegex.Matches(input);

        bool enabled = true;
        
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

        foreach (Match match in doMatches)
        {
            Console.WriteLine(match);
            
        }
        foreach (Match match in dontMatches)
        {
            Console.WriteLine(match);
            
        }
    }

    public int GetSum()
    {
        return _sum;
    }

    
    

}