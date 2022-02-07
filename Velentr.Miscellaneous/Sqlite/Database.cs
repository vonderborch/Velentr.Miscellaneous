/// <file>
/// Velentr.Miscellaneous\Sqlite\Database.cs
/// </file>
///
/// <copyright file="Database.cs" company="">
/// Copyright (c) 2022 Christian Webber. All rights reserved.
/// </copyright>
///
/// <summary>
/// Implements the database class.
/// </summary>
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace Velentr.Miscellaneous.Sqlite
{
    /// <summary>
    /// A database.
    /// </summary>
    ///
    /// <seealso cref="IDisposable"/>
    public class Database : IDisposable
    {
        /// <summary>
        /// The file.
        /// </summary>
        private string _file;

        /// <summary>
        /// True to in memory.
        /// </summary>
        private bool _inMemory;

        /// <summary>
        /// The timeout.
        /// </summary>
        private int _timeout;

        /// <summary>
        /// The connection string.
        /// </summary>
        private string _connectionString = "";

        /// <summary>
        /// The connection.
        /// </summary>
        private SqliteConnection _connection = null;

        /// <summary>
        /// Gets the connection.
        /// </summary>
        ///
        /// <value>
        /// The connection.
        /// </value>
        public SqliteConnection Connection => _connection;

        /// <summary>
        /// Constructor.
        /// </summary>
        ///
        /// <exception cref="ArgumentException">    Thrown when one or more arguments have unsupported or
        ///                                         illegal values. </exception>
        ///
        /// <param name="file">             (Optional) The file. </param>
        /// <param name="inMemory">         (Optional) True to in memory. </param>
        /// <param name="timeoutSeconds">   (Optional) The timeout in seconds. </param>
        public Database(string file = null, bool inMemory = false, int timeoutSeconds = 30)
        {
            if (string.IsNullOrWhiteSpace(file) && !inMemory)
            {
                file = "database.db";
            }

            if (timeoutSeconds <= 0)
            {
                throw new ArgumentException("The timeout must be at least 1 second!");
            }

            _file = file;
            _inMemory = inMemory;
            _timeout = timeoutSeconds;

            _connectionString = !_inMemory
                ? $"Data Source={_file}"
                : "Data Source=:memory:";

            _connection = new SqliteConnection(_connectionString);
            _connection.Open();
        }

        /// <summary>
        /// Executes the 'query' operation.
        /// </summary>
        ///
        /// <param name="command">  The command. </param>
        /// <param name="timeout">  (Optional) The timeout. </param>
        ///
        /// <returns>
        /// A List&lt;T&gt;
        /// </returns>
        public List<Dictionary<string, object>> ExecuteQuery(string command, int? timeout = null)
        {
            List<Dictionary<string, object>> results = null;
            using (var comm = new SqliteCommand(command))
            {
                results = ExecuteQuery(comm, timeout);
            }

            return results;
        }

        /// <summary>
        /// Executes the 'query' operation.
        /// </summary>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="command">  The command. </param>
        /// <param name="timeout">  (Optional) The timeout. </param>
        ///
        /// <returns>
        /// A List&lt;T&gt;
        /// </returns>
        public List<T> ExecuteQuery<T>(string command, int? timeout = null) where T : IModelParser
        {
            List<T> results = null;
            using (var comm = new SqliteCommand(command))
            {
                results = ExecuteQuery<T>(comm, timeout);
            }

            return results;
        }

        /// <summary>
        /// Executes the 'query' operation.
        /// </summary>
        ///
        /// <param name="command">  The command. </param>
        /// <param name="timeout">  (Optional) The timeout. </param>
        ///
        /// <returns>
        /// A List&lt;T&gt;
        /// </returns>
        public List<Dictionary<string, object>> ExecuteQuery(SqliteCommand command, int? timeout = null)
        {
            var output = new List<Dictionary<string, object>>();
            command.Connection = _connection;
            command.CommandTimeout = timeout ?? _timeout;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.GetValue(i);
                    }

                    output.Add(row);
                }
            }

            return output;
        }

        /// <summary>
        /// Executes the 'query' operation.
        /// </summary>
        ///
        /// <typeparam name="T">    Generic type parameter. </typeparam>
        /// <param name="command">  The command. </param>
        /// <param name="timeout">  (Optional) The timeout. </param>
        ///
        /// <returns>
        /// A List&lt;T&gt;
        /// </returns>
        public List<T> ExecuteQuery<T>(SqliteCommand command, int? timeout = null) where T : IModelParser
        {
            var type = typeof(T);
            var output = new List<T>();
            command.Connection = _connection;
            command.CommandTimeout = timeout ?? _timeout;

            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var row = (IModelParser)Activator.CreateInstance(type);
                    row.Parse(reader);
                    output.Add((T)row);
                }
            }

            return output;
        }

        /// <summary>
        /// Executes the 'non query' operation.
        /// </summary>
        ///
        /// <param name="command">  The command. </param>
        /// <param name="timeout">  (Optional) The timeout. </param>
        public void ExecuteNonQuery(string command, int? timeout = null)
        {
            using (var comm = new SqliteCommand(command))
            {
                ExecuteNonQuery(comm, timeout);
            }
        }

        /// <summary>
        /// Executes the 'non query' operation.
        /// </summary>
        ///
        /// <param name="command">  The command. </param>
        /// <param name="timeout">  (Optional) The timeout. </param>
        public void ExecuteNonQuery(SqliteCommand command, int? timeout = null)
        {
            command.Connection = _connection;
            command.CommandTimeout = timeout ?? _timeout;

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Removes the connection.
        /// </summary>
        public void RemoveConnection()
        {
            if (_connection != null)
            {
                _connection.Close();
                SqliteConnection.ClearPool(_connection);
                _connection.Dispose();
                _connection = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        /// <summary>
        /// Resets the connection.
        /// </summary>
        public void ResetConnection()
        {
            if (_connection != null)
            {
                RemoveConnection();
            }
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ///
        /// <seealso cref="IDisposable.Dispose()"/>
        public void Dispose()
        {
            _connection.Close();
            _connection.Dispose();
        }
    }
}