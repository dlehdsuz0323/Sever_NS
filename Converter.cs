using System.Text;
using System;
namespace Server
{
    public class Converter
    {
        public static bool IntToBoolean(int index)
        {
            if (index == 1)
                return true;
            else
                return false;
        }
        public static int BooleanToInt(bool index)
        {
            if (index)
                return 1;
            else
                return 0;
        }
        public static byte GetByte(byte[] recv, ref int index)
        {
            return recv[index++];
        }
        public static short GetShort(byte[] recv, ref int index)
        {
            short retval = BitConverter.ToInt16(recv, index);
            index += 2;
            return retval;
        }
        public static int GetInt(byte[] recv, ref int index)
        {
            int retval = BitConverter.ToInt32(recv, index);
            index += 4;
            return retval;
        }
        public static long GetLong(byte[] recv, ref int index)
        {
            long retval = BitConverter.ToInt64(recv, index);
            index += 8;
            return retval;
        }
        public static double GetDouble(byte[] recv, ref int index)
        {
            double retval = BitConverter.ToDouble(recv, index);
            index += 8;
            return retval;
        }
        public static string GetString(byte[] recv, ref int index)
        {
            byte[] temp = GetByteArray(recv, ref index);
            return Encoding.UTF8.GetString(temp, 0, temp.Length - 1);
        }
        public static byte[] GetByteArray(byte[] recv, ref int index)
        {
            short lenght = GetShort(recv, ref index);
            byte[] retval = new byte[lenght];
            for (int i = 0; i < lenght; i++)
            {
                retval[i] = GetByte(recv, ref index);
            }
            return retval;
        }
        public static short[] GetShortArray(byte[] recv, ref int index)
        {
            short lenght = GetShort(recv, ref index);
            short[] retval = new short[lenght / 2];
            for (int i = 0; i < lenght / 2; i++)
            {
                retval[i] = GetShort(recv, ref index);
            }
            return retval;
        }
        public static int[] GetIntArray(byte[] recv, ref int index)
        {
            short lenght = GetShort(recv, ref index);
            int[] retval = new int[lenght / 4];
            for (int i = 0; i < lenght / 4; i++)
            {
                retval[i] = GetInt(recv, ref index);
            }
            return retval;
        }
        public static long[] GetLongArray(byte[] recv, ref int index)
        {
            short lenght = GetShort(recv, ref index);
            long[] retval = new long[lenght / 8];
            for (int i = 0; i < lenght / 8; i++)
            {
                retval[i] = GetLong(recv, ref index);
            }
            return retval;
        }
        public static double[] GetDoubleArray(byte[] recv, ref int index)
        {
            short lenght = GetShort(recv, ref index);
            double[] retval = new double[lenght / 8];
            for (int i = 0; i < lenght / 8; i++)
            {
                retval[i] = GetDouble(recv, ref index);
            }
            return retval;
        }
        public static short SetByte(byte[] send, ref int index, byte data)
        {
            send[index++] = data;
            return 1;
        }
        public static short SetShort(byte[] send, ref int index, short data)
        {
            byte[] newByte = BitConverter.GetBytes(data);
            for (int i = 0; i < newByte.Length; i++)
            {
                SetByte(send, ref index, newByte[i]);
            }
            return (short)newByte.Length;
        }
        public static short SetInt(byte[] send, ref int index, int data)
        {
            byte[] newByte = BitConverter.GetBytes(data);
            for (int i = 0; i < newByte.Length; i++)
            {
                SetByte(send, ref index, newByte[i]);
            }
            return (short)newByte.Length;
        }
        public static short SetLong(byte[] send, ref int index, long data)
        {
            byte[] newByte = BitConverter.GetBytes(data);
            for (int i = 0; i < newByte.Length; i++)
            {
                SetByte(send, ref index, newByte[i]);
            }
            return (short)newByte.Length;
        }
        public static short SetDouble(byte[] send, ref int index, double data)
        {
            byte[] newByte = BitConverter.GetBytes(data);
            for (int i = 0; i < newByte.Length; i++)
            {
                SetByte(send, ref index, newByte[i]);
            }
            return (short)newByte.Length;
        }
        public static short SetByteArray(byte[] send, ref int index, byte[] data)
        {
            SetShort(send, ref index, (short)data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                SetByte(send, ref index, data[i]);
            }
            return (short)(data.Length + 2);
        }
        public static short SetShortArray(byte[] send, ref int index, short[] data)
        {
            SetShort(send, ref index, (short)data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                SetShort(send, ref index, data[i]);
            }
            return (short)(data.Length * 2 + 2);
        }
        public static short SetIntArray(byte[] send, ref int index, int[] data)
        {
            SetShort(send, ref index, (short)data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                SetInt(send, ref index, data[i]);
            }
            return (short)(data.Length * 4 + 2);
        }
        public static short SetLongArray(byte[] send, ref int index, long[] data)
        {
            SetShort(send, ref index, (short)data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                SetLong(send, ref index, data[i]);
            }
            return (short)(data.Length * 8 + 2);
        }
        public static short SetDoubleArray(byte[] send, ref int index, double[] data)
        {
            SetShort(send, ref index, (short)data.Length);
            for (int i = 0; i < data.Length; i++)
            {
                SetDouble(send, ref index, data[i]);
            }
            return (short)(data.Length * 8 + 2);
        }
        public static short SetString(byte[] send, ref int index, string data)
        {
            byte[] temp = Encoding.UTF8.GetBytes(data + '\0');
            SetByteArray(send, ref index, temp);
            index++;
            return (short)(temp.Length + 2);
        }
    }
}