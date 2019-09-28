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
	public string PATH = ""; // "D:\\Repos\\9DegreesOfHuman_Github\\APRLM_SPA\\Assets\\poses";
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

    //[Header("The path of where the new folder will be saved to after playing scene.")]
    //[Tooltip("Defaults to not creating a new folder, else made in Assets if folder gets name ")]
    //public string FOLDER_PATH = "";

    //it's always a .txt file...bitch
    const string FILE_EXTENSION = ".txt";

	string[] pathFolderFileExt = new string[5];

    static int awakeCount = 0; //this must be static
    int pausedCount = 0; //this doesn't have to be static

	private static MakeFile _instance;
	
	//public static MakeFile getInstance()
	public static MakeFile Instance { get { return _instance; } }
	
	//When executingInEditorMode:
	//Awake() will be called when scene starts and stops...
	//EditorApplication.isPaused will run 4 times, 2 when pause is pressed, 2 when released
	//so use counter to track the number of pauses and awakes
	void Awake()//Awake will always run twice in EditorMode
	{
		if (_instance != null && _instance != this)
		{
			Destroy(this.gameObject); // destroy self if instance prev exists other than self
			Debug.Log("Singleton exists, destroying self");
		}
		else
		{
			_instance = this;
			Debug.Log("Singleton created");
		}

		if (++awakeCount == 1) //this is used to detect pauses...
        {
			InitFileAndFolder();
			//print("Init path: " + PATH);
			//print("Init name: " + FILE_NAME);
			//print("Init desc: " + FILE_DESCRIPTION);
			//print("Init name folder: " + FOLDER_NAME);
			//print("Init path folder: " + FOLDER_PATH);
		}
    }

	bool isWindows()
	{
		return Application.platform == RuntimePlatform.WindowsEditor
			|| Application.platform == RuntimePlatform.WindowsPlayer;
	}

	bool isMac()
	{
		return Application.platform == RuntimePlatform.OSXEditor
			|| Application.platform == RuntimePlatform.OSXPlayer;
	}

	string getSlash()
	{
		string slashReturn = "";
		char slashMac = '\u2215';
		char slashWin = '\u005c';
		if (isWindows())
		{
			//SOURCE_PATH = SOURCE_WINDOWS_PATH;
			//print("you're on a pc");
			slashReturn = new String(slashWin, 1);
		}
		else if (isMac())
		{
			//SOURCE_PATH = SOURCE_OSX_PATH;
			//print("you're on a mac/linux");
			slashReturn = new String(slashMac, 1);
		}
		else
		{
			print("Unrecognized platform: " + Application.platform);
		}
		//print("Data path: " + getDataPath());

		//print("returning slash: " + slashReturn);
		return slashReturn;
	}

	string getDataPath()
	{
		if (isWindows())
		{
			return Application.dataPath.Replace("/", "\\");
		}
		return Application.dataPath;
	}

	string getFolderName()
	{
		string folderName = "";
		if (!FOLDER_NAME.Equals(""))
		{ // do we need to create the folder first?
			if (!FOLDER_NAME.EndsWith(getSlash()))
			{
				FOLDER_NAME = FOLDER_NAME + getSlash(); // trailing slash
			}
			folderName = FOLDER_NAME;
		}

		return folderName;
	}

	string getPath()
	{
		string filePath = pathFolderFileExt[0] + pathFolderFileExt[1] + pathFolderFileExt[2];
		//print("File Path: " + filePath);
		return filePath;
	}

	string getFileName()
	{
		string fileName = FILE_NAME.Length > 0 ? FILE_NAME : "myfile";
		string ymd_hms = DateTime.Now.ToString("yyyyMMdd_HHmmss");
		fileName = fileName + ymd_hms;
		//Debug.Log(DateTime.Now);                        // 09/28/2019 17:05:12
		//Debug.Log(DateTime.Now.ToString());             // 9/28/2019 5:05:12 PM
		//Debug.Log(DateTime.Now.ToShortDateString());    // 9/28/2019
		//Debug.Log(DateTime.Now.ToLongDateString());     // Saturday, September 28, 2019
		//Debug.Log(DateTime.Now.ToShortTimeString());    // 5:05 PM
		//Debug.Log(DateTime.Now.ToLongTimeString());     // 5:05:12 PM
		//Debug.Log(DateTime.Now.ToUniversalTime());      // 09/28/2019 22:05:12
		//Debug.Log(DateTime.Now.ToLocalTime());          // 09/28/2019 17:05:12
		//Debug.Log(DateTime.Now.ToString("yyyyMMdd_HHmmss"));// 20190928_170923


		return fileName;
	}

	string getFileLocation()
	{
		string fileLoc = string.Join("", pathFolderFileExt);
		//print("File Location: " + fileLoc);
		return fileLoc;
	}

	void InitFileAndFolder()
	{
		FILE_DESCRIPTION = "[Insert file description here]";

		pathFolderFileExt[0] = PATH.Length > 0 ? PATH : getDataPath();
		pathFolderFileExt[1] = getSlash(); // trailing slash
		pathFolderFileExt[2] = getFolderName();
		pathFolderFileExt[3] = getFileName();
		pathFolderFileExt[4] = FILE_EXTENSION;

		if (FOLDER_NAME.Length > 0)
		{
			try
			{
				var dirToCreate = getPath();
				var directory = Directory.CreateDirectory(dirToCreate);
				Debug.Assert(dirToCreate.Equals(directory));
				//print("Full name: " + directory.FullName);
				print("Create dir: " + dirToCreate);
				print("file " + getFileLocation());
				//PATH = directory.FullName;
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
		InitFileAndFolder();
		print("Write path " + getFileLocation());
		//If file doesn't exist at specified path...
		//if (!File.Exists(getFileLocation()))
  //      {
            print("writing...");
            File.WriteAllText(getFileLocation(), System.DateTime.Now + "\n" + FILE_DESCRIPTION + "\n" + message);
            print("wrote " + System.DateTime.Now + "\n" + FILE_DESCRIPTION + "\n" + message);
            //TODO FORMAT DATA , GET RANDOM DATA FROM CREATING A SKELETON REAL QUICK
        //}
        //If file does exist at that path...aka we have called this method twice in one session(which we will be doing, bro)
        //else if (File.Exists(getFileLocation()))
        //{
        //    File.AppendAllText(getFileLocation(), "\n" + message);
        //    print("wrote " + message);
        //}
        //if we already have joint data written here that means we either forgot to move the folder/file or we are trying to rewrite it
        //else if()//we have more than 26 lines of data, it means we have written joint data to this thing
    }

    private void OnApplicationQuit()
    {
        awakeCount++;
        print("quit" + awakeCount); //2
	}

	private void print(string msg)
	{
		Debug.Log(msg);
	}
}
