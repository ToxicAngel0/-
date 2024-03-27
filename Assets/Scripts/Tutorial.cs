using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour
{
    // Start is called before the first frame update
    public Text Tutorial_text;
    public GameObject Tutorial_window;
    public List<string> steps = new List<string>();
    void Start()
    {
        Tutorial_text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        { 
            End_Step();
        }
    }

    public void Start_Step(int step)
    {
        Tutorial_window.SetActive(true);
        Tutorial_text.text = steps[step];
    }

    public void End_Step()
    {
        Tutorial_window.SetActive(false);
        Tutorial_text.text = "";
    }
}
