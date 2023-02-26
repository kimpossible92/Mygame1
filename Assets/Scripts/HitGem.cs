using UnityEngine;
using System.Collections;
public class HitGem : MonoBehaviour
{
    public string type;
    public GetGem GetGem { get;set; }
    public void isGem(GetGem g)
    {
        GetGem = g;
    }
    public bool isequal(HitGem hitgem)
    {
        return hitgem != null && hitgem.type == type;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}