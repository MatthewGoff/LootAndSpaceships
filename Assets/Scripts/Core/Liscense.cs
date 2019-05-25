public class Liscense
{
    public int UID;
    public string Name;
    public int Team;
    public Player Player;
    public Spaceship Mother;

    public Liscense(int uid, string name, int team, Player player, Spaceship mother)
    {
        UID = uid;
        Name = name;
        Team = team;
        Player = player;
        Mother = mother;
    }
}