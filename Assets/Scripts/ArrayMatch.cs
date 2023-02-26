using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
public class ArrayMatch : MonoBehaviour
{
    public int SizeX=9;
    public int SizeY=11;
    public GetGem[,] gems;
    GetGem GetGem1;
    GetGem GetGem2;
    public GetGem this[int r, int c]
    {
        get { return gems[r, c]; }
        set { gems[r, c] = value; }
    }
    void Start()
    {
    }
    void Update()
    {

    }
    public void OnSwapping(GetGem gem1, GetGem gem2)
    {
        GetGem1 = gem1;
        GetGem2 = gem2;

        HitGem hitGem = gem1.hitGem;
        gem1.OnInit(gem2.hitGem);
        gem2.OnInit(hitGem);
    }
    public void Lastsp()
    {
        OnSwapping(GetGem1, GetGem2);
    }
    void Awake()
    {
        gems = new GetGem[SizeY, SizeX];
        for(int row = 0;row<SizeY;row++)
        {
            for (int c = 0; c < SizeX; c++)
            {
                gems[row, c] = new GetGem(row, c);
            }
        }
    }
    public IEnumerable<GetGem> GetNeighbours(IEnumerable<GetGem> gemsIe)
    {
        List<GetGem> gemslist = new List<GetGem>();
        foreach(var g in gemsIe)
        {
            gemslist.AddRange(GetProp(g).GetGems);
        }
        return gemslist.Distinct();
    }
    public NeighProp GetProp(GetGem gem)
    {
        NeighProp neighbour = new NeighProp();
        IEnumerable<GetGem> nhMatch = MatchesHorrizontally(gem);
        IEnumerable<GetGem> nvMatch = MatchesVertically(gem);
		if(GetBonusTypeWhite(nhMatch))
        {
			nhMatch = MatchHoriz(gem);
            if (neighbour.bt == 0) neighbour.bt |= 1;
        }
		if (GetBonusTypeWhite(nvMatch))
        {
			nvMatch = MatchesVer(gem);
            if (neighbour.bt == 0) neighbour.bt |= 1;
        }
        foreach(var g in nhMatch)
        {
            if (neighbour.gemms.Contains(g) == false) neighbour.gemms.Add(g);
        }
        foreach (var g in nvMatch)
        {
            if (neighbour.gemms.Contains(g) == false) neighbour.gemms.Add(g);
        }
        return neighbour;
    }
    private IEnumerable<GetGem> MatchesHorrizontally(GetGem gem)
    {
        List<GetGem> hormatch = new List<GetGem>();
        hormatch.Add(gem);
        if (gem.x != 0)
        {
            for (int col = gem.x - 1; col >= 0; col--)
            {
                GetGem l = gems[gem.y, col];
                if (l.match3(gem))
                {
                    hormatch.Add(l);
                }
                else
                {
                    break;
                }
            }
        }
        if (gem.x != SizeX - 1)
        {
            for (int col = gem.x + 1; col < SizeX; col++)
            {
                GetGem r = gems[gem.y, col];
                if (r.match3(gem))
                {
                    hormatch.Add(r);
                }
                else
                {
                    break;
                }
            }
        }
        if (hormatch.Count() < 3)
        {
            hormatch.Clear();
        }
        return hormatch.Distinct();
    }
	public IEnumerable<GetGem> MatchesVer(GetGem gem)
    {
        List<GetGem> vermatch = new List<GetGem> { gem };
		int rower1 = gem.y;
        int col1 = gem.x;
        for (int row = 0; row < SizeY;row++)
        {
            vermatch.Add(gems[row, col1]);
        }
        return vermatch;
    }
	public IEnumerable<GetGem> MatchHoriz(GetGem gem)
    {
        List<GetGem> hormatch = new List<GetGem> { gem };
        int row1 = gem.y;
        for (int col = 0; col < SizeX;col++)
        {
            hormatch.Add(gems[row1, col]);
        }
        return hormatch;
    }
    private IEnumerable<GetGem> MatchesVertically(GetGem gem)
    {
        List<GetGem> vermatch = new List<GetGem>();
        
        vermatch.Add(gem);
        if (gem.y != 0)
        {
            for (int row = gem.y - 1; row >= 0; row--)
            {
                GetGem down = gems[row, gem.x];
                if (down.match3(gem))
                {
                    vermatch.Add(down);
                }
                else
                {
                    break;
                }
            }
        }
        if (gem.y != SizeY - 1)
        {
            for (int row = gem.y + 1; row < SizeX; row++)
            {
                GetGem up = gems[row, gem.x];
                if (up.match3(gem))
                {
                    vermatch.Add(up);
                }
                else
                {
                    break;
                }
            }
        }
        if(vermatch.Count()<3)
        {
            vermatch.Clear();
        }
        return vermatch.Distinct();
    }

    public UpdateMatches UpdateAfter(IEnumerable<int> colum)
    {
        UpdateMatches afterMatch = new UpdateMatches();
        foreach (var c in colum)
        {
            for (int row = 0; row < SizeY-1; row++)
            {
                if (gems[row, c].hitGem==null)
                {
                    for (int row2 = row+1; row2 < SizeY; row2++)
                    {
                        if (!gems[row2, c].INull)
                        {
                            GetGem gem1 = gems[row, c];
                            GetGem gem2 = gems[row2, c];
                            gem1.OnInit(gem2.hitGem);
                            gem2.Nil();
                            afterMatch.MaxDistance = Mathf.Max(row2 - row, afterMatch.MaxDistance);
                            afterMatch.AddGemms(gem1);
                            break;
                        }
                    }
                }
            }
        }
        return afterMatch;
    }
    public bool GetBonusTypeWhite(IEnumerable<GetGem> gwhite)
    {
        if (gwhite.Count() >= 4)
        {
            foreach(var g in gwhite)
            {
                if (g.bonus == 0) return true;
            }
        }
        return false;
    }
    public IEnumerable<GetGem> GetEmptyColumnsInfo(int column)
    {
        List<GetGem> emptyMatches = new List<GetGem>();
        for (int row = 0; row < SizeY; row++)
        {
            if (gems[row, column] == null) emptyMatches.Add(new GetGem(row,column) { y = row, x = column });
        }
        return emptyMatches;
    }
    public IEnumerable<GetGem> NullGemsonc(int collum)
    {
        List<GetGem> gemsnull = new List<GetGem>();
        for (int i = 0; i < SizeY; i++)
        {
            if(gems[i,collum].INull)
            {
                gemsnull.Add(gems[i, collum]);
            }
        }
        return gemsnull;
    }
}