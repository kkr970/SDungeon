using UnityEngine;

//몬스터, 플레이어의 캐릭터 인터페이스
public interface ICharacter
{
    //speed를 기반으로 턴 순서를 정함
    //float로 숫자가 클수록 빠름
    void setTurn();
    float getTurn();

    //이름 반환
    string getName();

    //공격함
    void Attack(CharacterManager target);
    //공격받음
    /*
    void Damage(int damage);
    */
    
    //사망처리
    void Dead();

}
