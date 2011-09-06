using System;
using System.Collections.Generic;
using System.Text;

using Glider.Common.Objects;

namespace Pather.Helpers
{
	class DetectFollower
	{
		List<Helpers.Follower> Followers = new List<Helpers.Follower>();
		double AlertTime = GContext.Main.GetConfigDouble("FriendAlert");
		double LogoutTime = GContext.Main.GetConfigDouble("FriendLogout");

		const double TIMEOUT = 1;
		const int CHECKFORHUB = 6; //If more than 5 players around we are probably in a hub

		public bool CheckFollowers(List<String> partymembers)
		{
			bool found;
			TimeSpan telapsed;

			if (AlertTime <= 0 && LogoutTime <= 0)
				return false; //User does not want us to do follower logout
			//if only 1 is 0 we will act accordingly
			GPlayer[] PlayerList = GObjectList.GetPlayers();

			if (PlayerList.Length < CHECKFORHUB) //Only detect followers if we are not in a hub
			{
				foreach (GPlayer temp in PlayerList)
				{
					found = false;

					if (temp != GContext.Main.Me && !partymembers.Contains(temp.Name)) //Make sure the potential follower is not my soul
					{
						foreach (Follower tempf in Followers)
						{
							if (temp.GUID == tempf.player.GUID)
							{
								found = true;

								telapsed = DateTime.Now - tempf.firstseen; //Total time follower has been in range
								if (telapsed.TotalMinutes > LogoutTime && LogoutTime > 0)
								{
                                    PPather.WriteLine("Detection: Followed by {0} for too long. Stopping Glide.", tempf.player.Name);
									return true;
								}
								else if (telapsed.TotalMinutes > AlertTime && AlertTime > 0)
								{
									AlertFollower(tempf);
									tempf.alerted = true;                 //Don't play alert sound for this player again
								}

								tempf.lastseen = DateTime.Now;
							}
						}
						if (found == false)
						{
							Followers.Add(new Follower(temp)); // Add new follower to list
							PPather.WriteLine("Detection: New friend: {0}: {1} {2}",temp.Name,temp.PlayerRace,temp.PlayerClass);
						}
					}
				}
			}

			for (int i = Followers.Count - 1; i >= 0; i--)
			{
				telapsed = DateTime.Now - Followers[i].lastseen;
				if (telapsed.TotalMinutes > TIMEOUT)       // Test to see if follower has moved along
				{
					PPather.WriteLine("Detection: Removing friend: {0}",Followers[i].player.Name);
					Followers.RemoveAt(i);
				}
			}

			return false;
		}

		private void AlertFollower(Follower follower)
		{
			System.Media.SoundPlayer sound = new System.Media.SoundPlayer("PlayerNear.wav");
			if (follower.alerted == false)
			{
				TimeSpan telapsed = DateTime.Now - follower.firstseen;
				try
				{
					sound.Play();
				}
				catch
				{
				}
				PPather.WriteLine("Detection: Being followed for {0} seconds by: {1} ({2})",telapsed.TotalSeconds,
                    follower.player.Name,follower.player.GUID);
			}
		}
	}

	class Follower
	{
		public GUnit player;
		public DateTime firstseen;
		public DateTime lastseen;
		public bool alerted;

		public Follower(GUnit temp)
		{
			player = temp;
			firstseen = lastseen = DateTime.Now;
			alerted = false;
		}
	}
}