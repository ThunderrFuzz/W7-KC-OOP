using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionBoost : Unit
{
    private ResourcePile m_CurrentPile = null;
    public float productionMulti = 2;
   protected override void BuildingInRange()
    {
        if(m_CurrentPile == null) 
        {
            ResourcePile pile = m_Target as ResourcePile;

            if(pile != null)
            {
                m_CurrentPile = pile;
                m_CurrentPile.ProductionSpeed *= productionMulti;
            }
        }
    }

    void resetProductionBoost()
    {
        if(m_CurrentPile != null)
        {
            //reset the multiplier 
            m_CurrentPile.ProductionSpeed /= productionMulti;
            //resets the current resoruce pile 
            m_CurrentPile = null;
        }
    }

    public override void GoTo(Building target)
    {
        resetProductionBoost();
        base.GoTo(target);
    }
    public override void GoTo(Vector3 position)
    {
        resetProductionBoost();
        base.GoTo(position);
    }
}
