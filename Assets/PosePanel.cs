using UnityEngine;
using System.Collections.Generic;
using APRLM.Game;
using UnityEngine.UI;

public class PosePanel : MonoBehaviour
{
	//dragged in manually, else is last child
	[Tooltip("Dragged in manually!")]
	public Text poseText;

	private void Start()
	{
		//Request pose data from GM
		ParsePoseList(GameManager.Instance.poseList);
        HighlightCurrentPose();

    }

	private void ParsePoseList(List<Pose> poseList)
	{
		foreach (Pose p in poseList)
		{
			poseText.text += p.poseName.ToString() + "\n";
		}
	}
    //todo highlight the current pose we are about to capture / capturing
    private void HighlightCurrentPose()
    {
        //get the first line(first pose)
        string firstLineCached = poseText.text.Substring(0, poseText.text.IndexOf("\n", System.StringComparison.CurrentCulture));
        string firstLineUpdated = firstLineCached;
        //add html color shit to change it
        firstLineUpdated = "<color='red'>" + firstLineUpdated + "</color>";
        print(firstLineUpdated); //succesfully is turned red

        //assign the firstline back with the color html code
        //todo doesn't work, just delete first line then add new one from index 0 
        poseText.text = poseText.text.Replace(firstLineCached,firstLineUpdated);
        print(firstLineCached);
    }
}
