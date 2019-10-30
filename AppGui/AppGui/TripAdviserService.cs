using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using mmisharp;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;


namespace AppGui
{
    class TripAdviserService
    {
        private String startUrl = "https://www.tripadvisor.pt/";
        private IWebDriver driver;
        private Tts t = new Tts();

       
        public TripAdviserService()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.tripadvisor.pt/");
            driver.Manage().Window.Maximize();
           
        }

        public void baseURL()
        {
            OpenUrl(startUrl);
        }

        public void OpenUrl(String url)
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl(url);
        }


        public void ShowPlace(String place)
        {
            try
            {
                // nAinda não clica no sitio certo !! 
                driver.FindElement(By.XPath("//span[@class='ui_icon restaurants brand-quick-links-QuickLinkTileItem__icon--2iguo']")).Click();
                driver.FindElement(By.XPath("//input[@class='input-text-input-ManagedTextInput__managedInput--2RESp']")).SendKeys(place);
                System.Threading.Thread.Sleep(1000);
                driver.FindElement(By.XPath("//span[@class='common-typeahead-results-BasicResult__resultTitle--1TQbu' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + place.ToLower() +"')]")).Click();  
            }
            // when a method call is invalid for the object's current state.
            catch (InvalidOperationException e)
            {
                return;
            }// indicate that the element being requested does not exist.
            catch (NoSuchElementException e)
            {
                return;
            }
        }

        public void ShowFood(String food)
        {
            try
            {
                driver.FindElement(By.XPath("//span[@class='name' and  contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + food+"')]")).Click();

              
            }// when a method call is invalid for the object's current state.
            catch (InvalidOperationException e)
            {
                return;
            }// indicate that the element being requested does not exist.
            catch (NoSuchElementException e)
            {
                return;
            }
        }

        public void ShowEstablishment(String establishment)
        {
            try
            {
                driver.FindElement(By.XPath("//a[@onclick='event.preventDefault();' and  contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '" + establishment+ "')]")).Click();


            }// when a method call is invalid for the object's current state.
            catch (InvalidOperationException e)
            {
                return;
            }// indicate that the element being requested does not exist.
            catch (NoSuchElementException e)
            {
                return;
            }
        }

    }
}