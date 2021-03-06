﻿using System;
using System.Linq;

namespace SavedCardAdminTool
{
    public class Program
    {
        private static AdminTool _adminTool;

        private static StripeAPI _stripeApi = new StripeAPI();

        public static void Main(string[] args)
        {
            // _stripeApi.GetList().GetAwaiter().GetResult();
            _stripeApi.CreateCard("4242424242424242", "12", "21", "424").GetAwaiter().GetResult();
            
            Console.WriteLine("||||||||||||||||||||||||||||||||||");
            _adminTool = new AdminTool();
            Console.WriteLine(">>> Welcome to the Moonpig save card admin tool! <<<");
            var dateTimeNow = DateTime.Now;
            var format = "MMM ddd d HH:mm";
            Console.WriteLine($"...:::    Today is  {dateTimeNow.ToString(format)}   :::...");
            
            var shouldExit = false;
            while (!shouldExit)
            {
                Console.WriteLine("-- Main Menu --");
                Console.WriteLine("0 - Exit");
                Console.WriteLine("1 - List of All Customer");
                Console.WriteLine("2 - Remove a Customer");
                Console.WriteLine("3 - Join");
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
                        CustomerRemove();
                        Console.WriteLine("=======================");
                        break;
                    case "3":
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

        private static void CustomerRemove()
        {
            try
            {
                Console.WriteLine("Please select the customer you want to remove from the following list:");
                ShowListOfAllCustomers();
                Console.WriteLine("Enter the customer FULL NAME you want to remove:");
                var customerName = Console.ReadLine();
                var match = CustomerExist(customerName);
            
                while (match == null)
                {
                    Console.WriteLine("Name does not match, try again");
                    customerName = Console.ReadLine();
                    match = CustomerExist(customerName);
                }

                RemoveCustomerFromList(customerName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static Customer CustomerExist(string customerName)
        {
            var  match = _adminTool.AllCustomers
                .FirstOrDefault(customer =>  customer.FullName.Contains(customerName));
            return match;
        }

        private static void RemoveCustomerFromList(string customerName)
        {
            var customer = _adminTool.AllCustomers.Single(x => x.FullName == customerName);
            _adminTool.AllCustomers.Remove(customer);
            Console.WriteLine($"{customerName} was successfully removed from the list");
        }
        

        private static void CustomerJoin()
        {
            Console.WriteLine("To join Moonpig AdminTool we need your details");
            Console.WriteLine("Enter your Fullname:");
            var fullname = Console.ReadLine();
            Console.WriteLine("Your Email Address:");
            var email = Console.ReadLine();
            var newCustomer = new Customer(fullname,email);
            _adminTool.AllCustomers.Add(newCustomer);
            Console.WriteLine("\n Welcome to SAVE CARD ADMIN TOOL ");
            Console.WriteLine($"{newCustomer.FullName} has joined at {newCustomer.JoinDate}");
            
            CustomerJoinedOptions(newCustomer);
        }

        private static void CustomerJoinedOptions(Customer customer)
        {
            Console.WriteLine(" ________---------__________-----------________\n");
            Console.WriteLine("Options: \n1- Add Card \n2- List of your Cards \n3- Remove Card \n4- Remove Expired Cards \n5- Return to the Main Menu ");
            var option = Console.ReadLine();
            if (option == null) throw new ArgumentException(message: "not valid");
            
            switch (option)
            {
                case "1":
                    Console.WriteLine("|==> Add Card ");
                    CustomerAddCard(customer);
                    break;
                case "2":
                    Console.WriteLine("|==> List of Card ");
                    CustomerCardsList(customer);
                    break;
                case "3":
                    Console.WriteLine("|==> Remove Card ");
                    RemoveCustomerCard(customer);
                    break;
                case "4":
                    Console.WriteLine("|==> Remove Expired Cards");
                    RemoveCustomerExpiredCards(customer);
                    break;
                case "5":
                    Console.WriteLine("<==| return ");
                    break;
                default:
                    Console.WriteLine("Unknown option.\n");
                    break;
            }
        }

        private static void RemoveCustomerExpiredCards(Customer customer)
        {
            customer.RemoveExpiredCards();
            CustomerCardsList(customer);
        }

        private static void RemoveCustomerCard(Customer customer)
        {
            try
            {
                if (customer.SaveCards.Count == 0)
                {
                    Console.WriteLine("You don't have any saved card yet");
                    CustomerJoinedOptions(customer);  
                }
            
                foreach (var card in customer.SaveCards)
                {
                    var index = customer.SaveCards.FindIndex(x => x.Equals(card)) + 1;
                    Console.WriteLine($"{index}) Name On Card : {card.NameOnCard} Last 4 digit : {card.LastFourDigit}");
                }
                Console.WriteLine("Please select the card you want to delete: ");
                var deleteCardIndex = Console.ReadLine();

                Int32.TryParse(deleteCardIndex, out var valid);
                valid = valid - 1;

                var cardToDelete = customer.SaveCards[valid];
                customer.RemoveCard(cardToDelete);
            
                Console.WriteLine("number of your saved card : {0}", customer.SaveCards.Count);
                CustomerJoinedOptions(customer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                CustomerJoinedOptions(customer); 
            }
           
        }

        private static void CustomerCardsList(Customer customer)
        {
            if (customer.SaveCards.Count == 0)
            {
                Console.WriteLine("You don't have any saved card yet");
                CustomerJoinedOptions(customer);  
            }
            customer.PrintAllSaveCards();
            
            foreach (var card in customer.SaveCards)
            {
                var index = customer.SaveCards.FindIndex(x => x.Equals(card)) + 1;
                Console.WriteLine($"{index}) Name On Card : {card.NameOnCard} Last 4 digit : {card.LastFourDigit}");
            }

            CustomerJoinedOptions(customer);
        }

        private static void CustomerAddCard(Customer customer)
        {
            var card =  CustomerCreateCard(); 
            customer.SaveCard(card);
            Console.WriteLine($"New Card added, last 4 digit {customer.SaveCards.Last().LastFourDigit} , Name on card : {card.NameOnCard}");
            CustomerJoinedOptions(customer);
        }

        private static Card CustomerCreateCard()
        {
            Console.WriteLine("+ Add New Card +");
            Console.WriteLine("Please enter your card details:\nCARD NUMBER: (16 DIGIT)");
            var cardNumber = Console.ReadLine();
            
            while (cardNumber != null && (!Int64.TryParse(cardNumber, out long valid) || cardNumber.Length < 4 || cardNumber.Length !=16))
            {
                Console.WriteLine("INCORRECT CARD NUMBER\tTry again:");
               cardNumber = Console.ReadLine();
            }
            
            // var lastFourDigit = LastFourDigit(cardNumber);
            
            Console.WriteLine("NAME ON CARD:");
            var namOnCard = Console.ReadLine();
            
            Console.WriteLine("EXPIRY DATE: MONTH(MM)");
            var expDateMonth = Console.ReadLine();
            var monthInt = Int32.TryParse(expDateMonth, out int month);
            if (month >= 13)
            {
                Console.WriteLine("INCORRECT DATE (01-12)\tTry again:");
                expDateMonth = Console.ReadLine();
                monthInt = Int32.TryParse(expDateMonth, out int expMonth);
                month = expMonth;
            }
            while (expDateMonth != null && (!monthInt || expDateMonth.Length < 2  ))
            {
                Console.WriteLine("INCORRECT DATE FORMAT\tTry again:");
                expDateMonth = Console.ReadLine();
            }
           
            Console.WriteLine("EXPIRY DATE: YEAR(YY)");
            var expDateYear = Console.ReadLine();

            var yearInt = Int32.TryParse("20" + expDateYear, out int year);
            
            // var cardExpiryDate = new DateTime(year, month,DateTime.DaysInMonth(year, month));
            //
            // while (cardExpiryDate < DateTime.Now.Date)
            // { 
            //     Console.WriteLine("INCORRECT DATE (expired card cannot be added)\tTry again:");
            //     expDateYear = Console.ReadLine();
            //     yearInt =  Int32.TryParse("20" + expDateYear, out int expYear);
            //     cardExpiryDate = new DateTime(expYear, month,DateTime.DaysInMonth(expYear, month));
            // }

            while (!IsValidDate(expDateMonth,expDateYear))
            {
                Console.WriteLine("INCORRECT DATE (expired card cannot be added)\tTry again:");
                expDateYear = Console.ReadLine();
                IsValidDate(expDateMonth, expDateYear);
            }
            
            while (expDateYear != null && (!yearInt || expDateYear.Length < 2 ) )
            {
                Console.WriteLine("INCORRECT DATE FORMAT\tTry again:");
                expDateYear = Console.ReadLine();
            }

            Console.WriteLine("##################");
            
            var stripePayment = _stripeApi.CreateCard(cardNumber, expDateMonth, expDateYear, "424").GetAwaiter().GetResult();
            var stripeCard = new MyCard(stripePayment.Id);
            Console.WriteLine($"stripe id:{stripeCard.CardId}");
            var testCardGet = _stripeApi.GetCard(stripeCard.CardId);
            Console.WriteLine($"last four digit:{testCardGet.Card.LastFour}");
            Console.WriteLine($"card type: {testCardGet.Card.Brand}");
            
            return new Card(stripePayment.Card.Brand, stripePayment.Card.LastFour, $"{stripePayment.Card.ExpMonth}/{expDateYear}", namOnCard,stripePayment.Id);
        }

        private static string LastFourDigit(string cardNumber)
        {
            var fourDigit = cardNumber.Substring(cardNumber.Length - 4);
            return fourDigit;
        }

        private static void ShowListOfAllCustomers()
        {
            foreach (var customer in _adminTool.AllCustomers)
            {
                var index = _adminTool.AllCustomers.FindIndex(x => x.Equals(customer)) + 1;
                Console.WriteLine("{0}) {1} , Join Date: {2}",index, customer.FullName, customer.JoinDate);
            }
        }

        private static bool IsValidDate(string expMonth, string expYear )
        {
            var expiryMonth = Int32.Parse(expMonth);
            var expiryYear = Int32.Parse("20" + expYear);
            // DateTime.TryParse(expMonth, )
            var cardExpiryDate = new DateTime(expiryYear, expiryMonth,DateTime.DaysInMonth(expiryYear,expiryMonth));
            return cardExpiryDate >= DateTime.Now.Date;
        }
    }
}
