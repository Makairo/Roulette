using System;
using System.Collections.Generic;
using System.Text;
using static Roulette.Table;

namespace Roulette
{
    class Bets
    {
        //Determines what type of bet the user is going to play.
        public static string GetUserBet()
        {
            string output = "";
            Console.WriteLine("Please enter the type of bet you would like to make. Type \"HELP\" for assistance.");
            output = Console.ReadLine();

            //Checks for EXIT and HELP cases specifically.
            //Then checks length
            if (output.ToUpper() == "EXIT")
            {
                return "EXIT";
            }
            if (output.ToUpper() == "HELP")
            {
                BetsHelp();
                return GetUserBet();
            }
            if (output.Length > 2 || output.Length == 0)
            {
                return GetUserBet();
            }
            return output.ToUpper();
        }

        //Grabs the bin the user is selecting for the bet.
        public static int GetUserBinNumber()
        {
            int output = 0;
            Console.WriteLine("Please enter the number of the bin:");
            output = Int32.Parse(Console.ReadLine());

            if (output > 36 || output < 0)
            {
                Console.WriteLine("Not a valid bin number.");
                return GetUserBinNumber();
            }
            return output;
        }

        //Grabs a number from the user.
        public static int GetUserInt(string value)
        {
            int output = 0;
            Console.WriteLine($"Please enter your {value} number:");
            try
            {
                output = Int32.Parse(Console.ReadLine());
            }
            catch
            {
                return GetUserInt(value);
            }
            
            return output;
        }

        //Auto Assigns the color to the selection based on the number.
        public static string GetUserBinColor(int number)
        {
            string output = "";
            foreach (Tuple<int, string> item in Table.Board)
            {
                if(number == item.Item1)
                {
                    output = item.Item2;
                    break;
                }
            }
            return output;
        }

        //Handles retrieving the number and color for the user selected bin.
        public static Tuple<int, string> GetUserBin()
        {
            Tuple<int, string> output;
            int part1 = GetUserBinNumber();
            string part2 = GetUserBinColor(part1);
            output = Tuple.Create(part1,part2);
            Console.WriteLine($"You chose bin {output.Item1} {output.Item2}!");
            return output;
        }

        //Prints all possible bets on the menu.
        public static void BetsHelp()
        {
            Console.WriteLine("Types of bets are as follows: ");
            Console.WriteLine("Numbers: the number of the bin - Enter 'N'.");
            Console.WriteLine("Evens/Odds: even or odd numbers - Enter 'OE'.");
            Console.WriteLine("Reds/Blacks: red or black colored numbers - Enter 'RB'.");
            Console.WriteLine("Lows/Highs: low (1 – 18) or high (19 – 38) numbers. - Enter 'LH'.");
            Console.WriteLine("Dozens: row thirds, 1 – 12, 13 – 24, 25 – 36 - Enter 'D'.");
            Console.WriteLine("Columns: first, second, or third columns - Enter 'CO'.");
            Console.WriteLine("Street: rows, e.g., 1/2/3 or 22/23/24 - Enter 'ST'.");
            Console.WriteLine("6 Numbers: double rows, e.g., 1/2/3/4/5/6 or 22/23/24/25/26/26 - Enter '6N'.");
            Console.WriteLine("Split: at the edge of any two contiguous numbers, e.g., 1/2, 11/14, and 35/36 - Enter 'SP'.");
            Console.WriteLine("Corner: at the intersection of any four contiguous numbers, e.g., 1/2/4/5, or 23/24/26/27 - Enter 'CR'.");
            Console.WriteLine("Exit: Exits the application. - Enter 'EXIT'");
        }

        //Determines the bet that the user selects, send them to that method. Returns true / false to keep betting or exit.
        public static bool MakeBet()
        {
            string bet = GetUserBet();
            bool won = true;
            bool keepGoing = true;
            
            // Infinite loop for 2nd level input validation.
            // Will continue to here again if "default" case is hit.
            for (; ; )
            {
                switch (bet)
                {
                    case "N":
                        won = NumbersBet();
                        break;
                    case "OE":
                        won = OddsEvensBet();
                        break;
                    case "RB":
                        won = RedBlackBet();
                        break;
                    case "LH":
                        won = LowHighBet();
                        break;
                    case "D":
                        won = DozensBet();
                        break;
                    case "CO":
                        won = ColumnsBet();
                        break;
                    case "ST":
                        won = StreetBet();
                        break;
                    case "6N":
                        won = _6NumbersBet();
                        break;
                    case "SP":
                        won = SplitBet();
                        break;
                    case "CR":
                        won = CornerBet();
                        break;
                    case "EXIT":
                        keepGoing = false;
                        break;
                    default:
                        Console.WriteLine("Your bet was not valid, please try again.");
                        bet = GetUserBet();
                        continue;
                }
                Console.WriteLine($"You {(won == true ? "Won!" : "Lose!")}");
                return keepGoing;
            }
        }

        //Numbers Bet
        public static bool NumbersBet()
        {
            Tuple<int, string> UserBin = GetUserBin();
            Tuple<int, string> Wheel = SpinWheel();
            Console.WriteLine($"The wheel landed on {Wheel.Item1} {Wheel.Item2}.\nThe winning bet is {Wheel.Item1} {Wheel.Item2}.");
            if (UserBin.Item1 == Wheel.Item1)
            {
                return true;
            }
            return false;
        }

        //Odds / Even Bet
        public static bool OddsEvensBet()
        {
            string ooe = "";
            int mod = 0;
            int bet = 0;
            while (ooe.ToUpper() != "O" && ooe.ToUpper() != "E") 
            {
                Console.WriteLine("Odd or Even? ('O' or 'E')");
                ooe = Console.ReadLine().ToUpper();
            }
            if (ooe == "O") bet = 1;
            Tuple<int, string> Wheel = SpinWheel();
            mod = Wheel.Item1 % 2;
            Console.WriteLine($"The wheel landed on {Wheel.Item1} {Wheel.Item2}.");
            Console.Write($"The winning bets are {(mod == 1 ? "odds" : "evens")}: ");
            foreach (Tuple<int, string> item in Table.Board)
            {
                if (item.Item1 % 2 == mod && item.Item1 != 0)
                {
                    Console.Write(item.Item1 + " ");
                }
            }
            Console.Write("\n");
            if (bet == mod) return true; 
            return false;
        }

        //Color Bet
        public static bool RedBlackBet()
        {
            string rob = "";
            string bet = "";
            while (rob != "R" && rob != "B")
            {
                Console.WriteLine("Red or Black? ('R' or 'B')");
                rob = Console.ReadLine().ToUpper();
            }
            if (rob == "B") bet = "black";
            if (rob == "R") bet = "red";
            Tuple<int, string> Wheel = SpinWheel();
            Console.WriteLine($"The wheel landed on {Wheel.Item1} {Wheel.Item2}.");
            Console.Write($"The winning bets are {Wheel.Item2}: ");
            foreach (Tuple<int, string> item in Table.Board)
            {
                if (Wheel.Item2 == item.Item2)
                {
                    Console.Write(item.Item1 + " ");
                }
            }
            Console.Write("\n");
            if (bet == Wheel.Item2) return true;
            return false;
        }
        
        //Low or High Bet
        public static bool LowHighBet()
        {
            string rob = "";
            bool userBet = false;
            bool high = false;
            while (rob.ToUpper() != "L" && rob.ToUpper() != "H")
            {
                Console.WriteLine("Low or High? ('L' or 'H')");
                rob = Console.ReadLine();
            }
            if (rob == "H") userBet = true;
            Tuple<int, string> Wheel = SpinWheel();
            Console.WriteLine($"The wheel landed on {Wheel.Item1} {Wheel.Item2}.");
            Console.Write($"The winning bets are {(Wheel.Item1 >= 19 ? "Highs" : "Lows")}: ");
            if (Wheel.Item1 >= 19) high = true;
            foreach (Tuple<int, string> item in Table.Board)
            {
                if (high == true && item.Item1 >= 19)
                {
                    Console.Write(item.Item1 + " ");
                }
                if (high == false && item.Item1 <= 18 && item.Item1 != 0)
                {
                    Console.Write(item.Item1 + " ");
                }
            }
            Console.Write("\n");
            if (userBet == high) return true;
            return false;
        }

        //Dozens Bet
        public static bool DozensBet()
        {
            bool won = false;
            Console.WriteLine("Choose one bin inside one of the three ranges. Your bin will count for the entire range. (1-12, 13-24, 25-36)");
            Tuple<int, string> UserBin = GetUserBin();
            Tuple<int, string> Wheel = SpinWheel();
            Console.WriteLine($"The wheel landed on {Wheel.Item1} {Wheel.Item2}.\nThe winning bets are:");
            if (Wheel.Item1 > 0 && Wheel.Item1 <= 12)
            {
                foreach (Tuple<int, string> item in Table.Board)
                {
                    if (item.Item1 > 0 && item.Item1 <= 12)
                    {
                        Console.Write(item.Item1 + " ");
                    }

                }
                if (UserBin.Item1 > 0 && UserBin.Item1 <= 12) won = true;
            }
            if (Wheel.Item1 > 12 && Wheel.Item1 <= 24)
            {
                foreach (Tuple<int, string> item in Table.Board)
                {
                    if (item.Item1 > 12 && item.Item1 <= 24)
                    {
                        Console.Write(item.Item1 + " ");
                    }

                }
                if (UserBin.Item1 > 12 && UserBin.Item1 <= 24) won = true;
            }
            if (Wheel.Item1 > 24 && Wheel.Item1 <= 36)
            {
                foreach (Tuple<int, string> item in Table.Board)
                {
                    if (item.Item1 > 24 && item.Item1 <= 36)
                    {
                        Console.Write(item.Item1 + " ");
                    }

                }
                if (UserBin.Item1 > 24 && UserBin.Item1 <= 36) won = true;
            }
            Console.Write("\n");
            return won;
        }

        //Columns Bet
        public static bool ColumnsBet()
        {
            int column = 0;
            int resultCol = 0;
            
            Console.WriteLine("Choose a column: 1, 2 or 3");
            while (column != 1 && column != 2 && column != 3)
            {
                column = GetUserInt("column");
            }

            Tuple<int, string> Wheel = SpinWheel();
            int temp = Wheel.Item1 % 3;
            if (temp == 1) resultCol = 1;
            if (temp == 2) resultCol = 2;
            if (temp == 0) resultCol = 3;
            Console.WriteLine($"The wheel landed on {Wheel.Item1} {Wheel.Item2}.\nThe winning bets are Column {resultCol}:");
            foreach (Tuple<int, string> item in Table.Board)
            {
                if (item.Item1 % 3 == temp && item.Item1 != 0)
                {
                    Console.Write(item.Item1 + " ");
                }

            }
            Console.Write("\n");
            if (column == resultCol) return true;
            return false;
        }

        //Street Bet
        public static bool StreetBet()
        {
            int row = 0;
            int resultRow = 0;

            Console.WriteLine("Choose a row: 1 - 12");
            while (row < 1 || row > 12)
            {
                row = GetUserInt("row");
            }
            Tuple<int, string> Wheel = SpinWheel();
            resultRow = (Wheel.Item1 / 3) + 1;
            Console.WriteLine($"The wheel landed on {Wheel.Item1} {Wheel.Item2}.\nThe winning bets are Row {resultRow}:");
            foreach (Tuple<int, string> item in Table.Board)
            {
                if (((item.Item1 - 1) / 3) + 1 == resultRow)
                {
                    Console.Write(item.Item1 + " ");
                }
            }
            Console.Write("\n");
            if (row == resultRow) return true;
            return false;
        }

        //6 Number Bet
        public static bool _6NumbersBet()
        {
            int rowA = 0;
            int rowB = 0;
            int resultRow = 0;

            Console.WriteLine("Choose two contiguous rows: ");
            while (rowA < 1 || rowA > 12 || rowB < 1 || rowB > 12)
            {
                while (Math.Abs(rowA - rowB) != 1)
                {
                    Console.WriteLine("Choose your first row: 1 - 12");
                    rowA = GetUserInt("row");
                    Console.WriteLine("Choose your second row: 1 - 12");
                    rowB = GetUserInt("row");
                }
            }
            Tuple<int, string> Wheel = SpinWheel();
            resultRow = (Wheel.Item1 / 3) + 1;
            Console.WriteLine($"The wheel landed on {Wheel.Item1} {Wheel.Item2}.\nThe winning bets are Rows {resultRow} and ajacient rows:");
            foreach (Tuple<int, string> item in Table.Board)
            {
                if (((item.Item1 - 1) / 3) + 1 == resultRow || ((item.Item1 - 1) / 3) + 1 == resultRow + 1 || ((item.Item1 - 1) / 3) + 1 == resultRow - 1)
                {
                    if(item.Item1 != 0)Console.Write(item.Item1 + " ");
                }
            }
            Console.Write("\n");

            if (resultRow == rowA || resultRow == rowB) return true;
            return false;
        }

        //Returns a boolean wether or not the two bins as input are next to one another. For SplitBet();
        public static bool AreBinsContiguous(Tuple<int, string> binA, Tuple<int, string> binB)
        {
            // Grabs the num values from the bins
            int num1 = binA.Item1;
            int num2 = binB.Item1;

            // Mod, finds what column the first bin is in.
            int mod = num1 % 3;
            // Ensures search doesnt go out of bounds.
            int maxSearchVal = Board.Length - 1;


            // Nested if statements are to ensure the indexes are inbounds before checks.

            if (mod == 0) // Right column
            {
                if (num1 + 3 - 1 <= maxSearchVal) 
                {
                    if (num2 == Board[num1 + 3 - 1].Item1)
                    {
                        return true;
                    }
                }
                if (num1 - 3 - 1 >= 0)
                {
                    if (num2 == Board[num1 - 3 - 1].Item1)
                    {
                        return true;
                    }
                }
                if (num1 - 1 - 1 >= 0)
                {
                    if (num2 == Board[num1 - 1 - 1].Item1)
                    {
                        return true;
                    }
                }

            }
            if (mod == 2) // Center column
            {
                if (num1 + 3 - 1 <= maxSearchVal)
                {
                    if (num2 == Board[num1 + 3 - 1].Item1)
                    {
                        return true;
                    }
                }
                if (num1 + 1 - 1 <= maxSearchVal)
                {
                    if (num2 == Board[num1 + 1 - 1].Item1)
                    {
                        return true;
                    }
                }
                if (num1 - 3 - 1 >= 0)
                {
                    if (num2 == Board[num1 - 3 - 1].Item1)
                    {
                        return true;
                    }
                }
                if (num1 - 1 - 1 >= 0)
                {
                    if (num2 == Board[num1 - 1 - 1].Item1)
                    {
                        return true;
                    }
                }
            }
            if (mod == 1) // Left column
            {
                if (num1 + 3 - 1 <= maxSearchVal)
                {
                    if (num2 == Board[num1 + 3 - 1].Item1)
                    {
                        return true;
                    }
                }
                if (num1 - 3 - 1 >= 0)
                {
                    if (num2 == Board[num1 - 3 - 1].Item1)
                    {
                        return true;
                    }
                }
                if (num1 + 1 - 1 <= maxSearchVal)
                {
                    if (num2 == Board[num1 + 1 - 1].Item1)
                    {
                        return true;
                    }
                } 
            }
            return false;
        }

        //Split Bet
        public static bool SplitBet()
        {
            // Bool for input values are next to one another
            bool contiguous = false;
            // Bool for result
            bool result = false;

            // Input tuples / bin
            Tuple<int, string> binA = Board[0];
            Tuple<int, string> binB = Board[0];

            Console.WriteLine("Please enter a split for your bet: (Two contiguous bins) ");
            binA = GetUserBin();
            binB = GetUserBin();
            //Checks if they are next to each other.
            contiguous = AreBinsContiguous(binA, binB);

            // While not next to one another:
            while (contiguous == false)
            {
                Console.WriteLine("Bins are not contiguous. Please enter a split for your bet: (Two contiguous bins) ");
                binA = GetUserBin();
                binB = GetUserBin();
                //Checks if they are next to each other.
                contiguous = AreBinsContiguous(binA, binB);
            }

            Console.WriteLine($"You chose Split {(binA.Item1 > binB.Item1 ? $"{binB.Item1} / {binA.Item1}" : $"{binA.Item1} / {binB.Item1}")}");

            Tuple<int, string> Wheel = SpinWheel();
            Console.WriteLine($"The wheel landed on {Wheel.Item1} {Wheel.Item2}.\nThe winning bets are Splits:");
            foreach (Tuple<int, string> item in Table.Board)
            {
                if(AreBinsContiguous(Wheel, item))
                {
                    Console.WriteLine($"Split {(Wheel.Item1 > item.Item1 ? $"{item.Item1} / {Wheel.Item1}" : $"{Wheel.Item1} / {item.Item1}")}");
                    if (Wheel.Item1 == binA.Item1 || Wheel.Item1 == binB.Item1)
                    {
                        if (item.Item1 == binA.Item1 ||item.Item1 == binB.Item1)
                        {
                            result = true;
                        }
                    }
                }
            }
            Console.Write("\n");
            return result;
        }

        //Returns a boolean wether or not the two bins are part of the same CORNER. Is smart in scanning, only meant for a 3 width grid like this.
        public static bool IsCorner(Tuple<int, string> binA, Tuple<int, string> binB)
        {
            // Grabs the num values from the bins
            int num1 = binA.Item1;
            int num2 = binB.Item1;

            // Mod, finds what column the first bin is in.
            int mod = num1 % 3;
            // Ensures search doesnt go out of bounds.
            int maxSearchVal = Board.Length - 1;


            // Nested if statements are to ensure the indexes are inbounds before checks.

            if (mod == 0) // Right column
            {
                if (num1 + 3 - 1 <= maxSearchVal)
                {
                    if (num2 == Board[num1 + 3 - 1].Item1)
                    {
                        return true;
                    }
                }
                if (num1 + 2 - 1 <= maxSearchVal)
                {
                    if (num2 == Board[num1 + 2 - 1].Item1)
                    {
                        return true;
                    }
                }
                if (num1 - 1 - 1 >= 0)
                {
                    if (num2 == Board[num1 - 1 - 1].Item1)
                    {
                        return true;
                    }
                }

            }
            if (mod == 2) // Center column
            {
                if (num1 + 4 - 1 <= maxSearchVal)
                {
                    if (num2 == Board[num1 + 4 - 1].Item1)
                    {
                        return true;
                    }
                }
                if (num1 + 3 - 1 <= maxSearchVal)
                {
                    if (num2 == Board[num1 + 3 - 1].Item1)
                    {
                        return true;
                    }
                }
                if (num1 + 2 - 1 <= maxSearchVal)
                {
                    if (num2 == Board[num1 + 2 - 1].Item1)
                    {
                        return true;
                    }
                }
                if (num1 + 1 - 1 <= maxSearchVal)
                {
                    if (num2 == Board[num1 + 1 - 1].Item1)
                    {
                        return true;
                    }
                }
                
            }
            if (mod == 1) // Left column
            {
                if (num1 + 4 - 1 <= maxSearchVal)
                {
                    if (num2 == Board[num1 + 4 - 1].Item1)
                    {
                        return true;
                    }
                }
                if (num1 + 3 - 1 <= maxSearchVal)
                {
                    if (num2 == Board[num1 + 3 - 1].Item1)
                    {
                        return true;
                    }
                }
                if (num1 + 1 - 1 <= maxSearchVal)
                {
                    if (num2 == Board[num1 + 1 - 1].Item1)
                    {
                        return true;
                    }
                } 
            }
            return false;
        }

        //Corner Bet
        public static bool CornerBet()
        {
            // Bool for input values are next to one another
            bool contiguousA = false;
            bool contiguousB = false;
            bool cornerBet = false;
            // Bool for result
            bool result = false;

            // Input tuples / bin
            Tuple<int, string> binA = Board[0];
            Tuple<int, string> binB = Board[0];
            Tuple<int, string> binC = Board[0];
            Tuple<int, string> binD = Board[0];

            // Grab initial bin. The corner bet will be based off this value.
            // User will select TOP LEFT value of the corner.
            binA = GetUserBin();


            //Make sure its a valid starting square, so column 1 or 2.
            while (binA.Item1 % 3 == 0)
            {
                Console.WriteLine($"Bin {binA.Item1} {binB.Item2} is not a valid starting square for your corner bet.");
                binA = GetUserBin();
            }


            // Values are stored in array 1 higher than index.. simple math instead of calling another method.
            // If I was to optimize for errors, this would go first.
            int sIndex = binA.Item1 - 1;

            //Based on binA, assign the other 4 corners for easy win check.
            binB = Board[sIndex + 1];
            binC = Board[sIndex + 3];
            binD = Board[sIndex + 4];

            // Show player their corner.

            Console.WriteLine($"You have selected Corner {binA.Item1} / {binB.Item1} / {binC.Item1} / {binD.Item1}.");


            // While not next to one another:
            /*while (cornerBet == false)
            {
                Console.WriteLine("Select two splits to form your corner bet.");
                while (contiguousA == false)
                {
                    Console.WriteLine("Please enter the first split for your bet: (Two contiguous bins) ");
                    binA = GetUserBin();
                    binB = GetUserBin();
                    //Checks if they are next to each other.
                    contiguousA = AreBinsContiguous(binA, binB);
                }
                while (contiguousB == false)
                {
                    Console.WriteLine("Please enter the second split for your bet: (Two contiguous bins) ");
                    binC = GetUserBin();
                    binD = GetUserBin();
                    //Checks if they are next to each other.
                    contiguousB = AreBinsContiguous(binC, binD);
                }
                // Checks if first set is next to second set.
                if(AreBinsContiguous(binA, binC) == true && AreBinsContiguous(binB, binD) == true)
                {
                    cornerBet = true;
                }
            
            }
            */
            Tuple<int, string> Wheel = SpinWheel();
            Console.WriteLine($"The wheel landed on {Wheel.Item1} {Wheel.Item2}.\nThe winning bets are Corners:");
            for (int i = 0; i < Board.Length - 5;)
            {
                if (Board[i].Item1 == Wheel.Item1 || IsCorner(Board[i], Wheel))
                {
                    Console.WriteLine($"Corner {Board[i].Item1} / {Board[i+1].Item1} / {Board[i+3].Item1} / {Board[i+4].Item1}");
                }
                if (Board[i + 2].Item1 == Wheel.Item1 || IsCorner(Board[i + 2], Wheel))
                {
                    Console.WriteLine($"Corner {Board[i + 1].Item1} / {Board[i + 2].Item1} / {Board[i + 4].Item1} / {Board[i + 5].Item1}");

                }
                i += 3;
            }

            //If the ball lands on any of the squares you chose, you win.
            if (Wheel.Item1 == binA.Item1 || Wheel.Item1 == binB.Item1 || Wheel.Item1 == binC.Item1 || Wheel.Item1 == binD.Item1) result = true;

            return result;
        }
    }
}