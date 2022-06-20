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
    //정보 반환
    string getInfo();

    //공격함
    void Attack(CharacterManager target);
    //공격받음
    void onDamage(int damage);
    //mp사용
    void useMp(int mp);
    //스킵
    void skip();


    //hp, mp바 업데이트
    void updateHpBar();
    void updateMpBar();
    
    //사망처리
    void Dead();
}
