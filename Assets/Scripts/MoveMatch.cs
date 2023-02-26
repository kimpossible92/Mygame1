using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MoveMatch : MonoBehaviour
{
    public ArrayMatch GetArrayMatch;
    public int state = 0;// = State.Default;
    Vector2[] visibleSize;
    public HitGem[] HitGemPrefabs;
	public HitGem[] BonusPrefabs;
    HitGem GetHitGem;
    GetGem GetGem1;
    GetGem GetGem2;
    // Use this for initialization
    void Start()
    {
       // Gems();
    }
    public void loadSize(int x1, int y2)
    {
        GetArrayMatch.SizeX = x1;
        GetArrayMatch.SizeY = y2;
    }
    public void restarting()
    {
        Gems();
    }
    void Update()
    {
        switch (state)
        {
            case 0:
                if (Input.GetMouseButtonDown(0))
                {
                    var hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
                    if (hit.collider != null)
                    {
                        GetHitGem = hit.collider.GetComponent<HitGem>();
                        state = 1;
                    }
                }
                break;
            case 1:
                if (Input.GetMouseButtonDown(0))
                {
                    var hits = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector3.zero);
                    if (hits.collider != null && GetHitGem != hits.collider.gameObject)
                    {
                        HitGem GetHitGem2 = hits.collider.gameObject.GetComponent<HitGem>();
                        if (!((GetHitGem.GetGem.x == GetHitGem2.GetGem.x || GetHitGem.GetGem.y == GetHitGem.GetGem.y) && (Mathf.Abs(GetHitGem.GetGem.x - GetHitGem2.GetGem.x) <= 1 && Mathf.Abs(GetHitGem.GetGem.y - GetHitGem2.GetGem.y) <= 1)))
                        {
                            state = 0;
                        }
                        else
                        {
                            state = 2;
                            StartCoroutine(TryMatch(hits));
                        }
                    }
                }
                break;
        }

    }
    IEnumerator TryMatch(RaycastHit2D raycast2)
    {
        HitGem GetHitGem2 = raycast2.collider.gameObject.GetComponent<HitGem>();
        GetArrayMatch.OnSwapping(GetHitGem.GetGem, GetHitGem2.GetGem);
        GetHitGem.transform.TweenPosition(0.2f, GetHitGem2.transform.position);
        GetHitGem2.transform.TweenPosition(0.2f, GetHitGem.transform.position);
        yield return new WaitForSeconds(0.2f);
        var GetHitGemNeigbours = GetArrayMatch.GetProp(GetHitGem.GetGem);
        var GetHitGemNeigbours2 = GetArrayMatch.GetProp(GetHitGem2.GetGem);
        var matchs = GetHitGemNeigbours.GetGems.Union(GetHitGemNeigbours2.GetGems).Distinct();
        if(matchs.Count()<3)
        {
            GetHitGem.transform.TweenPosition(0.2f, GetHitGem2.transform.position);
            GetHitGem2.transform.TweenPosition(0.2f, GetHitGem.transform.position);
            yield return new WaitForSeconds(0.2f);
            GetArrayMatch.OnSwapping(GetHitGem2.GetGem, GetHitGem.GetGem);
            //GetArrayMatch.Lastsp();
        }
        GetGem bonusGem = null;
        NeighProp BonusProp = new NeighProp();
        bool addBonus = matchs.Count() >= 4 && GetHitGemNeigbours.bt == 0 && GetHitGemNeigbours2.bt == 0;
        if (addBonus)
        {
            var sameTypeGem = GetHitGemNeigbours.gemms.Count() > 0 ? GetHitGem : GetHitGem2;
            bonusGem = sameTypeGem.GetGem;
        }
        while(matchs.Count()>=3)
        {
            foreach (var i in matchs)
            {
                ScaleMatch(i.hitGem);
            }
			//if (addBonus) CreateBonus (bonusGem);
			//addBonus = false;
            var bottom = matchs.Select(gem => gem.x).Distinct();
            var updateafterMatch = GetArrayMatch.UpdateAfter(bottom);
            var newSpawn = GetNeighbourProp(bottom);
            int maxDistance = Mathf.Max(updateafterMatch.MaxDistance, newSpawn.MaxDistance);
            
            foreach (var i in newSpawn.GetGems)
            {
                i.hitGem.transform.TweenPosition(0.05f * maxDistance, new Vector2(-2.37f, -4.27f) + new Vector2(i.x * 0.7f, i.y * 0.7f));
            }
			foreach (var j in updateafterMatch.GetGems)
            {
                j.hitGem.transform.TweenPosition(0.05f * maxDistance, new Vector2(-2.37f, -4.27f) + new Vector2(j.x * 0.7f, j.y * 0.7f));
            }
            yield return new WaitForSeconds(0.05f * Mathf.Max(updateafterMatch.MaxDistance, newSpawn.MaxDistance));
            matchs = GetArrayMatch.GetNeighbours(newSpawn.GetGems).Union(GetArrayMatch.GetNeighbours(updateafterMatch.GetGems)).Distinct();
        }
        state = 0;
    }
    void Moved(int dist,IEnumerable<GetGem> gemsie)
    {
        foreach (var j in gemsie)
        {
            j.hitGem.transform.TweenPosition(0.05f * dist, new Vector2(-2.37f, -4.27f) + new Vector2(j.x * 0.7f, j.y * 0.7f));
        }
    }
    
    public void ScaleMatch(HitGem hitmatchs)
    {
        hitmatchs.GetGem.Nil();
        Destroy(hitmatchs.gameObject);
    }
    private void Gems()
    {
        for (int row = 0; row < GetArrayMatch.SizeY; row++)
        {
            for (int col = 0; col < GetArrayMatch.SizeX; col++)
            {
                if (!GetArrayMatch[row, col].INull)
                {
                    Destroy(obj: GetArrayMatch[row, col].hitGem.gameObject);
                }
            }
        }
        for (int row = 0; row < GetArrayMatch.SizeY; row++)
        {
            for (int col = 0; col < GetArrayMatch.SizeX; col++)
            {
                GetArrayMatch.gems[row, col].Nil();
            }
        }
        int g = GetArrayMatch.SizeX;
        visibleSize = new Vector2[g];
        for (int row = 0; row < GetArrayMatch.SizeY; row++)
        {
            for (int col = 0; col < GetArrayMatch.SizeX; col++)
            {
                HitGem hitPrefab = HitGemPrefabs[Random.Range(0, HitGemPrefabs.Length)];
                while (col >= 2 && GetArrayMatch[row, col - 1].hitGem.isequal(hitPrefab) && GetArrayMatch[row, col - 2].hitGem.isequal(hitPrefab))
                {
                    hitPrefab = HitGemPrefabs[Random.Range(0, HitGemPrefabs.Length)];
                }
                while (row >= 2 && GetArrayMatch[row - 1, col].hitGem.isequal(hitPrefab) && GetArrayMatch[row - 2, col].hitGem.isequal(hitPrefab))
                {
                    hitPrefab = HitGemPrefabs[Random.Range(0, HitGemPrefabs.Length)];
                }
                CreateGem(row, col, hitPrefab);
            }
        }
        for (int i = 0; i < GetArrayMatch.SizeX; i++)
        {
            visibleSize[i] = new Vector2(-2.37f, -4.27f) + new Vector2(i * 0.7f, GetArrayMatch.SizeY * 0.7f);
        }
    }
    private void CreateGem(int row,int c,HitGem hgem)
    {
        Vector2 vectorgem = new Vector2(-2.37f, -4.27f) + (new Vector2(c * 0.7f, row * 0.7f));
        HitGem gemit = ((GameObject)Instantiate(hgem.gameObject, vectorgem, Quaternion.identity)).GetComponent<HitGem>();
        gemit.transform.SetParent(GetArrayMatch.transform);
        GetArrayMatch[row, c].OnInit(gemit);
    }
    private UpdateMatches GetNeighbourProp(IEnumerable<int> colls)
    {
        UpdateMatches afterMatch = new UpdateMatches();
        foreach(int coll in colls)
        {
            var emptyGems = GetArrayMatch.NullGemsonc(coll);
            foreach(var g in emptyGems)
            {
                var pref = HitGemPrefabs[Random.Range(0, HitGemPrefabs.Length)];
                var initgem = ((GameObject)Instantiate(pref.gameObject, visibleSize[coll], Quaternion.identity)).GetComponent<HitGem>();
                initgem.transform.SetParent(GetArrayMatch.transform);
                g.OnInit(initgem);
                if (GetArrayMatch.SizeY - g.y>afterMatch.MaxDistance)
                {
                    afterMatch.MaxDistance = GetArrayMatch.SizeY - g.y;
                }
                afterMatch.AddGemms(g);
            }
        }
        return afterMatch;
    }
	public void CreateBonus(GetGem bGem)
	{
		int l = Random.Range (0, BonusPrefabs.Length);
		HitGem bonuses = ((GameObject)Instantiate (BonusPrefabs [l].gameObject, new Vector2 (-2.37f, -4.27f) + new Vector2 ((float)bGem.x * 0.7f, (float)bGem.y * 0.7f), Quaternion.identity)).GetComponent<HitGem> ();
		GetArrayMatch.gems [bGem.y, bGem.x] = bonuses.GetGem;
		bonuses.GetGem.bonus = 1;
	}
}
