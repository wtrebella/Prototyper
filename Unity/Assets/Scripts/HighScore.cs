using UnityEngine;
using System.Collections;
using System;

public class HighScore: MonoBehaviour {
	private string privateKey = "leaderboardTestKey";
	public const string AddScoreURL = "http://wtrebella.com/wp-content/uploads/LeaderboardTestFiles/AddScore.php?";
	public const string TopScoresURL = "http://wtrebella.com/wp-content/uploads/LeaderboardTestFiles/TopScores.php";
	public const string RankURL = "http://wtrebella.com/wp-content/uploads/LeaderboardTestFiles/GetRank.php?";
	private int highscore;
	private string username;
	private int rank;

	public void SetName(string name) {
		username = name;
	}

	public void Setscore(int givenscore) {
		highscore = givenscore;
	}

	public void StartProcess() {
		StartCoroutine(AddScore(username, highscore));
	}

	IEnumerator AddScore(string name, int score) {
		string hash = Md5Sum(name + score + privateKey);

		WWW ScorePost = new WWW(AddScoreURL + "name=" + WWW.EscapeURL(name) + "&score=" + score + "&hash=" + hash);
		yield return ScorePost;

		if (ScorePost.error == null)
		{
			StartCoroutine(GetRank(name));
		}
		else
		{
			Error();
		}
	}

	IEnumerator GetRank(string name)
	{
		//Try and grab the Rank
		WWW RankGrabAttempt = new WWW(RankURL + "name=" + WWW.EscapeURL(name));

		yield return RankGrabAttempt;

		if (RankGrabAttempt.error == null)
		{
			print("Grabbed rank apaz");
			rank = System.Int32.Parse(RankGrabAttempt.text); // Assign the rank to our variable. We could also use a TryParse and write an error dialogue.
			StartCoroutine(GetTopScores()); // Get our top scores
		}
		else
		{
			Error();
		}
	}

	IEnumerator GetTopScores()
	{
		WWW GetScoresAttempt = new WWW(TopScoresURL);
		yield return GetScoresAttempt;

		if (GetScoresAttempt.error != null)
		{
			Error();
		}
		else
		{
			print("Top scores apaz");
			//Collect up all our data
			string[] textlist = GetScoresAttempt.text.Split(new string[] { "\n", "\t" }, System.StringSplitOptions.RemoveEmptyEntries);

			//Split it into two smaller arrays
			string[] Names = new string[Mathf.FloorToInt(textlist.Length/2)];
			string[] Scores = new string[Names.Length];
			for (int i = 0; i < textlist.Length; i++)
			{
				if (i % 2 == 0)
				{     
					Names[Mathf.FloorToInt(i / 2)] = textlist[i];
				}
				else Scores[Mathf.FloorToInt(i / 2)] = textlist[i];
			}

			///Our top 10 scores
			for(int i=0;i<Names.Length;i++){
				Debug.Log((i + 1) + ": " + Scores[i] + ", " + Names[i]);
			}

			//If our player isn't in the top 10, add them to the bottom.
			if (rank > 10)
			{
				Debug.Log(rank + ": " + highscore + ", " + username);
			}

		}
	}

	void Error()
	{
		Debug.Log("Error!");
	}

	public static string Md5Sum(string strToEncrypt)
	{
		System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
		byte[] bytes = ue.GetBytes(strToEncrypt);

		// encrypt bytes
		System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
		byte[] hashBytes = md5.ComputeHash(bytes);

		// Convert the encrypted bytes back to a string (base 16)
		string hashString = "";

		for (int i = 0; i < hashBytes.Length; i++)
		{
			hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
		}

		return hashString.PadLeft(32, '0');
	}
}
