using System;
using System.Collections.Generic;
using System.Text;

namespace Roulette
{
    public static class Table
    {
        static Random Rando = new Random();
        public static Tuple<int, string> SpinWheel()
        {
            int slot = Rando.Next(0,37);
            return Board[slot];
        }
        public static Tuple<int, string>[] Board =
        {
            
            Tuple.Create(1, "red"),
            Tuple.Create(2, "black"),
            Tuple.Create(3, "red"),
            Tuple.Create(4, "black"),
            Tuple.Create(5, "red"),
            Tuple.Create(6, "black"),
            Tuple.Create(7, "red"),
            Tuple.Create(8, "black"),
            Tuple.Create(9, "red"),
            Tuple.Create(10, "black"),
            Tuple.Create(11, "black"),
            Tuple.Create(12, "red"),
            Tuple.Create(13, "black"),
            Tuple.Create(14, "red"),
            Tuple.Create(15, "black"),
            Tuple.Create(16, "red"),
            Tuple.Create(17, "black"),
            Tuple.Create(18, "red"),
            Tuple.Create(19, "red"),
            Tuple.Create(20, "black"),
            Tuple.Create(21, "red"),
            Tuple.Create(22, "black"),
            Tuple.Create(23, "red"),
            Tuple.Create(24, "black"),
            Tuple.Create(25, "red"),
            Tuple.Create(26, "black"),
            Tuple.Create(27, "red"),
            Tuple.Create(28, "black"),
            Tuple.Create(29, "black"),
            Tuple.Create(30, "red"),
            Tuple.Create(31, "black"),
            Tuple.Create(32, "red"),
            Tuple.Create(33, "black"),
            Tuple.Create(34, "red"),
            Tuple.Create(35, "black"),
            Tuple.Create(36, "red"),
            Tuple.Create(0, "green"),
            Tuple.Create(00, "green")
        };
    }
}
