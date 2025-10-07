using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class KinNoct
{
    private const string Host = "localhost";
    private const string User = "root";
    private const string Password = "";
    private const string Database = "KinNoct";

    // �������� ������ ������������
    public static int CreateNewUser(string username)
    {
        string query = $"SELECT sf_AddNewUser('{username}');";
        using (var bw = new BasesWorker(Host, User, Password, Database))
        {
            try
            {
                object result = bw.ExecuteScalar(query);
                int userId = Convert.ToInt32(result);

                Debug.Log($"User created: {username} | ID: {userId}");
                return userId;
            }
            catch (Exception ex)
            {
                Debug.LogError($"User creation failed: {ex.Message}");
                return -1;
            }
        }
    }

    // ������ ������� ������
    public static int StartGameSession(int userId)
    {
        string query = $"SELECT sf_StartGameSession({userId});";
        using (var bw = new BasesWorker(Host, User, Password, Database))
        {
            try
            {
                object result = bw.ExecuteScalar(query);
                int sessionId = Convert.ToInt32(result);

                Debug.Log($"Session started | UserID: {userId} | SessionID: {sessionId}");
                return sessionId;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Session start error: {ex.Message}");
                return -1;
            }
        }
    }

    // ������ ���������� �������
    public static bool CollectResource(int userId, string resourceName)
    {
        
        string safeResourceName = resourceName.Replace("'", "''");

        
        string query = $"SELECT sf_CollectResource({userId}, '{safeResourceName}');";

        // ������� ��������� BasesWorker ��� ������ � ����� ������
        using (var bw = new BasesWorker(Host, User, Password, Database))
        {
            try
            {
                // ��������� ������ � �������� ���������
                object result = bw.ExecuteScalar(query);

                // ����������� ��������� � bool
                bool success = Convert.ToBoolean(result);

                // �������� ���������
                Debug.Log($"Resource collected | UserID: {userId} | Resource: {resourceName} | Success: {success}");
                return success;
            }
            catch (Exception ex)
            {
                // �������� ������
                Debug.LogError($"Resource save error: {ex.Message}");
                return false;
            }
        }
    }


    // ���������� ������� ������
    public static bool EndGameSession(int sessionId, int score, int duration)
    {
        string query = $"SELECT sf_EndGameSession({sessionId}, {score}, {duration});";
        using (var bw = new BasesWorker(Host, User, Password, Database))
        {
            try
            {
                object result = bw.ExecuteScalar(query);
                bool success = Convert.ToBoolean(result);

                Debug.Log($"Session ended | SessionID: {sessionId} | Success: {success}");
                return success;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Session end error: {ex.Message}");
                return false;
            }
        }
    }

    // ��������� ���������� ������ (������)
    public static PlayerStats GetPlayerStats(int userId)
    {
        string query = $"SELECT * FROM player_stats WHERE user_id = {userId};";
        using (var bw = new BasesWorker(Host, User, Password, Database))
        {
            try
            {
                var results = bw.ExecuteReader(query);

                if (results.Count == 0)
                    return null;

                var firstRow = results[0];
                var stats = new PlayerStats
                {
                    TotalScore = Convert.ToInt32(firstRow["total_score"]),
                    BestScore = Convert.ToInt32(firstRow["best_score"]),
                    TotalPlayTime = Convert.ToInt32(firstRow["total_playtime"])
                };

                return stats;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Get player stats error: {ex.Message}");
                return null;
            }
        }
    }
}

public class PlayerStats
{
    public int TotalScore { get; set; }
    public int BestScore { get; set; }
    public int TotalPlayTime { get; set; }
}