using UnityEngine;
using AForge.Fuzzy;
using UnityEngine.AI;

public class FuzzyBot : MonoBehaviour
{

    public Transform player;
    NavMeshAgent agent;

    float speed, distance;
    FuzzySet fsNear, fsMed, fsFar;
    FuzzySet fsSlow, fsMedium, fsFast;
    LinguisticVariable lvDistance, lvSpeed;

    Database database;
    InferenceSystem infSystem;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Initializate();
    }

    private void Initializate()
    {
        SetDistanceFuzzySets();
        SetSpeedFuzzySets();
        AddRulesTODataBase();
    }


    private void SetDistanceFuzzySets()
    {
        fsNear = new FuzzySet("Near", new TrapezoidalFunction(1, 3 , TrapezoidalFunction.EdgeType.Right));
        fsMed = new FuzzySet("Med", new TrapezoidalFunction(1, 3, 5, 7));
        fsFar = new FuzzySet("Far", new TrapezoidalFunction(5, 7, TrapezoidalFunction.EdgeType.Left));

        lvDistance = new LinguisticVariable("Distance", 0, 10);
        lvDistance.AddLabel(fsNear);
        lvDistance.AddLabel(fsMed);
        lvDistance.AddLabel(fsFar);
        
    }
    
    private void SetSpeedFuzzySets()
    {
        fsSlow = new FuzzySet("Slow", new TrapezoidalFunction(1, 2, TrapezoidalFunction.EdgeType.Right));
        fsMedium = new FuzzySet("Medium", new TrapezoidalFunction(1, 2, 3, 4));
        fsFast = new FuzzySet("Fast", new TrapezoidalFunction(4, 5, TrapezoidalFunction.EdgeType.Left));
        lvSpeed = new LinguisticVariable("Speed", 0, 5);

        lvSpeed.AddLabel(fsSlow);
        lvSpeed.AddLabel(fsMedium);
        lvSpeed.AddLabel(fsFast);
    }


    private void AddRulesTODataBase()
    {
        database = new Database();
        database.AddVariable(lvDistance);
        database.AddVariable(lvSpeed);
        SetRules();  
    }

    private void SetRules()
    {
        infSystem = new InferenceSystem(database, new CentroidDefuzzifier(120));
        infSystem.NewRule("Rule 1", "IF Distance IS Near THEN Speed IS Slow");
        infSystem.NewRule("Rule 2", "IF Distance IS Med THEN Speed IS Medium");
        infSystem.NewRule("Rule 3", "IF Distance IS Far THEN Speed IS Fast");
    }

    void Update()
    {
        Run();
    }

    private void Run()
    {
        float distance = (player.position - transform.position).magnitude;
        infSystem.SetInput("Distance", distance);
        speed = infSystem.Evaluate("Speed");
        agent.speed = speed; //hesaplanan hiz degeri navmeshagent a aktarılıyor
    }
}
