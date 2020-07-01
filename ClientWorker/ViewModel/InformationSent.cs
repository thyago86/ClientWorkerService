using System;
using System.Collections.Generic;
using System.Text;

namespace ClientWorker.ViewModel
{
    
    public class InformationSent
    {
        public string NomeMaquina { get; set; }
        public string VersaoNet { get; set; }
        public string VersaoWindows { get; set; }
        public string Antivirus { get; set; }
        public string Firewall { get; set; }
        public string IP { get; set; }
        public List<HDDInformation> HDDInformation { get; set; }
        public InformationSent(string nomemaquina, string versaonet, string versaowindows, string antivirus, string firewall, string ip, List<HDDInformation> hDDInformation)
        {
            NomeMaquina = nomemaquina;
            VersaoNet = versaonet;
            VersaoWindows = versaowindows;
            IP = ip;
            Antivirus = antivirus;
            Firewall = firewall;
            HDDInformation = hDDInformation;
        }
        public InformationSent()
        {

        }
    }
    
    
}
