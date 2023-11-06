using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private float CheckPointPositionX, checkPointPositionY;
    void Start()
    {
        if (PlayerPrefs.GetFloat("CheckPointPositionX") != 0)
        {
            transform.position = (new Vector2(PlayerPrefs.GetFloat("CheckPointPositionX"), PlayerPrefs.GetFloat("checkPointPositionY")));
        }


    }

    public void ReachedCheckPoint(float x, float y)
    {
        // CheckPointPositionX = x;
        // checkPointPositionY = y;

        PlayerPrefs.SetFloat("CheckPointPositionX", x);
        PlayerPrefs.SetFloat("checkPointPositionY", y);

    }

}
