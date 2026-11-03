using UnityEngine;
public class YoYo : MonoBehaviour
{
    public GameObject yoyo;
    public GameObject reticle; // only just to see where it is just debug purposes
    public float maxDistance = 0; // if its zero then it keeps going indefinietly
    Vector3 starttt; // starting position for yoyo to return to
    Vector3 moveto=Vector3.zero; // store vector to move the yoyo in the direction of aim
    Vector3 aim; // store aim vector
    bool isThrown = false;
    bool retract= false;
    float yvalue = 100f; // the distance if not colliding with ground
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        starttt=yoyo.transform.localPosition;
        
    }
  
    void Update()
    {
        // aim using the camera and a raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // if it hits the ground thats where to start aiming
        if (Physics.Raycast(ray, out hit, 100f))
        {
            yvalue=hit.distance;
            //Debug.DrawLine(ray.origin, hit.point, Color.green);
            reticle.transform.position=hit.point;
            aim=hit.point;
        }
        else
        {
        // a distance from the ray cast is where to start aiming
        reticle.transform.position = ray.origin + (ray.direction * yvalue);   
                    aim= ray.origin + (ray.direction * yvalue);   
     
        }

        if (Input.GetMouseButtonUp(0))
        {
        retract=true;
        }
        
        // whenever left click is pressed start going forward until the button is lifted up then retract
        // or retract if max distance reached
       
        if (Input.GetMouseButton(0))
        {
            if(!isThrown && !retract){
            yoyo.transform.position+=10*Vector3.Normalize(moveto)*Time.deltaTime;
                if (Vector3.Distance(yoyo.transform.position, starttt+transform.position) >= maxDistance &&maxDistance!=0)
                {
                    retract=true;
                }
            }
        }
        else
        {
            if(!retract){
            yoyo.transform.position=transform.position+starttt;
            }
            // only update move to position when not pressing button so it only goes in a straight line
            moveto= new Vector3(aim.x,yoyo.transform.position.y,aim.z)-yoyo.transform.position;
        }
       
        if (retract)
        {
            //start going back to its starting position
            yoyo.transform.position = Vector3.MoveTowards(yoyo.transform.position,starttt+transform.position,10*Time.deltaTime);
            if (Vector3.Distance(yoyo.transform.position, starttt+transform.position) < 0.1f)
            {
                // update move position and stop retracting when its back
                moveto= new Vector3(reticle.transform.position.x,yoyo.transform.position.y,reticle.transform.position.z)-yoyo.transform.position;
                retract=false;
            }
        }

    }
}
