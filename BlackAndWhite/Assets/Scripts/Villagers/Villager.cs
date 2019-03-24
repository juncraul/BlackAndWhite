using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Villager : MonoBehaviour
{
    public Material Material;
    public DiscipleTypes Disciple;
    public NavMeshAgent agent;

    public enum DiscipleTypes
    {
        Farmer,
        Forester,
        Fisherman,
        Builder,
        Missionary,
        Craftsman,
        Trader,
        Breeder
    }

    private enum State
    {
        Idle,
        GoingForFood,
        GoingForWood,
        GoingToBuildingSite,
        Building
    }

    private GameObject target;
    private State currentState;
    private int woodInBag;
    private int foodInBag;

    private const float DISTANCE_OF_INTERACTION = 0.4f;
    private const int MAX_WOOD_CARRY = 250;
    private const int MAX_FOOD_CARRY = 250;

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        switch (Disciple)
        {
            case DiscipleTypes.Builder:
                DoBuild();
                break;
            default:
                break;
        }
    }

    void DoBuild()
    {
        switch (currentState)
        {
            case State.Idle:
                target = GetConstructionSite();
                if(target != null)
                {
                    currentState = State.GoingForWood;
                }
                break;
            case State.GoingForWood:
                target = GetWoodDeposit();
                if(target != null)
                {
                    Debug.Log($"Distance: {DistanceToObject(target)}");
                    if(DistanceToObject(target) > DISTANCE_OF_INTERACTION)
                    {
                        Debug.Log("Going to village store for wood.");
                        agent.SetDestination(target.transform.position);
                    }
                    else
                    {
                        Debug.Log("Collecting wood.");
                        if (target.GetComponent<VillageStore>().CollectWood(MAX_WOOD_CARRY))
                        {
                            Debug.Log("Wood collected.");
                            woodInBag = MAX_WOOD_CARRY;
                            currentState = State.GoingToBuildingSite;
                        }
                        else
                        {
                            currentState = State.Idle;
                        }
                    }
                }
                break;
            case State.GoingToBuildingSite:
                target = GetConstructionSite();
                if (target != null && woodInBag == MAX_WOOD_CARRY)
                {
                    if (DistanceToObject(target) > DISTANCE_OF_INTERACTION)
                    {
                        agent.SetDestination(target.transform.position);
                    }
                    else
                    {
                        if (target.GetComponent<Building>().Construct(woodInBag))
                        {
                            woodInBag = 0;
                            currentState = State.GoingForWood;
                        }
                        else
                        {
                            currentState = State.Idle;
                        }
                    }
                }
                break;
            case State.Building:
                break;
        }

    }

    protected void Init()
    {
        GetComponent<MeshRenderer>().material = Material;
        currentState = State.Idle;
    }

    private float DistanceToObject(GameObject gameObject)
    {
        float distanceToPositions = Vector3.Distance(transform.position, target.transform.position);
        return distanceToPositions - (transform.localScale + gameObject.transform.localScale).magnitude;
    }

    private GameObject GetConstructionSite()
    {
        GameObject buildings = GameObject.Find("Buildings");
        foreach (Transform child in buildings.transform)
        {
            if (child.gameObject.GetComponent<Building>().Constructed < 1)
            {
                return child.gameObject;
            }
        }
        return null;
    }

    private GameObject GetWoodDeposit()
    {
        GameObject buildings = GameObject.Find("Buildings");
        foreach (Transform child in buildings.transform)
        {
            if (child.gameObject.GetComponent<VillageStore>() != null && child.gameObject.GetComponent<VillageStore>().Wood >= 250)
            {
                return child.gameObject;
            }
        }
        return null;
    }
}
