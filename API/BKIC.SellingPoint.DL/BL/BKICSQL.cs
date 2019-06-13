using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

/// <summary>
/// Summary description for SQL
/// </summary>
public static class BKICSQL
{
    public static string CONNECTION_STRING = System.Configuration.ConfigurationManager.AppSettings["DBConnection"].ToString();

    public static string EncodePassword(string originalPassword)
    {
        //Declarations
        Byte[] originalBytes;
        Byte[] encodedBytes;
        MD5 md5;

        //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash    (encoded password)
        md5 = new MD5CryptoServiceProvider();
        originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
        encodedBytes = md5.ComputeHash(originalBytes);

        //Convert encoded bytes back to a 'readable' string
        return BitConverter.ToString(encodedBytes);
    }

    public static string RNGCharacterMask()
    {
        int maxSize = 5;
        int minSize = 3;
        char[] chars = new char[62];
        string a;
        a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        chars = a.ToCharArray();
        int size = maxSize;
        byte[] data = new byte[1];
        RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
        crypto.GetNonZeroBytes(data);
        size = maxSize;
        data = new byte[size];
        crypto.GetNonZeroBytes(data);
        StringBuilder result = new StringBuilder(size);
        foreach (byte b in data)
        { result.Append(chars[b % (chars.Length - 1)]); }
        return result.ToString();
    }

    public static DataTable wedt(string CommandName)
    {
        DataTable table = null;
        using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = CommandName;

                try
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        table = new DataTable();
                        da.Fill(table);
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        return table;
    }

    // This function will be used to execute R(CRUD) operation of parameterized commands
    public static DataTable edt(string CommandName, SqlParameter[] param)
    {
        DataTable table = new DataTable();

        using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = CommandName;
                cmd.Parameters.AddRange(param);

                try
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(table);
                    }
                }
                catch
                {
                    throw;
                }
            }
        }
        return table;
    }

    // This function will be used to execute CUD(CRUD) operation of parameterized commands
    public static void enq(string CommandName, SqlParameter[] pars)
    {
        //int result = 0;

        using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = CommandName;
                cmd.Parameters.AddRange(pars);

                try
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    // result = cmd.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }

        // return (result > 0);
    }

    public static void wenq(string CommandName)
    {
        // int result = 0;

        using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = CommandName;

                try
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    //  result = cmd.ExecuteNonQuery();
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }

        //return (result > 0);
    }

    public static DataSet weds(string CommandName)
    {
        DataSet table = null;
        using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = CommandName;

                try
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        table = new DataSet();
                        da.Fill(table);
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        return table;
    }

    // This function will be used to execute R(CRUD) operation of parameterized commands
    public static DataSet eds(string CommandName, SqlParameter[] param)
    {
        DataSet table = new DataSet();

        using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = CommandName;
                cmd.Parameters.AddRange(param);

                try
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(table);
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        return table;
    }

    public static object[] GetValues(string CommandName, SqlParameter[] param, List<SPOut> outParams)
    {
        DataSet table = new DataSet();

        using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
        {
            using (SqlCommand cmd = con.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = CommandName;
                cmd.Parameters.AddRange(param);
                SqlParameter[] outParameters = new SqlParameter[outParams.Count];
                object[] outValues = new object[outParams.Count];

                if (outParams.Count > 0)
                {
                    int i = 0;
                    foreach (var outFields in outParams)
                    {
                        SqlParameter outParam = new SqlParameter();
                        outParam.ParameterName = outFields.ParameterName;
                        outParam.SqlDbType = outFields.OutPutType;
                        outParam.Direction = System.Data.ParameterDirection.Output;

                        if (outFields.Size > 0)
                        {
                            outParam.Size = outFields.Size;
                        }

                        if (outFields.Precision > 0)
                        {
                            outParam.Precision = outFields.Precision;
                        }

                        if (outFields.Scale > 0)
                        {
                            outParam.Scale = outFields.Scale;
                        }

                        outParameters[i] = outParam;
                        i++;
                    }

                    cmd.Parameters.AddRange(outParameters);
                }

                try
                {
                    if (con.State != ConnectionState.Open)
                    {
                        con.Open();
                    }

                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();

                    for (int i = 0; i < outParams.Count; i++)
                    {
                        outValues[i] = outParameters[i].Value;
                    }
                }
                catch (Exception exc)
                {
                    throw;
                }

                return outValues;
            }
        }
    }
}

public class SPOut
{
    public string ParameterName { get; set; }
    public SqlDbType OutPutType { get; set; }
    public int Size { get; set; }
    public byte Precision { get; set; }
    public byte Scale { get; set; }
}