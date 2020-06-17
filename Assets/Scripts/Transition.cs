using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}
