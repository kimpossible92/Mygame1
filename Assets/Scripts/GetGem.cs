using UnityEngine;
using System.Collections;

public class GetGem
{
    public int x { get; set; }
    public int y { get; set; }
    public int bonus { get; set; }
    public HitGem hitGem { get; set; }
    public bool INull { get { return hitGem == null; } }
    public GetGem(int yr, int xc)
    {
        y = yr;
        x = xc;
    }
    public void OnInit(HitGem hitI)
    {
        hitGem = hitI;
        hitGem.isGem(this);
    }
    public bool match3(GetGem g)
    {
        return !INull && !g.INull && hitGem.isequal(g.hitGem);
    }
    // Use this for initialization
    void Start()
    {

    }
    public void Nil()
    {
        hitGem = null;
    }
    // Update is called once per frame
    void Update()
    {

    }
}

