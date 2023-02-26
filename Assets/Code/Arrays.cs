using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Arrays : MonoBehaviour {
    [SerializeField] OpenAppLevel levelsManager2;
    public static Arrays THIS;
    public int SizeX = 3;
    public int SizeY = 11;
    public Gem[,] gems;
    Gem GetGem1;
    Gem GetGem2;
    public Gem this[int r, int c]
    {
        get { return gems[r, c]; }
        set { gems[r, c] = value; }
    }
    public HitCandy[] ingredientsGems;
    public HitCandy[] ingredientsGems2;
    public HitCandy IngredientCurrent;
    public Gem currentGem;
    // Use this for initialization
    void Start() 
    {
    
    }
    private void Awake()
    {
        THIS = this;
        gems = new Gem[SizeY, SizeX];
        for (int row = 0; row < SizeY; row++)
        {
            for (int c = 0; c < SizeX; c++)
            {
                gems[row, c] = new Gem(row, c);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
    public NeighbourProp GetNewProp(Gem gem1,Gem gem2)
    {
        NeighbourProp neighbour = new NeighbourProp();
        IEnumerable<Gem> nhMatch = matchesOnes(gem1,gem2);
        foreach(var g in nhMatch)
        {
            neighbour.gemms.Add(g);
        }
        return neighbour;
    }
    private IEnumerable<Gem> matchesOnes(Gem gem1,Gem gem2)
    {
        List<Gem> hormatch = new List<Gem>();
        hormatch.Add(gem1);
        if (gem1.match3(gem2))
        {
            hormatch.Add(gem2);
        }
        if (hormatch.Count() < 1)
        {
            hormatch.Clear();
        }
        return hormatch.Distinct();
    }
    public NeighbourProp GetProp(Gem gem1)
    {
        NeighbourProp neighbour = new NeighbourProp();
        IEnumerable<Gem> nhMatch = GetMatchHorizontal(gem1);
        foreach (var g in nhMatch)
        {
            neighbour.gemms.Add(g);
        }
        return neighbour;
    }
    private IEnumerable<Gem> GetMatchHorizontal(Gem gem)
    {
        List<Gem> matches = new List<Gem>();
        matches.Add(gem);
        for(int column = 0; column < SizeX; column++)
        {
            if(gem.match3(gems[gem.y, column]))
            {
                matches.Add(gems[gem.y, column]);
            }
            else
            {
                //break;
            }
        }
        if (matches.Count() < 1)
        {
            matches.Clear();
        }
        return matches.Distinct();
    }
    public bool IsNulls(int row,int col)
    {
        if (levelsManager2.blocksp[row*levelsManager2.MaxX+col].types==0)
        {
            return true;
        }
        return false;
    }
    public IEnumerable<Gem> NullGemsonc(int collum)
    {
        List<Gem> gemsnull = new List<Gem>();
        for (int i = 0; i < SizeY; i++)
        {
            if (gems[i, collum].INull||IsNulls(i,collum)==true||levelsManager2.blocksp[i*levelsManager2.MaxX+collum].types==0)
            {
                gemsnull.Add(gems[i, collum]);
            }
        }
        return gemsnull;
    }
}