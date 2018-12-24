using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.GUI
{
    public class LoadSceneOnClick : MonoBehaviour
    {
        public void LoadByIndex(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}