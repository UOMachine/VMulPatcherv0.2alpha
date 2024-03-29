﻿using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Windows.Forms;
using System;
using Ultima;

namespace Ultima
{
    public sealed class ArtNonStatic
    {
        public event EventHandler SavingEvent;

        private FileIndex m_FileIndex = new FileIndex("Artidx.mul", "Art.mul", 0x13FDC, 4);
        private Bitmap[] m_Cache;
        private bool[] m_Removed;
        private Hashtable m_patched = new Hashtable();
        public bool Modified = false;

        private byte[] m_StreamBuffer;
        private byte[] Validbuffer;

        struct CheckSums
        {
            public byte[] checksum;
            public int pos;
            public int length;
            public int index;
        }

        private List<CheckSums> checksumsLand;
        private List<CheckSums> checksumsStatic;

        public ArtNonStatic()
        {
            m_Cache = new Bitmap[GetIdxLength()];
            m_Removed = new bool[GetIdxLength()];
        }

        public int GetMaxItemID()
        {
            if (GetIdxLength() == 0xC000)
                return 0x7FFF;

            if (GetIdxLength() == 0x13FDC)
                return 0xFFDB;

            return 0x3FFF;
        }

        public bool IsUOAHS()
        {
            return (GetIdxLength() == 0x13FDC);
        }

        public ushort GetLegalItemID(int itemID, bool checkmaxid = true)
        {
            if (itemID < 0)
                return 0;

            if (checkmaxid)
            {
                int max = GetMaxItemID();
                if (itemID > max)
                    return 0;
            }
            return (ushort)itemID;
        }

        public int GetIdxLength()
        {
            return (int)(m_FileIndex.IdxLength / 12);
        }
        /// <summary>
        /// ReReads Art.mul
        /// </summary>
        public void Reload()
        {
            m_FileIndex = new FileIndex("Artidx.mul", "Art.mul", 0x13FDC, 4);
            m_Cache = new Bitmap[GetIdxLength()];
            m_Removed = new bool[GetIdxLength()];
            m_patched.Clear();
            Modified = false;
        }

        /// <summary>
        /// Sets bmp of index in <see cref="m_Cache"/> of Static
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bmp"></param>
        public void ReplaceStatic(int index, Bitmap bmp)
        {
            index = Art.GetLegalItemID(index);
            index += 0x4000;

            m_Cache[index] = bmp;
            m_Removed[index] = false;
            if (m_patched.Contains(index))
                m_patched.Remove(index);
            Modified = true;
        }

        /// <summary>
        /// Sets bmp of index in <see cref="m_Cache"/> of Land
        /// </summary>
        /// <param name="index"></param>
        /// <param name="bmp"></param>
        public void ReplaceLand(int index, Bitmap bmp)
        {
            index &= 0x3FFF;
            m_Cache[index] = bmp;
            m_Removed[index] = false;
            if (m_patched.Contains(index))
                m_patched.Remove(index);
            Modified = true;
        }

        /// <summary>
        /// Removes Static index <see cref="m_Removed"/>
        /// </summary>
        /// <param name="index"></param>
        public void RemoveStatic(int index)
        {
            index = Art.GetLegalItemID(index);
            index += 0x4000;

            m_Removed[index] = true;
            Modified = true;
        }

        /// <summary>
        /// Removes Land index <see cref="m_Removed"/>
        /// </summary>
        /// <param name="index"></param>
        public void RemoveLand(int index)
        {
            index &= 0x3FFF;
            m_Removed[index] = true;
            Modified = true;
        }

        /// <summary>
        /// Tests if Static is definied (width and hight check)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public unsafe bool IsValidStatic(int index)
        {
            //index = GetLegalItemID(index);
            index += 0x4000;

            if (m_Removed[index])
                return false;
            if (m_Cache[index] != null)
                return true;

            int length, extra;
            bool patched;
            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);

            if (stream == null)
                return false;

            if (Validbuffer == null)
                Validbuffer = new byte[4];
            stream.Seek(4, SeekOrigin.Current);
            stream.Read(Validbuffer, 0, 4);
            fixed (byte* b = Validbuffer)
            {
                short* dat = (short*)b;
                if (*dat++ <= 0 || *dat <= 0)
                    return false;
                return true;
            }
        }

        /// <summary>
        /// Tests if LandTile is definied
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool IsValidLand(int index)
        {
            index &= 0x3FFF;
            if (m_Removed[index])
                return false;
            if (m_Cache[index] != null)
                return true;

            int length, extra;
            bool patched;

            return m_FileIndex.Valid(index, out length, out extra, out patched);
        }

        /// <summary>
        /// Returns Bitmap of LandTile (with Cache)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Bitmap GetLand(int index)
        {
            bool patched;
            return GetLand(index, out patched);
        }
        /// <summary>
        /// Returns Bitmap of LandTile (with Cache) and verdata bool
        /// </summary>
        /// <param name="index"></param>
        /// <param name="patched"></param>
        /// <returns></returns>
        public Bitmap GetLand(int index, out bool patched)
        {
            index &= 0x3FFF;
            if (m_patched.Contains(index))
                patched = (bool)m_patched[index];
            else
                patched = false;

            if (m_Removed[index])
                return null;
            if (m_Cache[index] != null)
                return m_Cache[index];

            int length, extra;
            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);
            if (stream == null)
                return null;
            if (patched)
                m_patched[index] = true;

            if (Files.CacheData)
                return m_Cache[index] = LoadLand(stream, length);
            else
                return LoadLand(stream, length);
        }

        public byte[] GetRawLand(int index)
        {
            index &= 0x3FFF;

            int length, extra;
            bool patched;
            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);
            if (stream == null)
                return null;
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            stream.Close();
            return buffer;
        }

        /// <summary>
        /// Returns Bitmap of Static (with Cache)
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Bitmap GetStatic(int index, bool checkmaxid = true)
        {
            bool patched;
            return GetStatic(index, out patched, checkmaxid);
        }
        /// <summary>
        /// Returns Bitmap of Static (with Cache) and verdata bool
        /// </summary>
        /// <param name="index"></param>
        /// <param name="patched"></param>
        /// <returns></returns>
        public Bitmap GetStatic(int index, out bool patched, bool checkmaxid = true)
        {
            //index = GetLegalItemID(index, checkmaxid);
            index += 0x4000;

            if (m_patched.Contains(index))
                patched = (bool)m_patched[index];
            else
                patched = false;

            if (m_Removed[index])
                return null;
            if (m_Cache[index] != null)
                return m_Cache[index];

            int length, extra;
            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);
            if (stream == null)
                return null;
            if (patched)
                m_patched[index] = true;

            if (Files.CacheData)
                return m_Cache[index] = LoadStatic(stream, length);
            else
                return LoadStatic(stream, length);
        }

        public byte[] GetRawStatic(int index)
        {
            index = GetLegalItemID(index);
            index += 0x4000;

            int length, extra;
            bool patched;
            Stream stream = m_FileIndex.Seek(index, out length, out extra, out patched);
            if (stream == null)
                return null;
            byte[] buffer = new byte[length];
            stream.Read(buffer, 0, length);
            stream.Close();
            return buffer;
        }

        public unsafe void Measure(Bitmap bmp, out int xMin, out int yMin, out int xMax, out int yMax)
        {
            xMin = yMin = 0;
            xMax = yMax = -1;

            if (bmp == null || bmp.Width <= 0 || bmp.Height <= 0)
                return;

            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);

            int delta = (bd.Stride >> 1) - bd.Width;
            int lineDelta = bd.Stride >> 1;

            ushort* pBuffer = (ushort*)bd.Scan0;
            ushort* pLineEnd = pBuffer + bd.Width;
            ushort* pEnd = pBuffer + (bd.Height * lineDelta);

            bool foundPixel = false;

            int x = 0, y = 0;

            while (pBuffer < pEnd)
            {
                while (pBuffer < pLineEnd)
                {
                    ushort c = *pBuffer++;

                    if ((c & 0x8000) != 0)
                    {
                        if (!foundPixel)
                        {
                            foundPixel = true;
                            xMin = xMax = x;
                            yMin = yMax = y;
                        }
                        else
                        {
                            if (x < xMin)
                                xMin = x;

                            if (y < yMin)
                                yMin = y;

                            if (x > xMax)
                                xMax = x;

                            if (y > yMax)
                                yMax = y;
                        }
                    }
                    ++x;
                }

                pBuffer += delta;
                pLineEnd += lineDelta;
                ++y;
                x = 0;
            }

            bmp.UnlockBits(bd);
        }

        private unsafe Bitmap LoadStatic(Stream stream, int length)
        {
            Bitmap bmp;
            if (m_StreamBuffer == null || m_StreamBuffer.Length < length)
                m_StreamBuffer = new byte[length];
            stream.Read(m_StreamBuffer, 0, length);
            stream.Close();

            fixed (byte* data = m_StreamBuffer)
            {
                ushort* bindata = (ushort*)data;
                int count = 2;
                //bin.ReadInt32();
                int width = bindata[count++];
                int height = bindata[count++];

                if (width <= 0 || height <= 0 || width > 1024 || height > 1024)
                    return null;

                int[] lookups = new int[height];

                int start = (height + 4);

                for (int i = 0; i < height; ++i)
                    lookups[i] = (int)(start + (bindata[count++]));

                bmp = new Bitmap(width, height, PixelFormat.Format16bppArgb1555);
                BitmapData bd = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);


                ushort* line = (ushort*)bd.Scan0;
                int delta = bd.Stride >> 1;


                for (int y = 0; y < height; ++y, line += delta)
                {
                    count = lookups[y];

                    ushort* cur = line;
                    ushort* end;
                    int xOffset, xRun;

                    while (((xOffset = bindata[count++]) + (xRun = bindata[count++])) != 0)
                    {
                        if (xOffset > delta)
                            break;
                        cur += xOffset;
                        if (xOffset + xRun > delta)
                            break;
                        end = cur + xRun;

                        while (cur < end)
                            *cur++ = (ushort)(bindata[count++] ^ 0x8000);
                    }
                }
                bmp.UnlockBits(bd);
            }
            return bmp;
        }

        private unsafe Bitmap LoadLand(Stream stream, int length)
        {
            Bitmap bmp = new Bitmap(44, 44, PixelFormat.Format16bppArgb1555);
            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, 44, 44), ImageLockMode.WriteOnly, PixelFormat.Format16bppArgb1555);
            if (m_StreamBuffer == null || m_StreamBuffer.Length < length)
                m_StreamBuffer = new byte[length];
            stream.Read(m_StreamBuffer, 0, length);
            stream.Close();
            fixed (byte* bindata = m_StreamBuffer)
            {
                ushort* bdata = (ushort*)bindata;
                int xOffset = 21;
                int xRun = 2;

                ushort* line = (ushort*)bd.Scan0;
                int delta = bd.Stride >> 1;

                for (int y = 0; y < 22; ++y, --xOffset, xRun += 2, line += delta)
                {
                    ushort* cur = line + xOffset;
                    ushort* end = cur + xRun;

                    while (cur < end)
                        *cur++ = (ushort)(*bdata++ | 0x8000);
                }

                xOffset = 0;
                xRun = 44;

                for (int y = 0; y < 22; ++y, ++xOffset, xRun -= 2, line += delta)
                {
                    ushort* cur = line + xOffset;
                    ushort* end = cur + xRun;

                    while (cur < end)
                        *cur++ = (ushort)(*bdata++ | 0x8000);
                }
            }
            bmp.UnlockBits(bd);
            return bmp;
        }


        /// <summary>
        /// Saves mul
        /// </summary>
        /// <param name="path"></param>
        public unsafe void Save(string path)
        {
            checksumsLand = new List<CheckSums>();
            checksumsStatic = new List<CheckSums>();
            string idx = Path.Combine(path, "artidx.mul");
            string mul = Path.Combine(path, "art.mul");
            using (FileStream fsidx = new FileStream(idx, FileMode.Create, FileAccess.Write, FileShare.Write),
                              fsmul = new FileStream(mul, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                MemoryStream memidx = new MemoryStream();
                MemoryStream memmul = new MemoryStream();
                SHA256Managed sha = new SHA256Managed();
                //StreamWriter Tex = new StreamWriter(new FileStream("d:/artlog.txt", FileMode.Create, FileAccess.ReadWrite));

                using (BinaryWriter binidx = new BinaryWriter(memidx),
                                    binmul = new BinaryWriter(memmul))
                {
                    for (int index = 0; index < GetIdxLength(); index++)
                    {
                        Bitmap bmp;
                        if (m_Cache[index] == null)
                        {
                            if (index < 0x4000)
                                bmp = GetLand(index);
                            else
                                bmp = GetStatic(index - 0x4000, false);
                        }
                        else
                            bmp = m_Cache[index];
                        if (bmp != null)
                        {
                            if (bmp.Width > 1024 || bmp.Height > 1024)
                                continue;
                        }
                        if ((bmp == null) || (m_Removed[index]))
                        {
                            binidx.Write((int)-1); // lookup
                            binidx.Write((int)0); // length
                            binidx.Write((int)-1); // extra
                            //Tex.WriteLine(System.String.Format("0x{0:X4} : 0x{1:X4} 0x{2:X4}", index, (int)-1, (int)-1));
                        }
                        else if (index < 0x4000)
                        {
                            //land
                            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
                            ushort* line = (ushort*)bd.Scan0;
                            int delta = bd.Stride >> 1;
                            binidx.Write((int)binmul.BaseStream.Position); //lookup
                            int length = (int)binmul.BaseStream.Position;
                            int x = 22;
                            int y = 0;
                            int linewidth = 2;
                            for (int m = 0; m < 22; ++m, ++y, line += delta, linewidth += 2)
                            {
                                --x;
                                ushort* cur = line;
                                for (int n = 0; n < linewidth; ++n)
                                    binmul.Write((ushort)(cur[x + n] ^ 0x8000));
                            }
                            x = 0;
                            linewidth = 44;
                            y = 22;
                            line = (ushort*)bd.Scan0;
                            line += delta * 22;
                            for (int m = 0; m < 22; m++, y++, line += delta, ++x, linewidth -= 2)
                            {
                                ushort* cur = line;
                                for (int n = 0; n < linewidth; n++)
                                    binmul.Write((ushort)(cur[x + n] ^ 0x8000));
                            }
                            int start = length;
                            length = (int)binmul.BaseStream.Position - length;
                            binidx.Write(length);
                            binidx.Write((int)0);
                            bmp.UnlockBits(bd);
                        }
                        else
                        {
                            // art
                            BitmapData bd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format16bppArgb1555);
                            ushort* line = (ushort*)bd.Scan0;
                            int delta = bd.Stride >> 1;
                            binidx.Write((int)binmul.BaseStream.Position); //lookup
                            int length = (int)binmul.BaseStream.Position;
                            binmul.Write((int)1234); // header
                            binmul.Write((short)bmp.Width);
                            binmul.Write((short)bmp.Height);
                            int lookup = (int)binmul.BaseStream.Position;
                            int streamloc = lookup + bmp.Height * 2;
                            int width = 0;
                            for (int i = 0; i < bmp.Height; ++i)// fill lookup
                                binmul.Write(width);
                            int X = 0;
                            for (int Y = 0; Y < bmp.Height; ++Y, line += delta)
                            {
                                ushort* cur = line;
                                width = (int)(binmul.BaseStream.Position - streamloc) / 2;
                                binmul.BaseStream.Seek(lookup + Y * 2, SeekOrigin.Begin);
                                binmul.Write(width);
                                binmul.BaseStream.Seek(streamloc + width * 2, SeekOrigin.Begin);
                                int i = 0;
                                int j = 0;
                                X = 0;
                                while (i < bmp.Width)
                                {
                                    i = X;
                                    for (i = X; i <= bmp.Width; ++i)
                                    {
                                        //first pixel set
                                        if (i < bmp.Width)
                                        {
                                            if (cur[i] != 0)
                                                break;
                                        }
                                    }
                                    if (i < bmp.Width)
                                    {
                                        for (j = (i + 1); j < bmp.Width; ++j)
                                        {
                                            //next non set pixel
                                            if (cur[j] == 0)
                                                break;
                                        }
                                        binmul.Write((short)(i - X)); //xoffset
                                        binmul.Write((short)(j - i)); //run
                                        for (int p = i; p < j; ++p)
                                            binmul.Write((ushort)(cur[p] ^ 0x8000)); // 79236 nefugnuje
                                        X = j;
                                    }
                                }
                                binmul.Write((short)0); //xOffset
                                binmul.Write((short)0); //Run
                            }
                            length = (int)binmul.BaseStream.Position - length;
                            binidx.Write(length);
                            binidx.Write((int)0);
                            bmp.UnlockBits(bd);
                        }
                        bmp = null;
                        m_Cache[index] = null;
                        if (SavingEvent != null)
                            SavingEvent(index, null);
                    }
                    memidx.WriteTo(fsidx);
                    memmul.WriteTo(fsmul);
                }
            }
        }

        private bool compareSaveImagesLand(byte[] newchecksum, out CheckSums sum)
        {
            sum = new CheckSums();
            for (int i = 0; i < checksumsLand.Count; ++i)
            {
                byte[] cmp = checksumsLand[i].checksum;
                if (((cmp == null) || (newchecksum == null))
                    || (cmp.Length != newchecksum.Length))
                {
                    return false;
                }
                bool valid = true;
                for (int j = 0; j < cmp.Length; ++j)
                {
                    if (cmp[j] != newchecksum[j])
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    sum = checksumsLand[i];
                    return true;
                }
            }
            return false;
        }
        private bool compareSaveImagesStatic(byte[] newchecksum, out CheckSums sum)
        {
            sum = new CheckSums();
            for (int i = 0; i < checksumsStatic.Count; ++i)
            {
                byte[] cmp = checksumsStatic[i].checksum;
                if (((cmp == null) || (newchecksum == null))
                    || (cmp.Length != newchecksum.Length))
                {
                    return false;
                }
                bool valid = true;
                for (int j = 0; j < cmp.Length; ++j)
                {
                    if (cmp[j] != newchecksum[j])
                    {
                        valid = false;
                        break;
                    }
                }
                if (valid)
                {
                    sum = checksumsStatic[i];
                    return true;
                }
            }
            return false;
        }
    }
}
