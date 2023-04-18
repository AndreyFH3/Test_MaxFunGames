using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private LayerMask _planetLayerMask;
    [SerializeField] private Difficult _difficult;
 

    public IEnumerator FindPlanet()
    {
        float waitTime = _difficult.DifficultyLevel();
        WaitForSeconds wfs = new WaitForSeconds(waitTime);

        while (true)
        {
            yield return wfs;
            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position, 1000, _planetLayerMask);
            if (cols.Length > 0)
            {
                Planet planetNearest = cols[0].GetComponent<Planet>();
                Planet enemyPlanet = null;

                foreach (Collider2D col in cols)
                {
                    if (col.transform.TryGetComponent(out Planet planet))
                    {
                        if(planet.Data != null && planet.Data.GetObjectType == ObjectType.enemy)
                        {
                            if(enemyPlanet == null)
                            {
                                enemyPlanet = planet;
                            }

                            else if (enemyPlanet.PlaneAmount <= planet.PlaneAmount)
                            {
                                enemyPlanet = planet;
                            }
                        }
                        if ((planet.Data == null || planet.Data.GetObjectType == ObjectType.player) && planetNearest.PlaneAmount == planet.PlaneAmount)
                        {
                            if (Vector3.Distance(transform.position, planetNearest.transform.position) > Vector3.Distance(transform.position, planet.transform.position))
                            {
                                planetNearest = planet;
                            }
                        }
                        else if (planetNearest.PlaneAmount >= planet.PlaneAmount)
                        {
                            planetNearest = planet;
                        }
                    }
                }
                if(enemyPlanet.Data != null)
                    enemyPlanet.SpawnPlaneToNextPlanet(planetNearest.transform);
            }
        }
    }

}
