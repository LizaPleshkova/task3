using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace task3
{
    class Program
    {

        static string GetHMAC(string step, string key)
        {
            byte[] bkey = Encoding.Default.GetBytes(key);
            using (var hmac = new HMACSHA256(bkey))
            {
                byte[] bstr = Encoding.Default.GetBytes(step);
                var bhash = hmac.ComputeHash(bstr);
                return BitConverter.ToString(bhash).Replace("-", string.Empty).ToLower();
            }
        }
        public string[] GetPrev(string[] arr)
        {
            int n = arr.Length, j = 0;
            string[] prev = new string[n / 2];

            for (int i = 0; i < n; i++)
            {
                if (i > n / 2)
                {
                    prev[j] = arr[i];
                    j++;
                }
            }
            return prev;
        }
        public string[] GetNext(string[] arr)
        {
            int n = arr.Length, j = 0;
            string[] next = new string[n / 2];

            for (int i = 0; i < n; i++)
            {
                if (i > 0 && i <= n / 2)
                {
                    next[j] = arr[i];
                    j++;
                }
            }
            return next;
        }

        public string[] GetCircleArr(string[] args, int moveComputer)
        {
            int n1 = args.Length;
            string[] tmp = new string[n1];

            for (int i = moveComputer, j = 0; i < n1 + moveComputer && j < n1; i++, j++)
            {
                tmp[j] = args[(i % n1)];
            }
            return tmp;
        }

        public string ChooseWinner(string[] args, int moveComputer, int moveUser)
        {
            string[] ciclyarr = new Program().GetCircleArr(args, moveComputer);
            string[] next = new Program().GetNext(ciclyarr);
            string[] prev = new Program().GetPrev(ciclyarr);

            if (next.Contains(args[moveUser - 1]))
            {
                return "You win!";
            }
            if (prev.Contains(args[moveUser - 1]))
            {
                return "Computer win!";
            }
            else
            {
                return "Draw in the game! Nobody won!";
            }
        }
        string GetSecretKey()
        {
            RNGCryptoServiceProvider rg = new RNGCryptoServiceProvider();
            byte[] values = new byte[16];

            rg.GetBytes(values);
            return Convert.ToBase64String(values);
        }

        static void Main(string[] args)
        {
            // проверки для args
            try
            {
                if (args.Length < 3) throw new Exception("Must be more than 3 parameters. \n For example: \n\t >task.exe rock paper scissors lizard Spock \n Please, try again");
                if (args.Length % 2 == 0) throw new Exception("Must be an odd number of arguments \n For example: \n\t >task.exe rock paper scissors lizard Spock \n Please, try again");
                if (args.Length != args.Distinct().Count()) throw new Exception("Your parameters contain duplicate values \n For example: \n\t >task.exe rock paper scissors lizard Spock \n Please, try again");
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
                return;
            }

            //step comp
            int n = args.Length, moveComputer, moveUser;
            string key, hmac;
            Random r = new Random();

            moveComputer = r.Next(0, args.Length - 1);

            key = new Program().GetSecretKey();

            hmac = GetHMAC(args[moveComputer], key);
            Console.WriteLine("\nHMAC:  " + hmac);

            // display menu options
            do
            {
                Console.WriteLine("\n\tMenu");
                for (int i = 0; i < args.Length; i++) Console.WriteLine("\t" + (i + 1) + " - " + args[i]);
                Console.WriteLine("\t0 - Exit");
                Console.WriteLine("\nEnter your move:");
                moveUser = int.Parse(Console.ReadLine());
                if (moveUser == 0) return;

            } while (!(moveUser > 0 && moveUser < args.Length + 1));

            Console.WriteLine("\nYour move:" + args[moveUser - 1]);
            Console.WriteLine("\nComputer move:" + args[moveComputer]);

            //winner\loser
            Console.WriteLine("\n" + new Program().ChooseWinner(args, moveComputer, moveUser));
            Console.WriteLine("\nHMAc key : " + key);

        }

    }
}

