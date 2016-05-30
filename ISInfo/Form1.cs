using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;

namespace ISInfo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

           
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        internal void Node( uint sig )
        {
            treeView1.Nodes.Add( "0x" + sig.ToString("X"));
        }

        internal void Node(uint sig, System.Xml.XmlNode node, System.IO.BinaryReader br)
        {
            long Start = br.BaseStream.Position;

            TreeNode trn = treeView1.Nodes.Add("0x" + sig.ToString("X"));

            XmlNodeList grps = node.SelectNodes("group");

            foreach (XmlNode grp in grps)
            {
                TreeNode trn2 = trn.Nodes.Add(grp.Attributes["title"].Value);

                XmlNodeList fs = grp.SelectNodes("field"); 
                
                foreach (XmlNode f in fs)
                {
                    string Value = "";

                    br.BaseStream.Position = Start -4  + int.Parse(f.Attributes["offset"].Value);

                    byte[] bytes = br.ReadBytes( int.Parse(f.Attributes["size"].Value) );

                    //Array.Reverse(bytes);
                    uint intval = 0;

                    if (bytes.Length == 4)
                        intval = BitConverter.ToUInt32(bytes, 0);
                    else if (bytes.Length == 2)
                        intval = BitConverter.ToUInt16(bytes, 0);
                    else if (bytes.Length == 1)
                        intval = bytes[0];

                    switch (f.Attributes["type"].Value)
                    {
                        case "hex":
                            {
                                if (bytes.Length == 2)
                                    Value = BitConverter.ToInt16(bytes, 0).ToString("X");
                                else
                                    Value = bytes[0].ToString("X");


                                break;
                            }
                        case "int":
                            {

                                Value = intval.ToString();

                                break;
                            }
                        case "spin-double":
                        case "double":
                            {
                                Value = BitConverter.ToInt32(bytes, 0).ToString();

                                if (f.Attributes["prec"].Specified)
                                {
                                    int prec = int.Parse(f.Attributes["prec"].Value);

                                    Value = ((double)intval / Math.Pow(10, prec)).ToString();
                                }
                                
                                break;
                            }
                        case "list":
                            {

                                Value = ParseList( f.ChildNodes, intval );



                                break;
                            }
                    }

                    TreeNode trn3 = trn2.Nodes.Add(f.Attributes["name"].Value + " = " + Value);

                    
                }
            }

            br.BaseStream.Position = Start;

        }

        private string ParseList(XmlNodeList xmlNodeList, uint intval)
        {
            foreach (XmlNode node in xmlNodeList)
            {
                if (int.Parse(node.Attributes["val"].Value) == intval)
                {
                    return node.Attributes["descr"].Value;
                }
            }

            return intval.ToString();
        }
    }
}
