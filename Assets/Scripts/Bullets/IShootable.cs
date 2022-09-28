using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Shootable {ignore, shotAt }
public interface IShootable 
{

    Shootable Shoot { get; set; }

    public void ShotAt(Bullet bullet);
}
