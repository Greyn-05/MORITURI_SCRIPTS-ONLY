using UnityEngine;

public class TodoUpdate : MonoBehaviour
{

    public GameObject key;

    private void Start()
    {
        key.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (key.activeSelf)
                key.SetActive(false);
            else
                key.SetActive(true);
        }

    }
}
