using System.ComponentModel.Design;
using System.IO.Compression;

namespace Footprints.Core;
using System.IO;

public class ApiClient
{

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
            Console.WriteLine("URL not found in dataset-links.csv. Exiting...");
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
            Console.WriteLine("localFile: " + localFile);
            string targetFilePath = localFile.Replace(".gz", "");
            if (File.Exists(targetFilePath))
            {
                Console.WriteLine("File exists! Download skipped.");
                return "";
            }

            using FileStream fileStream = File.Create(localFile);
            await responseStream.CopyToAsync(fileStream);

            Console.WriteLine($"File downloaded successfully to: {localFile}");
            return localFile;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Failed to download file. Status code: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
        return "";
    }

    public static void extractBF(string filepath)
    {
        if (!File.Exists(filepath))  // Handle non existant file
        {
            Console.WriteLine($"Filepath: {filepath}. Does not exist");
            return;
        }
        if (!filepath.Contains(".gz"))  // Handle not extractable file
        {
            Console.WriteLine($"File extension .gz not found");
            return;
        }
        string outputPath = filepath.Replace(".gz", "");
        
        using (FileStream compressedFileStream = File.Open(filepath, FileMode.Open))
        using (FileStream outputFileStream = File.Create(outputPath))
        using (GZipStream decompressor = new GZipStream(compressedFileStream, CompressionMode.Decompress))
        {
            decompressor.CopyTo(outputFileStream); // Decompress data to output file
        }
        Console.WriteLine($"{filepath} has been extracted to {outputPath}");
        File.Delete(filepath);
        Console.WriteLine($"{filepath} removed");
    }
}