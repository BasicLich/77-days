using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class LightController : MonoBehaviour {
    [Header("References")]
    [SerializeField] private Light2D _globalLight = null;

    [Header("Parameters")]
    [SerializeField] private float _dayIntensity = 0;
    [SerializeField] private float _nightIntensity = 0;
    [SerializeField] private float _transitionDuration = 0;

    private Coroutine _currTransition = null;

    public void RaiseSun() {
        if (_currTransition != null) {
            StopCoroutine(_currTransition);
        }
        _currTransition = StartCoroutine(TransitionLight(_dayIntensity));
    }

    public void SetSun() {
        if (_currTransition != null) {
            StopCoroutine(_currTransition);
        }
        _currTransition = StartCoroutine(TransitionLight(_nightIntensity));
    }

    private IEnumerator TransitionLight(float intensity) {
        float elapsedTime = 0;
        float t = 0;
        float startIntensity = _globalLight.intensity;
        while (_globalLight.intensity != intensity) {
            elapsedTime += Time.deltaTime;
            t = Mathf.SmoothStep(0, 1, elapsedTime / _transitionDuration);
            _globalLight.intensity = Mathf.Lerp(startIntensity, intensity, t);
            yield return null;
        }
        _currTransition = null;
    }
}
