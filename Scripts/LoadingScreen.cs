using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{

    public string LoadLevel;

    public GameObject loadingScreen;

    void Start()
    {

    }
    void Update()
    {

    }

    public void Load()
    {
        
        loadingScreen.SetActive(true);
        SceneManager.LoadScene(LoadLevel);
            
    }
}
