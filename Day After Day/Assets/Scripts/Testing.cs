using UnityEngine;
using UnityEngine.SceneManagement;

public class Testing : MonoBehaviour
{
    public void MoveToScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}