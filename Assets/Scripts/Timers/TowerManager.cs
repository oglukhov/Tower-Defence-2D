using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class TowerManager : MonoBehaviour
{
    public GameObject simpleTowerPrefab;
    public GameObject fireTowerPrefab;
    public GameObject frozenTowerPrefab;
    public GameObject laserTowerPrefab;

    private GameObject selectedTowerPrefab;
    private Button selectedButton;

    public Text coinBalanceText;
    public int coinBalance = 300;

    private int simpleTowerCost = 100;
    private int fireTowerCost = 150;
    private int frozenTowerCost = 200;
    private int laserTowerCost = 250;

    private Dictionary<GameObject, int> placedTowers = new Dictionary<GameObject, int>();

    // Public objects with 2D colliders
    public GameObject restrictedArea1;
    public GameObject restrictedArea2;
    public GameObject hint;

    void Start()
    {
        // Add listeners to tower buttons
        GameObject.Find("SimpleTowerButton").GetComponent<Button>().onClick.AddListener(() => SelectTower(simpleTowerPrefab, "SimpleTowerButton", simpleTowerCost));
        GameObject.Find("FireTowerButton").GetComponent<Button>().onClick.AddListener(() => SelectTower(fireTowerPrefab, "FireTowerButton", fireTowerCost));
        GameObject.Find("FrozenTowerButton").GetComponent<Button>().onClick.AddListener(() => SelectTower(frozenTowerPrefab, "FrozenTowerButton", frozenTowerCost));
        GameObject.Find("LaserTowerButton").GetComponent<Button>().onClick.AddListener(() => SelectTower(laserTowerPrefab, "LaserTowerButton", laserTowerCost));

        // Add listener to remove all towers button
        GameObject.Find("RemoveAllTowersButton").GetComponent<Button>().onClick.AddListener(RemoveAllTowers);

        UpdateCoinBalanceUI();
    }

    void Update()
    {
        // Check for mouse click and place tower if one is selected and player has enough coins
        if (Input.GetMouseButtonDown(0) && selectedTowerPrefab != null && HasEnoughCoins())
        {
            if (!IsPointerOverUIObject(-1) && !IsPointerOnUILayer(Input.mousePosition))
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                position.z = 0; // Set z-axis to 0 to place on 2D plane

                if (!IsOverlapping(position) && !IsInRestrictedArea(position))
                {
                    GameObject towerInstance = Instantiate(selectedTowerPrefab, position, Quaternion.identity);
                    placedTowers[towerInstance] = GetSelectedTowerCost();
                    DeductCoins();
                    DeselectTower();
                }
                else
                {
                    if (IsInRestrictedArea(position))
                    {
                        Debug.Log("Position is in restricted area.");
                    }
                    if (IsOverlapping(position))
                    {
                        Debug.Log("Position is overlapping.");
                    }
                    StartCoroutine(ShowHint());
                    Debug.Log("Cannot place tower here, either overlapping or in restricted area.");
                }
            }
        }

        // Check for touch input for mobile devices
        if (Input.touchCount > 0 && selectedTowerPrefab != null && HasEnoughCoins())
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && !IsPointerOverUIObject(touch.fingerId) && !IsPointerOnUILayer(touch.position))
            {
                Vector3 position = Camera.main.ScreenToWorldPoint(touch.position);
                position.z = 0; // Set z-axis to 0 to place on 2D plane

                if (!IsOverlapping(position) && !IsInRestrictedArea(position))
                {
                    GameObject towerInstance = Instantiate(selectedTowerPrefab, position, Quaternion.identity);
                    placedTowers[towerInstance] = GetSelectedTowerCost();
                    DeductCoins();
                    DeselectTower();
                }
                else
                {
                    if (IsInRestrictedArea(position))
                    {
                        Debug.Log("Position is in restricted area.");
                    }
                    if (IsOverlapping(position))
                    {
                        Debug.Log("Position is overlapping.");
                    }
                    StartCoroutine(ShowHint());
                    Debug.Log("Cannot place tower here, either overlapping or in restricted area.");
                }
            }
        }
    }

    void SelectTower(GameObject towerPrefab, string buttonName, int towerCost)
    {
        // Деактивуємо попередню кнопку, якщо одна вибрана
        if (selectedButton != null)
        {
            selectedButton.GetComponent<Image>().color = Color.white; // Default color
        }

        // Якщо недостатньо монет, показуємо анімацію і виходимо
        if (coinBalance < towerCost)
        {
            StartCoroutine(ShakeAndColorChange(0.5f, Color.red, Color.white));
            return;
        }

        // Вибираємо нову вежу
        selectedTowerPrefab = towerPrefab;
        selectedButton = GameObject.Find(buttonName).GetComponent<Button>();
        selectedButton.GetComponent<Image>().color = Color.yellow; // Highlight color
    }

    void DeselectTower()
    {
        selectedTowerPrefab = null;
        if (selectedButton != null)
        {
            selectedButton.GetComponent<Image>().color = Color.white; // Default color
            selectedButton = null;
        }
    }

    void UpdateCoinBalanceUI()
    {
        coinBalanceText.text = coinBalance.ToString();
    }

    bool HasEnoughCoins()
    {
        return coinBalance >= GetSelectedTowerCost();
    }

    int GetSelectedTowerCost()
    {
        if (selectedTowerPrefab == simpleTowerPrefab) return simpleTowerCost;
        else if (selectedTowerPrefab == fireTowerPrefab) return fireTowerCost;
        else if (selectedTowerPrefab == frozenTowerPrefab) return frozenTowerCost;
        else if (selectedTowerPrefab == laserTowerPrefab) return laserTowerCost;
        else return 0;
    }

    void DeductCoins()
    {
        coinBalance -= GetSelectedTowerCost();
        UpdateCoinBalanceUI();
    }

    public void RemoveAllTowers()
    {
        // Знайти всі вежі в сцені
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        // Знищити кожну вежу і повернути її вартість
        foreach (GameObject tower in towers)
        {
            if (placedTowers.ContainsKey(tower))
            {
                coinBalance += placedTowers[tower];
                Destroy(tower);
            }
        }

        // Очистити словник розміщених веж
        placedTowers.Clear();

        // Оновити UI балансу монет
        UpdateCoinBalanceUI();
    }

    public void AddCoins(int amount)
    {
        coinBalance += amount;
        UpdateCoinBalanceUI();
    }

    bool IsOverlapping(Vector3 position)
    {
        BoxCollider2D selectedTowerCollider = selectedTowerPrefab.GetComponent<BoxCollider2D>();
        Collider2D[] colliders = Physics2D.OverlapBoxAll(position, selectedTowerCollider.size, 0);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Tower") && collider is BoxCollider2D)
            {
                return true;
            }
        }
        return false;
    }

    bool IsPointerOverUIObject(int fingerId)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        if (fingerId >= 0)
        {
            eventData.position = Input.GetTouch(fingerId).position;
        }
        else
        {
            eventData.position = Input.mousePosition;
        }
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }

    bool IsPointerOnUILayer(Vector3 screenPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, LayerMask.GetMask("UI"));
        return hit.collider != null;
    }

    bool IsInRestrictedArea(Vector3 position)
    {
        Collider2D[] colliders = new Collider2D[2];
        colliders[0] = restrictedArea1.GetComponent<Collider2D>();
        colliders[1] = restrictedArea2.GetComponent<Collider2D>();

        foreach (Collider2D collider in colliders)
        {
            if (collider.OverlapPoint(position))
            {
                return true;
            }
        }
        return false;
    }

    private IEnumerator ShowHint()
    {
        // Робимо об'єкт активним
        hint.SetActive(true);
        
        // Чекаємо визначений час
        yield return new WaitForSeconds(3);
        
        // Робимо об'єкт неактивним
        hint.SetActive(false);
    }

    private IEnumerator ShakeAndColorChange(float duration, Color startColor, Color endColor)
    {
        coinBalanceText.color = startColor;
        Vector3 originalPosition = coinBalanceText.transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-0.1f, 0.1f);
            float y = originalPosition.y;
            coinBalanceText.transform.position = new Vector3(originalPosition.x + x, y, originalPosition.z);
            elapsed += Time.deltaTime;
            yield return null;
        }

        coinBalanceText.color = endColor;
        coinBalanceText.transform.position = originalPosition;
    }
}