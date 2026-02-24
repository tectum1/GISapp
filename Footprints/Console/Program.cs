using Footprints.Core;

class Program
{
    static async Task Main(string[] args)
    {
        double lat = 39.08096981794863; //Latitude
        double lon = -94.41063214609272; //Longitude
        int levelOfDetail = 9; //All Quadkey urls are at level 9
        //string downloadDir = Directory.GetCurrentDirectory();
        string downloadDir =  @"C:\Users\Dylan\RiderProjects\GISapp\Footprints";
        string quadKey = Quadkey.LatLonToQuadKey(lat, lon, levelOfDetail);
        Console.WriteLine("quadKey: " + quadKey);
        string downloadURL = ApiClient.quadkeyToUrl(quadKey);
        Console.WriteLine("downloadURL: " + downloadURL);
        string downloadedFile = await ApiClient.downloadBuildingFootprints(downloadURL, downloadDir, quadKey);
        if (downloadedFile == "")
        {
            Environment.Exit(-1);
        }
        ApiClient.extractBF(downloadedFile);
    }
}

