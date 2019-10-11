using System.Collections;
using UnityEngine;
using Microsoft.Azure.Kinect.Sensor.BodyTracking;

public class StringHelper : MonoBehaviour
{
    Random rand;
    MakeFile file;
    void Start()
    {
        rand = new Random();
        file = GetComponent<MakeFile>();
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        for (int i = 0; i < (int)JointId.Count; i++)
        {
            file.WriteToFile("jizz" + i);

            yield return new WaitForEndOfFrame();

        }
#if UNITY_EDITOR
        //Simulate pressing play button to stop scene from playing
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    //todo method to help format position and rotation data by joint name
}
