using System;
using System.Text;

namespace AIMCSU
{
    public class AIMCSUv1
    {
        Char[] char_ = new char[1114112];
        int[] index_ = new int[1114112];
        int[] useOfChar = new int[1114112];
        int index = 1;
        public bool needResort = false;
        public int beta = 1000;
        public double alpha = 0.1;
        int CmpTable(Char inChar)
        {
            for (int i = 0; i < index; i++)
                if (char_[i] == inChar)
                {
                    useOfChar[i]++;
                    return index_[i];
                }
            return -1;
        }
        public void checkUseOfChar()
        {

            {
                double tt = decisionSortIndexUse(useOfChar, index);

                if (tt > alpha)
                {
                    Resort();
                    needResort = true;
                }
                else
                {
                    for (int i = 1; i < index; i++)
                    {
                        useOfChar[i] = 0;
                    }
                }

            }
        }
        double decisionSortIndexUse(int[] indexUse, int indexUseLength)
        {
            double sum = 0;
            for (int i = 0; i < indexUseLength && i < indexUse.Length - 1; i++)
                for (int j = i + 1; j < indexUseLength && j < indexUse.Length; j++)
                    if (indexUse[i] < indexUse[j])
                        sum += 1;
            return (sum / (((indexUseLength - 1) * indexUseLength) / 2));
        }
        public void Resort()
        {
            Sorting(useOfChar, char_, index_, 1, index);
        }
        public byte[] makeTableforSending()
        {
            string Rez = "0";
            Rez += convertorInt2Bin(0, 5, 0);
            for (int i = 1; i < index; i++)
                Rez += convertorInt2Bin(char_[i], 16, 0);
            Rez += "000";
            return convertorByteString2byte(Rez);
        }
        int FindTable(char Inchar)
        {
            for (int i = 1; i < char_.Length && char_[i] != 0; i++)
                if (Inchar == char_[i])
                {


                    return index_[i];
                }
            return -1;
        }
        void addTable(char In)
        {
            useOfChar[index] = 0;
            char_[index] = In;
            index_[index++] = index - 1;

            return;
        }
        string convertorInt2Bin(int In, int LengthStr, int mood)
        {
            string Temp = "";
            for (; In > 1; )
            {
                Temp += (In % 2).ToString();
                In = Convert.ToInt16(In / 2);
            }
            Temp += In;
            string _Rez = "";
            if (mood == 0)
                for (int i = Temp.Length; i < LengthStr; i++)
                    Temp += '0';

            for (int i = Temp.Length - 1; i >= 0; i--)
                _Rez += Temp[i];
            if (mood == 2)
                for (int i = _Rez.Length; i < LengthStr; i++)
                    _Rez = '0' + _Rez;
            return _Rez;
        }
        public byte[] Compress(String InStr)
        {
            int Indexer = 0;
            String[] NewLine = new String[InStr.Length];
            String Rezalt = "";
            int[] IndexTableOrChar = new int[InStr.Length]; int IndexArrTableOrChar = 0;
            int[] CharLength = new int[InStr.Length]; int IndexArrCharOrIndexLength = 0;
            int[] IndexLength = new int[InStr.Length];
            for (int i = 0; i < InStr.Length; i++)
            {
                Indexer = CmpTable(InStr[i]);

                if (-1 == Indexer)
                {
                    IndexTableOrChar[IndexArrTableOrChar++] = 1;
                    addTable(InStr[i]);

                }
                else
                {
                    IndexLength[IndexArrCharOrIndexLength] = convertorInt2Bin(Indexer, 0, 1).Length;
                    IndexTableOrChar[IndexArrTableOrChar++] = 0;
                }

                CharLength[IndexArrCharOrIndexLength++] = convertorInt2Bin(Convert.ToInt32(InStr[i]), 0, 1).Length;
            }

            {
                int ContN = 0, IndexN = 0, MaxN;
                int ContM = 0, IndexM = 0, MaxM;
                int IndexArr = 0;
                for (int i = 0; i < InStr.Length; )
                {
                    MaxM = 0; MaxN = 0;
                    ContN = 0;
                    ContM = 0;
                    IndexN = i;
                    for (; i < InStr.Length && IndexTableOrChar[i] != 0; i++)
                    {
                        ContN++;
                        if (MaxN < CharLength[i])
                            MaxN = CharLength[i];
                    }
                    IndexM = i;
                    for (; i < InStr.Length && IndexTableOrChar[i] != 1; i++)
                    {
                        ContM++;
                        if (MaxM < IndexLength[i])
                            MaxM = IndexLength[i];
                        if (MaxN < CharLength[i])
                            MaxN = CharLength[i];
                    }
                    if (((MaxN * (ContM + ContN + 1)) + 5) <= ((MaxN * (ContN + 1)) + (MaxM * (ContM + 1)) + MaxN + 10))
                    {
                        NewLine[IndexArr] = "0";
                        for (int j = IndexN; j < IndexN + ContM + ContN; j++)
                            NewLine[IndexArr] += InStr[j];
                        IndexArr++;
                    }
                    else
                    {
                        if (IndexN + ContN > 0)
                        {
                            NewLine[IndexArr] = "0";
                            for (int j = IndexN; j < IndexN + ContN; j++)
                                NewLine[IndexArr] += InStr[j];
                            IndexArr++;
                        }
                        NewLine[IndexArr] = "1";
                        for (int j = IndexM; j < IndexM + ContM; j++)
                            NewLine[IndexArr] += InStr[j];
                        IndexArr++;

                    }

                }
            }
            ///////////////////////////////////////////////////////////////
            if (NewLine.Length > 1)
            {
                for (int i = 0; i < NewLine.Length && NewLine[i + 1] != null; i++)
                {
                    if (NewLine[i][0] == '0' && NewLine[i + 1][0] == '0')
                    {
                        for (int k = 1; k < NewLine[i + 1].Length; k++)
                            NewLine[i] += NewLine[i + 1][k];
                        int z = i + 1;
                        for (; z < NewLine.Length - 1 && NewLine[z] != null; z++)
                        {
                            NewLine[z] = NewLine[z + 1];
                        }
                        NewLine[z] = null;
                        i--;
                    }
                    else
                        if (NewLine[i][0] == '1' && NewLine[i + 1][1] == '0')
                        {
                            for (int k = 1; k < NewLine[i + 1].Length; k++)
                                NewLine[i] += NewLine[i + 1][k];
                            int z = i + 1;
                            for (; z < NewLine.Length - 1 && NewLine[z] != null; z++)
                            {
                                NewLine[z] = NewLine[z + 1];
                            }
                            NewLine[z] = null;
                            i--;
                        }
                }
            }
            //////////////////////////////////////////////////////////////
            {
                int indexArr = 0;
                for (int i = 0; i < NewLine.Length && NewLine[i] != null; i++)
                {

                    indexArr = 0;
                    int[] indexLengthCalculat = new int[NewLine[i].Length - 1];
                    if (NewLine[i][0] == '1')
                    {
                        for (int k = 1; k < NewLine[i].Length; k++)
                        {
                            Indexer = CmpTable(NewLine[i][k]);
                            indexLengthCalculat[indexArr++] = convertorInt2Bin(Indexer, 0, 1).Length;
                        }
                    }
                    else
                    {
                        for (int k = 1; k < NewLine[i].Length; k++)
                        {
                            indexLengthCalculat[indexArr++] = convertorInt2Bin(Convert.ToInt32(InStr[i]), 0, 1).Length;
                        }
                    }
                    int Index_Sp = 0;
                    int[] SpletArr = new int[indexLengthCalculat.Length + 2]; int indexSpletArr = 0;
                    int[] IndexSpletArr = new int[indexLengthCalculat.Length + 2];
                    int Sum = 0; int l = 0;
                    for (; l < indexLengthCalculat.Length; l++)
                    {
                        int[] StatNew = Splet(indexLengthCalculat, Index_Sp, Sum);
                        Sum = StatNew[2];
                        if (StatNew[1] < 0)
                            SpletArr[indexSpletArr] = 0;
                        else
                            SpletArr[indexSpletArr] = StatNew[1];
                        if (StatNew[3] != -1)
                            IndexSpletArr[indexSpletArr++] = StatNew[3] - 1;
                        else
                            IndexSpletArr[indexSpletArr++] = StatNew[0] - 1;
                        l = Index_Sp = StatNew[0];
                    }


                    for (l = 0; l < indexSpletArr && l < IndexSpletArr.Length; l++)
                    {
                        if (SpletArr[l] == 0)
                        {
                            for (; SpletArr.Length > 1 && l < indexSpletArr && SpletArr[l] == SpletArr[l + 1]; )
                            {
                                for (int k = l; k < SpletArr.Length - 1; k++)
                                {
                                    SpletArr[k] = SpletArr[k + 1];
                                    IndexSpletArr[k] = IndexSpletArr[k + 1];
                                }
                                indexSpletArr--;
                            }
                        }


                    }
                    String TempStr = "";
                    if (indexSpletArr != 0 && IndexSpletArr[indexSpletArr - 1] != NewLine[i].Length)
                    {
                        IndexSpletArr[indexSpletArr] = NewLine[i].Length;
                        indexSpletArr++;

                    }
                    if (indexSpletArr == 0)
                    {
                        TempStr = productionString(NewLine[i], 1, NewLine[i].Length);
                        if (NewLine[i][0] == '1')
                        {
                            Rezalt += CompressStr(TempStr, 1, true);
                        }
                        else
                        {
                            Rezalt += CompressStr(TempStr, 0, true);
                        }
                    }
                    bool Fl1 = false;
                    for (l = 0; l < indexSpletArr && l < IndexSpletArr.Length; l++)
                    {

                        if (SpletArr[l] == 0)
                            if (l == 0)
                                TempStr = productionString(NewLine[i], 1, IndexSpletArr[l]);
                            else
                            {
                                if (Fl1)
                                    TempStr = productionString(NewLine[i], IndexSpletArr[l - 1], IndexSpletArr[l]);
                                else
                                    TempStr = productionString(NewLine[i], IndexSpletArr[l - 1], IndexSpletArr[l]);
                            }
                        else
                        {

                            if (l == 0)
                            {
                                TempStr = productionString(NewLine[i], 1, IndexSpletArr[l]);
                                Fl1 = true;
                            }
                            else
                            {
                                TempStr = productionString(NewLine[i], IndexSpletArr[l - 1], IndexSpletArr[l]);

                            }
                        }
                        if (NewLine[i][0] == '1')
                        {
                            Rezalt += CompressStr(TempStr, 1, true);
                        }
                        else
                        {
                            Rezalt += CompressStr(TempStr, 0, true);
                        }
                    }

                }
            }
            return convertorByteString2byte(Rezalt);
        }
        String productionString(String Str, int A, int B)
        {
            String Temp = "";
            for (; A < B; A++)
                Temp += Str[A];
            return Temp;
        }
        String CompressStr(String InStr, int mode, bool AddNall)
        {
            int Indexer = 0;
            String Rez = "";
            bool CompressIsMood = true;
            if (mode == 0)
                CompressIsMood = false;
            int MaxLength = 0, TempLength = 0;
            for (int i = 0; i < InStr.Length; i++)
            {

                Indexer = CmpTable(InStr[i]);

                if (MaxLength < Indexer)
                    MaxLength = Indexer;
                if (TempLength < Convert.ToInt32(InStr[i]))
                    TempLength = Convert.ToInt32(InStr[i]);
            }
            if (CompressIsMood)
            {
                MaxLength = convertorInt2Bin(MaxLength, 0, 1).Length;
                Rez = "1";
                Rez += convertorInt2Bin(MaxLength, 5, 0);
            }
            else
            {

                MaxLength = convertorInt2Bin(TempLength, 0, 1).Length;
                Rez = "0";
                Rez += convertorInt2Bin(MaxLength, 5, 0);

            }
            for (int i = 0; i < InStr.Length; i++)
            {

                if (CompressIsMood)
                {
                    Indexer = CmpTable(InStr[i]);
                    if (i == InStr.Length - 1)
                        Rez += convertorInt2Bin(Indexer, MaxLength, 2);
                    else
                        Rez += convertorInt2Bin(Indexer, MaxLength, 0);
                }
                else
                {
                    if (i == InStr.Length - 1)
                        Rez += convertorInt2Bin(Convert.ToInt32(InStr[i]), MaxLength, 2);
                    else
                        Rez += convertorInt2Bin(Convert.ToInt32(InStr[i]), MaxLength, 0);
                }
            }
            if (AddNall)
                for (int k = 0; k < MaxLength; k++)
                    Rez += '0';
            return Rez;
        }
        int[] Splet(int[] indexLengthCalculat, int IndexStart, int Sum)
        {
            int Max = 0;
            int indexArr = 0;
            int[] maxCalculat = new int[indexLengthCalculat.Length + 1];
            int[] CalculatSize = new int[indexLengthCalculat.Length + 1];
            int[] indexCalculat = new int[indexLengthCalculat.Length + 3];

            indexArr = 0;
            indexCalculat[indexArr] = IndexStart + 1;
            Max = maxCalculat[indexArr++] = indexLengthCalculat[IndexStart];
            for (int k = IndexStart + 1; k < indexLengthCalculat.Length; k++)
            {
                if (Max < indexLengthCalculat[k])
                {
                    {
                        Max = maxCalculat[indexArr] = indexLengthCalculat[k];
                        indexCalculat[indexArr++] = k + 1;
                    }
                }
            }



            int kl = 0;
            double MaxL = double.MinValue;
            int BestIndex = -1;
            for (; kl < maxCalculat.Length && maxCalculat[kl] != 0; kl++)
            {
                if (maxCalculat[kl + 1] == 0)
                {

                    if (kl == 0)
                        MaxL = -1;
                    break;
                }
                CalculatSize[kl] = ((maxCalculat[kl + 1] * (indexCalculat[kl + 1] - IndexStart)) + 6) - (maxCalculat[kl] * (indexCalculat[kl + 1] - IndexStart) + maxCalculat[kl + 1] + 12) + Sum;
                if (MaxL < CalculatSize[kl])
                {
                    MaxL = CalculatSize[kl];
                    BestIndex = indexCalculat[kl + 1];

                }
            }
            for (int i = indexCalculat[kl]; indexCalculat[kl] - 1 >= 0 && i < indexLengthCalculat.Length && indexLengthCalculat[indexCalculat[kl] - 1] == indexLengthCalculat[i]; i++)
                indexCalculat[kl]++;
            if (indexArr == 1)
                indexCalculat[0] = indexLengthCalculat.Length;
            else
                indexCalculat[0] = indexCalculat[kl];
            if (MaxL > 0)
                indexCalculat[2] = Convert.ToInt32(MaxL);
            else
                indexCalculat[2] = 0;
            {
                indexCalculat[1] = Convert.ToInt32(MaxL);
                indexCalculat[2] += -maxCalculat[--indexArr];
                indexCalculat[3] = BestIndex;
            }


            return indexCalculat;
        }
        byte[] convertorByteString2byte(string byteStr)
        {
            byte[] Rez;
            int IndexArr = 0;

            for (int i = byteStr.Length - 1; i > 0 && byteStr[i] == '0'; i--, IndexArr++) ;
            string temper = "";
            for (int i = 0; i < byteStr.Length - IndexArr; i++)
                temper += byteStr[i];
            byteStr = temper;
            IndexArr = 0;
            if (0 == byteStr.Length % 8)
                Rez = new byte[byteStr.Length / 8];
            else
                Rez = new byte[(byteStr.Length / 8) + 1];
            string Temp = "";
            for (int i = 0; i < byteStr.Length; i += 8)
            {
                Temp = "";
                for (int j = i; j < i + 8 && j < byteStr.Length; j++)
                    Temp += byteStr[j];
                for (int k = Temp.Length; k < 8; k++)
                    Temp += '0';
                Rez[IndexArr++] = Convert.ToByte(convertorBin2Int(Temp));
            }
            return Rez;
        }
        Char[] convertorByteString2Unicode16bit(string byteStr)
        {
            Char[] Rez;
            int IndexArr = 0;

            for (int i = byteStr.Length - 1; i > 0 && byteStr[i] == '0'; i--, IndexArr++) ;
            string temper = "";
            for (int i = 0; i < byteStr.Length - IndexArr; i++)
                temper += byteStr[i];
            byteStr = temper;
            IndexArr = 0;
            if (0 == byteStr.Length % 16)
                Rez = new Char[byteStr.Length / 1];
            else
                Rez = new Char[(byteStr.Length / 16) + 1];
            string Temp = "";
            for (int i = 0; i < byteStr.Length; i += 16)
            {
                Temp = "";
                for (int j = i; j < i + 16 && j < byteStr.Length; j++)
                    Temp += byteStr[j];
                for (int k = Temp.Length; k < 16; k++)
                    Temp += '0';
                Rez[IndexArr++] = Convert.ToChar(convertorBin2Int(Temp));
            }
            return Rez;
        }
        int convertorBin2Int(string InBin)
        {
            int Sum = 0;
            int serializeBin = 1;

            for (int i = InBin.Length - 1; i >= 0; i--, serializeBin *= 2)
                if (InBin[i] == '1')
                    Sum += serializeBin;
            return Sum;
        }
        void HederDecompress(byte[] InByte, ref int readLength, ref String Rez)
        {
            if (InByte.Length * 8 < 5)
                Rez = "error to byte length!";
            string RezStringByte = convertorByte2ByteString(ref InByte);
            string temp = "";
            for (int i = 0; i < 5; i++)
                temp += RezStringByte[i];
            readLength = convertorBin2Int(temp);
        }
        public string Decompress(byte[] InByte)
        {
            if (InByte.Length * 8 < 5)
                return "error to byte length!";
            string RezStringByte = convertorByte2ByteString(ref InByte);

            int readLength;
            string Rez = "";
            string temp = "";
            Char CharDecode;
            bool CompressIsMood = true;
            if (RezStringByte[0] == '1')
                CompressIsMood = true;
            else
                CompressIsMood = false;
            for (int i = 1; i < 6; i++)
                temp += RezStringByte[i];
            readLength = convertorBin2Int(temp);
            if (readLength == 0 && !CompressIsMood)
            {
                string T_ = "";
                for (int i = 6; i < RezStringByte.Length - 3; i++)
                    T_ += RezStringByte[i];
                Char[] CharTable = convertorByteString2Unicode16bit(T_);
                index = CharTable.Length + 1;
                for (int i = 1; i < index; i++)
                    char_[i] = CharTable[i - 1];// newString[i];
                return null;
            }
            if (RezStringByte[0] == 48 && readLength == 0)
            {
                int indexReSort = 1;
                string temp_char = "";
                for (int i = 6; i < RezStringByte.Length - 3; i += 16)
                {
                    temp_char = "";
                    for (int k = i; k < 8 + i; k++)
                        temp_char += RezStringByte[k];

                    char_[indexReSort++] = Convert.ToChar(convertorBin2Int(temp_char));
                }
                return "";
            }
            bool Fl = false;
            int Cont_Heder = 0;
            for (int i = 6; i < RezStringByte.Length; )
            {
                temp = "";
                Fl = false;
                Cont_Heder = i;
                for (int j = i; j < Cont_Heder + readLength && j < RezStringByte.Length; j++)
                {
                    temp += RezStringByte[j];
                    i++;
                    if (RezStringByte[j] == '1')
                        Fl = true;
                }
                if (temp.Length < readLength)
                    for (int k = temp.Length; k < readLength; k++)
                        temp += '0';
                if (convertorBin2Int(temp) == 0)
                {
                    temp = "";
                    if (i >= RezStringByte.Length)
                        continue;
                    if (RezStringByte[i] == '1')
                        CompressIsMood = true;
                    else
                        CompressIsMood = false;
                    for (int h = i + 1; h < RezStringByte.Length && h < i + 6; h++)
                        temp += RezStringByte[h];
                    i += 6;
                    readLength = convertorBin2Int(temp);
                    continue;
                }
                if (Fl)
                {
                    if (CompressIsMood)
                        CharDecode = char_[index_[convertorBin2Int(temp)]];
                    else
                    {
                        CharDecode = Convert.ToChar(convertorBin2Int(temp));
                        if (FindTable(CharDecode) == -1)
                            addTable(CharDecode);
                    }
                    Rez += CharDecode;

                }
            }

            return Rez;
        }
        string convertorByte2ByteString(ref byte[] byteComp)
        {
            string Rez = "";
            for (int i = 0; i < byteComp.Length; i++)
            {
                Rez += convertorInt2Bin(Convert.ToInt32(Convert.ToChar(byteComp[i])), 8, 0);
            }

            return Rez;
        }
        public byte[] SaveTable()
        {
            string StringTable = "";
            StringTable += Convert.ToChar(index);
            for (int i = 1; i < index; i++)
                StringTable += char_[i];
            byte[] byteTable = Encoding.Unicode.GetBytes(StringTable);
            return byteTable;
        }
        public void loadTable(byte[] byteTable)
        {
            String newString = Encoding.Unicode.GetString(byteTable, 0, byteTable.Length);
            index = Convert.ToByte(newString[0]);
            for (int i = 1; i < index; i++)
                char_[i] = newString[i];
        }
        void Sorting(int[] useOfChars, char[] Table, int[] index_, int Start, int End)
        {
            int temp;
            char temp_char;
            int temp_index;
            for (int i = Start; i < End; i++)
                for (int j = Start; j < End; j++)
                    if (useOfChars[i] > useOfChars[j])
                    {
                        temp = useOfChars[i];
                        temp_char = Table[i];
                        temp_index = index_[i];
                        useOfChars[i] = useOfChars[j];
                        Table[i] = Table[j];
                        index_[i] = index_[j];
                        useOfChars[j] = temp;
                        Table[j] = temp_char;
                        index_[j] = temp_index;
                    }
            for (int i = Start; i < End; i++)
            {
                index_[i] = i;
                useOfChars[i] = 0;
            }
        }
    }

}
