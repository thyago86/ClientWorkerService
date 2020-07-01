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
            this.NomeMaquina = nomemaquina;
            this.VersaoNet = versaonet;
            this.VersaoWindows = versaowindows;
            this.IP = ip;
            this.Antivirus = antivirus;
            this.Firewall = firewall;
            this.HDDInformation = hDDInformation;

        }
        public InformationSent()
        {

        }
    }
    
    
}
