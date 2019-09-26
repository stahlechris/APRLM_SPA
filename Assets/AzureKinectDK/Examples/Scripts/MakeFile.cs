using UnityEngine;
using System.IO;
using System;
using Microsoft.Azure.Kinect.Sensor;
/// <summary>
/// WARNING: Using a backslash will make this script explode.
/// </summary>
///
[ExecuteInEditMode]
public class MakeFile : MonoBehaviour
{
    [Header("The path of where the file will be saved to after playing scene.")]
    [Tooltip("Defaults to saving file in Assets folder.")]
    public string FILE_PATH = "D:\\Repos\\9DegreesOfHuman_Github\\APRLM_SPA\\Assets\\poses\\myfile.txt";
	// TODO seems to make a folder within a folder?
    [Header("Make a cool name for your file")]
    [Tooltip("Defaults to 'myfile' ")]
    public string FILE_NAME = "";

    [Header("Hey bro?")]
    [Tooltip("Defaults to '[insert description here]'.")]
    public string FILE_DESCRIPTION = "";

    [Header("If you want to put your new file into a matching new folder, give that folder a name NOW.")]
    [Tooltip("Defaults to not creating a new folder.")]
    public string FOLDER_NAME = "";

    [Header("The path of where the new folder will be saved to after playing scene.")]
    [Tooltip("Defaults to not creating a new folder, else made in Assets if folder gets name ")]
    public string FOLDER_PATH = "";

    //it's always a .txt file...bitch
    const string FILE_EXTENSION = ".txt";

    static int awakeCount = 0; //this must be static
    int pausedCount = 0; //this doesn't have to be static

    //When executingInEditorMode:
    //Awake() will be called when scene starts and stops...
    //EditorApplication.isPaused will run 4 times, 2 when pause is pressed, 2 when released
    //so use counter to track the number of pauses and awakes
    void Awake()//Awake will always run twice in EditorMode
    {
        if (++awakeCount == 1) //this is used to detect pauses...
        {
            //InitFileAndFolder();
			Debug.Log("Init path: " + FILE_PATH);
		}
    }

	bool isWindows()
	{
		return Application.platform == RuntimePlatform.WindowsEditor;
	}

	string getSlash()
	{
		string slashMac = "/";
		string slashWin = "\\";
		if (Application.platform == RuntimePlatform.WindowsEditor)
		{
			//SOURCE_PATH = SOURCE_WINDOWS_PATH;
			Debug.Log("you're on a pc");
			return slashWin;
		}
		//else if (Application.platform == RuntimePlatform.OSXEditor)
		//{
		//	//SOURCE_PATH = SOURCE_OSX_PATH;
		//	Debug.Log("you're on a mac");
		//	return slashMac;
		//}
		Debug.Log("you're on a mac/linux");
		return slashMac;
	}

	string getDataPath()
	{
		if (isWindows())
		{
			return Application.dataPath.Replace("/", "\\");
		}
		return Application.dataPath;
	}

	void InitFileAndFolder()
    {
		Debug.Log("getDataPath " + getDataPath());
		FILE_NAME = "myfile.txt";
        FILE_PATH = getDataPath() + getSlash() + FILE_NAME;
        FILE_DESCRIPTION = "[Insert file description here]";

        //You gave the folder a name and the folder a path...
        if(FOLDER_NAME != "" && FOLDER_PATH != "")
        {
			try
			{
				var directory = Directory.CreateDirectory(FOLDER_PATH + getSlash() + FOLDER_NAME);

				FOLDER_PATH = directory.FullName;
				FILE_PATH = FOLDER_PATH + getSlash() + FILE_NAME;
			}
			catch (Exception e)
			{
				print("Failed to make the folder you wanted." + e);
			}
        }
        //You gave the folder a name, but didn't give it a path...
        else if (FOLDER_NAME != "" && FOLDER_PATH == "")
        {
            try
            {
                var directory = Directory.CreateDirectory(Application.dataPath + getSlash() + FOLDER_NAME);

                FOLDER_PATH = directory.FullName;
                FILE_PATH = FOLDER_PATH + getSlash() + FILE_NAME;
            }
            catch (Exception e)
            {
                print("Failed to make the folder you wanted." + e);
            }
        }
    }

    /// <summary>
    //Declare a MakeFile file = new MakeFile(); then...
    //file.WriteToFile("49");
    /// </summary>
    public void WriteToFile(string message)
    {
		Debug.Log("Write path " + FILE_PATH);
		//If file doesn't exist at specified path...
		if (!File.Exists(FILE_PATH))
        {
            print("writing...");
            File.WriteAllText(FILE_PATH, System.DateTime.Now + "\n" + FILE_DESCRIPTION + "\n" + message);
            print("wrote " + System.DateTime.Now + "\n" + FILE_DESCRIPTION + "\n" + message);
            //TODO FORMAT DATA , GET RANDOM DATA FROM CREATING A SKELETON REAL QUICK
        }
        //If file does exist at that path...aka we have called this method twice in one session(which we will be doing, bro)
        else if (File.Exists(FILE_PATH))
        {
            File.AppendAllText(FILE_PATH, "\n" + message);
            print("wrote " + message);
        }
        //if we already have joint data written here that means we either forgot to move the folder/file or we are trying to rewrite it
        //else if()//we have more than 26 lines of data, it means we have written joint data to this thing
    }
    private void OnApplicationQuit()
    {
        awakeCount++;
        print("quit" + awakeCount); //2
    }
}
