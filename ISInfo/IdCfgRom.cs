using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using brd;

namespace ISInfo
{
    public class IdCfgRom
    {
        const int BASE_ID_TAG = 0x4953;

        const int BASEMOD_CFGMEM_SIZE = 256;

        static public byte[] ReadIdCfgRom(brd.BRD b, string bDevs)
        {
            BRD_PuList[] PuList = b.puList;

            uint puBaseIcrId = 0;

            for (int ii = 0; ii < PuList.Length; ii++)
            {
                if (PuList[ii].puCode == BASE_ID_TAG)
                {
                    puBaseIcrId = PuList[ii].puId;
                } 
                
                if (PuList[ii].puCode == 0x1)
                {
                    puBaseIcrId = PuList[ii].puId;
                }
            }

            byte[] pBaseCfgMem = null;
            
            if (puBaseIcrId!=0)
            {
                pBaseCfgMem = new byte[BASEMOD_CFGMEM_SIZE];
                int err = b.puRead(puBaseIcrId, 0, pBaseCfgMem, BASEMOD_CFGMEM_SIZE);
            }

            return pBaseCfgMem;
        }
    }
}
