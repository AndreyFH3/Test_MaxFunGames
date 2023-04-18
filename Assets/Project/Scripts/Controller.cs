using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controller : MonoBehaviour
{
    List<Planet> planets = new List<Planet>();
    private Vector2 mouseScreenPosition;
    [SerializeField] private LayerMask _planetMask;

    public void SelectPlanet(InputAction.CallbackContext cntx)
    {
        if (cntx.performed)
        {

            Vector3 pos = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.zero, 1000, _planetMask);

            if (hit.collider != null && hit.transform.TryGetComponent(out Planet p))
            {
                if (p.Data != null && p.Data.GetObjectType == ObjectType.player)
                {
                    p.SelectPlanet();
                    planets.Add(p);
                }
                Debug.Log("pressed");
            }
        }
    }    

    public void TargetPlanet(InputAction.CallbackContext cntx)
    {

           if (planets.Count <= 0) return;
   
           Vector3 pos = Camera.main.ScreenToWorldPoint(mouseScreenPosition);
           RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.zero, 1000, _planetMask);
   
           if (hit.collider != null && hit.transform.TryGetComponent(out Planet p))
           {
   
               foreach (Planet planet in planets)
               {
                   if (planet.transform == hit.collider.transform) return;
               }
               foreach (Planet planet in planets)
               {
                   planet.SpawnPlaneToNextPlanet(p.transform);
               }
               DeselectPlanets();
           }
           DeselectPlanets();
    }

    private void DeselectPlanets()
    {
        foreach (Planet planet in planets)
        {
            planet.DeSelectPlanet();
        }
        planets.Clear();
    }

    public void MousePositionOnScreen(InputAction.CallbackContext cntx)
    {
        mouseScreenPosition = cntx.ReadValue<Vector2>();
    }

}
