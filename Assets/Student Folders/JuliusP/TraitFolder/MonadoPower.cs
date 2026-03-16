using UnityEngine;
using System.Collections;



public class MonadoPower : Trait
{
    

    //BOOLS FOR COOLDOWNS//
    bool BusterOnCooldwn = false, SpeedOnCooldwn = false, ShieldOnCooldwn = false;


    float originalDamageMult = -1f;


    //TIMERS FOR EACH ART//

    float BusterTimer = 5f;

    float SpeedTimer = 5f;

    float ShieldTimer = 5f;

    //PARTICLE SYSTEMS FOR EACH STATE//
    private ParticleSystem BusterParticles, SpeedParticles, ShieldParticles;

   
    //BOOLS TO TRACK IF EACH STATE IS ACTIVE//
    bool BusterisActive, SpeedisActive, ShieldState;


    //ENUM TO TRACK STATES//

    public enum MonadoArts
    {
        Default,
        
        Speed,

        Buster,

        Shield, 



    }


    //SET ACTIVE STATE TO DEFAULT//
    MonadoArts ActiveStates = MonadoArts.Default;
   
  
    
    public MonadoPower()
    {
        
        Type = Traits.MonadoArts_JuliusP;


        AddListen(EventTypes.Update);

        AddPreListen(EventTypes.Damage);



    }


      public override void TakeEvent(TraitInfo i, EventInfo e)
      {
        
        switch (e.Type)
        {
            case EventTypes.Update:
             {

                // ENABLE A NEW ART TO BE CHOSEN, WHEN ACTIVATESTATES IS SET BACK TO DEFAULT//
                if (ActiveStates == MonadoArts.Default)
                {
                     //PRESSING J IS FOR SPEED//
                    if (Input.GetKeyDown(KeyCode.J) && !SpeedOnCooldwn)
                    {
                            //SPEED ART ACTIVATED//
                            ActiveStates = MonadoArts.Speed; 
                            God.C(Speed());
                    }
                  
                    //PRESSING K IS FOR BUSTER//
                   else if (Input.GetKeyDown(KeyCode.K) && !BusterOnCooldwn)
                    {
                            //BUSTER ART ACTIVATED//
                            ActiveStates = MonadoArts.Buster; 
                            God.C(BusterState());
                    }   
                    //PRESSING L IS FOR SHIELD//
                  else if (Input.GetKeyDown(KeyCode.L) && !ShieldOnCooldwn)
                    {
                            //SHIELD ART ACTIVATED//
                             ActiveStates = MonadoArts.Shield;
                             God.C(Shield());
                    }
    
                }


                 break;
            }
      
      
      
        }

    
    
     }

        //USE PREVENT FOR DAMAGE//
      public override void PreEvent(TraitInfo i, EventInfo e)
       {
        
        switch (e.Type)
        {
            case EventTypes.Damage:
            {
                    //WHILE BUSTER IS ACTIVE, DAMAGE RECIEVED IS INCREASED//
                    if (BusterisActive)
                    {
                        //PLAYER TAKES MORE DAMAGE WHILE BUSTER IS ACTIVE, ELSE NORMAL DAMAGE//
                        float dmg = e.GetFloat(NumInfo.Default);
                        e.SetFloat(NumInfo.Default, dmg * 4f);     

                    }

                    break;
            }


        }
        }


     //COROUTINE FOR SPEED STATE//

    IEnumerator Speed()
    {

        if (SpeedOnCooldwn)
        {
            
            Debug.Log("Speed on Cdw");
            yield break;


        }

        SpeedOnCooldwn = true;
        
        
        ThingInfo Player = God.Session.Player;

        // SAVE THE ORIGINAL SPEED//
        float originalSpeed = Player.CurrentSpeed;
      
        //INCREASE SPEED//
        Player.CurrentSpeed = 9f;                
        
        Debug.Log("Speed Art ON");


        // SPAWN PARTICLE AURA
        Transform ThingPos = Player.Thing.transform;
        GameObject SpeedAura = Resources.Load<GameObject>("JuliusP/Gnomes/SpeedAura");
        GameObject Thingaura = GameObject.Instantiate(SpeedAura, ThingPos.position, Quaternion.identity);
        Thingaura.transform.SetParent(ThingPos);
        Thingaura.transform.localPosition = Vector3.zero;


        SpeedParticles = Thingaura.GetComponent<ParticleSystem>();
        if (SpeedParticles != null)
        {
        
        SpeedParticles.Play();
                
            
        }
       

        yield return new WaitForSeconds(3f);      


        // STOPS THE PARTICLES IMMEDIATELY//
        if (SpeedParticles != null)
        {
        SpeedParticles.Stop();
        GameObject.Destroy(Thingaura, 0.5f); 
        SpeedParticles = null;
       
        }
       
        //SPEED BACK TO NORMAL//
        Player.CurrentSpeed = originalSpeed;      
        Debug.Log("Speed Art DEACTIVATED");

       
        // START COOLDOWN
        yield return new WaitForSeconds(SpeedTimer);

        SpeedTimer += 5;
       
        SpeedOnCooldwn = false;

        //SET STATE TO DEFAULT TO ALLOW THE PLAYER TO CHOOSE ANOTHER STATE OR THE SAME STATE//
        ActiveStates = MonadoArts.Default;



    }

 //COROUTINE FOR BUSTER STATE//

  IEnumerator BusterState()
 {

    if (BusterOnCooldwn)
    {
        Debug.Log("Buster not active yet");

        yield break;

    }

    BusterOnCooldwn = true;
    

    //GETS THINGINFO OF THE PLAYER//
    ThingInfo Player = God.Session.Player;

    //THIS GETS THE THINGINGO OF THE PLAYER'S CURRENT WEAPON//
    ThingInfo Weapon = Player.CurrentHeld;


    // SPAWN BUSTER AURA
    Transform ThingPos = Player.Thing.transform;
    GameObject BusterAura = Resources.Load<GameObject>("JuliusP/Gnomes/BusterAura");
    GameObject Thingaura = GameObject.Instantiate(BusterAura, ThingPos.position, Quaternion.identity);
    Thingaura.transform.SetParent(ThingPos);
    Thingaura.transform.localPosition = Vector3.zero;

    BusterParticles = Thingaura.GetComponent<ParticleSystem>();
    if (BusterParticles != null)
    {
            
    BusterParticles.Play();
    }

    //CHECKS IF PLAYER HAS A WEAPON//
    if (Weapon == null)
    {
       // Debug.Log("Player is not holding anything!");
        yield break;
    }

    //GETS THE TOOL TRAIT ON THE WEAPON//
    TraitInfo tool = Weapon.Get(Traits.Tool);
    
    //IF THE WEAPON OF THE PLAYER DOSEN'T HAVE A TOOL TRAIT EXIT//
    if (tool == null)
    {
       // Debug.Log("Held weapon has no ToolTrait!");
        yield break;
    }

    // THIS STORES THE ORIGINAL DAMAGE VALUE ON THE WEAPON//
    if (originalDamageMult < 0f)
    {
        originalDamageMult = tool.GetFloat();
       // Debug.Log($"Stored original damage multiplier: {originalDamageMult}");
    }

    // BUFF THE DAMAGE (DOUBLE DAMAGE)//
    tool.Set(originalDamageMult * 2f);
    Debug.Log($"Buster Art ON: {Weapon.Type.Name} damage multiplier set to {tool.GetFloat()}");

    BusterisActive = true;

    // BUSTER LASTS FOR 5 SECONDS//
    yield return new WaitForSeconds(4f);

    // REVERT THE DAMAGE BACK TO THE ORIGINAL VALUE//
    tool.Set(originalDamageMult);
    BusterisActive = false;
    Debug.Log($"Buster Art OFF: {Weapon.Type.Name} damage multiplier reverted to {tool.GetFloat()}");

    
    // MAKES PARTICLES DISAPPEEAR//
    if (BusterParticles != null)
    {
        BusterParticles.Stop();
        GameObject.Destroy(Thingaura, 0.5f);
        BusterParticles = null;
    }
    
   
    yield return new WaitForSeconds(BusterTimer);

    BusterTimer += 5;

    BusterOnCooldwn = false;


    //SET STATE TO DEFAULT TO ALLOW THE PLAYER TO CHOOSE ANOTHER STATE OR THE SAME STATE//
    ActiveStates = MonadoArts.Default;


   
    Debug.Log("Buster Art ready again!");

 }



//FUNCTION FOR SHIELD, SHIELD BEHAVES AS NO STUN//

IEnumerator Shield()
{


       if (ShieldOnCooldwn)
        {
            
            Debug.Log("Shield on Cdw");
            yield break;


        }

    
    ThingInfo Player = God.Session.Player;      

    // SPAWN SHIELD AURA
    Transform ThingPos = Player.Thing.transform;
    GameObject ShieldAura = Resources.Load<GameObject>("JuliusP/Gnomes/ShieldAura");
    GameObject Thingaura = GameObject.Instantiate(ShieldAura, ThingPos.position, Quaternion.identity);
    Thingaura.transform.SetParent(ThingPos);
    Thingaura.transform.localPosition = Vector3.zero;

    ShieldParticles = Thingaura.GetComponent<ParticleSystem>();
    if (ShieldParticles != null)
    {
            
     ShieldParticles.Play();
    }

    


        ShieldOnCooldwn = true;


        //SAVE ORIGINAL SPEED//
        float originalSpeed = Player.CurrentSpeed;


        //SET PLAYER SPEED TO BE SLOW, WHILE SHIELD IS ACTIVE//
        Player.CurrentSpeed = 1f;


        if(!Player.Has(Traits.StunNegation_JuliusP))
        {

        //MAKES THE PLAYER RESISTANT TO STUN EFFECTS//  
         Player.AddTrait(Traits.StunNegation_JuliusP);


        }


         yield return new WaitForSeconds(ShieldTimer);


         // MAKES PARTICLES DISAPPEEAR//
        if (ShieldParticles != null)
        {
        ShieldParticles.Stop();
        GameObject.Destroy(Thingaura, 0.5f);
        ShieldParticles = null;
        }
    

   
        //REMOVES STUN TRAIT//
         Player.RemoveTrait(Traits.StunNegation_JuliusP);

         ShieldTimer += 5;

         ShieldOnCooldwn = false;

         //SET SPEED BACK TO NORMAL//
         Player.CurrentSpeed = originalSpeed;

         //SET STATE TO DEFAULT TO ALLOW THE PLAYER TO CHOOSE ANOTHER STATE OR THE SAME STATE//
         ActiveStates = MonadoArts.Default;





}



}