using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDestParticle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       Invoke("Idest",2.0f);
    }
    private void Idest()
    {
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
