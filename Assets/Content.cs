using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Content.cs adds Containers to a Content panel
/// </summary>
public class Content : MonoBehaviour
{
    GameObject pressEnterToAddNextPose; //found at runtime. The last obj in this hierarchy will be this empty holding the text
    public GameObject PressEnterToAddNextPose_prefab;//manually dragged in from prefab folder
    public GameObject Container_prefab;//manually dragged in from prefab folder
    /// <summary>
    /// The two built in callbacks for input fields are:
    /// 1.OnValueChanged is called every time a user enters a new character into the input field
    /// 2.OnEndEdit is called after the user clicks away from the input field && when the user presses enter
    /// </summary>
    public void OnEndEdit_LinkedToInputField()
    {
        //todo set raycastTarget to true on Container's,InputFields.getComponent(text)

        print("onEndEdit_linkedToInputField called!");
        //1. Delete "press enter to..." object
        pressEnterToAddNextPose = transform.GetChild(transform.childCount - 1).gameObject;
        Destroy(pressEnterToAddNextPose);
        //2. Instantiate a new container and store a reference to it
        GameObject container_clone = Instantiate(Container_prefab, transform);
        //3.Instantiate a "press enter to..." prefab
        Instantiate(PressEnterToAddNextPose_prefab,transform);
        //4.The Container_prefab's InputField component script needs to-
        //-register for OnEndEdit callbacks to hit this method
        //4a.Get a reference to Container_prefab's last child's InputField
        InputField inputField = container_clone.GetComponentInChildren<InputField>();
        //4b.Add a listener to the inputField's OnEndEdit callback
        inputField.onEndEdit.AddListener(delegate{ OnEndEdit_LinkedToInputField(); });

        //todo there is a bug where you can't go back and edit the input fields
        //bc the text components "raycastTarget" gets set to false after you OnEndEdit
    }
    public void Submit_LinkedToButton()
    {
        //todo
        //ask user if they really want to submit
        //Gather all text from each inputField's text
        //Make a Pose.cs scriptable object for each of the texts
        //store each Pose in a poselist
        //this is GameManager's new poselist
    }
}
