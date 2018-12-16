using UnityEngine;

public class ChangeSpeedButton : MonoBehaviour {

    public void Click()
    {
        Time.timeScale = (Time.timeScale) % 10 + 1;
    }
}
