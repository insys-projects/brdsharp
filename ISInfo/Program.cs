using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.IO;

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
                }


            }



            Application.Run(form);
        }

        static Dictionary<uint, XmlNode> _structs = new Dictionary<uint, XmlNode>(); 
        static Dictionary<uint, XmlNode> _sstructs = new Dictionary<uint, XmlNode>();
        
        static Dictionary<uint, string> _devices = new Dictionary<uint, string>();
        static Dictionary<uint, string> _sdevices = new Dictionary<uint, string>();

        private static void ParseDatabase()
        {
            string[] files = Directory.GetFiles( Path.GetDirectoryName(Application.ExecutablePath) + "/data", "*.xml" );

            for( int i=0; i<files.Length; i++ )
            {
                XmlDocument xmlDoc = new XmlDocument();

               

                xmlDoc.Load( files[i] );

                XmlNodeList devNode = xmlDoc.SelectNodes("/icr/dev");

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

                        if (node2.Attributes["base"] != null) 
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
