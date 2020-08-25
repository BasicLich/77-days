using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HutCanvas : MonoBehaviour {
    [Header("References")]
    public Image playerSprite;
    public Transform playerDestination;
    public GameObject guidanceText;
    public TextMeshProUGUI scoutText;

    [Space]

    public float slideSpeed;
    public CardEffectsSet aid;
    private bool aidGiven;


    [Header("Dialogue")]
    public List<Dialogue> hueDialogue;
    public List<Dialogue> mereDialogue;
    public List<Dialogue> renegadeDialogue;
    private List<Dialogue> currDialogue;
    private int _dialogueIndex;
    private bool _dialogueActive;

    private void Start() {
        PlayerBackground background = GameManager.instance.player.playerBackground;
        playerSprite.sprite = background.sprite;
        switch (background.backgroundType) {
            case (BackgroundType.HUE):
                currDialogue = hueDialogue;
                break;
            case (BackgroundType.RENEGADE):
                currDialogue = renegadeDialogue;
                break;
            case (BackgroundType.MERE):
                currDialogue = mereDialogue;
                break;
            default:
                currDialogue = hueDialogue;
                break;

        }
        guidanceText.SetActive(false);
        _dialogueActive = false;
        _dialogueIndex = 0;
        StartCoroutine(StartDialogue());
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            TryNext();
        }
        if (Input.GetMouseButtonDown(1)) {
            TrySkip();
        }
    }

    private IEnumerator SlidePlayerIn() {
        float elapsedTime = 0;
        float t = 0;
        Transform player = playerSprite.transform;
        Transform destination = playerDestination;
        Vector3 start = player.position;
        Vector3 end = destination.position;
        float slideDuration = Vector3.Distance(start, end) / slideSpeed;
        while (player.transform.position != destination.position) {
            elapsedTime += Time.deltaTime;
            t = Mathf.SmoothStep(0, 1, elapsedTime / slideDuration);
            player.transform.position = Vector3.Lerp(start, end, t);
            yield return null;
        }
    }

    private IEnumerator StartDialogue() {
        yield return new WaitForSeconds(0.25f);
        yield return StartCoroutine(SlidePlayerIn());
        yield return new WaitForSeconds(1f);
        guidanceText.SetActive(true);
        _dialogueActive = true;
        ShowDialogue();
    }

    private void ShowDialogue() {
        scoutText.text = currDialogue[_dialogueIndex].GetDialogue();
    }

    private void TryNext() {
        if (!_dialogueActive) {
            return;
        }
        // End dialogue if done or hasn't started
        if (_dialogueIndex == currDialogue.Count - 1) {
            Leave();
            return;
        }
        _dialogueIndex++;
        ShowDialogue();
    }

    private void TrySkip() {
        if (!_dialogueActive) {
            return;
        }
        Leave();
    }

    private void Leave() {
        GameManager.instance.map.currLocation.EndActivity();
        if (!aidGiven) {
            if (GameManager.instance.player.CanApplyEffects(aid)) {
                aid.ApplyEffects();
            }
            aidGiven = true;
        }
    }
}
