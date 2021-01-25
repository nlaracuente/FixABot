using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    GameObject titleMenu;

    [SerializeField]
    GameObject howToPlayMenu;

    [SerializeField]
    WeekMenuController weekMenuController;

    [SerializeField]
    GameObject repairMenu;

    [SerializeField]
    RobotRepairResults repairResults;

    [SerializeField]
    ResultsMenu resultsMenu;

    int currentDay = 1;
    [SerializeField] int totalDays = 1;

    /// <summary>
    /// Total robots to repair
    /// </summary>
    [SerializeField] int totalRobotsToRepair = 3;

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

    int totalRepairedRobots = 0;
    int totalRobotsSpawned = 0;

    [SerializeField]
    RobotStand robotStand;

    [System.Serializable]
    public struct PuzzleConfig
    {
        public ColorName colorName;
        public Color color;
    }
    [SerializeField]
    List<PuzzleConfig> puzzleConfigs;
    Dictionary<ColorName, Color> colorNameMapping;
    public Dictionary<ColorName, Color> ColorNameMapping 
    {
        get {
            if (colorNameMapping == null || colorNameMapping.Count() < 1)
                BuildColorNameMapping();

            return colorNameMapping;
        }
    }

    public Camera MainCamera { get { return Camera.main; } }

    bool repaired;

    private void Start()
    {
        if(robotStand == null)
            robotStand = FindObjectOfType<RobotStand>();

        if (repairResults == null)
            repairResults = FindObjectOfType<RobotRepairResults>();

        BuildColorNameMapping();
        ResetMenus();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    private void BuildColorNameMapping()
    {
        colorNameMapping = new Dictionary<ColorName, Color>();
        foreach (var config in puzzleConfigs)
        {
            if (!colorNameMapping.ContainsKey(config.colorName))
                colorNameMapping.Add(config.colorName, config.color);
        }
    }

    void ResetMenus()
    {
        if (titleMenu == null)
            return;

        titleMenu?.SetActive(true);
        howToPlayMenu?.SetActive(false);
        weekMenuController?.gameObject.SetActive(false);
        repairMenu?.SetActive(false);
        repairResults?.gameObject.SetActive(false);
        resultsMenu?.gameObject.SetActive(false);
    }

    public ColorName[] GetShuffledColorNames()
    {
        var colorNames = Utility.GetEnumValues<ColorName>();
        var shuffledColors = Utility.ShuffleArray(colorNames, RandomNumbers.Seed);
        return shuffledColors;
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
        titleMenu.SetActive(false);
        yield return StartCoroutine(HowToPlayRoutine());
        yield return StartCoroutine(WeekRoutine());
        resultsMenu.gameObject.SetActive(true);
        resultsMenu.SetResults(totalRepairedRobots, totalRobotsSpawned);
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

        totalRepairedRobots = 0;
        totalRobotsSpawned = 0;       

        currentDay = 1;
        while (currentDay <= totalDays)
        {
            weekMenuController.SetDayText(currentDay, totalDays);
            yield return StartCoroutine(DayRoutine());
            currentDay++;
        }
        weekMenuController.gameObject.SetActive(false);
    }

    IEnumerator DayRoutine()
    {
        // Build the robots to repair for the day
        // Trigger RepairRoutine for each robot
        var totalRobots = totalRobotsToRepair;
        var curRobot = 1;

        totalRobotsSpawned += totalRobots;
        weekMenuController.SetRepairedText(totalRepairedRobots, totalRobotsSpawned);

        var limits = minMaxBrokenPartsPerDay[currentDay];
        while (curRobot <= totalRobots)
        {
            weekMenuController.SetRobotText(curRobot, totalRobots);
            var totalBrokenParts = RGN.Between(limits.min, limits.max);

            // Build Robot
            robotStand.ResetRotation();
            currentRobot = Instantiate(robotPrefab, robotStand.transform).GetComponent<Robot>();
            currentRobot.name = $"Robot_{currentDay}_{curRobot}";
            currentRobot.Initialize(totalBrokenParts);            

            yield return StartCoroutine(RepairRoutine());            
            curRobot++;
        }
    }

    public void Repaired()
    {
        repaired = true;
        AudioManager.instance.ArrowClicked();
    }
    IEnumerator RepairRoutine()
    {
        // Wait until the player clicks on "repaired"
        repaired = false;
        repairMenu.SetActive(true);
        while (!repaired)
            yield return null;

        repairMenu.SetActive(false);

        // Validate Repair Job
        if (currentRobot.IsRepaired)
            totalRepairedRobots += 1;

        // Update results
        weekMenuController.SetRepairedText(totalRepairedRobots, totalRobotsSpawned);

        repairResults.gameObject.SetActive(true);
        repairResults.SetResults(currentRobot.IsRepaired);
        yield return new WaitForSeconds(.75f);
        repairResults.gameObject.SetActive(false);

        DestroyImmediate(currentRobot.gameObject);
        yield return new WaitForEndOfFrame();

        repairMenu.SetActive(false);
    }
}

