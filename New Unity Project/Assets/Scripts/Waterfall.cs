using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Waterfall : MonoBehaviour
{

    public Tile tile1;
    public Tile tile2;
    public Tilemap tilemap;


    // Start is called before the first frame update
    void Start()

    {
        InvokeRepeating("Waterfall1", 0.0f, 0.5f);
        InvokeRepeating("Waterfall2", 0.25f, 0.5f);


    }

    void Waterfall1()
    {
            int x1 = 53;
            int y1 = -5;

            do
            {

                do
                {
                    tilemap.SetTile(new Vector3Int(x1, y1, 0), tile1);

                    x1++;
                } while (x1 < 58);

                y1 = y1 - 2;
                x1 = 53;
            } while (y1 > -15);


            int x2 = 53;
            int y2 = -6;
            do
            {

                do
                {
                    tilemap.SetTile(new Vector3Int(x2, y2, 0), tile2);

                    x2++;
                } while (x2 < 58);

                y2 = y2 - 2;
                x2 = 53;
            } while (y2 > -16);


    }


    void Waterfall2()
    {

            int x1 = 53;
            int y1 = -5;

            do
            {

                do
                {
                    tilemap.SetTile(new Vector3Int(x1, y1, 0), tile2);

                    x1++;
                } while (x1 < 58);

                y1 = y1 - 2;
                x1 = 53;
            } while (y1 > -15);


            int x2 = 53;
            int y2 = -6;
            do
            {

                do
                {
                    tilemap.SetTile(new Vector3Int(x2, y2, 0), tile1);

                    x2++;
                } while (x2 < 58);

                y2 = y2 - 2;
                x2 = 53;
            } while (y2 > -16);

    }

}
