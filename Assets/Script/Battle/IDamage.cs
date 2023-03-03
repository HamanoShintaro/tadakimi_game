/// <summary>
/// ダメージの処理をするインターフェイス(enemy or player のタグをつける)
/// </summary>
public interface IDamage
{
    void Damage(float attackPower = 0, float kb = 0);
}