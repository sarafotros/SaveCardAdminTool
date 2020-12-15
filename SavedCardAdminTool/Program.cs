using System;
using System.Globalization;
using System.Linq;

namespace SavedCardAdminTool
{
    public class Program
    {
        private static AdminTool _adminTool;

        public static void Main(string[] args)
        {
            _adminTool = new AdminTool();
            Console.WriteLine(">>> Welcome to the Moonpig save card admin tool! <<<");
            var dateTimeNow = DateTime.Now;
            var format = "MMM ddd d HH:mm";
            Console.WriteLine($"  ...:::     Today is  {dateTimeNow.ToString(format)}    :::...");
            
            var shouldExit = false;
            while (!shouldExit)
            {
                Console.WriteLine("-- Main Menu --");
                Console.WriteLine("0 - Exit");
                Console.WriteLine("1 - List of All Customer");
                Console.WriteLine("2 - Join");
                Console.WriteLine("---------------");
                Console.Write("Please pick an option: ");    

                var key = Console.ReadLine();
                switch (key)
                {
                    case "0":
                        shouldExit = true;
                        break;
                    case "1":
                        Console.WriteLine("List of all customers:");
                        ShowListOfAllCustomers();
                        Console.WriteLine("=======================");
                        break;
                    case "2":
                        Console.WriteLine("Join");
                        CustomerJoin();
                        Console.WriteLine("=======================");
                        break;
                    default:
                        Console.WriteLine("Unknown option.\n");
                        continue;
                }
            }

            Console.WriteLine(">>> Thank you for using the Moonpig save card admin tool. Goodbye! <<<");
        }

        private static void CustomerJoin()
        {
            Console.WriteLine("To join Moonpig AdminTool we need your details");
            Console.WriteLine("Enter your Fullname:");
            var fullname = Console.ReadLine();
            Console.WriteLine("Your Email Address:");
            var email = Console.ReadLine();
            var id = DateTime.Now.Year.ToString() + "3e3" ;
            var newCustomer = new Customer(fullname,email,id);
            _adminTool.AllCustomers.Add(newCustomer);
            Console.WriteLine("\n Welcome to SAVE CARD ADMIN TOOL ");
            Console.WriteLine($"{newCustomer.FullName} has joined at {newCustomer.JoinDate}");
            Console.WriteLine(" ________---------__________-----------________\n");
            Console.WriteLine("Options: \n1- Add Card \n2- List of your Cards \n3- Remove Card \n4- Return to the Main Menu ");
            CustomerJoinedOptions(newCustomer);
        }

        private static void CustomerJoinedOptions(Customer customer)
        {
            var option = Console.ReadLine();
            switch (option)
            {
                case "1":
                    Console.WriteLine("|==> Add Card ");
                    CustomerAddCard(customer);
                    break;
                case "2":
                    Console.WriteLine("|==> List of Card ");
                    break;
                case "3":
                    Console.WriteLine("|==> Remove Card ");
                    break;
                case "4":
                    Console.WriteLine("<==| return ");
                    break;
                default:
                    Console.WriteLine("Unknown option.\n");
                    break;
            }
        }

        private static void CustomerAddCard(Customer customer)
        {
            CustomerCreateCard(customer);
            var card = new Card("Visa", "2313", "12/24", customer.JoinDate.ToString(), customer.FullName);
           customer.SaveCard(card);
           Console.WriteLine($"New Card added, last 4 digit {customer.SaveCards.Last().LastFourDigit}");
        }

        private static Card CustomerCreateCard(Customer customer)
        {
            Console.WriteLine("+ Add New Card +");
            Console.WriteLine("Please enter your card details:\nCARD NUMBER:");
            var cardNumber = Console.ReadLine();
            
            while (!Int32.TryParse(cardNumber, out int valid))
            {
                Console.WriteLine("INCORRECT CARD NUMBER\tTry again:");
               cardNumber = Console.ReadLine();
            }
            
          
            var lastFourDigit = LastFourDigit(cardNumber);
            Console.WriteLine("NAME ON CARD:");
            var namOnCard = Console.ReadLine();
            Console.WriteLine("EXPIRY DATE (MM/YY)");
            var expDate = Console.ReadLine();
            Console.WriteLine("CARD TYPE:   -VISA -MASTER -AMEX  -DEBIT");
            var cardType = Console.ReadLine();
           
            
            return new Card("Visa", "2313", "12/24", customer.JoinDate.ToString(), customer.FullName);
        }

        private static string LastFourDigit(string cardNumber)
        {
            var fourDigit = cardNumber.Substring(cardNumber.Length - 4);
            Console.WriteLine(fourDigit);
            return fourDigit;
        }

        private static void ShowListOfAllCustomers()
        {
            foreach (var customer in _adminTool.AllCustomers)
            {
                Console.WriteLine("- {0},Join Date: {1}", customer.FullName, customer.JoinDate);
            }
        }
    }
}
