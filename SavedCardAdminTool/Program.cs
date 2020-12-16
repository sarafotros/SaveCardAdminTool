﻿using System;
using System.Diagnostics;
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
                    CustomerCardsList(customer);
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

        private static void CustomerCardsList(Customer customer)
        {
            if(customer.SaveCards.Count == 0)
                Console.WriteLine("You don't have any saved card yet");
            CustomerJoinedOptions(customer);
            
            foreach (var card in customer.SaveCards)
            {
                Console.WriteLine($"{customer.SaveCards.Count+1}) Name On Card : {card.NameOnCard} Last 4 digit : {card.LastFourDigit}");
            }
        }

        private static void CustomerAddCard(Customer customer)
        {
            var card =  CustomerCreateCard(); 
            customer.SaveCard(card);
            Console.WriteLine($"New Card added, last 4 digit {customer.SaveCards.Last().LastFourDigit} , Name on card : {card.NameOnCard}");
        }

        private static Card CustomerCreateCard()
        {
            Console.WriteLine("+ Add New Card +");
            Console.WriteLine("Please enter your card details:\nCARD NUMBER:");
            var cardNumber = Console.ReadLine();
            
            while (!Int64.TryParse(cardNumber, out long valid) || cardNumber.Length <4)
            {
                Console.WriteLine("INCORRECT CARD NUMBER\tTry again:");
               cardNumber = Console.ReadLine();
            }
            
            var lastFourDigit = LastFourDigit(cardNumber);
            
            Console.WriteLine("NAME ON CARD:");
            var namOnCard = Console.ReadLine();
            
            Console.WriteLine("EXPIRY DATE: MONTH(MM)");
            var expDateMonth = Console.ReadLine();
            while (!Int32.TryParse(expDateMonth, out int valid) || expDateMonth.Length < 2)
            {
                Console.WriteLine("INCORRECT DATE FORMAT\tTry again:");
                expDateMonth = Console.ReadLine();
            }
            
            Console.WriteLine("EXPIRY DATE: YEAR(YY)");
            var expDateYear = Console.ReadLine();
            while (!Int32.TryParse(expDateYear, out int valid) || expDateYear.Length < 2 )
            {
                Console.WriteLine("INCORRECT DATE FORMAT\tTry again:");
                expDateYear = Console.ReadLine();
            }
            
            Console.WriteLine("CARD TYPE:   -VISA -MASTER -AMEX  -DEBIT");
            var cardType = Console.ReadLine();

            
            return new Card(cardType, lastFourDigit, $"{expDateMonth}/{expDateYear}", namOnCard);
        }

        private static string LastFourDigit(string cardNumber)
        {
            if (cardNumber.Length < 4)
            {
                Console.WriteLine("");
            }
            var fourDigit = cardNumber.Substring(cardNumber.Length - 4);
            Console.WriteLine(fourDigit);
            return fourDigit;
        }

        private static void ShowListOfAllCustomers()
        {
            foreach (var customer in _adminTool.AllCustomers)
            {
                Console.WriteLine("- {0} , Join Date: {1}", customer.FullName, customer.JoinDate);
            }
        }
    }
}
