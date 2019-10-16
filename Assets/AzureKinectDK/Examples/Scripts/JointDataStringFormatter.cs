using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Kinect.Sensor.BodyTracking;

public static class JointDataStringFormatter
{
	public static string formatJointDataToText(Vector3 singleJointPosition, JointId joint)
	{
		string formattedLine = "Avg " + joint.ToString() + ": " + singleJointPosition.ToString() + "\n";
		return formattedLine;
	}

	public static string formatJointDataToText(Vector3 singleJointPosition, Quaternion singleJointRotation, JointId joint)
	{
		string formattedLine = "Joint " + joint.ToString() + " position: " + singleJointPosition.ToString() + "\n";
		formattedLine += "Joint " + joint.ToString() + " rotation: " + singleJointRotation.ToString() + "\n";

		return formattedLine;
	}
}
