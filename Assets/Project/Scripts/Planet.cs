using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;


public class Planet : MonoBehaviour, IProviderUI, IGenerator
{
    
    [SerializeField, Range(5,25), Description("Amount plane generate per second")] private int shipPerSecond = 5;
    [SerializeField, Range(50,150), Description("Amount plane generate per second")] private int _maxPlaneOnPlanet = 100;
    [Header("Plane on start")]
    [SerializeField] private int _maxShips = 50;
    [SerializeField] private int _minShips = 5;
    [Header("Plane")]
    [SerializeField] private PlaneAI _plane;
    [Header("Planet Text")]
    [SerializeField] private TextMeshProUGUI planeAmountText;
    
    private protected GameObjectData _data; 
    public bool IsSelected { get; private protected set; }
    
    public GameObjectData Data { get => _data; private protected set => _data = value; }

    private void Awake()
    {
        OnStartPlaneAmount();
    }

    public int PlaneAmount { get; private set; }

    public void SetData(GameObjectData data)
    { 
        Data = data;
        GetComponent<SpriteRenderer>().color = Data.GetObjectColor;
    }
    public void OnStartPlaneAmount(int amount = 0)
    {
        PlaneAmount  = amount == 0 ? Random.Range(_minShips, _maxShips) : amount;
        SetText();
    }

    public void SelectPlanet()
    {
        GetComponent<SpriteRenderer>().color += new Color(15, 15, 15, 0);
        IsSelected = true;
    }

    public void DeSelectPlanet()
    {
        GetComponent<SpriteRenderer>().color -= new Color(15, 15, 15, 0);
        IsSelected = false;
    }

    public void SpawnPlaneToNextPlanet(Transform targetPosition)
    {
        int amount = PlaneAmount / 2;
        while (amount > 0)
        {
            amount--;
            PlaneAmount--;
            PlaneAI plane = Instantiate(_plane, 
                transform.position +
                new Vector3(Random.Range(-1.0f, 1.0f),
                        Random.Range(-1.0f, 1.0f),
                        0),
                        Quaternion.identity);
            plane.SetTarget(targetPosition);
            plane.SetData(Data);
            if (PlaneAmount <= 0)
            {
                SetText();
            }
        }
        SetText();

    }

    public IEnumerator Generate()
    {
        WaitForSeconds wfs = new WaitForSeconds(1.0f/shipPerSecond);

        while (true)
        {
            if(PlaneAmount < _maxPlaneOnPlanet)
            {
                PlaneAmount++;
                SetText();
            }

            yield return wfs;
        }
    }

    public void SetText()
    {
        planeAmountText.text = PlaneAmount.ToString();
    }


    public void Capture(PlaneAI plane)
    {
        if (Data == null)
        {
            PlaneAmount--;
            if (PlaneAmount <= 0)
            {
                SetData(plane.Data);
                StartCoroutine(Generate());
            }
            Destroy(plane.gameObject);
        }
        else if (plane.Data != Data)
        {
            PlaneAmount--;
            if (PlaneAmount <= 0)
            {
                if (IsSelected) DeSelectPlanet();
                SetData(plane.Data);
                Destroy(plane.gameObject);
            }
        }
        else if(plane.Target == transform)
        {
            PlaneAmount++;
            Destroy(plane.gameObject);

        }
        SetText();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 2);
    }
}
