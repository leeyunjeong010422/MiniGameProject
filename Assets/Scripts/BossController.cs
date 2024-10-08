using System.Collections;
using UnityEngine;

public class BossController : MonoBehaviour
{
    [SerializeField] Sprite warningSprite;
    [SerializeField] GameObject[] attackParticlePrefabs; //공격 타입 배열
    [SerializeField] float warningDuration = 1f; //깜빡이는 시간
    [SerializeField] float blinkInterval = 0.2f; //깜빡이는 간격
    [SerializeField] Vector2 warningSize = new Vector2(10f, 10f); //경고 이미지 크기

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(3f);
        StartCoroutine(AttackPattern());
    }

    //공격 패턴 처리 코루틴
    private IEnumerator AttackPattern()
    {
        while (true)
        {
            int attackCount = Random.Range(1, 3); //1~2회 랜덤으로 동시 공격

            for (int i = 0; i < attackCount; i++)
            {
                //각 공격을 마다 코루틴으로 실행
                StartCoroutine(RealAttack());
            }

            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator RealAttack()
    {

        //공격 위치를 랜덤으로 생성
        Vector2 attackPosition = new Vector2(-10, Random.Range(-35f, 40f));

        //공격 예정 위치에 경고 이미지를 생성
        GameObject warning = CreateWarning(attackPosition);

        SoundManager.Instance.PlayWarningSound();

        //2초 동안 깜빡임
        yield return StartCoroutine(BlinkEffect(warning));

        //공격 파티클 생성 및 재생 (랜덤 선택)
        int randomIndex = Random.Range(0, attackParticlePrefabs.Length);

        //랜덤으로 선택한 프리팹으로 파티클 생성
        GameObject particle = Instantiate(attackParticlePrefabs[randomIndex], attackPosition, Quaternion.identity);
        ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();

        if (particleSystem != null)
        {
            SoundManager.Instance.PlayBossSound();
            particleSystem.Play(); //파티클 재생
        }

        Destroy(warning); //경고 이미지 제거

        //공격 파티클이 모두 끝날 때까지 대기
        yield return new WaitForSeconds(particleSystem.main.duration);
        Destroy(particle); //파티클 제거

        //공격 후 잠시 대기 (n초 후 다시 공격)
        //yield return new WaitForSeconds(1f);
    }


    private GameObject CreateWarning(Vector3 position)
    {
        //경고 이미지를 표시할 GameObject 생성
        GameObject warning = new GameObject("AttackWarning");
        SpriteRenderer spriteRenderer = warning.AddComponent<SpriteRenderer>();

        spriteRenderer.sprite = warningSprite; //스프라이트 설정
        spriteRenderer.color = Color.red; //빨간색 설정
        warning.transform.localScale = warningSize; //이미지 크기 조절
        warning.transform.position = position; //위치설정

        return warning;
    }

    private IEnumerator BlinkEffect(GameObject warning)
    {
        SpriteRenderer renderer = warning.GetComponent<SpriteRenderer>(); //이미지 가져오기

        float elapsedTime = 0f; //경과된 시간 (초기 0초)
        bool isVisible = true; //지금 이미지가 보이나? 안 보이나? (상태)

        while (elapsedTime < warningDuration) //경과한 시간이 warningDuration 보다 작을 때까지 반복
        {
            //이미지 표시 상태 변경, isVisible 값을 반대로 바꿈 (true -> false, false -> true)로!
            renderer.enabled = isVisible;
            isVisible = !isVisible;
            elapsedTime += blinkInterval; //지난 시간에 blinkInterval를 더함
                                          //현재는 blinkIntervaldl 0.2니까 계속해서 0.2초씩 증가함
            
            yield return new WaitForSeconds(blinkInterval);
        }

        renderer.enabled = true;
    }
}
