using MySqlConnector;
using System.Collections.Generic;
using System.Data;
using System;
using UnityEngine;

public class BasesWorker : IDisposable
{
    private MySqlConnection connection;
    private string connectionString;

    public BasesWorker(string host, string user, string password, string database)
    {
        connectionString = $"SERVER={host};DATABASE={database};UID={user};PWD={password};";
    }

    public MySqlConnection GetConnection()
    {
        if (connection == null)
        {
            connection = new MySqlConnection(connectionString);
        }

        if (connection.State != ConnectionState.Open)
        {
            connection.Open();
        }

        return connection;
    }

    public int ExecuteNonQuery(string query)
    {
        try
        {
            Debug.Log($"Executing non-query: {query}");
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                int result = cmd.ExecuteNonQuery();
                Debug.Log($"Rows affected: {result}");
                return result;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"ExecuteNonQuery failed: {query}\nError: {ex.Message}");
            throw;
        }
    }

    public object ExecuteScalar(string query)
    {
        try
        {
            Debug.Log($"Executing scalar: {query}");
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            {
                object result = cmd.ExecuteScalar();
                Debug.Log($"Scalar result: {result}");
                return result;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"ExecuteScalar failed: {query}\nError: {ex.Message}");
            throw;
        }
    }

    public List<Dictionary<string, object>> ExecuteReader(string query)
    {
        var results = new List<Dictionary<string, object>>();
        try
        {
            Debug.Log($"Executing reader: {query}");
            using (var conn = GetConnection())
            using (var cmd = new MySqlCommand(query, conn))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.GetValue(i);
                    }
                    results.Add(row);
                }
            }
            Debug.Log($"Returned {results.Count} rows");
            return results;
        }
        catch (Exception ex)
        {
            Debug.LogError($"ExecuteReader failed: {query}\nError: {ex.Message}");
            throw;
        }
    }

    public void Dispose()
    {
        if (connection != null)
        {
            if (connection.State == ConnectionState.Open)
                connection.Close();

            connection.Dispose();
            connection = null;
        }
    }
}