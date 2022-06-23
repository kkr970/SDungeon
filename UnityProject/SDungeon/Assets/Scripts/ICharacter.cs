
//몬스터, 플레이어의 캐릭터 인터페이스
public interface ICharacter
{
    //턴 숫자가 클수록 빠르게 행동
    void setTurn();
    float getTurn();


    //공격함
    void attack(CharacterManager target);
    //스킬 사용, 사용가능에 대한 true,false 반환
    bool skill(CharacterManager target, int num);
    string skill_1_Info();
    //스킵, mp회복
    void skip();



    //공격받음
    void onDamage(int damage);
    //mp사용
    void useMp(int mp);
    //mp회복
    void recoverMP(int mp);
    //hp회복
    void recoverHP(int hp);
    //hp, mp바 업데이트
    void updateHpBar();
    void updateMpBar();

    //상태이상 처리
    void checkEffect();
    

    //사망처리
    void dead();
    //이름 반환
    string getName();
    //정보 반환
    string getInfo();
    //스텟 정보 반환
    string getStatInfo();

    //Enemy전용
    //AI, 행동 선택 (Attack, Skill1, Skill2 ... )
    string enemyActionAI();
}
