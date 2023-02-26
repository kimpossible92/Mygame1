using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class NeighProp  {
    public List<GetGem> gemms = new List<GetGem>();
    public int bt { get; set; }
    public IEnumerable<GetGem> GetGems
    {
        get { return gemms.Distinct(); }
    }
    public void AddGemms(GetGem gem)
    {
        if (!gemms.Contains(gem)) gemms.Add(gem);
    }
    public bool BonusTypeWhite(int bt1)
    {
        return bt1 == 1; 
    }
}
