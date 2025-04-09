using System;
using System.IO;
using System.Collections;
using System.Text.Encodings.Web;

namespace hmc;

enum Language
{
    JAVA,
    CS,
    NONE,
}

class Program
{

    private static string GetLang(Language lang, string defExt = ".java")
    {
        switch (lang)
        {
            case Language.JAVA:
                return ".java";
            case Language.CS:
                return ".cs";

            default:
                return defExt;

        }
    }

    private static List<string> GetSourceFiles(string rootDirPath, string ext = ".java")
    {

        // Initialize list
        List<string> javaFiles = new List<string>();

        // Get all files
        string[] allFiles = Directory.GetFiles(rootDirPath, "*.*", SearchOption.AllDirectories);
        //List<string> javaFiles = Directory.GetFiles(rootDirPath, $"*.*", SearchOption.AllDirectories).ToList();

        // Filter
        foreach (string file in allFiles)
        {

            if (file.EndsWith(ext))
            {
                javaFiles.Add(file);
            }

        }

        return javaFiles;

    }

    private static void ParseJavaSourceFile(string filePath, HashSet<string> operators,
    HashSet<string> distinctOperators, HashSet<string> distinctOperands,
    ref int totalOperators, ref int totalOperands)
    {
        string code = File.ReadAllText(filePath);
        string[] tokens = code.Split(new[] { ' ', '\t', '\n', '\r', '(', ')', '{', '}', '[', ']', ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string token in tokens)
        {
            if (operators.Contains(token))
            {
                distinctOperators.Add(token);
                totalOperators++;
            }
            else if (!IsKeyword(token) && !IsLiteral(token))
            {
                distinctOperands.Add(token);
                totalOperands++;
            }
        }
    }

    private static HashSet<string> javaOperators = new() {
        "+", "-", "*", "/", "=", "==", "!=", "<", ">", "<=", ">=", "&&", "||", "!", "%", "++", "--", "+=", "-=", "*=", "/=", "?", ":"
    };

    private static HashSet<string> javaKeywords = new() {
        "public", "private", "protected", "class", "static", "void", "int", "float", "double", "if", "else", "for", "while", "return", "new", "import", "package"
    };

    private static bool IsKeyword(string token) => javaKeywords.Contains(token);
    private static bool IsLiteral(string token) => int.TryParse(token, out _) || token.StartsWith("\"");

    private static void ProduceReport(string rootProjectName, string[] javaFiles, string distinctOperands,
    string distinctOperators, int totalOperands, int totalOperators, double difficulty, double volume)
    {

        const string REPORT_DIR = "reports";
        string reportPath = Path.GetFullPath(REPORT_DIR);

        if (!Directory.Exists(reportPath))
        {
            Directory.CreateDirectory(reportPath);
        }

        string currDate = DateTime.Now.ToShortDateString();
        string outName = currDate;
        string[] allReports = Directory.GetFiles(reportPath, "*.*", SearchOption.TopDirectoryOnly);
        int id = 0;

        foreach (string existingReport in allReports)
        {
            while (existingReport.Contains(outName + $"_{id}"))
            {
                id++;
            }
            
        }
        
        outName += $"_{id}";
        outName += $"_{rootProjectName.Split('\\')[rootProjectName.Split('\\').Length-2]}";
        outName += ".txt";

        string reportContent = @$"
=====================   Report  =======================
Halstead Complexity Measurement
Date: {currDate}
Root project name: {rootProjectName}
---------------------------
*There are too many distinct operands, as such they will be added to external file*
Total number of operands: [ {totalOperands} ]
---------------------------
List of distincts operators: {distinctOperators}
Total number of operators: {totalOperators}
---------------------------
> Difficulty: {difficulty}
> Volume: {volume}
=====================   Report  =======================
        ";


        File.WriteAllText(Path.Join(reportPath, outName), reportContent);

    }


    public static void Main(string[] args)
    {

        if (args[0].Equals("-h") || args[0].Equals("--help"))
        {
            string helpMsg = @$"
            usage: hcm <arg>
            args:
            -h | --help                                                     display this help message
            file source of the java file e.g. /path/to/java/project/dir/    will run the halstead metric report on the target java project
            -no | --no-out                                                  will run without outputing report
            ";

            Console.WriteLine(helpMsg);
            return;
        }

        // Get all java file paths
        List<string> javaFile = GetSourceFiles(rootDirPath: args[0]);

        // Initialize ds
        HashSet<string> distinctOperators = new();
        HashSet<string> distinctOperands = new();
        int totalOperators = 0, totalOperands = 0;

        foreach (var path in javaFile)
        {
            ParseJavaSourceFile(path, javaOperators, distinctOperators, distinctOperands, ref totalOperators, ref totalOperands);
        }

        // Set Halstead Parameters
        int n1 = distinctOperators.Count;
        int n2 = distinctOperands.Count;
        int N1 = totalOperators;
        int N2 = totalOperands;

        double difficulty = (n1 / 2.0) * (N2 / (double)n2);
        double volume = (N1 + N2) * Math.Log2(n1 + n2);

        Console.WriteLine($"Difficulty: {difficulty}");
        Console.WriteLine($"Volume: {volume}");

        if (args.Length > 2)
        {
            if(!args[1].Equals("--no-out") || !args[1].Equals("-no")){
                return;
            }
        }
        ProduceReport(rootProjectName: args[0], javaFiles: javaFile.ToArray(), distinctOperands: string.Join(" ", distinctOperands), distinctOperators: string.Join("\n", distinctOperators), totalOperands: totalOperands, totalOperators: totalOperators, difficulty: difficulty, volume: volume);
    }


}