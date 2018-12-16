using UnityEngine;
using UnityEngine.UI;

public class CircularProgress : MonoBehaviour
{
    public Image loading;

    public void UpdateProgress(float percentage)
    {
        loading.fillAmount = percentage;
    }
}
