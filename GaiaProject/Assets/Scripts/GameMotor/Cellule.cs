﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LegumeEngine
{
    public class Cellule : MonoBehaviour
    {

        public Legume Legume;
        [SerializeField] private Transform LegumePosition;
        [SerializeField] private float _quality = 1;
        [SerializeField] private int _buff = 0;


        public KeyValuePair<int, int> Position;

        // Use this for initialization
        private void Start()
        {

        }

        // Update is called once per frame
        private void Update()
        {
        }

        public void Planter(LegumeManager.Type type)
        {
            if (!Legume)
            {
                GameObject legumePrefab = LegumeManager.GetInstance().GetLegumeInfo(type).m_Prefab;
                GameObject legume = Instantiate(legumePrefab);
                legume.name = type.ToString();
                legume.transform.SetParent(LegumePosition, false);

                Legume = legume.GetComponent<Legume>();
                Legume.M_Cellule = this;
            }
        }

        public void Supprimer()
        {
            Destroy(Legume.gameObject);
            Legume = null;
        }

        public void Recolter()
        {
            int recolte = Legume.Recolter(); //TODO Recolte
            if (recolte < 0)
            {
                Fertiliser(-recolte);
            }
        }

        public void Fertiliser(int buff)
        {
            if (buff > 1)
            {
                int x = Position.Key;
                int y = Position.Value;

                Cellule cell = Grille.GetInstance().GetCellule(x + 1, y);
                if (cell)
                    cell.Fertiliser(buff - 1);

                cell = Grille.GetInstance().GetCellule(x - 1, y);
                if (cell)
                    cell.Fertiliser(buff - 1);

                cell = Grille.GetInstance().GetCellule(x, y + 1);
                if (cell)
                    cell.Fertiliser(buff - 1);

                cell = Grille.GetInstance().GetCellule(x, y - 1);
                if (cell)
                    cell.Fertiliser(buff - 1);
            }
        }

        public int Distance(Cellule cell)
        {
            int distX = Mathf.Abs(Position.Key - cell.Position.Key);
            int distY = Mathf.Abs(Position.Value - cell.Position.Value);
            return distX + distY;
        }

        public int GetQuality()
        {
            return Mathf.FloorToInt(_quality*Mathf.Pow(1.25f, _buff));
        }

        private void OnMouseUp()
        {
            if (Legume)
                Recolter();
            //else 
            //	Planter (LegumeManager.Type.Tomate);
        }


        public void OnDrop_Fertiliser()
        {



            _quality++;

        }
    }
}
