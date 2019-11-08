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


        public void ShowPlace(String place, String confidence)
        {
            try
            {
                /** 
                 * Se a confiança for maior que 80 -> No feedback
                 *  >60e  menor ou igual que 80 -> Avisar a açºao
                 *  menor que 60 pedir para repetir
                 * **/
                if (Convert.ToDouble(confidence) <= 80 && Convert.ToDouble(confidence) >= 60)
                {
                    String r = "A procurar restaurantes na cidade " + place;
                    //verificação se botão de procurar cidades já se encontra aberto
                    if (FindElementIfExists(driver, By.XPath("//div[@class='_2EFRp_bb']")) != null)
                    {

                        t.Speak(r);
                        driver.FindElement(By.XPath("//div[@class='_2EFRp_bb']")).Click();
                        System.Threading.Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//span[@class='ui_icon restaurants brand-quick-links-QuickLinkTileItem__icon--2iguo']")).Click();
                        driver.FindElement(By.XPath("//input[@class='input-text-input-ManagedTextInput__managedInput--2RESp']")).SendKeys(place);
                        System.Threading.Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//span[@class='common-typeahead-results-BasicResult__resultTitle--1TQbu' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + place.ToLower() + "')]")).Click();
                    }
                    //verificação se cidade já foi procurada e queremos procurar outra cidade
                    else if (FindElementIfExists(driver, By.XPath("//span[@class='ui_icon caret-down brand-global-nav-geopill-GeoPill__icon--3Uykj']")) != null)
                    {
                        t.Speak(r);
                        driver.FindElement(By.XPath("//span[@class='ui_icon caret-down brand-global-nav-geopill-GeoPill__icon--3Uykj']")).Click();
                        System.Threading.Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//input[@class='input-text-input-ManagedTextInput__managedInput--2RESp']")).SendKeys(place);
                        System.Threading.Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//span[@class='common-typeahead-results-BasicResult__resultTitle--1TQbu' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + place.ToLower() + "')]")).Click();
                    }
                    //caso seja o caso default, estamos na página principal sem ter procurado nenhuma cidade
                    else
                    {
                        t.Speak(r);
                        driver.FindElement(By.XPath("//span[@class='ui_icon restaurants brand-quick-links-QuickLinkTileItem__icon--2iguo']")).Click();
                        driver.FindElement(By.XPath("//input[@class='input-text-input-ManagedTextInput__managedInput--2RESp']")).SendKeys(place);
                        System.Threading.Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//span[@class='common-typeahead-results-BasicResult__resultTitle--1TQbu' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + place.ToLower() + "')]")).Click();

                    }
                }
                else if(Convert.ToDouble(confidence) < 60)
                {
                    t.Speak("Não consegui entender, será possível repetir?");
                    System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    //verificação se botão de procurar cidades já se encontra aberto
                    if (FindElementIfExists(driver, By.XPath("//div[@class='_2EFRp_bb']")) != null)
                    {
                        driver.FindElement(By.XPath("//div[@class='_2EFRp_bb']")).Click();
                        System.Threading.Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//span[@class='ui_icon restaurants brand-quick-links-QuickLinkTileItem__icon--2iguo']")).Click();
                        driver.FindElement(By.XPath("//input[@class='input-text-input-ManagedTextInput__managedInput--2RESp']")).SendKeys(place);
                        System.Threading.Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//span[@class='common-typeahead-results-BasicResult__resultTitle--1TQbu' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + place.ToLower() + "')]")).Click();
                    }
                    //verificação se cidade já foi procurada e queremos procurar outra cidade
                    else if (FindElementIfExists(driver, By.XPath("//span[@class='ui_icon caret-down brand-global-nav-geopill-GeoPill__icon--3Uykj']")) != null)
                    {
                        driver.FindElement(By.XPath("//span[@class='ui_icon caret-down brand-global-nav-geopill-GeoPill__icon--3Uykj']")).Click();
                        System.Threading.Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//input[@class='input-text-input-ManagedTextInput__managedInput--2RESp']")).SendKeys(place);
                        System.Threading.Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//span[@class='common-typeahead-results-BasicResult__resultTitle--1TQbu' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + place.ToLower() + "')]")).Click();
                    }
                    //caso seja o caso default, estamos na página principal sem ter procurado nenhuma cidade
                    else
                    {
                        driver.FindElement(By.XPath("//span[@class='ui_icon restaurants brand-quick-links-QuickLinkTileItem__icon--2iguo']")).Click();
                        driver.FindElement(By.XPath("//input[@class='input-text-input-ManagedTextInput__managedInput--2RESp']")).SendKeys(place);
                        System.Threading.Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//span[@class='common-typeahead-results-BasicResult__resultTitle--1TQbu' and contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + place.ToLower() + "')]")).Click();

                    }
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

        public void Clear(String confidence)
        {
            try
            {

                System.Threading.Thread.Sleep(1000);

                ////span[@class='clear' and contains(Limpar tudo )]
                if (FindElementIfExists(driver, By.XPath("//span[@class='clear']")) != null)
                {                   
                    if(Convert.ToDouble(confidence) <= 80 && Convert.ToDouble(confidence) >=60)
                    {
                        t.Speak("A Limpar filtros");
                        System.Threading.Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//span[@class='clear']")).Click();
                    }
                    else if (Convert.ToDouble(confidence) < 60)
                    {
                        t.Speak("Não consegui entender, será possível repetir?");
                        System.Threading.Thread.Sleep(1000);
                    }
                    else
                    {
                        System.Threading.Thread.Sleep(1000);
                        driver.FindElement(By.XPath("//span[@class='clear']")).Click();
                    }
                    
                }
                else
                {
                    System.Threading.Thread.Sleep(1000);
                    t.Speak("Botão limpar não encontrado!");
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
                t.Speak("Erro!!!");
            }
        }


        public void ShowFood(String food, String confidence)
        {
            try
            {
                driver.FindElement(By.XPath("//span[@class='name' and  contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'" + food.ToLower() +"')]")).Click();

              
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

        public void ShowEstablishment(String establishment, String confidence)
        {
            try
            {
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