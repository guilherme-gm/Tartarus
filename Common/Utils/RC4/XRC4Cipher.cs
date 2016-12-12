/**
* This file is part of Tartarus Emulator.
* 
* Tartarus is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
* 
* Tartarus is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
* GNU General Public License for more details.
* 
* You should have received a copy of the GNU General Public License
* along with Tartarus.  If not, see<http://www.gnu.org/licenses/>.
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.RC4
{
    public class XRC4Cipher
    {
        RC4Cipher m_pImpl;

        public XRC4Cipher(string pKey)
        {
            m_pImpl = new RC4Cipher();
            Clear();
            m_pImpl.Init(pKey);
        }

        public byte[] Peek(ref byte[] pSrc, int len)
        {
            RC4Cipher.State backup;
            m_pImpl.SaveStateTo(out backup);

            byte[] pDst = DoCipher(ref pSrc, len);

            m_pImpl.LoadStateFrom(ref backup);

            return pDst;
        }

        public byte[] DoCipher(ref byte[] pSrc, int len = 0)
        {
            if (len == 0) len = pSrc.Length;
            byte[] pDst = m_pImpl.Code(ref pSrc, len);

            return pDst;
        }

        public byte[] Encode(ref byte[] pSrc, int len, bool bIsPeek = false)
        {
            if (bIsPeek)
                return Peek(ref pSrc, len);
            else
                return DoCipher(ref pSrc, len);
        }

        public byte[] Decode(ref byte[] pSrc, int len, bool bIsPeek = false)
        {
            if (bIsPeek)
                return Peek(ref pSrc, len);
            else
                return DoCipher(ref pSrc, len);
        }

        public void Clear()
        {
            m_pImpl.Init("Neat & Simple");
        }
    }
}