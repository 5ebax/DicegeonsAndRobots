using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollMusic : MonoBehaviour
{
    private AudioManager audioM;
    private float volume = 0.3F;

    private void Awake()
    {
        audioM = AudioManager.Instance;
    }

    public void SetMusic(int resultLucky, int resultUnlucky, int resultEnemy)
    {
        switch (resultLucky)
        {
            case 1:
                audioM.ChangeVolume("V1", volume);
                break;
            case 2:
                audioM.ChangeVolume("V2", volume);
                break;
            case 3:
                audioM.ChangeVolume("V3", volume);
                break;
            case 4:
                audioM.ChangeVolume("V4", volume);
                break;
            case 5:
                audioM.ChangeVolume("V5", volume);
                break;
            case 6:
                audioM.ChangeVolume("V6", volume);
                break;
        }

        switch (resultUnlucky)
        {
            case 1:
                audioM.ChangeVolume("DV1", volume);
                break;
            case 2:
                audioM.ChangeVolume("DV2", volume);
                break;
            case 3:
                audioM.ChangeVolume("DV3", volume);
                break;
            case 4:
                audioM.ChangeVolume("DV4", volume);
                break;
            case 5:
                audioM.ChangeVolume("DV5", volume);
                break;
            case 6:
                audioM.ChangeVolume("DV6", volume);
                break;
        }

        switch (resultEnemy)
        {
            case 1:
                audioM.ChangeVolume("E1", volume);
                break;
            case 2:
                audioM.ChangeVolume("E2", volume);
                break;
            case 3:
                audioM.ChangeVolume("E3", volume);
                break;
            case 4:
                audioM.ChangeVolume("E4", volume);
                break;
        }
    }

}
