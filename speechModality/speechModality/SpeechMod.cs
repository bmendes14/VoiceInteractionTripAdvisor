using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using mmisharp;
using Microsoft.Speech.Recognition;

namespace speechModality
{
    public class SpeechMod
    {
        private SpeechRecognitionEngine sre;
        private Grammar gr;
        private Grammar toLoad;
        private Grammar loaded;
        private bool done;
        private bool grammarLoaded;
        public event EventHandler<SpeechEventArg> Recognized;
        private Tts t;
        protected virtual void onRecognized(SpeechEventArg msg)
        {
            EventHandler<SpeechEventArg> handler = Recognized;
            if (handler != null)
            {
                handler(this, msg);
            }
        }


        private LifeCycleEvents lce;
        private MmiCommunication mmic;


        private Queue<SpeechRecognizedEventArgs> eventQueue = new Queue<SpeechRecognizedEventArgs>();

        

        public SpeechMod()
        {
            //init Text-To-Speech
            t = new Tts();
            //init LifeCycleEvents..
            lce = new LifeCycleEvents("ASR", "FUSION","speech-1", "acoustic", "command"); // LifeCycleEvents(string source, string target, string id, string medium, string mode)
            //mmic = new MmiCommunication("localhost",9876,"User1", "ASR");  //PORT TO FUSION - uncomment this line to work with fusion later
            mmic = new MmiCommunication("localhost", 8000, "User1", "ASR"); // MmiCommunication(string IMhost, int portIM, string UserOD, string thisModalityName)

            mmic.Send(lce.NewContextRequest());

            //load pt recognizer
            sre = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("pt-PT"));
            gr = new Grammar(Environment.CurrentDirectory + "\\ptG.grxml", "rootRule");
            sre.LoadGrammar(gr);
            grammarLoaded = false;

            
            sre.SetInputToDefaultAudioDevice();
            sre.RecognizeAsync(RecognizeMode.Multiple);
            sre.SpeechRecognized += Sre_SpeechRecognized;
            sre.SpeechHypothesized += Sre_SpeechHypothesized;

            sre.RecognizerUpdateReached +=
        new EventHandler<RecognizerUpdateReachedEventArgs>(recognizer_RecognizerUpdateReached);

            var fileSystemWatcher = new FileSystemWatcher();

            fileSystemWatcher.Changed += FileSystemWatcher_Changed;

            

           

            fileSystemWatcher.Path = Environment.CurrentDirectory + @"\..\..";
            
            fileSystemWatcher.EnableRaisingEvents = true;
        }


        public  void recognizer_RecognizerUpdateReached(object sender, RecognizerUpdateReachedEventArgs e)
        {
            if (done == false)
            {
                if (grammarLoaded == true)
                {
                    sre.UnloadGrammar(loaded);
                    Console.WriteLine("Grammar unloaded");
                }
                sre.LoadGrammar(toLoad);
                Console.WriteLine("Grammar loaded");
                grammarLoaded = true;
                done = true;
                loaded = toLoad;
                
            }
            
            
        
            

        }


        private  void FileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("Pahthhhh ");
            GrammarBuilder builder = new GrammarBuilder();
            builder.AppendRuleReference(Environment.CurrentDirectory + @"\..\..\Restaurants.grxml", "main");

           
            toLoad = new Grammar(builder);
            done = false;
            sre.RequestRecognizerUpdate();
        }

        private void Sre_SpeechHypothesized(object sender, SpeechHypothesizedEventArgs e)
        {
            onRecognized(new SpeechEventArg() { Text = e.Result.Text, Confidence = e.Result.Confidence, Final = false });
        }


        private bool processRequest(SpeechRecognizedEventArgs e)
        {
           if(eventQueue.Count == 0 || (eventQueue.Count!=0 && e.Result.Semantics.ToArray()[0].Value.Value.Equals("Confirm")))
            {  
                switch (e.Result.Semantics.ToArray()[0].Value.Value)
                {
                    // TODO: Decide wether to send (feeback and request)
                    case "cidades":
                        if (e.Result.Confidence < 0.20)
                        {
                            t.Speak("Não consegui entender, por favor repita");
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.20 && e.Result.Confidence < 0.60)
                        {
                            // obtem informação se é mesmo isto
                            Console.WriteLine(e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            t.Speak("Não consegui entender, Será que quis procurar a cidade  " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            eventQueue.Enqueue(e);
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.60 && e.Result.Confidence < 0.80)
                        {
                            // procura e obtem info
                            t.Speak("Estou a procura da cidade " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            return true;
                        }
                        else
                        {
                            return true;
                        }

                    case "Comidas":
                        if (e.Result.Confidence < 0.20)
                        {
                            t.Speak("Não consegui entender, por favor repita");
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.20 && e.Result.Confidence < 0.60)
                        {
                            // obtem informação se é mesmo isto
                            Console.WriteLine(e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            t.Speak("Não consegui entender, Será que quis procurar por   " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            eventQueue.Enqueue(e);
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.60 && e.Result.Confidence < 0.80)
                        {
                            // procura e obtem info
                            t.Speak("Estou a procura de  " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                        
                    case "Tipodeestabelecimento":
                        if (e.Result.Confidence < 0.20)
                        {
                            t.Speak("Não consegui entender, por favor repita");
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.20 && e.Result.Confidence < 0.60)
                        {
                            // obtem informação se é mesmo isto
                            Console.WriteLine(e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            t.Speak("Não consegui entender, Será que quis procurar por   " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            eventQueue.Enqueue(e);
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.60 && e.Result.Confidence < 0.80)
                        {
                            // procura e obtem info
                            t.Speak("Estou a procura de  " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            return true;
                        }
                        else
                        {
                            return true;
                        }

                    case "Limpar":
                        if (e.Result.Confidence < 0.20)
                        {
                            t.Speak("Não consegui entender, por favor repita");
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.20 && e.Result.Confidence < 0.60)
                        {
                            // obtem informação se é mesmo isto
                            Console.WriteLine(e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            t.Speak("Não consegui entender, Será que quis limpar os filtros ?");
                            eventQueue.Enqueue(e);
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.60 && e.Result.Confidence < 0.80)
                        {
                            // procura e obtem info
                            t.Speak("Estou a limpar os filtros ");
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                        
                    case "Confirm":
                        if (e.Result.Semantics.ToArray()[1].Value.Value.ToString().Equals("Sim"))
                        {
                            e = eventQueue.Dequeue();

                            t.Speak("Estou a efectuar a acção pedida!!");

                            string json = "{ \"recognized\": [";
                            foreach (var resultSemantic in e.Result.Semantics)
                            {
                                json += "\"" + resultSemantic.Value.Value + "\", ";
                            }
                            json = json.Substring(0, json.Length - 2);
                            json += "] }";

                            var exNot = lce.ExtensionNotification(e.Result.Audio.StartTime + "", e.Result.Audio.StartTime.Add(e.Result.Audio.Duration) + "", e.Result.Confidence, json);

                            mmic.Send(exNot);
                            return false;
                        }
                        else
                        {
                            t.Speak("Pesquisa cancelada, por fovor qual é a proxima ação");
                            eventQueue.Dequeue();
                            return false;
                        }

                    case "Restaurantes":
                        if (e.Result.Confidence < 0.20)
                        {
                            t.Speak("Não consegui entender, por favor repita");
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.20 && e.Result.Confidence < 0.60)
                        {
                            // obtem informação se é mesmo isto
                            Console.WriteLine(e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            t.Speak("Não consegui entender, Será que quis procurar o restaurante  " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            eventQueue.Enqueue(e);
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.60 && e.Result.Confidence < 0.80)
                        {
                            // procura e obtem info
                            t.Speak("Estou a abrir o restaurante " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                       
                    case "TipodeReserva":
                        if (e.Result.Confidence < 0.20)
                        {
                            t.Speak("Não consegui entender, por favor repita");
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.20 && e.Result.Confidence < 0.60)
                        {
                            // obtem informação se é mesmo isto
                            Console.WriteLine(e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            t.Speak("Não consegui entender, Será que quis procurar o tipo de reserva  " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            eventQueue.Enqueue(e);
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.60 && e.Result.Confidence < 0.80)
                        {
                            // procura e obtem info
                            t.Speak("Estou a procura do tipo de reserva " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    case "Cozinhasepratos":
                        if (e.Result.Confidence < 0.20)
                        {
                            t.Speak("Não consegui entender, por favor repita");
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.20 && e.Result.Confidence < 0.60)
                        {
                            // obtem informação se é mesmo isto
                            Console.WriteLine(e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            t.Speak("Não consegui entender, Será que quis procurar o tipo de comida  " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            eventQueue.Enqueue(e);
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.60 && e.Result.Confidence < 0.80)
                        {
                            // procura e obtem info
                            t.Speak("Estou a procura do tipo de comida " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    case "Preco":
                        if (e.Result.Confidence < 0.20)
                        {
                            t.Speak("Não consegui entender, por favor repita");
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.20 && e.Result.Confidence < 0.60)
                        {
                            // obtem informação se é mesmo isto
                            Console.WriteLine(e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            t.Speak("Não consegui entender, Será que quis procurar o preço " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            eventQueue.Enqueue(e);
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.60 && e.Result.Confidence < 0.80)
                        {
                            // procura e obtem info
                            t.Speak("Estou a procura do preço " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    case "Caracteristicasdorestaurante":
                        if (e.Result.Confidence < 0.20)
                        {
                            t.Speak("Não consegui entender, por favor repita");
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.20 && e.Result.Confidence < 0.60)
                        {
                            // obtem informação se é mesmo isto
                            Console.WriteLine(e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            t.Speak("Não consegui entender, Será que quis procurar restaurantes com a característica  " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            eventQueue.Enqueue(e);
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.60 && e.Result.Confidence < 0.80)
                        {
                            // procura e obtem info
                            t.Speak("Estou a procura de restaurantes com a característica " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    case "Bonspara":
                        if (e.Result.Confidence < 0.20)
                        {
                            t.Speak("Não consegui entender, por favor repita");
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.20 && e.Result.Confidence < 0.60)
                        {
                            // obtem informação se é mesmo isto
                            Console.WriteLine(e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            t.Speak("Não consegui entender, Será que quis procurar restaurantes bons para  " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            eventQueue.Enqueue(e);
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.60 && e.Result.Confidence < 0.80)
                        {
                            // procura e obtem info
                            t.Speak("Estou a procurar restaurantes bons para " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    case "Refeicoes":
                        if (e.Result.Confidence < 0.20)
                        {
                            t.Speak("Não consegui entender, por favor repita");
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.20 && e.Result.Confidence < 0.60)
                        {
                            // obtem informação se é mesmo isto
                            Console.WriteLine(e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            t.Speak("Não consegui entender, Será que quis procurar a refeição  " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            eventQueue.Enqueue(e);
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.60 && e.Result.Confidence < 0.80)
                        {
                            // procura e obtem info
                            t.Speak("Estou à procura da refeição " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            return true;
                        }
                        else
                        {
                            return true;
                        }
                    case "Restricoesalimentares":
                        if (e.Result.Confidence < 0.20)
                        {
                            t.Speak("Não consegui entender, por favor repita");
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.20 && e.Result.Confidence < 0.60)
                        {
                            // obtem informação se é mesmo isto
                            Console.WriteLine(e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            t.Speak("Não consegui entender, Será que quis procurar a restrição alimentar  " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            eventQueue.Enqueue(e);
                            return false;
                        }
                        else if (e.Result.Confidence >= 0.60 && e.Result.Confidence < 0.80)
                        {
                            // procura e obtem info
                            t.Speak("Estou a procura da restrição alimentar " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                            return true;
                        }
                        else
                        {
                            return true;
                        }

                }

            }
            else
            {
                e = eventQueue.Peek();
                switch (e.Result.Semantics.ToArray()[0].Value.Value)
                {
                    // TODO: Decide wether to send (feeback and request)
                    case "cidades":
                        t.Speak("Por favor confirme que quer pesquisar a cidade " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                        break;
                    case "Comidas":
                        t.Speak("Por favor confirme que quer pesquisar pelo tipo de comida " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                        break;
                    case "Tipodeestabelecimento":
                        t.Speak("Por favor confirme que quer pesquisar pelo tipo de estabelecimento " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                        break;
                    case "Limpar":
                        t.Speak("Por favor confirme que quer lempar todos os filtros");
                        break;
                    case "Restaurantes":
                        t.Speak("Por favor confirme que quer pesquisar pelo Restaurante " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                        break;
                    case "TipodeReserva":
                        t.Speak("Por favor confirme que quer pesquisar pelo tipo de reserva " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                        break;
                    case "Cozinhasepratos":
                        t.Speak("Por favor confirme que quer pesquisar pelo tipo de cozinha " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                        break;
                    case "Preco":
                        t.Speak("Por favor confirme que quer pesquisar pelo tipo de preço " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                        break;
                    case "Caracteristicasdorestaurante":
                        t.Speak("Por favor confirme que quer pesquisar pela seguinte caracteristica do restaurante " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                        break;
                    case "Bonspara":
                        t.Speak("Por favor confirme se o restaurante que quer é bom para " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                        break;
                    case "Refeicoes":
                        t.Speak("A refeição que prefere é " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                        break;
                    case "Restricoesalimentares":
                        t.Speak("A sua restrição é  " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                        break;
                    case "Mais":
                        t.Speak("Quer ver mais informação sobre " + e.Result.Semantics.ToArray()[1].Value.Value.ToString());
                        break;

                }


            }

            return false;

        }

        private void Sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            onRecognized(new SpeechEventArg(){Text = e.Result.Text, Confidence = e.Result.Confidence, Final = true});
            
            //SEND
            // IMPORTANT TO KEEP THE FORMAT {"recognized":["SHAPE","COLOR"]}
            string json = "{ \"recognized\": [";
            foreach (var resultSemantic in e.Result.Semantics)
            {
                json+= "\"" + resultSemantic.Value.Value +"\", ";
            }
            json = json.Substring(0, json.Length - 2);
            json += "] }";

            var exNot = lce.ExtensionNotification(e.Result.Audio.StartTime+"", e.Result.Audio.StartTime.Add(e.Result.Audio.Duration)+"",e.Result.Confidence, json);

            bool pR = processRequest(e);

            if (pR)
            {
                mmic.Send(exNot);
            }
           
        }
    }
}
