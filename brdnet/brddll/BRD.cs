using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;

using BRD_API = brd_internal.BRD_API;

namespace brd
{



    public enum BRDcode
    {
        BRDerr_OK = 0
    }
    



    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    public struct BRD_Info
    {



        public UInt32 size;		// sizeof(BRD_Info)
        public Int32 code;		// reserved
        public UInt32 boardType;	// Board Type

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string name;	// Device Name ASCII

        public UInt32 pid;		// Board Phisical ID
        public Int32 busType;	// Bus Type (Unknown, PCI, ISA, RS232, 1394, USB, PCMCIA, VME, ... )
        public Int32 bus;		// Bus Number
        public Int32 dev;		// Dev Number
        public Int32 slot;		// Slot Number
        public UInt32 verMajor;	// Driver Version
        public UInt32 verMinor;	// Driver Version

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public UInt32[] subunitType;// Subunit Type Code

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public UInt32[] baseaddr;	// Base Addresses 

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
        public UInt32[] vectors;	// Interrupt Vectors
    }
    
    
    [StructLayout(LayoutKind.Sequential, CharSet = BRD_API.brdcharset, Pack = 4)]
    public struct BRD_PuList
    {

        /*
         * 	U32		puId;				// Programmable Unit ID
	        U32		puCode;				// Programmable Unit Code
	        U32		puAttr;				// Programmable Unit Attribute
	        BRDCHAR	puDescription[128];	// Programmable Unit Description Text
         * 
         */

        public UInt32 puId;		// sizeof(BRD_Info)
        public UInt32 puCode;		// reserved
        public UInt32 puAttr;	// Board Type

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
        public string puDescription;	// Device Name ASCII

    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
    public struct BRD_ServList
    {

        /*
	        BRDCHAR	name[16];		// Service name with Number
	        U32		attr;			// Attributes of Service (Look BRDattr_XXX constants)
         * 
         */

        


        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
        public string name;	// Device Name ASCII
        public UInt32 attr;		// sizeof(BRD_Info)

    }


    enum BRDctrl
    {
         BRD_SECTIONSIZE = 256,
             
             
        BRDctrl_DAQ         = BRD_SECTIONSIZE * 8,	// DAQ control
	    BRDctrl_CMN			= BRD_SECTIONSIZE*9,	// common control parameters
	    BRDctrl_ADC			= BRD_SECTIONSIZE*10,	// ADC control parameters
	    BRDctrl_DAC			= BRD_SECTIONSIZE*11,	// DAC control parameters
	    BRDctrl_DDC			= BRD_SECTIONSIZE*12,	// DDC control parameters
	    BRDctrl_PIO			= BRD_SECTIONSIZE*13,	// PIO control parameters
	    BRDctrl_STREAM		= BRD_SECTIONSIZE*14,	// STREAM control parameters
	    BRDctrl_SVEAM		= BRD_SECTIONSIZE*15,	// SVEAM control parameters
	    BRDctrl_DIOIN		= BRD_SECTIONSIZE*16,	// DIOIN control parameters
	    BRDctrl_DIOOUT		= BRD_SECTIONSIZE*17,	// DIOOUT control parameters
	    BRDctrl_SDRAM		= BRD_SECTIONSIZE*18,	// SDRAM control parameters
	    BRDctrl_DSPNODE		= BRD_SECTIONSIZE*19,	// DSP node control parameters
	    BRDctrl_TEST		= BRD_SECTIONSIZE*20,	// Test control parameters
	    BRDctrl_FOTR		= BRD_SECTIONSIZE*21,	// FOTR control parameters
	    BRDctrl_QM			= BRD_SECTIONSIZE*22,	// QM9857 control parameters
	    BRDctrl_GC5016		= BRD_SECTIONSIZE*23,	// GC5016 control parameters
	    BRDctrl_DDS			= BRD_SECTIONSIZE*24,	// DDS control parameters
	    BRDctrl_SYNCDAC		= BRD_SECTIONSIZE*25,	// SyncDac control parameters
	    BRDctrl_ILLEGAL
    };

   enum BRDctrl_REG
   {
	
        BRDctrl_CMN = BRDctrl.BRDctrl_CMN,
       
        BRDctrl_REG_READDIR			= BRDctrl_CMN + 0,		// read value from direct register (PBRD_Reg)
        BRDctrl_REG_READSDIR		= BRDctrl_CMN + 1,		// read buffer from direct register (PBRD_Reg)
        BRDctrl_REG_READIND			= BRDctrl_CMN + 2,		// read value from indirect register (PBRD_Reg)
        BRDctrl_REG_READSIND		= BRDctrl_CMN + 3,		// read buffer from indirect register (PBRD_Reg)
        BRDctrl_REG_WRITEDIR		= BRDctrl_CMN + 6,		// write value in direct register (PBRD_Reg)
        BRDctrl_REG_WRITESDIR		= BRDctrl_CMN + 7,		// write buffer in direct register (PBRD_Reg)
        BRDctrl_REG_WRITEIND		= BRDctrl_CMN + 8,		// write value in indirect register (PBRD_Reg)
        BRDctrl_REG_WRITESIND		= BRDctrl_CMN + 9,		// write buffer in indirect register (PBRD_Reg)
        BRDctrl_REG_SETSTATIRQ		= BRDctrl_CMN + 10,		// set interrupt by status register (PBRD_Irq)
        BRDctrl_REG_CLEARSTATIRQ	= BRDctrl_CMN + 11,		// clear interrupt by status register (U32)

        BRDctrl_REG_ILLEGAL
};
    [StructLayout(LayoutKind.Sequential)]
    public struct BRD_Reg 
    {
	    public Int32 tetr;
        public Int32 reg;
        public Int32 val;
        public IntPtr pBuf;
        public Int32 bytes; 
    }

    public class Service
    {
        internal Int32 _h;
        internal UInt32 nodeId = 0;

        ~Service()
        {
            BRD_API.BRD_release(_h, nodeId);
        }

        public Int32 heandle
        {
            get { return _h; }
        }

        public Int32 ctrl<T>(UInt32 cmd, ref T arg)
        {
            return BRD_API.BRD_ctrl(_h, 0, cmd, ref arg );
        }

    }

    public class RGIO :Service
    {
       


        public Int32 ri( Int32 tetr, Int32 reg )
        {
            BRD_Reg io = default( BRD_Reg);


            io.tetr = tetr;
            io.reg = reg;
            //io.bytes = 4;




            int err = BRD_API.BRD_ctrl(_h, 0, (uint)BRDctrl_REG.BRDctrl_REG_READIND, ref io);

           
            
            return io.val;
        }

        public void wi(Int32 tetr, Int32 reg, Int32 val )
        {
            BRD_Reg io = default(BRD_Reg);


            io.tetr = tetr;
            io.reg = reg;
            io.bytes = 4;

            io.val = val;


           

            int err = BRD_API.BRD_ctrl(_h, 0, (uint)BRDctrl_REG.BRDctrl_REG_WRITEIND, ref io );



            
        }

        public Int32 rd(Int32 tetr, Int32 reg)        
        {
            BRD_Reg io = default(BRD_Reg);


            io.tetr = tetr;
            io.reg = reg;
            //io.bytes = 4;




            int err = BRD_API.BRD_ctrl(_h, 0, (uint)BRDctrl_REG.BRDctrl_REG_READDIR, ref io);



            return io.val;
        }

        public void wd(Int32 tetr, Int32 reg, Int32 val )
        {
            BRD_Reg io = default(BRD_Reg);


            io.tetr = tetr;
            io.reg = reg;
            io.bytes = 4;

            io.val = val;


          

            int err = BRD_API.BRD_ctrl(_h, 0, (uint)BRDctrl_REG.BRDctrl_REG_WRITEDIR, ref io );


        }
    }

    public class LidList 
    {
        protected Dictionary<int, BRD_Info> _list = new Dictionary<int, BRD_Info>();

        public BRD_Info this[int index]
        {
            get { return _list[index]; }
            set { _list[index] = value; }
        }

        public IEnumerator<BRD_Info> GetEnumerator()
        {
            return _list.Values.GetEnumerator();
        }

        public Dictionary<int, BRD_Info>.KeyCollection Keys { get { return _list.Keys; } }

        public int Count { get { return _list.Count; } }
    }

    public class ServiceList
    {
        internal BRD_ServList[] _list;

        public BRD_ServList this[int index]
        {
            get { return _list[index]; }
            
        }



        public int Length { get { return _list.Length; } }
    }

    public class BRD
    {
        static BRD()
        {            
            Int32 nn = 0;

            BRD_API.BRD_init(null, out nn);

            _GetLidList();
        }

        private static void _GetLidList()
        {
            UInt32 pItemReal;

            BRD_API.BRD_lidList(null, 0, out pItemReal);

            UInt32[] pList = new UInt32[pItemReal];

            BRD_API.BRD_lidList(pList, pItemReal, out pItemReal);

            BRD_Info info = default(BRD_Info);

            _info = new LidList();

            for (int i = 0; i < pItemReal; i++)
            {




                info.size = (uint)Marshal.SizeOf(info);


                int err = BRD_API.BRD_getInfo(pList[i], ref  info);

                uint lid = pList[i];

                _info[(int)lid] = info;

            }

            _lidList = _info.Keys.ToArray<int>();
        }

        protected static LidList _info;

        protected static int[] _lidList;

        public static int[] lidList
        {
            get
            {
                return _lidList;
            }
        }
            
        public static LidList list
        {
            get
            {
                if (_info != null)
                    return _info;



                return _info;
            }
        }

        public BRD_Info getInfo()
        {
            BRD_Info info = default(BRD_Info);
            
            BRD_API.BRD_getInfo( (uint)_lid, ref  info);

            return info;
        }

        public static void cleanup()
        {
            BRD_API.BRD_cleanup( );
        }

        protected Int32 _h;
        protected Int32 _lid;

        protected BRD(string name)
        {
           

            UInt32 flag = 0;

            name = name.ToUpper();

            foreach (var lid in lidList)
            {
              
                
                if( list[lid].name.Contains( name ) )
                {
                   
                    
                    _h = BRD_API.BRD_open((UInt32)lid, flag, out flag);
                    _lid = lid;

                    return;
                }
            }

            
        }

        protected BRD(int lid = -1)
        {

            UInt32 flag = 0;

            if( lid == -1 )
            {
                UInt32 pItemReal;

                BRD_API.BRD_lidList(null, 0, out pItemReal);

                UInt32[] pList = new UInt32[10];

                BRD_API.BRD_lidList(pList, 10, out pItemReal);

                lid = (int)pList[0];
            }

            _h = BRD_API.BRD_open((UInt32)lid, flag, out flag); 
            _lid = lid;
        }

        ~BRD()
        {
            BRD_API.BRD_close(_h);
        }

        protected ServiceList _serviceList;

        public ServiceList serviceList
        {
            get
            {
                if (_serviceList != null)
                    return _serviceList;

                UInt32 pItemReal;

                BRD_ServList[] pList = null;

                BRD_API.BRD_serviceList(_h, 0, pList, 0, out pItemReal);

                pList = new BRD_ServList[pItemReal];

                BRD_API.BRD_serviceList(_h, 0, pList, pItemReal, out pItemReal);

                _serviceList = new ServiceList();

                _serviceList._list = pList;

                return _serviceList;
            }
        }

        public Int32 heandle
        {
            get { return _h;  }
        }

        protected RGIO _rgio = null;

        public RGIO rgio
        {
            get {
                
                if( _rgio == null )
                    _rgio = capture<RGIO>("REG0");

                return _rgio;
            
            
            }
           
        }

        public T capture<T>(string name, UInt32 pMode = 0 ) where T :Service,new()
        {
            Int32 _sh = BRD_API.BRD_capture(_h, 0, out pMode, "REG0", 10000 );

            Service s = new T();

            s._h = _sh;

            return (T)s;
        }




        public static BRD open( int lid = -1 )
        {
            return new BRD(lid);
        }

        protected BRD_PuList[] _puList;

        public BRD_PuList[] puList
        {
            get
            {
                if (_puList != null)
                    return _puList;

                UInt32 pItemReal;

                BRD_PuList[] pList = null;

                BRD_API.BRD_puList(_h,  pList, 0, out pItemReal);

                pList = new BRD_PuList[pItemReal];

                BRD_API.BRD_puList(_h, pList, pItemReal, out pItemReal);

                _puList = pList;

      

                return _puList;
            }
        }

        public int puRead(uint puId, int p, byte[] pBaseCfgMem, int size)
        {
            return BRD_API.BRD_puRead(_h, puId, (uint)p, pBaseCfgMem, (uint)size );
        }
    }
}
