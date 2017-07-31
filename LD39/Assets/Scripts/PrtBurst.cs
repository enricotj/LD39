using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrtBurst : MonoBehaviour
{

    private ParticleSystem ps;
    private float killTimer;

	// Use this for initialization
	void Start ()
    {
        ps = GetComponent<ParticleSystem>();
        ps.Emit(1);
        killTimer = ps.main.duration;
	}
	
	// Update is called once per frame
	void Update ()
    {
        killTimer -= Time.deltaTime;
        if (killTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
}
