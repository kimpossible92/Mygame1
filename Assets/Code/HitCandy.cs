using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HitCandy : MonoBehaviour {
    [SerializeField] public Sprite[] sprites;
    public string type;
    public Block _block => GetBlock;
    private Block GetBlock;
    public void SetBlock(Block block)
    {
        GetBlock = block;
    }
    public Gem GetGem { get; private set; }
    public bool isBonus;
    public int BonusMatchType;
    public bool isSwirl=false;
    public int seconds = 0;
    [SerializeField]
    private TextMeshPro textMesh;
    public void enableTextMesh()
    {
        textMesh.enabled = true;
    }
    public void DisenableTextMesh()
    {
        textMesh.enabled = false;
    }
    public void setIntType(int tp)
    {
        intType = tp;
    }
    [SerializeField] 
    private int intType = 0;
    public int ToIntType()
    {
        return intType;
    }
    public void setBlockAnimation()
    {
        GetComponent<Animator>().SetBool("block", false);
    }
    public void isGem(Gem g)
    {
        GetGem = g;
    }
    public bool isequal(HitCandy hitCandy)
    {
        return hitCandy != null && hitCandy.type == type;
    }
    public bool isEmpty(List<HitCandy> Hits,HitCandy candy)
    {
        int hcount = 0;
        foreach(var h in Hits)
        {
            if (candy.type == h.type)
            {
                hcount++;
                return false;
            }
        }
        if (hcount != 0) { return false; }
        else { return true; }
    }
    private void Update()
    {
        if (textMesh != null) { textMesh.text = intType.ToString(); return; }
        if (isSwirl == false)
        {
           //if(sprites!=null) GetComponent<SpriteRenderer>().sprite = sprites[GetGem.level];
        }
       
    }
}
