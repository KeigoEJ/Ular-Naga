using UnityEngine;
using UnityEngine.SceneManagement;

public class NextoScene : MonoBehaviour
{
    [SerializeField] string sceneName;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadTheNextScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
