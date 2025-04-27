using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightCycleController : MonoBehaviour
{
    [Header("Cycle Durations (seconds)")]
    public float dayDuration = 30f;
    public float nightDuration = 10f;

    [Header("Light Colors")]
    public Color dayColor = Color.white;
    public Color nightColor = new Color(0.2f, 0.2f, 0.6f);

    private Light2D playerLight;
    private CoverDetector2D coverDetector;

    void Awake()
    {
        playerLight = GetComponent<Light2D>();
        coverDetector = GetComponent<CoverDetector2D>();
        // start in “day” mode
        playerLight.color = dayColor;
        coverDetector.enabled = true;
    }

    void Start()
    {
        StartCoroutine(DayNightCycle());
    }

    private IEnumerator DayNightCycle()
    {
        while (true)
        {
            // — DAY —
            yield return new WaitForSeconds(dayDuration);

            // switch to night
            playerLight.color = nightColor;
            coverDetector.enabled = false;  // temporarily disable death checks

            // — NIGHT —
            yield return new WaitForSeconds(nightDuration);

            // back to day
            playerLight.color = dayColor;
            coverDetector.enabled = true;
        }
    }
}
