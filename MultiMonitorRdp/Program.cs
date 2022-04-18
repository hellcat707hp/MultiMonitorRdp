using System.Diagnostics;
using System.Text.RegularExpressions;

const string HelpText =
    $"MultiMonitorRDP \n" +
    $"    - First argument should be which monitors you want to use from left-to-right as a 0-based list \"0,1,2,3,...\".\n" +
    $"          Example: For the two rightmost monitors in a 3 monitor setup, use \"1,2\".\n" +
    $"          Note: RDP can only use select monitors that share an edge.\n" +
    $"    - Second argument should be the path to the .rdp file you want to use for connection.";

if (args == null || args.Length < 2)
{
    Console.WriteLine(HelpText);
    return;
}

string[] desiredMonitorStrings = args[0].Split(',');
if (desiredMonitorStrings.Length == 0) {
    Console.WriteLine(HelpText);
    return;
}

List<int> desiredMonitors = new List<int>(); 
foreach(string argMon in desiredMonitorStrings)
{
    int outNum = -1;
    bool success = int.TryParse(argMon, out outNum);
    if (success)
    {
        desiredMonitors.Add(outNum);
    }
}

if (desiredMonitors.Count == 0)
{
    //You didnt enter any valid monitor numbers
    Console.WriteLine(HelpText);
    return;
}

var displaysltr = (from display in Screen.AllScreens
                   orderby display.Bounds.Left select display).ToList();


//Figure out the actual display numbers that Windows is using so we can hand those to RDP
List<int> displayNumbersToUse = new List<int>();
foreach (var desiredDisplayNum in desiredMonitors)
{
    if (displaysltr.Count-1 < desiredDisplayNum)
    {
        Console.WriteLine($"The desired display \"{desiredDisplayNum}\" does not exist.");
        return;
    }
    var display = displaysltr[desiredDisplayNum];
    int displayNumber = int.Parse(Regex.Match(display.DeviceName, @"\d+").Value) - 1;
    displayNumbersToUse.Add(displayNumber);
}

if (!File.Exists(args[1]))
{
    Console.WriteLine("The supplied RDP file does not exist.");
    return;
}

//Write the monitor settings to the RDP file.
string monitorString = string.Join(',', displayNumbersToUse);
List<string> lines = File.ReadAllLines(args[1]).ToList();
int monitorsLineToEdit = lines.FindIndex(x => x.StartsWith("selectedmonitors"));
if (monitorsLineToEdit == -1)
{
    lines.Add($"selectedmonitors:s:{monitorString}");
} 
else
{
    lines[monitorsLineToEdit] = $"selectedmonitors:s:{monitorString}";
}

int useMultiLineToEdit = lines.FindIndex(x => x.StartsWith("use multimon"));
if (useMultiLineToEdit == -1)
{
    lines.Add($"use multimon:i:1");
}
else
{
    lines[useMultiLineToEdit] = $"use multimon:i:1";
}
File.WriteAllLines(args[1], lines);

//Start RDP with our updated connection file
ProcessStartInfo startInfo = new ProcessStartInfo("mstsc.exe", $"\"{args[1]}\"");
Process.Start(startInfo);