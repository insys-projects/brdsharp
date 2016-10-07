using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.IO;
using System.Text;

using brd;

namespace ISInfo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 

        static int workMode = 0;
        static string file = "";


        [STAThread]
        static void Main( string[] args )
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            XmlDocument xmlDoc = new XmlDocument();

            ParseArgs(args);

            ParseDatabase();

            if (workMode == 1)
            {
                xmlDoc.Load(file);
            }
            else
            {
                BrdInfo( xmlDoc );
            }

            Form1 form = new Form1();
            
            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                switch (node.Name)
                {
                    case "device":
                        {
                            break;
                        }
                    case "icr":
                        {

                            ParseICR(ParseBinary(node.InnerText), form, node.Attributes["type"].Value);
                            break;
                        }
                    case "pld":
                        {

                            ParsePld( node.ChildNodes , form );
                            break;
                        }
                }


            }



            Application.Run(form);
        }

        private static void BrdInfo(XmlDocument xmlDoc)
        {
            BRD dev = BRD.open();

            BRD_Info info = dev.getInfo();

            XmlNode root = xmlDoc.CreateNode(XmlNodeType.Element, "insys", "");

            XmlElement node;

            node = xmlDoc.CreateElement("device");

            root.AppendChild(node);

            node = xmlDoc.CreateElement("pld");
            root.AppendChild(node);
            for (int i = 0; i < 16; i++)
            {
                if (dev.rgio.ri(i, 0x100) == 0 )
                    continue;

                XmlElement trd = xmlDoc.CreateElement("trd");

                StringBuilder sb = new StringBuilder();

                for (int j = 0; j < 4; j++)
                {
                    int val = dev.rgio.ri(i, 0x100 + j);

                    sb.Append(val.ToString("X4"));
                }


                trd.SetAttribute("id", i.ToString());

                trd.AppendChild(xmlDoc.CreateCDataSection(sb.ToString()));

                node.AppendChild(trd);
            }

            node = xmlDoc.CreateElement("icr");
            node.SetAttribute("type", "base");

            {
               

                byte[] idcfgrom = IdCfgRom.ReadIdCfgRom( dev, "" );

                node.AppendChild(
                    xmlDoc.CreateCDataSection(ByteToHex.ByteArrayToStringFastest(idcfgrom))
                    );
            }

            


            root.AppendChild(node);

            xmlDoc.AppendChild(root);
        }

        private static void ParsePld(XmlNodeList xmlNodeList, Form1 form)
        {
            foreach (XmlNode node in xmlNodeList)
            {
                switch (node.Name)
                {
                    case "trd":
                        {
                            int id = 0;

                            if (node.Attributes.GetNamedItem("id") != null)
                                id = int.Parse(node.Attributes["id"].Value);

                            TrdNode(ParseBinary(node.InnerText),form, id );
                            break;
                        }
                }


            }
        }

        private static void TrdNode(byte[] p, Form1 form, int id)
        {
            MemoryStream ms = new MemoryStream(p);

            BinaryReader br = new BinaryReader(ms);

            uint tag = br.ReadUInt16();

            string name = "TRD " + id.ToString();

            if( _trd.ContainsKey( tag ) )
                name = _trd[tag];

            if( _pstructs.ContainsKey(tag) )
                form.TrdNode(name, br, _pstructs[tag]);
            else
                form.TrdNode(name, br, _pstructs[0x1]);
        }

        static Dictionary<uint, XmlNode> _structs = new Dictionary<uint, XmlNode>(); 
        static Dictionary<uint, XmlNode> _sstructs = new Dictionary<uint, XmlNode>();
        
        static Dictionary<uint, string> _devices = new Dictionary<uint, string>();
        static Dictionary<uint, string> _sdevices = new Dictionary<uint, string>();

        static Dictionary<uint, string> _trd = new Dictionary<uint, string>();
        static Dictionary<uint, XmlNode> _pstructs = new Dictionary<uint, XmlNode>();

        private static void ParseDatabase()
        {
            string[] files = Directory.GetFiles( Path.GetDirectoryName(Application.ExecutablePath) + "/data", "*.xml" );

            for( int i=0; i<files.Length; i++ )
            {
                XmlDocument xmlDoc = new XmlDocument();

               

                xmlDoc.Load( files[i] );

                XmlNodeList devNode = xmlDoc.SelectNodes("/icr/dev");

                if (devNode.Count > 0)
                {
                    foreach (XmlNode node in devNode)
                    {

                        string name = node.Attributes["name"].Value;
                        string type = node.Attributes["type"].Value;

                        if (String.IsNullOrEmpty(type))
                            continue;

                        uint itype = Convert.ToUInt32(type, 16);

                        if (node.Attributes["base"] != null)
                            _devices[itype] = name;
                        else
                            _sdevices[itype] = name;
                    }
                }
                else
                {
                    devNode = xmlDoc.SelectNodes("/icr/trd");

                    foreach (XmlNode node in devNode)
                    {

                        string type = node.Attributes["type"].Value;
                        uint itype = Convert.ToUInt32(type, 16);
                        string name = node.Attributes["name"].Value;

                        _trd[itype] = name;
                    }

                }



                ParseStruct(xmlDoc.SelectNodes("/icr/struct"), devNode);

            }
        }

        private static void ParseStruct(XmlNodeList xmlNodeList, XmlNodeList devNode )
        {
            uint itype;
            
            foreach (XmlNode node in xmlNodeList)
            {
                string tag = node.Attributes["tag"].Value;

                if (String.IsNullOrEmpty(tag))
                {
                    foreach (XmlNode node2 in devNode)
                    {
                        tag = node2.Attributes["type"].Value;
                        itype = Convert.ToUInt32(tag, 16);

                        if (devNode[0].Name == "trd")
                            _pstructs[itype] = node;
                        else if (node2.Attributes["base"] != null) 
                            _structs[itype] = node;
                        else
                            _sstructs[itype] = node;
                    }

                    continue;
                }

                itype = Convert.ToUInt32(tag, 16);

         
                if (devNode[0].Attributes["base"] != null)
                    _structs[itype] = node;
                else
                    _sstructs[itype] = node;
            }
        }

        private static void ParseICR(byte[] p, Form1 form, string type )
        {
           
            MemoryStream ms = new MemoryStream(p );

            BinaryReader br = new BinaryReader(ms);
            
            Dictionary<uint, XmlNode> _structs;

            if (type == "base")
                _structs = Program._structs;
            else
                _structs = Program._sstructs;

            while (ms.Length > ms.Position + 1)
            {
                uint sig = br.ReadUInt16();

                if (_structs.Keys.Contains(sig))
                {
                    XmlNode node = _structs[sig];


                    int xsize = Convert.ToInt32(node.Attributes["size"].Value);

                    uint size = br.ReadUInt16();

                    if (xsize != size + 4)
                    {
                        ms.Position-=3; 
                        continue;
                    }

                    long endsize = ms.Position + size;

                    form.Node( sig, node, br  );

                    ms.Position = endsize;
                }
                else
                {
                   ms.Position--;
                }


            }
        }

        private static byte[] ParseBinary(string p)
        {
            StringReader tr = new StringReader( p );

            List<byte> bytes = new List<byte>();

            while (true)
            {
                string line = tr.ReadLine();

                if ( line == null )
                    break; 
                
                if ( String.IsNullOrEmpty(line) )
                    continue;

                bytes.AddRange(HexToByte.StringToByteArrayFastest(line));
            }

            return bytes.ToArray();
        }

        private static void ParseArgs(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                if( args[i] == "-f" )
                {
                    workMode = 1;
                    file = args[++i];
                }
            }
        }
    }
}
