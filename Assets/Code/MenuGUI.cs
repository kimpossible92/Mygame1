using UnityEngine;
using System.Collections;

public class MenuGUI : MonoBehaviour
{
    public void MenuTounamentClick()
    {
        if (PlayerPrefs.GetInt("TourQual") >= 10)
        {
        }
        else
        {
            GameObject.Find("CanvasGlobal").transform.Find("TournamentQualification").gameObject.SetActive(true);
            if (PlayerPrefs.GetInt("TourQual") >= 10)
            {
                GameObject.Find("CanvasGlobal").transform.Find("TournamentQualification").transform.Find("Image").transform.Find("Join").gameObject.SetActive(true);
                GameObject.Find("CanvasGlobal").transform.Find("TournamentQualification").transform.Find("Image").transform.Find("CheckTarget").transform.Find("Check").gameObject.SetActive(true);
            }
            else
            {
                GameObject.Find("CanvasGlobal").transform.Find("TournamentQualification").transform.Find("Image").transform.Find("Join").gameObject.SetActive(false);
                GameObject.Find("CanvasGlobal").transform.Find("TournamentQualification").transform.Find("Image").transform.Find("CheckTarget").transform.Find("Check").gameObject.SetActive(false);
            }
            //Tournament.tournament.kubokButton();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            PlayerPrefs.SetInt("TourQual", (PlayerPrefs.GetInt("TourQual")+10));
        }
    }
}
