using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class manager : MonoBehaviour
{
    public Text _text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Display()
    {
        _text.text = "Hello world";
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene("Scene2");
    }
}
