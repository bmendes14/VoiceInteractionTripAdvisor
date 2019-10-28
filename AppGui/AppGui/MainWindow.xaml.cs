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

        private void MmiC_Message(object sender, MmiEventArgs e)
        {
            Console.WriteLine(e.Message);
            var doc = XDocument.Parse(e.Message);
            var com = doc.Descendants("command").FirstOrDefault().Value;
            
            dynamic json = JsonConvert.DeserializeObject(com);

            

            switch (json.recognized[0].ToString())
            {
                case "cidades":
                    service.ShowPlace(json.recognized[1].ToString());
                    break;

                case "Comidas":
                    service.ShowFood(json.recognized[1].ToString());
                    break;

                case "Tipodeestabelecimento":
                    service.ShowEstablishment(json.recognized[1].ToString());
                    break;

            }


        }
        }
}
