using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchEffectManager : MonoBehaviour
{
    public GameObject effect;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !SceneManager.GetActiveScene().name.Contains("Map_"))
        {
            GameObject go = Instantiate(effect, transform);
            go.transform.position = Input.mousePosition;
        }
    }

}
