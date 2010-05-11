﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenMetaverse;
using Aurora.DataManager;
using Aurora.Framework;

namespace Aurora.DataManager.Frontends
{
    public class AgentFrontend
    {
        private IGenericData GD = null;
        public AgentFrontend()
        {
            GD = Aurora.DataManager.DataManager.GetDefaultGenericPlugin();
        }

        public IAgentInfo GetAgent(UUID agentID)
        {
            IAgentInfo agent = new IAgentInfo();
            List<string> query = GD.Query("PrincipalID", agentID, "agentgeneral", "Mac,IP,AcceptTOS,RealFirst,RealLast,Address,Zip,Country,TempBanned,PermaBanned,IsMinor,MaxMaturity,Language,LanguageIsPublic");
            
            if (query.Count == 0 || query.Count == 1)
                //Couldn't find it, return null then.
                return null;

            agent.Mac = query[0];
            agent.IP = query[1];
            agent.AcceptTOS = bool.Parse(query[2]);
            agent.RealFirst = query[3];
            agent.RealLast = query[4];
            agent.RealAddress = query[5];
            agent.RealZip = query[6];
            agent.RealCountry = query[7];
            agent.TempBanned = int.Parse(query[8]);
            agent.PermaBanned = int.Parse(query[9]);
            agent.IsMinor = bool.Parse(query[10]);
            agent.MaxMaturity = int.Parse(query[11]);
            agent.Language = query[12];
            agent.LanguageIsPublic = bool.Parse(query[13]);
            return agent;
        }

        public void UpdateAgent(IAgentInfo agent)
        {
            List<object> SetValues = new List<object>();
            List<string> SetRows = new List<string>();
            SetRows.Add("Mac");
            SetRows.Add("IP");
            SetRows.Add("AcceptTOS");
            SetRows.Add("RealFirst");
            SetRows.Add("RealLast");
            SetRows.Add("Address");
            SetRows.Add("Zip");
            SetRows.Add("Country");
            SetRows.Add("TempBanned");
            SetRows.Add("PermaBanned");
            SetRows.Add("IsMinor");
            SetRows.Add("MaxMaturity");
            SetRows.Add("Language");
            SetRows.Add("LanguageIsPublic");
            SetValues.Add(agent.Mac);
            SetValues.Add(agent.IP);
            SetValues.Add(agent.AcceptTOS);
            SetValues.Add(agent.RealFirst);
            SetValues.Add(agent.RealLast);
            SetValues.Add(agent.RealAddress);
            SetValues.Add(agent.RealZip);
            SetValues.Add(agent.RealCountry);
            SetValues.Add(agent.TempBanned);
            SetValues.Add(agent.PermaBanned);
            SetValues.Add(agent.IsMinor);
            SetValues.Add(agent.MaxMaturity);
            SetValues.Add(agent.Language);
            SetValues.Add(agent.LanguageIsPublic);
            List<object> KeyValue = new List<object>();
            List<string> KeyRow = new List<string>();
            KeyRow.Add("PrincipalID");
            KeyValue.Add(agent.PrincipalID);
            GD.Update("agentgeneral", SetValues.ToArray(), SetRows.ToArray(), KeyRow.ToArray(), KeyValue.ToArray());
        }

        public void CreateNewAgent(UUID agentID)
        {
            List<object> values = new List<object>();
            values.Add(agentID.ToString());
            values.Add(" ");
            values.Add(" ");
            values.Add(true);
            values.Add(" ");
            values.Add(" ");
            values.Add(" ");
            values.Add(" ");
            values.Add(" ");
            values.Add(0);
            values.Add(0);
            values.Add(true);
            values.Add(2);
            values.Add("en-us");
            values.Add(true);
            var GD = Aurora.DataManager.DataManager.GetDefaultGenericPlugin();
            GD.Insert("agentgeneral", values.ToArray());
        }
    }
}