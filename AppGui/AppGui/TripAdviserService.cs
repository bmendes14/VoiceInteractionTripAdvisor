﻿using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using mmisharp;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;


namespace AppGui
{
    class TripAdviserService
    {
        private String startUrl = "https://www.tripadvisor.pt/";
        private IWebDriver driver;

       
       
        public TripAdviserService()
        {
            driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://www.tripadvisor.pt/");
            driver.Manage().Window.Maximize();
           
        }

  


        public void openMoreType()
            //DONE
        {
            try
            {
                driver.FindElement(By.XPath("//div[@onclick=\"ta.restaurant_filter.expandVFilters(this); ta.restaurant_list_tracking.clickMoreFilters();\"]")).Click();
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

        public void openMoreCharact()
        {//done
            try
            {
                driver.FindElement(By.XPath("//div[@onclick=\"ta.restaurant_list_tracking.clickMoreFilters(); (ta.prwidgets.getjs(this,'handlers')).openOverlay('diningOptions');\"]")).Click();
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

        public void openGoodFor()
        {
            try
            {
                driver.FindElement(By.XPath("//div[@onclick=\"ta.restaurant_list_tracking.clickMoreFilters(); (ta.prwidgets.getjs(this,'handlers')).openOverlay('restaurantStyles');\"]")).Click();
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

        public void openKitchenDishes()
        {//DONE
            try
            {
                driver.FindElement(By.XPath("//div[@onclick=\"ta.restaurant_list_tracking.clickMoreFilters(); (ta.prwidgets.getjs(this,'handlers')).openOverlay('cuisine');\"]")).Click();
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

        public void ShowReserves(String reserveType)
        {
            try
            {
             driver.FindElement(By.XPath("//div[@class='label filterName' and  contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '" + reserveType.ToLower() + "')]")).Click();
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

        //Kitchen types

        public void ShowKitchenTypes(String kitchenType)
        {
            try
            {
                driver.FindElement(By.XPath("//div[@title='"+kitchenType+"']")).Click();
                System.Threading.Thread.Sleep(2000);
                driver.FindElement(By.XPath("//div[@class=\"applyButton ui_button primary\"]")).Click();


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

        //PRICE

        public void ShowPrice(String price)
        {
            try
            {
                driver.FindElement(By.XPath("//div[@class='label filterName' and  contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '" + price.ToLower() + "')]")).Click();


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

        //Characteristics

        public void ShowCharacteristics(String charact)
        {
            try
            {
                driver.FindElement(By.XPath("//div[@title = '" + charact + "']")).Click();
                System.Threading.Thread.Sleep(1000);
                driver.FindElement(By.XPath("//div[@class=\"applyButton ui_button primary\"]")).Click();


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

        //Good For

        public void ShowGoodFor(String good)
        {
            try
            {
                driver.FindElement(By.XPath("//div[@title = '" + good + "']")).Click();
                System.Threading.Thread.Sleep(1000);
                driver.FindElement(By.XPath("//div[@class=\"applyButton ui_button primary\"]")).Click();


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

        //Meals

        public void ShowMeals(String meal)
        {
            try
            {
                driver.FindElement(By.XPath("//div[@class='label filterName' and  contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '" + meal.ToLower() + "')]")).Click();


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


        //Restrictions


        public void ShowRestrictions(String rest)
        {
            try
            {
                driver.FindElement(By.XPath("//div[@class='label filterName' and  contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '" + rest.ToLower() + "')]")).Click();


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


        public List<String> getAllRestaurantsMain()
        {
            List<String> all;
            all = new List<string>(driver.FindElements(By.XPath("//a[@class='poiTitle']")).Select(iw => iw.Text));

            return all;
        }

        public List<String> getAllRestaurantsFilter()
        {
            List<String> all = new List<String>();
            bool result = false;
            int attempts = 0;
            Console.WriteLine("Restaurantes Filtro");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));

            System.Threading.Thread.Sleep(2000);
            wait.Until(drv => drv.FindElements(By.XPath("//a[@class='restaurants-list-ListCell__restaurantName--2aSdo']")));
            all = new List<string>(driver.FindElements(By.ClassName("restaurants-list-ListCell__restaurantName--2aSdo")).Select(iw => iw.Text));
            all.ForEach(Console.WriteLine);
            result = true;
            
              
            

                
            return all;
        }

        public static IWebElement FindElementIfExists(IWebDriver driver, By by)
        {
            var elements = driver.FindElements(by);
            return (elements.Count >= 1) ? elements.First() : null;
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

        public void clickRestaurant(List<String> res, String name)
        {
            foreach(String item in res)
            {
                if (item != String.Empty)
                {
                    Regex reg = new Regex("[*'\",_&#^@]");
                    string temp = reg.Replace(item, " ");
                    try
                    {
                        string[] words = temp.Split('.');
                        if (words[1].TrimStart().CompareTo(name) == 0) 
                        {
                            if(FindElementIfExists(driver, (By.XPath("//a[@class='restaurants-list-ListCell__restaurantName--2aSdo' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + name.ToLower() + "')]"))) != null)
                            {
                                IWebElement link = (driver.FindElement(By.XPath("//a[@class='restaurants-list-ListCell__restaurantName--2aSdo' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + name.ToLower() + "')]")));
                                String href = link.GetAttribute("href");
                                driver.Navigate().GoToUrl("https://www.tripadvisor.pt/" + href);

                            }
                            else
                            {
                                IWebElement link = (driver.FindElement(By.XPath("//a[@class='poiTitle' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + name.ToLower() + "')]")));
                                String href = link.GetAttribute("href");
                                driver.Navigate().GoToUrl("https://www.tripadvisor.pt/" + href);

                            }

                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        Console.WriteLine("Could not split string");

                        if (temp.CompareTo(name) == 0)
                        {
                            if (FindElementIfExists(driver, (By.XPath("//a[@class='restaurants-list-ListCell__restaurantName--2aSdo' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + name.ToLower() + "')]"))) != null)
                            {
                                IWebElement link = (driver.FindElement(By.XPath("//a[@class='restaurants-list-ListCell__restaurantName--2aSdo' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + name.ToLower() + "')]")));
                                String href = link.GetAttribute("href");
                                driver.Navigate().GoToUrl("https://www.tripadvisor.pt/" + href);

                            }
                            else
                            {
                                IWebElement link = (driver.FindElement(By.XPath("//a[@class='poiTitle' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + name.ToLower() + "')]")));
                                String href = link.GetAttribute("href");
                                driver.Navigate().GoToUrl("https://www.tripadvisor.pt/" + href);

                            }
                        }
                    }
                }

            }

        }

        public void ShowPlace(String place)
        {
            try
            {
                
               //verificação se botão de procurar cidades já se encontra aberto
                if(FindElementIfExists(driver,By.XPath("//div[@class='_2EFRp_bb']"))!=null)
                {
                    driver.FindElement(By.XPath("//div[@class='_2EFRp_bb']")).Click();
                    System.Threading.Thread.Sleep(1500);
                    driver.FindElement(By.XPath("//span[@class='ui_icon restaurants brand-quick-links-QuickLinkTileItem__icon--2iguo']")).Click();
                    driver.FindElement(By.XPath("//input[@class='Smftgery']")).SendKeys(place);
                    System.Threading.Thread.Sleep(1500);
                    driver.FindElement(By.XPath("//span[@class='common-typeahead-results-BasicResult__resultTitle--1TQbu' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + place.ToLower() + "')]")).Click();
                }
                //verificação se cidade já foi procurada e queremos procurar outra cidade
                   else if(FindElementIfExists(driver,By.XPath("//span[@class='ui_icon caret-down brand-global-nav-geopill-GeoPill__icon--3Uykj']"))!=null)
                {
                    driver.FindElement(By.XPath("//span[@class='ui_icon caret-down brand-global-nav-geopill-GeoPill__icon--3Uykj']")).Click();
                    System.Threading.Thread.Sleep(1500);
                    driver.FindElement(By.XPath("//input[@class='Smftgery']")).SendKeys(place);
                    System.Threading.Thread.Sleep(1500);
                    driver.FindElement(By.XPath("//span[@class='common-typeahead-results-BasicResult__resultTitle--1TQbu' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + place.ToLower() + "')]")).Click();
                }
                //caso seja o caso default, estamos na página principal sem ter procurado nenhuma cidade
                else
                {
                    driver.FindElement(By.XPath("//span[@class='ui_icon restaurants brand-quick-links-QuickLinkTileItem__icon--2iguo']")).Click();
                    System.Threading.Thread.Sleep(1500);
                    driver.FindElement(By.XPath("//input[@class='Smftgery']")).SendKeys(place);
                    System.Threading.Thread.Sleep(1500);
                    driver.FindElement(By.XPath("//span[@class='common-typeahead-results-BasicResult__resultTitle--1TQbu' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + place.ToLower() + "')]")).Click();

                }
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

        public void Clear()
        {
            try
            {

                System.Threading.Thread.Sleep(1000);

                ////span[@class='clear' and contains(Limpar tudo )]
                if (FindElementIfExists(driver, By.XPath("//span[@class='clear']")) != null)
                {                   
                 
                    System.Threading.Thread.Sleep(1000);
                    driver.FindElement(By.XPath("//span[@class='clear']")).Click();
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
           
                }
                }// when a method call is invalid for the object's current state.
            catch (InvalidOperationException e)
            {
                return;
            }// indicate that the element being requested does not exist.
            catch (NoSuchElementException e)
            {
                return;
            }
            catch(Exception e)
            {
                Console.WriteLine("Error");
            }
        }


        public bool ShowFood(String food)
        {
            try
            {
                driver.FindElement(By.XPath("//span[@class='name' and  contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + food.ToLower() +"')]")).Click();

              
            }// when a method call is invalid for the object's current state.
            catch (InvalidOperationException e)
            {
                return true;
            }// indicate that the element being requested does not exist.
            catch (NoSuchElementException e)
            {
                return true;
            }
            return true;
        }

        public void ShowEstablishment(String establishment)
        {
            try
            {    
                System.Threading.Thread.Sleep(1000);
                driver.FindElement(By.XPath("//a[@onclick='event.preventDefault();' and  contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '" + establishment.ToLower() + "')]")).Click();
                    
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