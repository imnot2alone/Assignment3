using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;


public class UpdateUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private GameObject ObjectPrefab;
    [SerializeField] private int totalobjects = 3;
    [SerializeField] private GameObject levelCompleteTextObject;
    [SerializeField] private LevelCompleteController levelCompleteController;
    [SerializeField] private TextMeshProUGUI instructionText;



    private TextMeshProUGUI UIText;
    private string ObjectID;
    private bool levelCompleteShown = false;

    private void Awake()
    {
        UIText = GetComponent<TextMeshProUGUI>();
        ObjectID = ObjectPrefab.GetComponent<solarscript>().ID; // Going to look for the object script, and then for the 'ID' component

        if (levelCompleteTextObject != null)
        {
            levelCompleteTextObject.SetActive(false);
        }
        if (instructionText != null)
        {

            StartCoroutine(ShowInstructionsForTime(10f));
        }
    }

    private void LateUpdate()
    {
        int collected = PlayerPrefs.GetInt(ObjectID, 0);
        UIText.text = $"{collected}/{totalobjects}";

        /*if (collected >= totalobjects && !levelCompleteShown)
        {
            levelCompleteShown = true;
            ShowLevelCompleteMessage();
        }*/
        if (collected >= totalobjects && !levelCompleteShown)
        {
            levelCompleteShown = true;
            PlayerPrefs.SetInt("Level1Completed", 1);

            if (levelCompleteController != null)
                levelCompleteController.Show("Level 1 Completed!");
            else
                Debug.LogWarning("LevelCompleteController not assigned");
        }

        //UIText.text = PlayerPrefs.GetInt(ObjectID).ToString();
    }// playerprefs is a good way easily access the public ID 


    private void ShowLevelCompleteMessage()
    {
        if (levelCompleteTextObject != null)
        {
            levelCompleteTextObject.SetActive(true);
        }
        PlayerPrefs.SetInt("Level1Completed", 1);
        PlayerPrefs.Save();
    }
    private IEnumerator ShowInstructionsForTime(float seconds)
    {
        instructionText.gameObject.SetActive(true);
        yield return new WaitForSeconds(seconds);
        instructionText.gameObject.SetActive(false);
    }
}
