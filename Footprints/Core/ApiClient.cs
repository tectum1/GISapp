using System.ComponentModel.Design;
using System.IO.Compression;

namespace Footprints.Core;
using System.IO;

public class ApiClient
{
    
    public static Action<string>? Logger { get; set; }
    private static void WriteLog(string message)
    {
        if (Logger != null) Logger(message);
        else Console.WriteLine(message);
    }

    public static string quadkeyToUrl(string quadkey)
    {
        string readText = File.ReadAllText("..\\..\\..\\..\\Core\\dataset-links.csv");
        string[] textLines = readText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        int lastIdx = textLines.Length;
        string[] foundSplit = [];
        for (int i = 0; i < lastIdx; i++)
        {
            if (textLines[i].Contains(quadkey))
            {
                foundSplit = textLines[i].Split(",");
            }

        }
        string returnURL = "";
        if (foundSplit.Length > 0)
        {
            returnURL = foundSplit[2];
        }

        if (returnURL == "")
        {
            WriteLog("URL not found in dataset-links.csv. Exiting...");
            Environment.Exit(-1);
        }
        
        return returnURL;
    }

    
    public static async Task<string> downloadBuildingFootprints(string url, string dir, string quadkey)
    {
        using var client = new HttpClient(); // For simple examples, a new instance works.
        try
        {
            // Get the file stream from the URL
            using Stream responseStream = await client.GetStreamAsync(url);
        
            // Create a local file stream and copy the data
            string localFile = dir + "\\MS_BuildingFootprints_" + quadkey + ".geojson.gz";
            WriteLog("localFile: " + localFile);
            string targetFilePath = localFile.Replace(".gz", "");
            if (File.Exists(targetFilePath))
            {
                WriteLog("File exists! Download skipped.");
                return "";
            }

            using FileStream fileStream = File.Create(localFile);
            await responseStream.CopyToAsync(fileStream);

            WriteLog($"File downloaded successfully to: {localFile}");
            return localFile;
        }
        catch (HttpRequestException ex)
        {
            WriteLog($"Failed to download file. Status code: {ex.Message}");
        }
        catch (Exception ex)
        {
            WriteLog($"An error occurred: {ex.Message}");
        }
        return "";
    }

    public static void extractBF(string filepath)
    {
        if (!File.Exists(filepath))  // Handle non existant file
        {
            WriteLog($"Filepath: {filepath}. Does not exist");
            return;
        }
        if (!filepath.Contains(".gz"))  // Handle not extractable file
        {
            WriteLog($"File extension .gz not found");
            return;
        }
        string outputPath = filepath.Replace(".gz", "");
        
        using (FileStream compressedFileStream = File.Open(filepath, FileMode.Open))
        using (FileStream outputFileStream = File.Create(outputPath))
        using (GZipStream decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress))
        {
            decompressor.CopyTo(outputFileStream); // Decompress data to output file
        }
        WriteLog($"{filepath} has been extracted to {outputPath}");
        File.Delete(filepath);
        WriteLog($"{filepath} removed");
        string readText = File.ReadAllText(outputPath);
        string[] textLines = readText.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
        int lastIdx = textLines.Length;
        int featuresFound = 0;
        for (int i=0; i<lastIdx; i++)
        {
            if (textLines[i].Contains("Feature"))
            {
                featuresFound++;
            }
        }
        WriteLog($"{featuresFound} Building Footprints Downloaded...");
    }
    
}