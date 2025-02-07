using UnityEngine;

public class CachedZombieData : MonoBehaviour
{
    private AutoAttack _autoAttack;
    private NPCMovement _npcMovement;
    private Health _health;
    private DetectInteractable _detectNecromanceZombies;
    private Animator _animator;
    private MeshRenderer _meshRenderer;
    private NecromanceHorde _necromanceHorde;
    private ZombiePlayerHordeRegistry _zombiePlayerHordeRegistry;

    //I use Lazy Initialization to have both performance - this allows the script to get components only when they are needed, reducing the risk of frame drops
    public AutoAttack AutoAttack => _autoAttack ??= GetComponent<AutoAttack>();
    public NPCMovement NPCMovement => _npcMovement ??= GetComponent<NPCMovement>();
    public Health Health => _health ??= GetComponent<Health>();
    public Animator Animator => _animator ??= GetComponentInChildren<Animator>();
    public MeshRenderer MeshRenderer => _meshRenderer ??= GetComponentInChildren<MeshRenderer>();

    //Make zombiePlayerHordeRegistry as traditional getter/setter to pass a reference inside NecromanceHorde to an NPC
    public ZombiePlayerHordeRegistry ZombiePlayerHordeRegistry
    {
        get => _zombiePlayerHordeRegistry ??= GetComponentInParent<ZombiePlayerHordeRegistry>();
        set => _zombiePlayerHordeRegistry = value;
    }
    
    //Make NecromanceHorde as traditional getter/setter to pass a reference inside NecromanceHorde to an NPC
    public NecromanceHorde NecromanceHorde
    {
        get => _necromanceHorde ??= GetComponentInParent<NecromanceHorde>();
        set => _necromanceHorde = value;
    }
}
