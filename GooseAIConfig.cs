using Newtonsoft.Json;
using System;
using System.IO;

public class GooseAIConfig
{
    public string api_key;
    public string endpoint;
    public string model;
    public float chanceToSpeak;
    public string systemPrompt;
    public string bubbleText;
    public string bubbleEnterText;
    public string boxType;

    //fonts next?

    public static GooseAIConfig Load()
    {
        // path of the .dll
        string dllPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
        string dllDir = System.IO.Path.GetDirectoryName(dllPath);

        // look for gooseai.json in that exact same folder
        string configPath = System.IO.Path.Combine(dllDir, "gooseai.json");

        if (!System.IO.File.Exists(configPath))
            throw new Exception("Could not find config file at: " + configPath);

        string json = System.IO.File.ReadAllText(configPath);
        return Newtonsoft.Json.JsonConvert.DeserializeObject<GooseAIConfig>(json);
    }

}
