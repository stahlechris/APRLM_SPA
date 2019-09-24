using System;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Kinect.Sensor;
using Microsoft.Azure.Kinect.Sensor.BodyTracking;
using Stahle.Utility;
using APRLM.Game;
using APRLM.Utilities;
using UnityEngine.UI;

public class DebugRenderer : PersistantSingleton<DebugRenderer>
{
    Device device;
    BodyTracker tracker;
    Skeleton skeleton;
    [SerializeField]
    GameObject[] debugObjects;
    public Renderer renderer;
    [SerializeField]
    public List<Skeleton> skeletons = new List<Skeleton>();
	GameObject[] blockman2;
	public Text JointPositionArea_Text;
	public GameObject CapturedResultsRoot;
	public GameObject blockmanCapturedParent;



	public bool canUpdate;

    protected override void Awake()
    {
		print("DebugRenderer Awake");
        base.Awake();
    }

	public void Start()
	{
		print("DebugRenderer start");
		if (CapturedResultsRoot.activeInHierarchy)
		{
			CapturedResultsRoot.SetActive(false);
		}
		debugObjects = GameManager.Instance.blockman;
		foreach (GameObject go in debugObjects)
		{
			go.SetActive(true);
		}
        print("Blockman was fetched from GM and set active here in DebugRenderer");
		InitCamera();
    }

    private void InitCamera()
    {
        this.device = Device.Open(0);
		var config = new DeviceConfiguration
		{
			ColorResolution = ColorResolution.r720p,
			ColorFormat = ImageFormat.ColorBGRA32,
			DepthMode = DepthMode.NFOV_Unbinned
        };
        device.StartCameras(config);

        //declare and initialize a calibration for the camera
        var calibration = device.GetCalibration(config.DepthMode, config.ColorResolution);
        //initialize a tracker with the calibration we just made
        this.tracker = BodyTracker.Create(calibration);
        renderer = GetComponent<Renderer>();
        canUpdate = true;
    }

    void Update() 
    {
        if (canUpdate)
        {
			streamCameraAsTexture();
			captureSkeletons();
			if (skeletons.Count > 4) // and the current scene is CaptureScene
			{
				CapturedResultsRoot.SetActive(true);
				JointPositionArea_Text.text = "";
				averageOutSkeletons();
				showNewBlockman(true);
				//JointPositionArea_Text.text = "hallo you kilt my fader prepare to di";
			}

		}//end if(canUpdate) 
	}//end Update()

	void streamCameraAsTexture()
	{
		//this streams camera output as a texture to a plane in the scene
		using (Capture capture = device.GetCapture())
		{
			tracker.EnqueueCapture(capture);
			var color = capture.Color;
			if (color.WidthPixels > 0)
			{
				Texture2D tex = new Texture2D(color.WidthPixels, color.HeightPixels, TextureFormat.BGRA32, false);
				tex.LoadRawTextureData(color.GetBufferCopy());
				tex.Apply();
				renderer.material.mainTexture = tex;
			}
		}
	}

	void captureSkeletons()
	{
		//this gets skeleton data from frames and pulls individual joint data from the skeleton to apply to blocks that represent joints
		using (var frame = tracker.PopResult())
		{
			Debug.LogFormat("{0} bodies found.", frame.NumBodies);
			if (frame.NumBodies > 0)
			{
				var bodyId = frame.GetBodyId(0);
				//Debug.LogFormat("bodyId={0}", bodyId);
				this.skeleton = frame.GetSkeleton(0);
				skeletons.Add(this.skeleton);
				for (var i = 0; i < (int)JointId.Count; i++)
				{
					var joint = this.skeleton.Joints[i];
					var pos = joint.Position;
					Debug.Log("pos: " + (JointId)i + " " + pos[0] + " " + pos[1] + " " + pos[2]);
					var rot = joint.Orientation;
					Debug.Log("rot " + (JointId)i + " " + rot[0] + " " + rot[1] + " " + rot[2] + " " + rot[3]); // Length 4
					var v = new Vector3(pos[0], -pos[1], pos[2]) * 0.004f;
					var r = new Quaternion(rot[1], rot[2], rot[3], rot[0]);
					var obj = debugObjects[i];
					obj.transform.SetPositionAndRotation(v, r);
				}
			}
		}
	}

	void averageOutSkeletons()
	{
		Debug.Log("creating blockman 2 stuff");
		blockman2 = GameManager.Instance.blockmanCaptured;
		foreach (GameObject go in blockman2)
		{
			go.SetActive(true);
		}

		// skeletons is a List<Skeleton> of size 5
		for (int i = 0; i < (int)JointId.Count; i++)
		{
			List<Vector3> positionsOfSameJointPositions = new List<Vector3>();

			for (int j = 0; j < skeletons.Count; j++)
			{
				float[] pos = skeletons[j].Joints[i].Position;

				Vector3 posV3 = new Vector3(pos[0], -pos[1], pos[2]) * 0.004f;
				positionsOfSameJointPositions.Add(posV3);

			}
			Vector3 averageOfSingleJointI = Vector3Helper.FindAveragePosition(positionsOfSameJointPositions);

			float[] rot = skeletons[0].Joints[i].Orientation;
			Quaternion rotationOfFirstSkeleton = new Quaternion(rot[1], rot[2], rot[3], rot[0]);

			var jointCube = blockman2[i];
			jointCube.transform.SetPositionAndRotation(averageOfSingleJointI, rotationOfFirstSkeleton);


			//now that blockman2 joint is set, format and print that data
			JointPositionArea_Text.text += JointDataStringFormatter.formatJointDataToText(averageOfSingleJointI, (JointId)i);



		}

		Debug.Log("we have enough skeletons");
		//GameManager.Instance.currentState = GameState.CaptureCompleted;
		//Disable this Update loop's logic from running
		canUpdate = false;
		//GameManager.Instance.LoadScene((int)SceneEnums.Scenes.GetReady);
	}

	void showNewBlockman(bool shouldShow)
	{
		if (blockmanCapturedParent == null)
		{
			blockmanCapturedParent = new GameObject();
		}

		foreach (GameObject go in GameManager.Instance.blockman)
		{
			go.transform.SetParent(blockmanCapturedParent.transform);
			go.SetActive(!shouldShow);
		}

		foreach (GameObject go in GameManager.Instance.blockmanCaptured)
		{
			go.SetActive(shouldShow);
		}

		//blockmanCapturedParent.transform.Translate(new Vector3());
	}

	void resetSkeletons()
	{
		skeletons.Clear();
	}

	public void TESTAAJSHDFLJASDHFLKAJSHDFLKAJSDHFKJH()
	{
		//enable canUpdate for skeleton capturing
		canUpdate = true;
		// load new additive scene/panel

		// load skeleton into new blockman
		//GameManager.MakeBlockManCaptured();
		// does this look ok yes no
		// no, redo
		// yes, save and indicate moving to next pose?
	}

    private void OnDisable()
    {
        //todo test if only called once at the end of the program, if so, renable the below
        print("DebugRenderer onDisable was called");
		device.StopCameras();
		//k4a_device_close(device) here.
		if (tracker != null)
		{
			tracker.Dispose();
		}
		if (device != null)
		{
			device.Dispose();
		}
	}


	public void poseAccepted_linkToButton()
	{
		Debug.Log("pose accepted");
		CapturedResultsRoot.SetActive(false);
		showNewBlockman(false);
		resetSkeletons();
		canUpdate = true;
	}

	public void poseDeclined_linkToButton()
	{
		Debug.Log("pose declined");
		CapturedResultsRoot.SetActive(false);
		showNewBlockman(false);
		resetSkeletons();
		canUpdate = true;
	}


}
