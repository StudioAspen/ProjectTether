using System.Collections.Generic;
using _Scripts.Runtime.Managers.Game_States;

namespace _Scripts.Runtime.Combat.States.ContextData
{
    /*
        We do not strictly need these 'context' classes, but I think it makes more sense
        to have the states not have a reference to the class managing them, for that allows states to do more
        than necessary. Thus, we pass data needed from the manager via context classes 
    */
    
    public class GameStateContext
    //probably don't need inputsystemactions
    {
        public Stack<GameState> PreviousStates { get; private set; }
        
        public GameStateContext( Stack<GameState> previousStates)
        {
           PreviousStates = previousStates; 
        } 
        
    }
}