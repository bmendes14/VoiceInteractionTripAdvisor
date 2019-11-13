using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using mmisharp;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace AppGui
{

    public partial class MainWindow : Window
    {
        TripAdviserService service;
        private MmiCommunication mmiC;
        public MainWindow()
        {

            InitializeComponent();
            service = new TripAdviserService();

            mmiC = new MmiCommunication("localhost",8000, "User1", "GUI");
            mmiC.Message += MmiC_Message;
            mmiC.Start();

        }

        private void writeToFile(List<String> res)
        {

            string outRes = "<?xml version=\"1.0\"?><grammar xml:lang =\"pt-PT\" version =\"1.0\" xmlns =\"http://www.w3.org/2001/06/grammar\" tag-format =\"semantics/1.0\" ><rule id=\"main\" scope=\"public\"><one-of><item><ruleref uri=\"#rest\"/></item></one-of></rule><rule id=\"rest\"><one-of><item><tag>out.Restau = \"Restaurantes\";</tag><one-of><item><ruleref uri=\"#Restaurantes\"/><tag>out.Restaurant = rules.Restaurantes.Restaurantes;</tag></item></one-of></item></one-of></rule>";
            outRes += "<rule id=\"Restaurantes\"><item repeat=\"1\"><one-of> ";

            foreach (String item in res)
            {
                if (item != String.Empty)
                {
                    Regex reg = new Regex("[*'\",_&#^@]");
                    string temp = reg.Replace(item, " ");

                    outRes += "<item>" + temp + "<tag> out.Restaurantes =" + "\"" + temp + "\"" + "</tag></item>";
                }
               
            }
            outRes += "</one-of></item></rule></grammar>";
            Console.WriteLine("Writing to file 2");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Fausto\Documents\IM\Project\VoiceInteractionTripAdvisor\speechModality\speechModality\Restaurants.grxml"))
            {
                file.WriteLine(outRes);
            }
            
        }

        private void writeToFile2(List<String> res)
        {

            string outRes = "<?xml version=\"1.0\"?><grammar xml:lang =\"pt-PT\" version =\"1.0\" xmlns =\"http://www.w3.org/2001/06/grammar\" tag-format =\"semantics/1.0\" ><rule id=\"main\" scope=\"public\"><one-of><item><ruleref uri=\"#rest\"/></item></one-of></rule><rule id=\"rest\"><one-of><item><tag>out.Restau = \"Restaurantes\";</tag><one-of><item><ruleref uri=\"#Restaurantes\"/><tag>out.Restaurant = rules.Restaurantes.Restaurantes;</tag></item></one-of></item></one-of></rule>";
            outRes += "<rule id=\"Restaurantes\"><item repeat=\"1\"><one-of> ";

            foreach (String item in res)
            {
                if (item != String.Empty)
                {
                    Regex reg = new Regex("[*'\",_&#^@]");
                    string temp = reg.Replace(item, " ");
                    try
                    {
                        string[] words = temp.Split('.');

                        outRes += "<item>" + words[1].TrimStart() + "<tag> out.Restaurantes =" + "\"" + words[1].TrimStart() + "\"" + "</tag></item>";

                    }
                    catch(Exception e) {
                        Console.WriteLine(e.ToString());
                        Console.WriteLine("Could not split string");

                        outRes += "<item>" + item + "<tag> out.Restaurantes =" + "\"" + item + "\"" + "</tag></item>";

                    }
                }

            }
            outRes += "</one-of></item></rule></grammar>";
            Console.WriteLine("Writing to file 2");
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Fausto\Documents\IM\Project\VoiceInteractionTripAdvisor\speechModality\speechModality\Restaurants.grxml"))
            {
                file.WriteLine(outRes);
            }

        }

        private void MmiC_Message(object sender, MmiEventArgs e)
        {
            Console.WriteLine(e.Message);
            var doc = XDocument.Parse(e.Message);
            var com = doc.Descendants("command").FirstOrDefault().Value;
            
            dynamic json = JsonConvert.DeserializeObject(com);


            List<String> res = new List<string>();
            switch (json.recognized[0].ToString())
            {
                
                case "cidades":
                    service.ShowPlace(json.recognized[1].ToString());
                    System.Threading.Thread.Sleep(500);
                    res = service.getAllRestaurantsMain();
                    if (res.Count() > 0)
                    {
                        writeToFile(res);
                        res.ForEach(Console.WriteLine);
                    }
                    break;

                case "Comidas":
                    bool check = service.ShowFood(json.recognized[1].ToString());
                    while (!check)
                    {
                        System.Threading.Thread.Sleep(1000);
                    }
                    res = service.getAllRestaurantsFilter();
                    if (res.Count() > 0)
                    {
                        writeToFile2(res);
                        res.ForEach(Console.WriteLine);
                    }
                    break;

                case "Tipodeestabelecimento":
                    service.ShowEstablishment(json.recognized[1].ToString());
                    System.Threading.Thread.Sleep(500);
                    res = service.getAllRestaurantsFilter();
                    if (res.Count() > 0)
                    {
                        writeToFile2(res);
                        res.ForEach(Console.WriteLine);
                    }
                    break;
                case "Limpar":
                    service.Clear();
                    System.Threading.Thread.Sleep(500);
                    res = service.getAllRestaurantsMain();
                    if (res.Count() > 0)
                    {
                        writeToFile(res);
                        res.ForEach(Console.WriteLine);
                    }

                    break;
                case "Restaurantes":
                    Console.WriteLine("Restaurante detected");
                    service.clickRestaurant(res,json.recognized[1].ToString());
                    break;

            }


        }
        }
}
