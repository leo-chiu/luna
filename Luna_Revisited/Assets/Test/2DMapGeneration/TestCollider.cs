using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
       // SceneController.instance.FadeAndLoadScene("Second Scene");
    }
    public void switchScenes()
    {
        //SceneController.instance.FadeAndLoadScene("Starting Scene");
    }
}
