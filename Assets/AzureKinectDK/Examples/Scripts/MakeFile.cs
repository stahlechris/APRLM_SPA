using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using Microsoft.Azure.Kinect.Sensor;
using Environment = System.Environment;

/// <summary>
/// WARNING: Using a backslash will make this script explode.
/// </summary>
///
[ExecuteInEditMode]
public class MakeFile : MonoBehaviour  //TODO this is making a folder within the folder you created and putting the file in the sub folder
{
	[Header("The path of where the file will be saved to after playing scene.")]
	[Tooltip("Defaults to saving file in Assets folder.")]
	public string PATH = ""; // "D:\\Repos\\9DegreesOfHuman_Github\\APRLM_SPA\\Assets\\poses";
	// TODO seems to make a folder within a folder?

    [Header("Make a cool name for your file")]
    [Tooltip("Defaults to 'myfile' ")]
    public string FILE_NAME = "";

    [Header("Hey bro??")]
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

	List<int> intList = new List<int>();

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
#if UNITY_EDITOR_OSX
		char slashMac = '\u2215';
		slashReturn = new String(slashMac, 1);
		return slashReturn;
#endif

#if UNITY_EDITOR_WIN
		char slashWin = '\u005c';
		slashReturn = new String(slashWin, 1);
		return slashReturn;
#endif

		print("Unrecognized platform: " + Application.platform);
		return slashReturn;
	}

	string getDataPath()
	{
		String returnPath = "returnPath";
#if UNITY_EDITOR_OSX
		returnPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments); // /Users/username
		returnPath += "/Documents/APRLM";
		print("returnPath:" + returnPath); //returnPath:/Users/stahle/Documents/APRLM
#endif

#if UNITY_EDITOR_WIN
		returnPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		returnPath += "\\APRLM";
#endif
		return returnPath;
	}

	string getFolderName(bool writeRaw)
	{
		string folderName = "";
		if (!FOLDER_NAME.Equals(""))
		{
			if (!FOLDER_NAME.EndsWith(getSlash(),StringComparison.CurrentCulture))
			{
				FOLDER_NAME = FOLDER_NAME + getSlash();
			}
			folderName = FOLDER_NAME;
		}

		if (writeRaw)
		{
			folderName = "raw" + getSlash() + folderName;
		}
		else
		{
			folderName = "avg" + getSlash() + folderName;
		}

		return folderName;
	}

	string getPath()
	{
		string filePath = pathFolderFileExt[0] + pathFolderFileExt[1] + pathFolderFileExt[2];
		return filePath;
	}

	string getFileName()
	{
		string fileName = FILE_NAME.Length > 0 ? FILE_NAME : "myfile";
		string ymd_hms = DateTime.Now.ToString("yyyyMMdd_HHmmss");
		fileName = ymd_hms + fileName;
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
		return fileLoc;
	}

	void InitFileAndFolder(bool writeRaw=false)
	{
		FILE_DESCRIPTION = "[Insert file description here]";

		pathFolderFileExt[0] = PATH.Length > 0 ? PATH : getDataPath();
		pathFolderFileExt[1] = getSlash();
		pathFolderFileExt[2] = getFolderName(writeRaw);
		pathFolderFileExt[3] = getFileName();
		pathFolderFileExt[4] = FILE_EXTENSION;

		if (FOLDER_NAME.Length > 0)
		{
			try
			{
				var dirToCreate = getPath();
				var directory = Directory.CreateDirectory(dirToCreate);
				char[] dirAsCharArr = directory.ToString().ToCharArray();
				string charAt = "";
				int idx = 0;
				Array.ForEach(dirAsCharArr, c =>
				{
					charAt = directory.ToString().Substring(idx, idx + 1);
					print(charAt + ": " + (int)c);
					idx += 1;
				});
//#if UNITY_EDITOR_OSX
//				directory = Directory
//					.CreateDirectory("⁩/⁨Users⁩/⁨stahle⁩/Unity2019⁩/⁨APRLM_SPA⁩/Assets/poseMacTest"); // these worked
//				pathFolderFileExt[0] = "⁩/⁨Users⁩/⁨stahle⁩/Unity2019⁩/⁨APRLM_SPA⁩/Assets"; // kind of?
//				pathFolderFileExt[1] = "⁩/";
//				pathFolderFileExt[2] = "poseMacTest/⁩";
//#endif
				Debug.Log("DirToCreate: " + dirToCreate); //DirToCreate: /Users/stahle/Documents/APRLM∕raw∕poseList∕
				Debug.Log("directory: " + directory);
				Debug.Log("file " + getFileLocation()); //file ⁩/⁨Users⁩/⁨stahle⁩/Unity2019⁩/⁨APRLM_SPA⁩/Assets⁩/poseMacTest/⁩20191016_075725myfile.txt
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
    public void WriteToFile(string message)  //TODO make a check to see if folder exists. (aka so we don't have to delete manually after every test capture)
    {
		InitFileAndFolder();
		print("Write path " + getFileLocation());

        print("writing...");
        File.WriteAllText(getFileLocation(), System.DateTime.Now + "\n" + FILE_DESCRIPTION + "\n" + message);
        print("wrote " + System.DateTime.Now + "\n" + FILE_DESCRIPTION + "\n" + message);
	}

	public void WriteRawToFile(string message)  //TODO make a check to see if folder exists. (aka so we don't have to delete manually after every test capture)
	{
		bool writeRawData = true;
		InitFileAndFolder(writeRawData);
		print("Write raw path " + getFileLocation()); //Write raw path ⁩/⁨Users⁩/⁨stahle⁩/Unity2019⁩/⁨APRLM_SPA⁩/Assets⁩/poseMacTest/⁩20191016_075725myfile.txt
		print("writing raw...");
		File.WriteAllText(getFileLocation(), System.DateTime.Now + "\n" + FILE_DESCRIPTION + "\n" + message);
		print("wrote raw " + System.DateTime.Now + "\n" + FILE_DESCRIPTION + "\n" + message);

	}

	private void OnApplicationQuit()
    {
        awakeCount++;
        print("quit" + awakeCount);
	}

	private void print(string msg)
	{
		Debug.Log(msg);
	}
}
