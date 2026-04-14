using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Paramètres de spawn")]
    [SerializeField] private GameObject enemyPrefab;
// donne a modifer 
    [SerializeField] private int ennemisParVague = 3;
    [SerializeField] private float tempsEntreVagues = 5f;
    [SerializeField] private float delaiEntreEnnemis = 0.5f;
    [SerializeField] private int maxEnnemis = 15;
    
    [Header("Zone de spawn")]
    [SerializeField] private float rayonSpawn = 10f;
    [SerializeField] private float distanceMinJoueur = 8f;
    
    private Transform player;
    private float prochainSpawn;
    private bool vagueEnCours = false;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        prochainSpawn = Time.time + tempsEntreVagues;
        
    }
    
    void Update()
    {
        if (Time.time >= prochainSpawn && !vagueEnCours && GetNombreEnnemis() < maxEnnemis)
        {
            StartCoroutine(SpawnVague());
            prochainSpawn = Time.time + tempsEntreVagues;
        }
    }
    
    System.Collections.IEnumerator SpawnVague()
    {
        vagueEnCours = true;
        
        int ennemisActuels = GetNombreEnnemis();
        int ennemisASpawner = Mathf.Min(ennemisParVague, maxEnnemis - ennemisActuels);
        
        
        for (int i = 0; i < ennemisASpawner; i++)
        {
            SpawnEnnemi();
            yield return new WaitForSeconds(delaiEntreEnnemis);
        }
        
        vagueEnCours = false;
    }
    
    void SpawnEnnemi()
    {
        Vector3 positionSpawn = GetPositionSpawnAleatoire();
        GameObject nouvelEnnemi = Instantiate(enemyPrefab, positionSpawn, Quaternion.identity);
        
    }
    
    Vector3 GetPositionSpawnAleatoire()
    {
        Vector3 position;
        int tentatives = 0;
        const int maxTentatives = 50;
        
        do
        {
            float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
            float distance = Random.Range(distanceMinJoueur, rayonSpawn);
            
            position = transform.position + new Vector3(
                Mathf.Cos(angle) * distance,
                Mathf.Sin(angle) * distance,
                0
            );
            
            tentatives++;
            
            if (tentatives >= maxTentatives)
            {
               return transform.position;
            }
            
        } while (Vector3.Distance(position, player.position) < distanceMinJoueur);
        
        return position;
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rayonSpawn);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distanceMinJoueur);
    }
    
    int GetNombreEnnemis()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
    
    public void DemarrerVagueImmediate()
    {
        if (!vagueEnCours && GetNombreEnnemis() < maxEnnemis)
        {
            StartCoroutine(SpawnVague());
        }
    }
}
