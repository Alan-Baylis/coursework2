using UnityEngine;
using System.Collections;

public class ElementController : MonoBehaviour
{
    public Transform outPoint;
    public Transform inPoint;

    protected const string inPointName = "inPoint";
    protected const string outPointName = "outPoint";

	// Use this for initialization
	void Start ()
	{
	    inPoint = transform.FindChild(inPointName);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
