using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move2 : MonoBehaviour
{
    //public static MoveLayer THIS;
    public string Name;
    public string urlOnTournament;
    [HideInInspector] public int movecount;
    [HideInInspector] public int limitMove;
    public UnityEngine.UI.Text GetTextMove;
    public int state = 0;
    Vector2[] visibleSize;
    private int SizeX;
    private int SizeY;
    public GameObject[] bug;
    [SerializeField] List<Vector2> ylist2 = new List<Vector2>();
    [SerializeField] Arrays GetArrays;
    [SerializeField] HitCandy[] GetCandyPrefab;
    [SerializeField] HitCandy SwirlCandy;
    [SerializeField] HitCandy[] GetCandieSecons;
    [SerializeField] NewAppLevel levelsApps;
    [SerializeField] NewAppLevel AppLevel;
    HitCandy GetHitGem;
    [SerializeField] Sprite sprite12;
    [SerializeField] HitCandy[] GetBonusPrefab;
    [SerializeField] HitCandy swirlPrefab;
    [SerializeField] Sprite[] GetIngredientSprite;
    [SerializeField] GameObject plus5Seconds;
    bool load;
    int Ending = 0;
    public int End() { return Ending; }
    public bool loaldo() { return load; }
    public void loadMove(bool load1)
    {
        load = load1;
    }
    public void restarting()
    {
        SizeX = AppLevel.MaxX;
        SizeY = AppLevel.MaxY;
        Gems();
    }
    void Start()
    {
        
    }
    public List<HitCandy> hitCandies2 = new List<HitCandy>();
    private int blockCount = 0;
    IEnumerator Match(RaycastHit2D raycast2)
    {
        
        if (GetBlock != null) 
        {
            if (GetHitGem != null&&GetBlock.col==GetHitGem._block.col)
            {
                GetHitGem.transform.position = GetBlock.transform.position;
                blockCount++;
                if (blockCount == hitCandies2.Count)
                {
                    NextRowCandies();
                }
            } 
        }
        yield return new WaitForSeconds(0.3f);
    }
    IEnumerator SetNextRow()
    {
        if (GetBlock != null)
        {
            if (GetHitGem != null && GetBlock.col == GetHitGem._block.col)
            {
                GetHitGem.transform.position = GetBlock.transform.position;
                blockCount++;
                if (blockCount == hitCandies2.Count)
                {
                    RotateRow(0);
                    RotateRow(1);
                    RotateRow(2);
                }
            }
        }
        yield return new WaitForSeconds(0.3f);
    }
    protected void NextRowCandies()
    {
        _GetBlocks.Clear();
        if (currentRow > SizeY+1) { AppLevel.nazad2(); return; }
        hitCandies2.Clear();
        for (int c = 0; c < SizeX; c++)
        {
            hitCandies2.Add(GetArrays[currentRow, c].hitGem);
            GetArrays[currentRow, c].hitGem.enableTextMesh();
            _GetBlocks.Add(levelsApps.blocksp[currentRow * SizeX + c]);
        }
        nullrow = false;
        Invoke("SetCurrentRow", 7f);
    }
    protected void RotateRow(int col)
    {
        for (int r = 0; r < SizeY; r++)
        {
            int newRow = r -1;
            if (newRow == -1) { newRow = SizeY-1; }
            GetArrays[r, col].hitGem.transform.position = levelsApps.blocksp[newRow * levelsApps.MaxX + col].transform.position;
            GetArrays[r, col].OnInit(GetArrays[newRow, col].hitGem);
            
        }
    }
    public bool IsNulls(int row, int col)
    {
        if (levelsApps.blocksp[row * levelsApps.MaxX + col].types == 0)
        {
            return true;
        }
        return false;
    }
    [SerializeField] float listPositionPlus=2;
    [SerializeField] GameObject ObjectStart;
    [SerializeField] GameObject UnVisibleWall;
    [SerializeField] bool nullrow = false;
    public void SetNull() {  currentRow = 0; hitCandies2.Clear(); _GetBlocks.Clear(); nullrow = true; }
    public void SetCurrentRow()
    {
        if (nullrow) { return; }
        List<int> xpositions = new List<int>();
        int row1 = SizeY - 1;
        foreach (var cand in hitCandies2)
        {
            cand.transform.position = new Vector3(cand.transform.position.x, cand.transform.position.y+row1+listPositionPlus);
        }
        foreach(var c1 in hitCandies2)
        {
            var xpos1 = Random.Range(0, SizeX);
            while (xpositions.Contains(xpos1))
            {
                xpos1 = Random.Range(0, SizeX);
            }
            xpositions.Add(xpos1);
            c1.transform.position = new Vector3(xpos1*levelsApps.blckWH() + ObjectStart.transform.position.x, ObjectStart.transform.position.y);
        }
        currentRow++;
        for (int row = currentRow; row < SizeY; row++)
        {
            for (int col = 0; col < SizeX; col++)
            {
                GetArrays[row, col].hitGem.DisenableTextMesh();
            }
        }

        ObjectStart.GetComponent<HitCandy>().DisenableTextMesh();
    }
    protected List<Block> _GetBlocks = new List<Block>();
    public void CreateGem(int row, int c, HitCandy hgem)
    {
        float xc = levelsApps.blckWH();
        float yr = levelsApps.blckWH();
        Vector2 vectorgem = levelsApps.vector2position + (new Vector2(c * xc, row * yr));
        HitCandy gemit = ((GameObject)Instantiate(hgem.gameObject, new Vector3(vectorgem.x, vectorgem.y, -0.1f), Quaternion.identity)).GetComponent<HitCandy>();
        gemit.transform.SetParent(GetArrays.transform);
        gemit.SetBlock(levelsApps.blocksp[row * SizeX + c]);
        levelsApps.blocksp[row * SizeX + c].candy = gemit;
        GetArrays[row, c].OnInit(gemit);
        if (currentRow==row) { hitCandies2.Add(gemit); _GetBlocks.Add(levelsApps.blocksp[row * SizeX + c]); }
        if (IsNulls(row, c) == true && !gemit.isSwirl) { Destroy(gemit.gameObject); }
    }
    public void loadSize(int x1,int y2)
    {
        SizeX = x1;
        SizeY = y2;
    }
    int currentRow = 0;
    private void Gems()
    {
        float xc = levelsApps.blckWH();//
        float yr = levelsApps.blckWH();//

        for (int row = 0; row < GetArrays.SizeY; row++)
        {
            for (int col = 0; col < GetArrays.SizeX; col++)
            {
                if (GetArrays[row, col].INull == false)
                {
                    Destroy(obj: GetArrays[row, col].hitGem.gameObject);
                }
            }
        }
        for (int row = 0; row < SizeY; row++)
        {
            for (int col = 0; col < SizeX; col++)
            {
                GetArrays.gems[row, col].Nil();
            }
        }
        List<int> hitCandies = new List<int>();
        int g = SizeX;
        visibleSize = new Vector2[g];
        for (int row = 0; row < SizeY; row++)
        {
            for (int col = 0; col < SizeX; col++)
            {
                //candylimit = GetCandyPrefab.Length - 1;
                HitCandy hitCandy = GetCandyPrefab[0];
                hitCandy.setIntType(Random.Range(1, 99));
                while (hitCandies.Contains(hitCandy.ToIntType()))
                {
                    hitCandy.setIntType(Random.Range(1, 99));
                }
                hitCandies.Add(hitCandy.ToIntType());
                CreateGem(row, col, hitCandy);
            }
        }

        for (int i = 0; i < SizeX; i++)
        {
            visibleSize[i] = levelsApps.vector2position + new Vector2(i * xc, SizeY * yr); //new Vector2(-2.37f, -4.27f) + new Vector2(i * 0.7f, SizeY * 0.7f);
        }
        nullrow = false;
        //if(levelsApps.modeLvl!=2)LoadHelp();
        Invoke("SetCurrentRow", 7f);
    }
    public void GetDestroyAlls()
    {
        //GetBlocks.Clear(); 
        state = 0;
        for (int row = 0; row < GetArrays.SizeY; row++)
        {
            for (int col = 0; col < GetArrays.SizeX; col++)
            {
                if (GetArrays[row, col].INull == false)
                {
                    Destroy(obj: GetArrays[row, col].hitGem.gameObject);
                }
            }
        }
    }
    Block GetBlock;
    public void SetSecretVisible()
    {
        if (_GetBlocks.Count > 0)
        {
            //print("sadsaldkl");
            GetBlock = _GetBlocks[Random.Range(0, _GetBlocks.Count)];
            GetBlock.setViewText(true);
            Invoke("NotSecretVisible", 3);
        }
    }
    private void NotSecretVisible()
    {
        if (GetBlock != null)
        {
            GetBlock.setViewText(false);
            GetBlock = null;
        }
    }
    public void moves()
    {
        switch (state)
        {
            case 0:
                if (Input.GetMouseButtonDown(0))
                {
                    var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
                    if (hit.collider != null)
                    {
                        GetHitGem = hit.collider.GetComponent<HitCandy>();
                        GetBlock = hit.collider.GetComponent<Block>();
                        if (GetHitGem != null) { print("hit0"); }
                        if (GetBlock != null) { print("block"); }
                        state = 1;
                        
                    }
                }
                break;
            case 1:
                if (Input.GetMouseButtonUp(0))
                {
                    var hits = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
                    if (hits.collider != null)
                    {
                        GetBlock = hits.collider.gameObject.GetComponent<Block>();
                        if (GetBlock != null) { print("block"); StartCoroutine(Match(hits)); state = 0; }
                    }
                }
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        GetTextMove.text = string.Format("{0}", movecount);
        //if (movecount != limitMove)
        //{
            GetTextMove.gameObject.SetActive(true);
            moves();
        //}
    }
}
