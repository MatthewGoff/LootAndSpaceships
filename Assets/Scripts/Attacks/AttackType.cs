public enum AttackType : byte
{
    Bullet = 0b0000_0001,
    Rocket = 0b0000_0010,
    EMP = 0b0000_0100,
    Harpoon = 0b0000_1000,
    Flamethrower = 0b0001_0000,
    Laser = 0b0010_0000,
    Mine = 0b0100_0000
}