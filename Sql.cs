using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows;

namespace BEB_csharp05
{
    class Sql
    {
            readonly string dbConnection;

            /// <summary>
            ///     Single Param Constructor for specifying the DB file.
            /// </summary>
            /// <param name="inputFile">The File containing the DB</param>
            public Sql(string inputFile)
            {
                dbConnection = string.Format("Data Source={0}; Version = 3;", inputFile);
            }

            /// <summary>
            ///     Allows the programmer to run a query against the Database.
            /// </summary>
            /// <param name="sql">The SQL to run</param>
            /// <returns>A DataTable containing the result set.</returns>
            public DataTable GetDataTable(string sql)
            {
                DataTable dt = new DataTable();
                try
                {
                    SQLiteConnection cnn = new SQLiteConnection(dbConnection);
                    cnn.Open();
                    SQLiteCommand mycommand = new SQLiteCommand(cnn)
                    {
                        CommandText = sql
                    };
                    //mycommand.Parameters.Add(sql);
                    SQLiteDataReader reader = mycommand.ExecuteReader();
                    dt.Load(reader);
                    reader.Close();
                    cnn.Close();
                }
                catch (SQLiteException e)
                {
                    IO.Log(2018062101, "GetDataTable " + e.GetType() + " | " + sql + " | " + e.Message); ;
                    //throw new Exception(e.Message);
                }
                catch (Exception e)
                {
                    IO.Log(2018062102, "GetDataTable " + e.GetType() + " | " + sql + " | " + e.Message);
                    //throw new Exception(e.Message);
                }
                return dt;
            }

            /// <summary>
            ///     Allows the programmer to interact with the database for purposes other than a query.
            /// </summary>
            /// <param name="sql">The SQL to be run.</param>
            /// <returns>An Integer containing the number of rows updated.</returns>
            public int ExecuteNonQuery(string sql)
            {
                SQLiteConnection cnn = new SQLiteConnection(dbConnection);
                cnn.Open();
                SQLiteCommand mycommand = new SQLiteCommand(cnn)
                {
                    //mycommand.Parameters.Add(sql);
                    CommandText = sql
                };
                int rowsUpdated = mycommand.ExecuteNonQuery();
                cnn.Close();
                return rowsUpdated;
            }

            /// <summary>
            ///     Allows the programmer to retrieve single items from the DB.
            /// </summary>
            /// <param name="sql">The query to run.</param>
            /// <returns>A string.</returns>
            public string ExecuteScalar(string sql)
            {
                SQLiteConnection cnn = new SQLiteConnection(dbConnection);
                cnn.Open();
                SQLiteCommand mycommand = new SQLiteCommand(cnn)
                {
                    CommandText = sql
                };
                //mycommand.Parameters.Add(sql);
                object value = mycommand.ExecuteScalar();
                cnn.Close();
                if (value != null)
                {
                    return value.ToString();
                }
                return "";
            }

            /// <summary>
            ///     Allows the programmer to easily update rows in the DB.
            /// </summary>
            /// <param name="tableName">The table to update.</param>
            /// <param name="data">A dictionary containing Column names and their new values.</param>
            /// <param name="where">The where clause for the update statement.</param>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            public bool Update(String tableName, Dictionary<String, String> data, String where)
            {
                String vals = "";
                Boolean returnCode = true;
                if (data.Count >= 1)
                {
                    foreach (KeyValuePair<String, String> val in data)
                    {
                        vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.ToString());
                    }
                    vals = vals.Substring(0, vals.Length - 1);
                }
                try
                {
                    ExecuteNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, where));
                }
                catch
                {
                    returnCode = false;
                }
                return returnCode;
            }

            /// <summary>
            ///     Allows the programmer to easily delete rows from the DB.
            /// </summary>
            /// <param name="tableName">The table from which to delete.</param>
            /// <param name="where">The where clause for the delete.</param>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            public bool Delete(String tableName, String where)
            {
                Boolean returnCode = true;
                try
                {
                    this.ExecuteNonQuery(String.Format("delete from {0} where {1};", tableName, where));
                }
                catch (Exception fail)
                {
                    IO.Log(2018062103, fail.Message);
                    returnCode = false;
                }
                return returnCode;
            }

            /// <summary>
            ///     Allows the programmer to easily insert into the DB
            /// </summary>
            /// <param name="tableName">The table into which we insert the data.</param>
            /// <param name="data">A dictionary containing the column names and data for the insert.</param>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            public bool Insert(String tableName, Dictionary<String, String> data)
            {
                String columns = "";
                String values = "";
                Boolean returnCode = true;
                foreach (KeyValuePair<String, String> val in data)
                {
                    columns += String.Format(" {0},", val.Key.ToString());
                    values += String.Format(" '{0}',", val.Value);
                }

                columns = columns.Substring(0, columns.Length - 1);
                values = values.Substring(0, values.Length - 1);
                try
                {
                    this.ExecuteNonQuery(String.Format("insert into {0}({1}) values({2});", tableName, columns, values));
                }
                catch (Exception fail)
                {
                    IO.Log(2018062104, fail.GetType() + " | " + fail.Message + " | " + fail.InnerException);
                    returnCode = false;
                }
                return returnCode;
            }

            /// <summary>
            ///     Allows the programmer to easily delete all data from the DB.
            /// </summary>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            public bool ClearDB()
            {
                DataTable tables;
                try
                {
                    tables = GetDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;");
                    foreach (DataRow table in tables.Rows)
                    {
                        this.ClearTable(table["NAME"].ToString());
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            ///     Allows the user to easily clear all data from a specific table.
            /// </summary>
            /// <param name="table">The name of the table to clear.</param>
            /// <returns>A boolean true or false to signify success or failure.</returns>
            public bool ClearTable(String table)
            {
                try
                {

                    this.ExecuteNonQuery(String.Format("delete from {0};", table));
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            /// <summary>
            ///     Erzeugt die notwendigen Tabellen in der SQLite-Datei. 
            /// </summary>
            public void SqlDeclareTables()
            {
                try
                {
                    //"Date","Time","Annahme => Fermenter 1","Annahme => Fermenter 2","Feststoffe=> Fermenter 1","Feststoffe=> Fermenter 2"
                    ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS
                                        [Input] (
                                        [Id]      INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                        [Datei]   TEXT UNIQUE,
                                        [Zeit]    TEXT NOT NULL,
                                        [Annahme] INT,
                                        [Feststoff] INT
                                        )");

                    // Date,"Time","Gasfüllstand gesamt %","Gasfüllstand Fermenter 1%","Gasfüllstand Fermenter 2%","Gasfüllstand Nachgärer  %","Gasfüllstand Endlager  %","Gasanalyse % CH4","Gasanalyse % O2","Gasanalyse ppm H2S","BHKW 1 kW","BHKW 2 kW","Temp. Fermenter 1","Temp. Fermenter 2","Temp. Nachgärer","Temp. Endlager"
                    ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS
                                        [Operation] (
                                        [Id]      INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
                                        [Datei]   TEXT UNIQUE,
                                        [Zeit]    TEXT NOT NULL,
                                        [Gasfüllstand_ges] INT,
                                        [Gasfüllstand_F1] INT,
                                        [Gasfüllstand_F2] INT,
                                        [Gasfüllstand_NG] INT,
                                        [Gasfüllstand_GPL] INT,
                                        [Gasanalyse_CH4] FLOAT,
                                        [Gasanalyse_O2] FLOAT, 
                                        [Gasanalyse_H2S] INT,
                                        [BHKW_kW] INT,
                                        [Temp_F1] FLOAT,
                                        [Temp_F2] FLOAT,
                                        [Temp_NG] FLOAT
                                        )");

                        // 2063	2063	38	VER DG 16           	2	6	1	1	Reiner Bruns        	Dohmstraße          	Bassen              	Mais                	Gieschen Quelkhorn  	10600	kg	        	        	16420	H	13.10.2013	17:08:48	27020
                        ExecuteNonQuery(@"CREATE TABLE IF NOT EXISTS
                                        [Scales] (
                                        [LfdNr]         INTEGER NOT NULL PRIMARY KEY,
                                        [Datei]         TEXT NOT NULL,
                                        [SchlagNr]      INT,
                                        [Sorte]         TEXT NOT NULL,
                                        [Nettogewicht]  INT,
                                        [Kunde_Name]    TEXT NOT NULL,
                                        [Kennzeichen]   TEXT,
                                        [Erst_Gewicht]  INT,
                                        [Tara_Hand]     TEXT,
                                        [Zweit_Gewicht] INT,
                                        [Zeit]          TEXT NOT NULL
                                        )");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "CREATE TABLE IF NOT EXISTS [Scales] | " );
                }
            }

        }


    }
