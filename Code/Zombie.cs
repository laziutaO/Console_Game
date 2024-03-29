﻿using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using static Constants;

public class Zombie : BasicStats, IHitable
{
    public int wayCounter = 0;
    public int Speed { get; set; } = 15;
    public int Health { get; set; } = 5;

    public MonstersSpawns monstrSp;

    ConfigClass conf = JsonConvert.DeserializeObject<ConfigClass>(File.ReadAllText(@"jsconfig.json"));

    public const int wayLength = 27;

    public int EngagedXcoord = 95;

    public void AnimateEnemy()
    {
        if (wayCounter < wayLength)
        {
            int xCoordZombie = monstrSp.XRightSpawn - wayCounter;
            DrawEnemy(xCoordZombie, monstrSp.YRightSpawn,
                new DrawZombie
                {
                    Head = @"(+_+)",
                    Body = @"--| |",
                    Legs = @"  / \"
                });
            ClearSpace(xCoordZombie + zombieLngth, monstrSp.YRightSpawn-1);
            EngagedXcoord = monstrSp.XRightSpawn - wayCounter;
            wayCounter++;
         }
    }

    private void ClearSpace(int xCoord, int yCoord)
    {
        string[] spaces = { new String(' ', 1), new String(' ', 1 ), new String(' ', 1)};
        foreach (var space in spaces)
        {
            CleanOrWriteSymbol(xCoord, yCoord, space);
            yCoord++;
        }
    }
    
    public async void GetDamaged()
    {
        int xCoordZombie = monstrSp.XRightSpawn - wayCounter + 1;
        SetColor("Red");
        DrawEnemy(xCoordZombie, monstrSp.YRightSpawn,
                new DrawZombie 
                {
                    Head = @"(+_+)",
                    Body = @"--| |",
                    Legs = @"  / \"
                });
        await Task.Run(() => Health--);
        SetColor("White");
    }
    
    public void DrawEnemy(int coordX, int coordY, DrawZombie drZombie)
    {
        CleanOrWriteSymbol(coordX, coordY - 1, drZombie.Head);
        CleanOrWriteSymbol(coordX, coordY, drZombie.Body);
        CleanOrWriteSymbol(coordX, coordY + 1, drZombie.Legs);
    }

    public new bool CheckOnHit(int bulletCoord, int enemyCoord)
    {
        int addCoord = 2;
        return bulletCoord == enemyCoord + addCoord;
    }

    public struct DrawZombie
    {
        public string Head { get; set; }
        public string Body { get; set; }
        public string Legs { get; set; }
    }
}
