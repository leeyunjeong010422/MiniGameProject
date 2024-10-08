using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//게임 화면 오른쪽 끝에서 랜덤 위치에 파티클을 생성하고 왼쪽으로 이동시킴
public class RandomParticleSpawner : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;
    [SerializeField] float spawnInterval = 5f; //3초마다 파티클 생성
    [SerializeField] Vector2 spawnArea = new Vector2(0f, 0.5f); //생성 범위(Y축만 랜덤)
    [SerializeField] float moveSpeed = 80f;
    [SerializeField] float particleLifetime = 3f; //파티클 생존 시간 (5초 뒤에 파괴)

    [SerializeField] Sprite warningSprite;
    [SerializeField] Vector2 warningSize = new Vector2(10f, 10f);
    [SerializeField] float warningDuration = 2f; //지속 시간
    [SerializeField] float blinkInterval = 0.2f; //깜빡임 간격

    private Camera mainCamera;
    private Queue<(ParticleSystem particle, float spawnTime)> activeParticles = new Queue<(ParticleSystem, float)>(); //현재 활성화된 파티클과 시간을 저장
    private WaitForSeconds waitForSpawnInterval; //생성 주기 대기를 위한 코루틴
    private WaitForSeconds startRandomParticle; //처음에 파티클 생성을 언제 시작할 건지 호출하는 코루틴

    private void Start()
    {
        mainCamera = Camera.main; //메인 카메라를 가져옴
        waitForSpawnInterval = new WaitForSeconds(spawnInterval);
        startRandomParticle = new WaitForSeconds(5f); //5초 뒤에 파티클 생성을 시작한다는 것
        StartCoroutine(StartRandomParticle());
    }

    private IEnumerator StartRandomParticle()
    {
        yield return startRandomParticle; //위에 설정한 시간(5초) 대기 후
        StartCoroutine(SpawnParticles()); //파티클 생성 코루틴 시작
    }

    private IEnumerator SpawnParticles()
    {
        while (true)
        {
            int randomCount = Random.Range(1, 3);

            for (int j = 0; j < randomCount; j++)
            {

                //카메라 뷰포트에서 오른쪽 끝 부분의 월드 좌표(뷰포트 좌표 (1, y, z))
                //카메라 이해: https://ansohxxn.github.io/unitydocs/camera/
                //          : https://m.blog.naver.com/corncho456/221727952827
                Vector2 warningPosition = mainCamera.ViewportToWorldPoint(new Vector2(0.9f, Random.Range(0.2f, 0.8f)));
                warningPosition.y += Random.Range(-spawnArea.y, spawnArea.y); //Y축 범위에서 랜덤하게 위치 설정

                //공격 예정 위치에 경고 이미지 생성
                SoundManager.Instance.PlayWarningSound();
                GameObject warning = CreateWarning(warningPosition);
                yield return StartCoroutine(BlinkEffect(warning));
                Destroy(warning);

                //파티클 생성 (위치: 위에 warningPosition가 발생한 위치보다 조금 더 오른쪽에서 생성)
                Vector2 spawnPosition = mainCamera.ViewportToWorldPoint(new Vector2(1.1f, mainCamera.WorldToViewportPoint(warningPosition).y));
                ParticleSystem newParticle = Instantiate(particle, spawnPosition, Quaternion.identity);
                newParticle.Play();
                activeParticles.Enqueue((newParticle, Time.time)); //생성된 파티클과 생성 시간 추가
            }

            yield return waitForSpawnInterval;
        }
    }

    private void FixedUpdate()
    {
        int particleCount = activeParticles.Count;

        //모든 활성화된 파티클 이동 처리
        for (int i = 0; i < particleCount; i++)
        {
            var (particle, spawnTime) = activeParticles.Dequeue(); //큐에서 첫번째 파티클과 생성시간을 꺼냄

            if (particle != null && particle.isPlaying)
            {
                particle.transform.position += Vector3.left * moveSpeed * Time.deltaTime; //왼쪽으로 이동

                //5초가 지나면 파티클을 제거
                if (Time.time - spawnTime > particleLifetime)
                {
                    Destroy(particle.gameObject);
                }
                else
                {
                    //아직 생존시간(5초)가 남아있으면 파티클을 큐에 다시 추가
                    //계속해서 왼쪽으로 움직여야 하기 때문에
                    activeParticles.Enqueue((particle, spawnTime));
                }
            }
        }
    }

    private GameObject CreateWarning(Vector3 position)
    {
        //경고 이미지를 표시할 GameObject 생성
        GameObject warning = new GameObject("ParticleWarning");
        SpriteRenderer spriteRenderer = warning.AddComponent<SpriteRenderer>();

        spriteRenderer.sprite = warningSprite;
        spriteRenderer.color = Color.red;
        warning.transform.localScale = warningSize; //이미지 크기 조절
        warning.transform.position = position; //위치 설정

        warning.transform.SetParent(mainCamera.transform);

        return warning;
    }

    private IEnumerator BlinkEffect(GameObject warning)
    {
        SpriteRenderer renderer = warning.GetComponent<SpriteRenderer>(); //이미지 가져오기

        float elapsedTime = 0f;
        bool isVisible = true;

        while (elapsedTime < warningDuration) //경과한 시간이 warningDuration보다 작을 때
        {
            renderer.enabled = isVisible;
            isVisible = !isVisible;
            elapsedTime += blinkInterval;
            yield return new WaitForSeconds(blinkInterval);
        }

        renderer.enabled = true;
    }
}
