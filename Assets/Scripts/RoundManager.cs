using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PG
{
    public class RoundManager : MonoBehaviour
    {
        public int currentRound = 1;
        // Start is called before the first frame update
        void Start()
        {
            CalculateInitiativeOrder();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void CalculateInitiativeOrder()
        {
           List<UnitManager> units = new List<UnitManager> ();
           units = FindObjectsOfType<UnitManager>().ToList<UnitManager>();
           foreach (UnitManager unit in units)
           {
                unit.initiative = Random.Range(1, 20);
           }
        }

        public void NextRound()
        {
            currentRound++;
            //UPDATE UI WITH ROUND NUMBER
        }
    }
}
