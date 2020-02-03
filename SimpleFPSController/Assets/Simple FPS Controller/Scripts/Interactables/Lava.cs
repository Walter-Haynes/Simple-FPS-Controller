using UnityEngine;
using UnityEngine.SceneManagement;

public class Lava : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Diamond.st.ShowTransition(SceneManager.GetActiveScene().buildIndex);
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }
}
