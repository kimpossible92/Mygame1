using UnityEngine;
using System.Collections;

public class Gem
{
    public int x { get; set; }
    public int y { get; set; }
    public int level=0;
    public int bonus { get; set; }
    public HitCandy hitGem { get; private set; }
    public bool INull { get { return hitGem == null; } }
    OpenAppLevel levelsApps;
    public void setlvlApps(OpenAppLevel level)
    {
        levelsApps = level;
    }
    public Gem(int yr, int xc)
    {
        y = yr;
        x = xc;
    }
    public Gem OnInit2(HitCandy hitI)
    {
        hitGem = hitI;
        hitGem.isGem(this); 
        //hitI.transform.position = levelsApps.blocksp[y * levelsApps.MaxX + x].transform.position;
        return this;
    }
    public void OnInit(HitCandy hitI)
    {
        hitGem = hitI;
        hitGem.isGem(this);
       
    }
    public bool match3(Gem g)
    {
        return !INull && !g.INull && hitGem.isequal(g.hitGem);
    }
    public void Nil()
    {
        hitGem = null;
    }
}

