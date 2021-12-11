using UnityEngine;

public class Bullet 
{
   
    public float bullet_Speed;
    public float timeToShoot_Bullet;
    public Material material_Bullet;
    public Sprite imag;

    public Bullet(float bulletSpeed, float timeToShootBullet, Material materialBullet, Sprite imag)
    {
        this.bullet_Speed = bulletSpeed;
        this.timeToShoot_Bullet = timeToShootBullet;
        this.material_Bullet = materialBullet;
        this.imag = imag;
    }
    
    public Sprite GetImage()
    {
        return this.imag;

    }

    public Material GetMaterial()
    {
        return this.material_Bullet;

    }






}
