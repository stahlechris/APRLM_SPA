using UnityEngine;
using System.Collections.Generic;
using APRLM.Game;
using UnityEngine.UI;

public class PosePanel : MonoBehaviour
{
	//dragged in manually, else is last child
	[Tooltip("Dragged in manually!")]
	public Text poseText;

    public Text capturedPoseText;//dragged in manually
    string firstLineCached;
    string firstLineUpdated;
    void Start()
	{
		//Request pose data from GM
		ParsePoseList(GameManager.Instance.poseList);
    }

	void ParsePoseList(List<Pose> poseList)
	{
		foreach (Pose p in poseList)
		{
			poseText.text += p.poseName.ToString() + "\n";
		}
	}
    //todo this doesn't doesn't work when we go to add the next succesfull pose
    public void MarkPoseAsSuccesfullyCaptured_LinkedToButton()//(accept pose) save data to file button
    {
        //take the first line from pose text and put it in captured pose text
        capturedPoseText.text += firstLineCached + "\n";
        //delete firstline from pose text
        poseText.text = poseText.text.Replace(firstLineUpdated, "");
        //trim the whitespace away
        poseText.text = poseText.text.Trim();
        //clear the caches (CASH-SHAY'S...don't @ me)
        firstLineCached = "";
        firstLineUpdated = "";
    }
    public void HighlightCurrentPose_LinkedToButton()//record pose button
    {
        //todo this doesn't highlight during the next pose capture, it is getting the "" we want to it to get the next line
        //get the first line(first pose)
        //todo i think this doesn't work when theres only one pose in there
        //add an if check to see if a newline character is there or keep up with pose list.Count to know when we are on the last line
        firstLineCached = poseText.text.Substring(0, poseText.text.IndexOf("\n", System.StringComparison.CurrentCulture));
        //bug this is gonna get just the "" the second and subsequent times
        firstLineUpdated = firstLineCached;
        //add html code to change the color of this string
        firstLineUpdated = "<color='red'>" + firstLineUpdated + "</color>";
        //assign the firstline back with the color html code (strings are immutable, bro)
        poseText.text = poseText.text.Replace(firstLineCached,firstLineUpdated);
    }

}
