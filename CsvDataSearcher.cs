using Microsoft.VisualBasic.FileIO;

internal class CsvDataSearcher
{
    private static void Main(string[] args)
    {
        if (ValidateInputs(args))
        {
            string csvFilePath = args[0];
            int index = int.Parse(args[1]);

            string searchKey = args[2];

            ParseCsvFile(csvFilePath, index, searchKey);
        }
        else
            Console.WriteLine("Failed reading CSV file, please adjust inputs for ./CsvSearch.exe <csvFilePath> <columnNumber> <searchKey>");
    }

    private static bool ValidateInputs(string[] args)
    {
        if (args.Length != 3)
        {
            Console.WriteLine("Usage: ./CsvSearch.exe <csvFilePath> <columnNumber> <searchKey>");
            return false;
        }

        if (!File.Exists(args[0]))
        {
            Console.WriteLine("The specified CSV file does not exist.");
            return false;
        }

        if (int.TryParse(args[1], out int index) && index < 0)
        {
            Console.WriteLine("Invalid column number.");
            return false;
        }

        if (string.IsNullOrWhiteSpace(args[2]))
        {
            Console.WriteLine("Search key cannot be empty.");
            return false;
        }

        return true;
    }

    private static void ParseCsvFile(string csvFilePath, int columnNumber, string searchKey)
    {
        List<string[]> searchResult = new();
        try
        {
            using TextFieldParser parser = new(csvFilePath);
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");

            while (!parser.EndOfData)
            {
                string[] fields = parser.ReadFields();
                if (columnNumber < fields.Length && fields[columnNumber].Trim().Equals(searchKey, StringComparison.OrdinalIgnoreCase))
                    searchResult.Add(fields);
            }
            PrintResult(searchResult);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred: " + ex.Message);
        }
    }

    private static void PrintResult(List<string[]> searchResult)
    {
        if (searchResult.Count > 0)
            foreach (var array in searchResult)
                Console.WriteLine(string.Join(", ", array));
        else
            Console.WriteLine("No match found for the provided search key.");
    }
}