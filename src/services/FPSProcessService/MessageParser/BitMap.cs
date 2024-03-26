using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using CredECard.Common.BusinessService;

namespace ContisGroup.MessageParser.ISO8586Parser
{
    /// <author>Prashant Soni</author>
    /// <created>03-Aug-2010</created>
    /// <summary>This class manage bitmap of message received and generate new bitmap at time of response.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// 	<para>The argument <paramref name="FieldNumber"/> is out of range.</para>
    /// </exception>
    public class BitMap
    {
        private BitArray _bitmap = null;
        private int _bitmapSizeinByte = 0;
        private const int BITMAPSTARTINDEX = 2;
        private const int BITARRAYSIZE = 64;

        /// <author>Prashant Soni</author>
        /// <created>03-Aug-2010</created>
        /// <summary>Constructor
        /// </summary>
        /// <param name="receivedMessage">It is a Received message bytes
        /// </param>
        public BitMap(byte[] receivedMessage)
        {
            _bitmap = ExtractBitmap(receivedMessage);
        }

        /// <author>Prashant Soni</author>
        /// <created>07-Sep-2011</created>
        /// <summary>Constructor which received message and bitmap byte length as a parameter.
        /// </summary>
        /// <param name="receivedMessage">
        /// </param>
        /// <param name="bitmapByteLength">
        /// </param>
        public BitMap(byte[] receivedMessage,int bitmapByteLength)
        {
            _bitmap = ExtractBitmap(receivedMessage,0, bitmapByteLength);
        }

        /// <author>Prashant Soni</author>
        /// <created>07-Sep-2011</created>
        /// <summary>constructor with received message, bitmap start index and bitmap byte length as a parameter
        /// </summary>
        /// <param name="receivedMessage">
        /// </param>
        /// <param name="startIndex">
        /// </param>
        /// <param name="bitmapByteLength">
        /// </param>
        public BitMap(byte[] receivedMessage,int startIndex, int bitmapByteLength)
        {
            _bitmap = ExtractBitmap(receivedMessage,startIndex, bitmapByteLength);
        }

        /// <author>Prashant Soni</author>
        /// <created>03-Aug-2010</created>
        /// <summary>Default Constructor
        /// </summary>
        public BitMap()
        {
            _bitmap = new BitArray(BITARRAYSIZE);
        }

        /// <author>Prashant Soni</author>
        /// <created>03-Aug-2010</created>
        /// <summary>It is a bitmap size converted into Byte length.
        /// </summary>
        /// <value>
        /// </value>
        public int BitmapSizeinByte
        {
            get
            {
                return _bitmapSizeinByte;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>03-Aug-2010</created>
        /// <summary>It is a Length of bitmap.
        /// </summary>
        /// <value>
        /// </value>
        public int Length
        {
            get
            {
                return _bitmap.Length;
            }
        }

        /// <author>Prashant Soni</author>
        /// <created>03-Aug-2010</created>
        /// <summary>This method allow to check whether provided fieldno is Set as exist or not.
        /// </summary>
        /// <param name="fieldNo">It is a ISO 8583 message field number.
        /// </param>
        /// <returns>It returns boolean value. if field value set, then true else false.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">If field no. is out of available number then exception thrown.
        /// 	<para>The argument <paramref name="FieldNumber"/> is out of range.</para>
        /// </exception>
        public bool IsSet(int fieldNo)
        {
            if (fieldNo > _bitmap.Length) throw new ArgumentOutOfRangeException("FieldNumber");

            return _bitmap[fieldNo - 1];
        }

        /// <author>Prashant Soni</author>
        /// <created>03-Aug-2010</created>
        /// <summary>This method set provided value to the bitmap against the passed field number. 
        /// </summary>
        /// <param name="fieldNo">It is a integer field no.
        /// </param>
        /// <param name="value">It is a boolean value.
        /// </param>
        public void Set(int fieldNo, bool value)
        {
            if (_bitmap == null) _bitmap = new BitArray(BITARRAYSIZE);

            if (fieldNo > _bitmap.Length)
            {
                int newBitmapCount = (_bitmap.Length / BITARRAYSIZE) + 1;
                if (fieldNo > (_bitmap.Length * newBitmapCount)) newBitmapCount +=1;
                byte[] newByteArr = new byte[(BITARRAYSIZE / 8) * newBitmapCount];
                _bitmapSizeinByte = newByteArr.Length;

                _bitmap.CopyTo(newByteArr, 0);
                _bitmap = new BitArray(newByteArr);
                _bitmap.Set(0, true);
                if (newBitmapCount > 2) _bitmap.Set(BITARRAYSIZE, true);
            }

            _bitmap.Set(fieldNo - 1, value);
        }

        /// <author>Prashant Soni</author>
        /// <created>03-Aug-2010</created>
        /// <summary>This method returns bitmap into byte array.
        /// </summary>
        /// <returns>It returns Byte array.
        /// </returns>
        public byte[] GetBytes()
        {
            if (_bitmapSizeinByte == 0)
                _bitmapSizeinByte = _bitmap.Length / 8;
           
            byte[] newByteArr = new byte[_bitmapSizeinByte];

            BitArray arr = _bitmap;
            arr = ReverseBitArray(arr);
            arr.CopyTo(newByteArr, 0);

            newByteArr = ReverseByteArray(newByteArr);
            return newByteArr;
        }

        /// <author>Prashant Soni</author>
        /// <created>03-Aug-2010</created>
        /// <summary>This method set all bits as 0 into bitmap.
        /// </summary>
        public void Clear()
        {
            if (_bitmap == null) return;

            _bitmap.SetAll(false);
           
        }

        /// <author>Prashant Soni</author>
        /// <created>07-Sep-2011</created>
        /// <summary>This method extracts bits from byte. so it converts byte into bitarray.
        /// </summary>
        /// <param name="bitmapByte">it is a Bytes to convert into bitarray.
        /// </param>
        /// <param name="startIndex">it is a start index from which needs to convert bitarray.
        /// </param>
        /// <param name="bitmapLength">length of bitmap e.g. 1 byte bitmap or 2 byte etc..
        /// </param>
        /// <returns> it returns bitarray.
        /// </returns>
        private BitArray ExtractBitmap(byte[] bitmapByte,int startIndex, int bitmapLength)
        {
            //int startIndex = 0;

           // int byteArraySize = (BITARRAYSIZE / 8);
            byte[] byteArray = new byte[bitmapLength];            

            Array.Copy(bitmapByte,startIndex, byteArray, 0, bitmapLength);

            byteArray = ReverseByteArray(byteArray);
            BitArray temp = new BitArray(byteArray);
            temp = ReverseBitArray(temp);

            _bitmapSizeinByte = bitmapLength;

            return temp;
        }


        /// <author>Prashant Soni</author>
        /// <created>03-Aug-2010</created>
        /// <summary>This method extracts bitmap from received byte array message.
        /// </summary>
        /// <param name="receiveMessage">it is a byte array.
        /// </param>
        /// <returns>It returns bitarray object.
        /// </returns>
        private BitArray ExtractBitmap(byte[] receiveMessage)
        {
            int byteArraySize = (BITARRAYSIZE / 8);
            byte[] byteArray = new byte[byteArraySize];
           
            Array.Copy(receiveMessage,BITMAPSTARTINDEX,byteArray,0,byteArraySize);
            
            byteArray = ReverseByteArray(byteArray);
            BitArray temp = new BitArray(byteArray);
            temp = ReverseBitArray(temp);

            //check if first bitmap's first field exist, if yes then initialise second bitmap.
            if (temp[0])
            {
                //j = startField;
                byteArraySize = byteArraySize * 2;
                byteArray = new byte[byteArraySize];
                //copy data into secondary bitmap and reverse bytearray.
                Array.Copy(receiveMessage, BITMAPSTARTINDEX,byteArray,0 , byteArraySize);
                byteArray = ReverseByteArray(byteArray);
                
                temp = new BitArray(byteArray);
                temp = ReverseBitArray(temp);
            }

            _bitmapSizeinByte = byteArraySize;
            return temp;
        }

        /// <author>Prashant Soni</author>
        /// <created>07-Oct-2010</created>
        /// <summary>This method overrides ToString method and returns comma seperated bit exist into bitmap.
        /// </summary>
        /// <returns>It returns string value.
        /// </returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();

            if (_bitmap != null)
            {
                for (int i = 0; i < _bitmap.Length; i++)
                {
                    if (_bitmap[i]) builder.AppendFormat("{0},", (i+1).ToString());
                }
            }
            return builder.ToString();
        }

        /// <author>Prashant Soni</author>
        /// <created>07-Sep-2011</created>
        /// <summary>this methid copy current bitmap into destination bitmap.
        /// </summary>
        /// <param name="destinationBitmap"> a Bitmap where we wants to copy this bitmap.
        /// </param>
        public void Copy(ref BitMap destinationBitmap)
        {        
            byte[] destByteArr = null;

            //check if destination bitmpa is null or not, if null then create new bitmap, otherwise copy this bitmap into destination bitmap.
            if (destinationBitmap == null) 
            {
                destinationBitmap = new BitMap();

                destinationBitmap._bitmap = new BitArray(this._bitmap.Length);
                destByteArr = new byte[_bitmap.Length/8];
                _bitmap.CopyTo(destByteArr, 0);
            }
            else
            {
                int startindex = destinationBitmap._bitmap.Length / 8;
                destByteArr = new byte[(_bitmap.Length + destinationBitmap._bitmap.Length )/ 8];
                destinationBitmap._bitmap.CopyTo(destByteArr,0);
                _bitmap.CopyTo(destByteArr, startindex); 
            }

            destinationBitmap._bitmap = new BitArray(destByteArr);

        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>This method Reverse given byte array as parameter.
        /// </summary>
        /// <param name="arry"> it is a byte Array.
        /// </param>
        /// <returns>returns  reversed byte array.
        /// </returns>
        private byte[] ReverseByteArray(byte[] arry)
        {
            byte[] revArray = new byte[arry.Length];
            for (int i = arry.Length - 1, j = 0; i >= 0; i--, j++)
                revArray[j] = arry[i];

            return revArray;
        }

        /// <author>Prashant Soni</author>
        /// <created>15-Jun-2010</created>
        /// <summary>This method Reverse given bit array as parameter.
        /// </summary>
        /// <param name="bit">it is a bitArray object.
        /// </param>
        /// <returns>returns  reversed bitArray.
        /// </returns>
        private BitArray ReverseBitArray(BitArray bit)
        {
            BitArray revArray = new BitArray(bit.Length);
            for (int i = bit.Length - 1, j = 0; i >= 0; i--, j++)
                revArray[j] = bit[i];

            return revArray;
        }
    }
}
