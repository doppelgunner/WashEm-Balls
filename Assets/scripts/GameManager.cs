using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private int MaxWave = 30;
    [SerializeField]
    private GameObject spawnerLeft;
    [SerializeField]
    private GameObject spawnerRight;
    [SerializeField]
    private GameObject leftBall;
    [SerializeField]
    private GameObject rightBall;
    [SerializeField]
    private float spawnTime = 1f;
    [SerializeField]
    private float delayBeforeStart = 3f;
    [SerializeField]
    private float delayBeforeEnd = 3f;

    [SerializeField]
    private GameObject dirtDirt;
    [SerializeField]
    private GameObject peeDirt;
    [SerializeField]
    private GameObject poopDirt;
    [SerializeField]
    private GameObject combDirt;

    [SerializeField]
    private int startingMoney = 5;
    [SerializeField]
    private int lives = 50;

    [SerializeField]
    private GameObject waterTurret;
    [SerializeField]
    private GameObject waterGun;
    [SerializeField]
    private GameObject stickySoap;
    [SerializeField]
    private GameObject bucketCatapult;

    [SerializeField]
    private Animator notifAnimator;
    [SerializeField]
    private Text notifText;

    [SerializeField]
    private Text coinText;
    [SerializeField]
    private Text lifeText;

    private static Text _coinText;
    private static Text _lifeText;

    private static Weapon.Type _selectedWeapon;

    private static int _currentWave;
    private static int _spawnCounter;

    private static int _currentMoney;
    private static int _currentLives;
    private static GameObject _selectedTile;

    private static GameManager _instance;

    private static List<GameObject> _highlightedCells; //REMOVE HIGHLIGHTED CELLS

    [SerializeField]
    private int _spawnLimit = 15;
    private WaveType _currentWaveType;

    private static GameObject _waterGun;
    private static GameObject _waterTurret;
    private static GameObject _stickySoap;
    private static GameObject _bucketCatapult;

    private static int _ballsOnScene;
    private static int _dirtsOnScene;

    private static float _delayBeforeEnd;
    private static float _delayBeforeStart;

    private static bool _gameOver;

    // Use this for initialization
    void Start () {
        _instance = this;

        _gameOver = false;

        _delayBeforeEnd = delayBeforeEnd;
        _delayBeforeStart = delayBeforeStart;

        _currentWave = 1;
        _spawnCounter = 0;
        _highlightedCells = new List<GameObject>();
        _selectedWeapon = Weapon.Type.NULL;

        _currentMoney = startingMoney;
        _currentLives = lives;

        //make it static
        _waterGun = waterGun;
        _waterTurret = waterTurret;
        _stickySoap = stickySoap;
        _bucketCatapult = bucketCatapult;
        _coinText = coinText;
        _lifeText = lifeText;

        //set Uis
        _coinText.text = "" + _currentMoney + "$";
        _lifeText.text = "" + _currentLives;

        _ballsOnScene = 0;
        _dirtsOnScene = 0;

        //START WAVE
        StartCoroutine(StartWave(3f));
    }

    // Update is called once per frame
    void Update () {
        if (EventSystem.current.IsPointerOverGameObject()) return; //EXIT UPDATE LOOP
        if (Input.GetMouseButton(0)) {
            if (_selectedTile != null) {
                Tile tile = _selectedTile.GetComponent<Tile>();

                if (tile.IsPlaceable()) {
                    InstantiateSelectedWeapon(_selectedTile.transform);
                    tile.SetPlaceable(false);

                    RemoveSelectedWeapon();
                    AudioController.PlayPlaceSound(1f);
                } else {
                    RemoveSelectedWeapon();
                }
            }
        }

        if (Input.GetMouseButton(1)) {
            RemoveSelectedWeapon();
        }
        
    }

    public void SpawnBalls() {

        if (_spawnCounter >= _spawnLimit) CancelInvoke();
        else SpawnLeft();

        if (_spawnCounter >= _spawnLimit) CancelInvoke();
        else SpawnRight();
    }

    private void SpawnLeft() {
        GameObject left = Instantiate(leftBall) as GameObject;
        left.transform.position = spawnerLeft.transform.position;
        GameObject dirt = getDirt(_currentWaveType);
        dirt.transform.parent = left.transform;
        dirt.transform.localPosition = Vector3.zero;

        Ball ballScript = left.GetComponent<Ball>();
        Dirt dirtScript = dirt.GetComponent<Dirt>();
        ballScript.AddSpeed(dirtScript.GetSpeed());
        dirtScript.MultiplyMaxHealth(((int)(_currentWave / 5)) + 1);

        _spawnCounter++;

        IncreaseBallsOnScene();
    }

    private void SpawnRight() {
        GameObject right = Instantiate(rightBall) as GameObject;
        right.transform.position = spawnerRight.transform.position;
        GameObject dirt = getDirt(_currentWaveType);
        dirt.transform.parent = right.transform;
        dirt.transform.localPosition = Vector3.zero;

        Ball ballScript = right.GetComponent<Ball>();
        Dirt dirtScript = dirt.GetComponent<Dirt>();
        ballScript.AddSpeed(dirtScript.GetSpeed());
        dirtScript.MultiplyMaxHealth(((int)(_currentWave / 5)) + 1);

        _spawnCounter++;

        IncreaseBallsOnScene();
    }

    public GameObject getDirt(Dirt.Type type) {
        switch (type) {
            case Dirt.Type.COMB:
                return Instantiate(combDirt) as GameObject;
            case Dirt.Type.POOP:
                return Instantiate(poopDirt) as GameObject;
            case Dirt.Type.PEE:
                return Instantiate(peeDirt) as GameObject;
            case Dirt.Type.DIRT:
                return Instantiate(dirtDirt) as GameObject;

            default:
                return null;
        }
    }

    public GameObject getDirt(WaveType type) {
        switch (type) {
            case WaveType.DIRT:
                return Instantiate(dirtDirt) as GameObject;
            case WaveType.PEE:
                return Instantiate(peeDirt) as GameObject;
            case WaveType.POOP:
                return Instantiate(poopDirt) as GameObject;
            case WaveType.RANDOM:
                return getDirt(randomDirt());
            case WaveType.COMB:
                return Instantiate(combDirt) as GameObject;

            default:
                return null;
        }
    }

    public WaveType getwaveType() {
        if (_currentWave == 30) {
            return WaveType.COMB;
        } else if (_currentWave % 5 == 1) {
            return WaveType.DIRT;
        } else if (_currentWave % 5 == 2) {
            return WaveType.PEE;
        } else if (_currentWave % 5 == 3) {
            return WaveType.POOP;
        } else if (_currentWave % 5 == 4) {
            return WaveType.RANDOM;
        } else if (_currentWave % 5 == 0) {
            return WaveType.COMB;
        }

        return WaveType.NULL;
    }

    public enum WaveType {
        DIRT, PEE, POOP, COMB, RANDOM, NULL
    }

    public Dirt.Type randomDirt() {
        int i = Random.Range(0,3);
        if (i == 0) return Dirt.Type.DIRT;
        if (i == 1) return Dirt.Type.PEE;
        if (i == 2) return Dirt.Type.POOP;

        return Dirt.Type.NULL;
    }

    public static void AddHighlightedCell(GameObject cell) {
        _highlightedCells.Add(cell);
    }

    public static void RemoveHighlightedCell(GameObject cell) {
        _highlightedCells.Remove(cell);
    }

    public static int GetHighlightedCellsCount() {
        return _highlightedCells.Count;
    }

    public static Weapon.Type GetSelectedWeapon() {
        return _selectedWeapon;
    }

    public static void SelectWeapon(Weapon.Type type) {
        if (_currentMoney < Weapon.GetCost(type)) return;
        _selectedWeapon = type;
    }

    public static void InstantiateSelectedWeapon(Transform parent) {
        GameObject gameObject = null;
        switch (GameManager.GetSelectedWeapon())
        {
            case Weapon.Type.NULL:
                return;
            case Weapon.Type.WATER_GUN:
                gameObject = Instantiate(_waterGun) as GameObject;
                break;
            case Weapon.Type.WATER_TURRET:
                gameObject = Instantiate(_waterTurret) as GameObject;
                break;
            case Weapon.Type.STICKY_SOAP:
                gameObject = Instantiate(_stickySoap) as GameObject;
                break;
            case Weapon.Type.BUCKET_CATAPULT:
                gameObject = Instantiate(_bucketCatapult) as GameObject;
                break;
        }

        gameObject.transform.parent = parent;
        gameObject.transform.localPosition = Vector2.zero;

        AddMoney(-Weapon.GetCost());    //minus money
    }

    public static void AddMoney(int m) {
        _currentMoney += m;
        if (_currentMoney < 0) _currentMoney = 0;
        _coinText.text = "" + _currentMoney + "$";
    }

    public static GameObject GetSelectedTile() {
        return _selectedTile;
    }

    public static void SetSelectedTile(GameObject tile) {
        _selectedTile = tile;
    }

    public static void RemoveSelectedTile(GameObject tile) {
        if (tile == null || _selectedTile == null) return;
        if (_selectedTile.Equals(tile)) {
            _selectedTile = null;
        }
    }

    public static void RemoveSelectedTile()
    {
        if (_selectedTile == null) return;
        _selectedTile = null;
    }

    public static void RemoveSelectedWeapon() {
        _selectedWeapon = Weapon.Type.NULL;

        if (_selectedTile != null) {
            _selectedTile.GetComponent<Tile>().UnHighlightTile();
        }

        RemoveSelectedTile();
    }

    public static void AddLife(int n) {
        _currentLives += n;
        if (_currentLives < 0) _currentLives = 0;
        _lifeText.text = "" + _currentLives;
    }


    public static void DecreaseBallsOnScene() {
        _ballsOnScene -= 1;

        if (_ballsOnScene <= 0) {
            _ballsOnScene = 0;

            if (!_gameOver) {
                _instance.StartCoroutine(_instance.EndWave(_delayBeforeEnd));
            }
        }
    }

    public static void IncreaseBallsOnScene() {
        _ballsOnScene += 1;
    }

    IEnumerator StartWave(float f) {
        yield return new WaitForSeconds(f);
        _currentWaveType = getwaveType();

        notifText.text = "WAVE: " + _currentWave;
        notifAnimator.SetTrigger("GoIn");
        StartCoroutine(OutNotification(2f));
        InvokeRepeating("SpawnBalls", _delayBeforeStart, spawnTime);
    }

    IEnumerator EndWave(float f) {
        yield return new WaitForSeconds(f);
        if (_currentWave == MaxWave) {
            StartCoroutine(Win(4f));
        } else {
            _spawnCounter = 0;
            _currentWave++;
            GetHarder();
            StartCoroutine(StartWave(0f));
        }
    }

    void GetHarder() {
        if (_currentWave % 2 == 0) _spawnLimit += 2;
        spawnTime -= (_currentWave / 10);
        if (spawnTime < 0.5) {
            spawnTime = 0.5f;
        }
    }

    IEnumerator OutNotification(float f) {
        yield return new WaitForSeconds(f);
        notifAnimator.SetTrigger("GoOut");
    }

    public static void CheckLose() {
        if (_currentLives == 0) {
            _gameOver = true;
            _instance.StartCoroutine(_instance.GameOver(4f));
        } 
    }

    IEnumerator Win(float f) {
        notifText.text = "YOU WIN";
        notifAnimator.SetTrigger("GoIn");
        yield return new WaitForSeconds(f);

        //JUMP TO WIN SCENE
        SceneManager.LoadScene("Win");
    }

    IEnumerator GameOver(float f) {
        notifText.text = "GAME OVER";
        notifAnimator.SetTrigger("GoIn");
        yield return new WaitForSeconds(f);

        //JUMP TO LOSE SCENE
        SceneManager.LoadScene("Lose");
    }

    public static bool IsGameOver() {
        return _gameOver;
    }
}
