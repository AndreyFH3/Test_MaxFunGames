using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class PlanetGenerator : MonoBehaviour, IGenerator
{
    [SerializeField, Range(20, 40)] private int maxPlanets;
    [SerializeField, Range(0.5f, 3)] private float maxPlanetsScale;

    [SerializeField] private Transform maxPosition;
    [SerializeField] private Transform minPosition;

    [SerializeField] private Planet planetReference;
    private List<Planet> planets = new List<Planet>();
    [SerializeField] private GameObjectData[] gameObjectDatas;


    [SerializeField] private LayerMask planetMask;
    [SerializeField] private Button startGame;
    [SerializeField] private EnemyAI _enemyAI;
    
    private void Start()
    {
        StartCoroutine(Generate());
        startGame.interactable = false;
    }

    public IEnumerator Generate()
    {
        yield return null;
        CreatePlanets();
        foreach (Planet planet in planets)
        {
            IsPlanetTouchAnother(planet.transform);
            yield return null;
        }
        SetPlayers();
        startGame.interactable = true;
        startGame.onClick.AddListener(() => StartCoroutine(_enemyAI.FindPlanet()));
    }
    private void CreatePlanets()
    {
        int range = Random.Range(15, maxPlanets);
        while (range > 0)
        {
            range--;
            Planet planet = Instantiate(planetReference,
                new Vector3(
                    Random.Range(minPosition.position.x, maxPosition.position.x),
                    Random.Range(minPosition.position.y, maxPosition.position.y),
                    0),
                Quaternion.identity,
                transform);
            planet.name = $"planet {planets.Count}";
            planets.Add(planet);
        }
    }

    private void SetPlayers()
    {
        foreach (GameObjectData god in gameObjectDatas)
        {
            Planet planet = planets[Random.Range(0, planets.Count)];
            planet.SetData(god);
            planet.OnStartPlaneAmount(50);
            startGame.onClick.AddListener(()=>StartCoroutine(planet.Generate()));
            planets.Remove(planet);
        }
    }

    private bool IsPlanetTouchAnother(Transform planet)
    {
        Collider2D[] collides = Physics2D.OverlapCircleAll(planet.position, 2f, planetMask);
        if (collides.Length > 0)
        {
            Vector3 newPosition = new Vector3(
                    Random.Range(minPosition.position.x, maxPosition.position.x),
                    Random.Range(minPosition.position.y, maxPosition.position.y),
                    0
                    );

            planet.position = newPosition;
            Collider2D[] cols = Physics2D.OverlapCircleAll(planet.position, 2f, planetMask);
            if (cols.Length > 0)
                return IsPlanetTouchAnother(planet);

        }
        return false;
    }
}