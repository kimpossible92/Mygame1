using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {
    public HitCandy candy;
    public int row;
    public int col;
    public int types;
    public List<GameObject> block = new List<GameObject>();
    private int ccc;
    public bool emptyes=false;
    [SerializeField] Sprite GetSprite1;
    [HideInInspector] public int modelvlsquare;
    public int addScore;
    [SerializeField] TMPro.TextMeshPro Text1;
    public void setViewText(bool txt)
    {
        Text1.gameObject.SetActive(txt);
        if (txt == true)
        {
            Text1.text = candy.ToIntType().ToString();
        }
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public static void DestroyBlocks()
    {

    }
}
public class SquareBlocks
{
    public int blck;
    public void Changeblck(int bl) { blck = bl; }
    public int block() { return blck; }
    public int obstacle;
}
