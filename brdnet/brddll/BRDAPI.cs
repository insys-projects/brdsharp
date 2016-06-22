using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;
using brd;

namespace brd_internal
{
    class BRD_API
    {

#if BRD64
        const string brddll = "brd64.dll";
        const CharSet brdcharset = CharSet.Unicode;
#else
        const string brddll = "brd.dll";
        const CharSet brdcharset = CharSet.ASCII;
#endif


        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_init(String format, out Int32 i);


        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_initEx(UInt32 mode, IntPtr pSrc, String logFile, out Int32 pNum);


        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_reinit( out Int32 pNum );

        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_cleanup();


        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_lidList(UInt32[] pList, UInt32 item, out UInt32 pItemReal);
        

        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_open(UInt32 lid, UInt32 flag, out UInt32 flagReal);

        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_close(Int32 h);


        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_getInfo(UInt32 lid, ref  BRD_Info pInfo);

    
        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_load(Int32 h, UInt32 nodeId, string fileName, Int32 argc, string[] argv);


        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_puLoad(Int32 h, UInt32 nodeId, string fileName, out UInt32 state );


        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_puState(Int32 h, UInt32 puId, out UInt32 state );


        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_puList(Int32 h, UInt32 puId, out UInt32 state);

        //BRD_API S32		STDCALL BRD_puRead (BRD_Handle handle, U32 puId, U32 offset, void *hostAdr, U32 size );
        //BRD_API S32		STDCALL BRD_puWrite(BRD_Handle handle, U32 puId, U32 offset, void *hostAdr, U32 size );
        //BRD_API S32		STDCALL BRD_puEnable(BRD_Handle handle, U32 puId );


        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_reset(Int32 h, UInt32 nodeId);

        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_start(Int32 h, UInt32 nodeId);

        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_stop(Int32 h, UInt32 nodeId);

        //BRD_API S32		STDCALL BRD_symbol (BRD_Handle handle, const BRDCHAR *fileName, const BRDCHAR *symbName, U32 *val );
        //BRD_API S32		STDCALL BRD_version(BRD_Handle handle, BRD_Version *pVersion );


        [DllImport(brddll, CallingConvention = CallingConvention.StdCall, CharSet = brdcharset )]
        public static extern Int32 BRD_capture(Int32 h, UInt32 nodeId, out UInt32 pMode, string servName, Int32 timeout);

        //[DllImport(brddll, CallingConvention = CallingConvention.StdCall, EntryPoint="BRD_ctrl")]
        //private static extern Int32 _BRD_ctrl(Int32 h, UInt32 nodeId, UInt32 cmd, IntPtr arg);
        
        [DllImport(brddll, CallingConvention = CallingConvention.StdCall, EntryPoint = "BRD_ctrl")]
        private static extern Int32 _BRD_ctrl(Int32 h, UInt32 nodeId, UInt32 cmd, IntPtr arg);

        public static Int32 BRD_ctrl<T>(Int32 h, UInt32 nodeId, UInt32 cmd, ref T arg)
        {
            int structSizeInBytes = Marshal.SizeOf(typeof(T));

            IntPtr aBunchOfParametersPtr = Marshal.AllocHGlobal(structSizeInBytes);

            Marshal.StructureToPtr(arg, aBunchOfParametersPtr, false);

            int result = _BRD_ctrl(h, nodeId, cmd, aBunchOfParametersPtr);

            arg = (T)Marshal.PtrToStructure(aBunchOfParametersPtr, typeof(T));
            
            Marshal.FreeHGlobal(aBunchOfParametersPtr);

            return result;
        }

        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_release(Int32 h, UInt32 nodeId);


        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_serviceList(Int32 h, UInt32 nodeId, [In,Out] BRD_ServList[] pList, UInt32 item, out UInt32 pItemReal);


        [DllImport(brddll, CallingConvention = CallingConvention.StdCall)]
        public static extern Int32 BRD_ctrl(Int32 h, UInt32 nodeId, UInt32 cmd, IntPtr arg );



    }

}
