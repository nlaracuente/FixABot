using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject tileMenu;

    [SerializeField]
    GameObject howToPlayMenu;

    [SerializeField]
    WeekMenuController weekMenuController;

    [SerializeField]
    GameObject repairMenu;

    [SerializeField]
    GameObject resultsMenu;

    int currentDay = 1;
    [SerializeField] int totalDays = 3;

    [System.Serializable]
    struct MinMax
    {
        public int min;
        public int max;
    }

    [SerializeField]
    List<MinMax> minMaxBrokenPartsPerDay;

    [SerializeField]
    Robot robotPrefab;
    Robot currentRobot;

    bool repaired;

    private void Start()
    {
        ResetMenus();
    }

    void ResetMenus()
    {
        tileMenu.SetActive(true);
        howToPlayMenu.SetActive(false);
        weekMenuController.gameObject.SetActive(false);
        repairMenu.SetActive(false);
        resultsMenu.SetActive(false);
    }

    public void StartGame()
    {
        ResetMenus();
        StartCoroutine(GameRoutine());
    }
    
    /// <summary>
    /// Initializes game
    /// Show How to play
    /// Start the week
    /// Display results 
    /// Ask to play again
    /// </summary>
    /// <returns></returns>
    IEnumerator GameRoutine()
    {
        tileMenu.SetActive(false);
        yield return StartCoroutine(HowToPlayRoutine());
        yield return StartCoroutine(WeekRoutine());
        resultsMenu.SetActive(true);
    }

    public void HowToPlayMenuClosed() => howToPlayMenu.SetActive(false);
    IEnumerator HowToPlayRoutine()
    {
        howToPlayMenu.SetActive(true);
        while(howToPlayMenu.activeSelf)
            yield return null;
    }
 
    IEnumerator WeekRoutine()
    {
        weekMenuController.gameObject.SetActive(true);

        currentDay = 1;
        while (currentDay <= totalDays)
        {
            weekMenuController.SetDayText(currentDay, totalDays);
            yield return StartCoroutine(DayRoutine());
            // Report Day's earnings
            currentDay++;
        }
        weekMenuController.gameObject.SetActive(false);
    }

    IEnumerator DayRoutine()
    {
        // Build the robots to repair for the day
        // Trigger RepairRoutine for each robot
        var totalRobots = currentDay;
        var curRobot = 1;

        var limits = minMaxBrokenPartsPerDay[currentDay];
        while (curRobot <= totalRobots)
        {
            weekMenuController.SetRobotText(curRobot, totalRobots);
            var totalBrokenParts = RGN.Between(limits.min, limits.max);

            // Spawn Robot Prefab
            // Configure to allow no more than totalBrokenParts
            // Play Animation to show the new robot
            // Wait for animation to finish playing
            currentRobot = Instantiate(robotPrefab).GetComponent<Robot>();
            currentRobot.name = $"Robot_{currentDay}_{curRobot}";
            currentRobot.Initialize(totalBrokenParts);            

            yield return StartCoroutine(RepairRoutine());
            
            // Validate repair job
            curRobot++;
        }
    }


    public void Repaired() => repaired = true;
    IEnumerator RepairRoutine()
    {
        // Wait until the player clicks on "repaired"
        repaired = false;
        repairMenu.SetActive(true);
        while (!repaired)
            yield return null;

        // Destroy for now
        DestroyImmediate(currentRobot.gameObject);
        yield return new WaitForEndOfFrame();

        repairMenu.SetActive(false);
    }
}
