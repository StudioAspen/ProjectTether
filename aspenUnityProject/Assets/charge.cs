using UnityEngine;

public class charge : MonoBehaviour
{
    bool charged=false;
    void Start()
    {
                    GetComponent<Renderer>().material.SetColor("_Color", Color.darkBlue);
    }
    void OnTriggerEnter(Collider other)
    {
        // hold 1 charged
        if (other.tag == "charge")
        {
            charged=true;
            GetComponent<Renderer>().material.SetColor("_Color", Color.yellow); // just for debug

        }  
        if (other.transform.tag == "machine")
        {
            if(charged){
            charged=false; // charge transfered
            other.transform.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow); // just for debug
            GetComponent<Renderer>().material.SetColor("_Color", Color.darkBlue); // just for debug
            }
        }
       
    }
   
}
