# SDungeon
Unity 공부에 이어 Unity를 이용한 턴제형식전투 게임  
Study Unity Repository : https://github.com/kkr970/Study_Unity  
제작 : Shin Icksu // kkr970  
제작 기간 : 2022-06-09 ~ 제작 중  


게임 설명
-------------------------------
* 턴제 형식의 전투 방식 캐릭터들의 속도를 가지고 결정  
* 게임 클리어 조건 : 적을 모두 처치  
* 게임 실패 조건 : 아군이 모두 사망  
* 적 AI
  * 행동 선택  
    * 공격  
    * 스킬사용  
  * 플레이어 타겟 선택
    플레이어의 Hide를 기반으로 선택(랜덤성 있음)  
    
    
캐릭터 설명
--------------------------------
Knight : 기사, 기본 체력과 공격력이 강함  
ArchMage : 마법사, 받는 피해의 50%(내림)을 MP로 대신 받음  
Priest : 사제, 적군과 아군에 대해 스킬 사용이 달라짐, HP회복이 가능  


능력치
--------------------------------
Speed : 턴에 우선권을 가질 가능성이 높아짐  
Hide : 공격을 받을 확률이 낮아짐  
Power : 기본 공격, 물리 공격이 강해짐, 최대 HP증가  
Magic : 마법 공격이 높아짐, 최대 MP증가  
Lucky : 미구현  
Wisdom : 미구현  


상태이상
---------------------------------
Stun : 스턴, 기절, 다음에 오는 1턴 스킵  


조작
----------------------------------
* 마우스  
  * 왼 클릭 : 행동의 선택, 타겟의 선택  
  * 우 클릭 : 해당 캐릭터(Player, Enemy)의 능력치를 볼 수 있음  
* 키보드  
  * ESC Escape : 매뉴, 일시정지  


> 나머지 구상중