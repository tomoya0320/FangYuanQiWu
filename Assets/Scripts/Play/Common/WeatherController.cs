using DigitalRuby.WeatherMaker;
using UnityEngine;

public enum WeatherType
{
    Fine,
    Rain,
    Snow,
    TotalNum,
}
public class WeatherController : MonoBehaviour
{
    public static int weatherType;
    public static float timer;

    private float timeSpan;
    private readonly float timeDelta = 60.0f;
    private readonly float intensity = 0.5f;

    private void LateUpdate()
    {
        timeSpan = Time.realtimeSinceStartup - timer;
        if (timeSpan > timeDelta)
        {
            weatherType = (weatherType + (int)(timeSpan / timeDelta)) % (int)WeatherType.TotalNum;
            switch ((WeatherType)weatherType)
            {
                case WeatherType.Fine:
                    {
                        SetNoneWeather();
                        break;
                    }
                case WeatherType.Rain:
                    {
                        SetRainWeather();
                        break;
                    }
                case WeatherType.Snow:
                    {
                        SetSnowWeather();
                        break;
                    }
            }
            timer = Time.realtimeSinceStartup;
        }
        if ((WeatherType)weatherType == WeatherType.Rain && Random.Range(0, 1.0f) < 0.001f)
        {
            LightningStrikeButtonClicked();
        }
        DawnDuskSliderChanged((0.5f * 86400.0f + Time.realtimeSinceStartup * 100.0f) % 86400.0f);
    }

    public void SetNoneWeather()
    {
        if (WeatherMakerPrecipitationManagerScript.Instance != null)
        {
            WeatherMakerPrecipitationManagerScript.Instance.Precipitation = WeatherMakerPrecipitationType.None;
            WeatherMakerPrecipitationManagerScript.Instance.PrecipitationIntensity = intensity;
        }
    }

    /// <summary>
    /// Rain change handler
    /// </summary>
    /// <param name="isOn">True if on</param>
    public void SetRainWeather()
    {
        if (WeatherMakerPrecipitationManagerScript.Instance != null)
        {
            WeatherMakerPrecipitationManagerScript.Instance.Precipitation = WeatherMakerPrecipitationType.Rain;
            WeatherMakerPrecipitationManagerScript.Instance.PrecipitationIntensity = intensity;
        }
    }

    /// <summary>
    /// Snow change handler
    /// </summary>
    /// <param name="isOn">True if on</param>
    public void SetSnowWeather()
    {
        if (WeatherMakerPrecipitationManagerScript.Instance != null)
        {
            WeatherMakerPrecipitationManagerScript.Instance.Precipitation = WeatherMakerPrecipitationType.Snow;
            WeatherMakerPrecipitationManagerScript.Instance.PrecipitationIntensity = intensity;
        }
    }

    /// <summary>
    /// Hail change handler
    /// </summary>
    /// <param name="isOn">True if on</param>
    public void SetHailWeather()
    {
        if (WeatherMakerPrecipitationManagerScript.Instance != null)
        {
            WeatherMakerPrecipitationManagerScript.Instance.Precipitation = WeatherMakerPrecipitationType.None;
            WeatherMakerPrecipitationManagerScript.Instance.PrecipitationIntensity = intensity;
        }
    }

    /// <summary>
    /// Sleet change handler
    /// </summary>
    /// <param name="isOn">True if on</param>
    public void SetSleetWeather()
    {
        if (WeatherMakerPrecipitationManagerScript.Instance != null)
        {
            WeatherMakerPrecipitationManagerScript.Instance.Precipitation = WeatherMakerPrecipitationType.None;
            WeatherMakerPrecipitationManagerScript.Instance.PrecipitationIntensity = intensity;
        }
    }
    /// <summary>
    /// Lightning bolt button click handler
    /// </summary>
    public void LightningStrikeButtonClicked()
    {
        if (WeatherMakerThunderAndLightningScript.Instance != null)
        {
            WeatherMakerThunderAndLightningScript.Instance.CallIntenseLightning();
        }
    }

    /// <summary>
    /// Time of day slider change handler
    /// </summary>
    /// <param name="val">New value in seconds, 0 to 86400</param>
    public void DawnDuskSliderChanged(float val)
    {
        if (WeatherMakerDayNightCycleManagerScript.Instance != null)
        {
            WeatherMakerDayNightCycleManagerScript.Instance.TimeOfDay = val;
        }
    }
}
