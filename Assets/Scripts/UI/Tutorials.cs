using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorials : MonoBehaviour {
    public GameObject townTutorial;
    public GameObject gatheringTutorial;
    public GameObject nighttimeTutorial;

    public void ShowTownTutorial() {
        if (!GameManager.instance.FLAG_TOWN_TUTORIAL_SEEN) {
            gameObject.SetActive(true);
            townTutorial.SetActive(true);
            GameManager.instance.FLAG_TOWN_TUTORIAL_SEEN = true;
        }
    }

    public void ShowGatheringTutorial() {
        if (!GameManager.instance.FLAG_GATHERING_TUTORIAL_SEEN) {
            gameObject.SetActive(true);
            gatheringTutorial.SetActive(true);
            GameManager.instance.FLAG_GATHERING_TUTORIAL_SEEN = true;
        }
    }

    public void ShowNighttimeTutorial() {
        if (!GameManager.instance.FLAG_NIGHT_TUTORIAL_SEEN) {
            gameObject.SetActive(true);
            nighttimeTutorial.SetActive(true);
            GameManager.instance.FLAG_NIGHT_TUTORIAL_SEEN = true;
            GameManager.instance.map.SetAllowMovement(false);
        }
    }
}
