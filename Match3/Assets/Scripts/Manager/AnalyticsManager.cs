using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsManager : MonoBehaviour
{
    public void FinishLevel(int score)
    {
        Analytics.CustomEvent("level_finished", new Dictionary<string, object>
        {
            {"score", score}
        });
    }

    public void SawAd(bool saw)
    {
        Analytics.CustomEvent("sawAd", new Dictionary<string, object>
        {
            {"score", saw}
        });
    }
}
