using UnityEngine;

//몬스터, 플레이어의 캐릭터 인터페이스
public interface ICharacter
{
    //데미지를 받음
    void onDamage(float damage);

    //speed를 기반으로 턴 순서를 정함
    //float로 숫자가 클수록 빠름
    void setTurn();
    float getTurn();

    //이름 반환
    string getName();
}
