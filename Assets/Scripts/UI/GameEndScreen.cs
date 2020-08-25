using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEndScreen : MonoBehaviour {
    [SerializeField] private Image _background = null;
    [SerializeField] private GameObject _evacuateEnding = null;
    [SerializeField] private GameObject _missileEnding = null;
    [SerializeField] private LoseScreen _loseScreen = null;
    [SerializeField] private GameObject _restartButton = null;
    [SerializeField] private float _fadeTime = 0;

    public IEnumerator ShowEndScreen(GameEndType gameEndType) {
        yield return StartCoroutine(FadeBackgroundIn());
        yield return new WaitForSeconds(0.5f);

        switch (gameEndType) {
            case (GameEndType.SHIP_ESCAPE):
                _evacuateEnding.SetActive(true);
                break;
            case (GameEndType.MAGIC_MISSILE):
                _missileEnding.SetActive(true);
                break;
            case (GameEndType.NO_HEALTH):
                _loseScreen.gameObject.SetActive(true);
                _loseScreen.SetDetails("you ran out of health");
                break;
            case (GameEndType.STARVATION):
                _loseScreen.gameObject.SetActive(true);
                _loseScreen.SetDetails("you ran out of food");
                break;
            case (GameEndType.DEBT):
                _loseScreen.gameObject.SetActive(true);
                _loseScreen.SetDetails("you ran afoul of the debt collectors");
                break;
            case (GameEndType.THEY_ARRIVE):
                _loseScreen.gameObject.SetActive(true);
                _loseScreen.SetDetails("the empire is upon you");
                break;
            default:
                Debug.LogWarning("Unhandled GameEndType");
                break;
        }
        _restartButton.SetActive(true);
    }

    private IEnumerator FadeBackgroundIn() {
        _background.gameObject.SetActive(true);
        float elapsedTime = 0;
        float alpha = 0;
        Color colour;
        while (_background.color.a < 1) {
            elapsedTime += Time.deltaTime;
            alpha = Mathf.Lerp(0, 1, elapsedTime / _fadeTime);
            colour = _background.color;
            colour.a = alpha;
            _background.color = colour;
            yield return null;
        }
    }
}
