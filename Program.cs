using System;
using static Roulette.Table;
using static Roulette.Bets;

namespace Roulette
{
    class Program
    {
        
        
        
        static void Main(string[] args)
        {
            bool go = true;
            Console.WriteLine("Welcome to Roulette!");
            while(go == true)
            {
                go = MakeBet(); 
            }            
        }
    }
}
