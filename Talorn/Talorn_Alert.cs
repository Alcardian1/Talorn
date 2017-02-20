﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Talorn
{
    public class Talorn_Alert
    {
        string id;
        //AlertTime activation;
        //AlertTime expiry;
        long activation = -1;
        long expiry = -1;

        string missionType = "Unknown";
        string faction = "Unknown";
        string location = "Unknown";
        string levelOverride = "Unknown";
        //String enemySpec;
        int minEnemyLevel = -1;
        int maxEnemyLevel = -1;
        //double difficulty;
        //int seed;
        bool archwingRequired = false;
        bool sharkwingMission = false;
        int maxWaveNum = 0;

        int credits = 0;
        string[] items = null;
        //List<string> items = new List<string>();
        List<Tuple<string, int>> countedItems = new List<Tuple<string, int>>();

        public Talorn_Alert(String alertString)
        {
            //List<string> buffer = new List<string>();
            string temp = "";

            //ID
            temp = alertString.Substring(alertString.IndexOf("_id"));
            temp = temp.Substring(temp.IndexOf("{"));
            temp = temp.Substring(temp.IndexOf(":"));
            temp = temp.Substring(temp.IndexOf('"'));
            temp = temp.Remove(temp.IndexOf('}'));
            temp = temp.Remove(temp.Length - 1);
            temp = temp.Substring(temp.LastIndexOf("\"") + 1);
            //buffer.Add("ID: " + temp);
            id = temp;

            //Activation
            temp = alertString.Substring(alertString.IndexOf("Activation"));
            temp = temp.Substring(temp.IndexOf("{"));
            temp = temp.Substring(1);
            temp = temp.Remove(temp.IndexOf('}'));
            temp = temp.Remove(temp.Length - 1);
            temp = temp.Substring(temp.LastIndexOf("\"") + 1);
            //buffer.Add("Activation: " + temp);
            {
                long j;
                if (Int64.TryParse(temp, out j))
                {
                    activation = j;
                }
                else
                {
                    //Could not parse if this happened
                    activation = -2;
                }
            }

            //Expiry
            temp = alertString.Substring(alertString.IndexOf("Expiry"));
            temp = temp.Substring(temp.IndexOf("{"));
            temp = temp.Substring(1);
            temp = temp.Remove(temp.IndexOf('}'));
            temp = temp.Remove(temp.Length - 1);
            temp = temp.Substring(temp.LastIndexOf("\"") + 1);
            //buffer.Add("Expiry: " + temp);
            {
                long j;
                if (Int64.TryParse(temp, out j))
                {
                    expiry = j;
                }
                else
                {
                    //Could not parse if this happened
                    expiry = -2;
                }
            }

            //Mission Type
            temp = alertString.Substring(alertString.IndexOf("missionType"));
            temp = temp.Substring(temp.IndexOf(":"));
            temp = temp.Substring(1);
            temp = temp.Remove(temp.IndexOf(','));
            temp = temp.Remove(temp.Length - 1);
            temp = temp.Substring(temp.LastIndexOf("\"") + 1);
            //buffer.Add("Mission Type: " + temp);
            missionType = temp;

            //faction
            temp = alertString.Substring(alertString.IndexOf("faction"));
            temp = temp.Substring(temp.IndexOf(":"));
            temp = temp.Substring(1);
            temp = temp.Remove(temp.IndexOf(','));
            temp = temp.Remove(temp.Length - 1);
            temp = temp.Substring(temp.LastIndexOf("\"") + 1);
            //buffer.Add("Faction: " + temp);
            faction = temp;

            //location
            temp = alertString.Substring(alertString.IndexOf("location"));
            temp = temp.Substring(temp.IndexOf(":"));
            temp = temp.Substring(1);
            temp = temp.Remove(temp.IndexOf(','));
            temp = temp.Remove(temp.Length - 1);
            temp = temp.Substring(temp.LastIndexOf("\"") + 1);
            //buffer.Add("Location: " + temp);
            location = temp;

            //levelOverride, not always there
            if (alertString.IndexOf("levelOverride") > -1)
            {
                temp = alertString.Substring(alertString.IndexOf("levelOverride"));
                temp = temp.Substring(temp.IndexOf(":"));
                temp = temp.Substring(1);
                temp = temp.Remove(temp.IndexOf(','));
                temp = temp.Remove(temp.Length - 1);
                temp = temp.Substring(temp.LastIndexOf("\"") + 1);
                //buffer.Add("levelOverride: " + temp);
                levelOverride = temp;
            }

            //int minEnemyLevel
            //int maxEnemyLevel

            //Archwing
            if (alertString.IndexOf("archwingRequired") > -1)
            {
                temp = alertString.Substring(alertString.IndexOf("archwingRequired"));
                temp = temp.Substring(temp.IndexOf(":"));
                temp = temp.Substring(1);
                temp = temp.Remove(temp.IndexOf(','));
                //buffer.Add("archwingRequired: " + temp);
                archwingRequired = (temp.ToLower() == "true");
            }

            //shardwing
            if (alertString.IndexOf("isSharkwingMission") > -1)
            {
                temp = alertString.Substring(alertString.IndexOf("isSharkwingMission"));
                temp = temp.Substring(temp.IndexOf(":"));
                temp = temp.Substring(1);
                temp = temp.Remove(temp.IndexOf(','));
                //buffer.Add("isSharkwingMission: " + temp);
                sharkwingMission = (temp.ToLower() == "true");
            }

            //MissionReward
            //if (false)
            if (alertString.IndexOf("missionReward") > -1)
            {
                //MissionReward, credits
                if (alertString.IndexOf("\"credits\":") > -1)
                {
                    temp = alertString.Substring(alertString.IndexOf("\"credits\":"));
                    temp = temp.Substring(temp.IndexOf(":"));
                    temp = temp.Substring(1);
                    if (temp.IndexOf(',') > -1)
                    {
                        temp = temp.Remove(temp.IndexOf(','));
                    }
                    else
                    {
                        temp = temp.Remove(temp.IndexOf('}'));
                    }
                    //buffer.Add("Credtis: " + temp);
                    {
                        int j;
                        if (Int32.TryParse(temp, out j))
                        {
                            credits = j;
                        }
                        else
                        {
                            //Could not parse if this happened
                            credits = -2;
                        }
                    }
                }

                //MissionReward, items
                if (alertString.IndexOf("\"items\":") > -1)
                {
                    temp = alertString.Substring(alertString.IndexOf("\"items\":"));
                    temp = temp.Substring(temp.IndexOf(":"));
                    temp = temp.Substring(1);
                    if (temp.IndexOf(',') > -1)
                    {
                        temp = temp.Remove(temp.IndexOf(','));
                    }
                    else
                    {
                        temp = temp.Remove(temp.IndexOf('}'));
                    }

                    if (temp.Length > 0 && temp[0] == '[')
                    {
                        temp = temp.Substring(1);
                    }
                    if (temp.Length > 0 && temp[temp.Length - 1] == ']')
                    {
                        temp = temp.Remove(temp.Length - 1);
                    }
                    //buffer.Add("Items: " + temp);
                    items = temp.Split(',');

                }

                //MissionReward, counteditems
                if (alertString.IndexOf("\"countedItems\":") > -1)
                {
                    temp = alertString.Substring(alertString.IndexOf("\"countedItems\":"));
                    temp = temp.Substring(temp.IndexOf(":"));
                    temp = temp.Substring(1);
                    if (temp.IndexOf(',') > -1)
                    {
                        temp = temp.Remove(temp.IndexOf(','));
                    }
                    else
                    {
                        temp = temp.Remove(temp.IndexOf('}'));
                    }
                    if (temp[0] == '[' && temp.Length > 0)
                    {
                        temp = temp.Substring(1);
                    }
                    //buffer.Add("Counted Items: " + temp);
                    //TODO finish this so it shows the count
                    countedItems.Add(new Tuple<string, int>(temp, -1));
                }
            }
        }

        // Prints the alert data
        public string printAlert()
        {
            string tmp = "ID: " + id;
            tmp += "\nActivation: " + activation;
            tmp += "\nExpiry: " + expiry;

            //string missionType;
            if (missionType != "Unknown")
            {
                tmp += "\nExpiry: " + missionType;
            }

            //string faction;
            if (faction != "Unknown")
            {
                tmp += "\nFaction: " + faction;
            }

            //string location;
            if (location != "Unknown")
            {
                tmp += "\nLocation: " + location;
            }

            //string levelOverride;
            if (levelOverride != "Unknown")
            {
                tmp += "\nlevelOverride: " + levelOverride;
            }
            //int minEnemyLevel;
            //int maxEnemyLevel;

            //bool archwingRequired = false;
            if (archwingRequired)
            {
                tmp += "\nArchwing Required";
            }

            //bool sharkwingMission = false;
            if (sharkwingMission)
            {
                tmp += "\nSharkwing Mission";
            }
            //int maxWaveNum = 0;

            //int credits = 0;
            if (credits != 0)
            {
                tmp += "\nCredits: " + credits;
            }

            //string[] items;
            if (items != null)
            {
                tmp += "\nItems: ";
                foreach (string item in items)
                {
                    tmp += item + ", ";
                }
                tmp = tmp.Remove(tmp.Length-2);
            }

            //List<Tuple<string, int>> countedItems;
            if (countedItems.Count > 0)
            {
                tmp += "\nCounted Items: ";
                foreach (Tuple<string, int> item in countedItems)
                {
                    tmp += item + ", ";
                }
                tmp = tmp.Remove(tmp.Length - 2);
            }

            return tmp;
        }
    }
}