using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewCoins : MonoBehaviour
{
    [SerializeField]private int Coins=5;
    public int _coins() { return Coins; }
    public void increment(int c) { Coins += c; }
    public void decrement(int c) { Coins -= c; }
    public void RandomAdd() { Coins += Random.Range(1, 3); }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<UnityEngine.UI.Text>().text = Coins.ToString();
    }
}
