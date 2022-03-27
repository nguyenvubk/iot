using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class animation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
	public Image image1;
    [SerializeField]
	public Image image2;
    public bool b = true;
    public float speed=0.5f;

    public float time = 0f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(b)
		{
			time+=Time.deltaTime*speed;
			image1.fillAmount= time;
            image2.fillAmount= time;
            if(time>1)
		    {
						
			    time=0;
		    }
        }
    }
}
