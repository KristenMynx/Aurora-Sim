﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using log4net;
using Nini.Config;
using Aurora.Simulation.Base;
using OpenSim.Services.Interfaces;
using OpenSim.Framework;
using OpenSim.Framework.Servers.HttpServer;

using OpenMetaverse;
using Aurora.DataManager;
using Aurora.Framework;
using Aurora.Services.DataService;
using OpenMetaverse.StructuredData;

namespace OpenSim.Server.Handlers.Caps
{
    public class AuroraCAPSHandler : IService
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public IHttpServer m_server = null;
        public string Name
        {
            get { return GetType().Name; }
        }

        public void Initialize(IConfigSource config, IRegistryCore registry)
        {
        }

        public void PostInitialize(IConfigSource config, IRegistryCore registry)
        {
        }

        public void Start(IConfigSource config, IRegistryCore registry)
        {
        }

        public void PostStart(IConfigSource config, IRegistryCore registry)
        {
            IConfig handlerConfig = config.Configs["Handlers"];
            if (handlerConfig.GetString("CAPSHandler", "") != Name)
                return;
            ICapsService m_capsService = registry.RequestModuleInterface<ICapsService>();

            string Password = handlerConfig.GetString("CAPSHandlerPassword", String.Empty);
            if (Password != "" & m_capsService != null)
            {
                m_server = registry.RequestModuleInterface<ISimulationBase>().GetHttpServer(handlerConfig.GetUInt("CAPSHandlerPort"));
                //This handler allows sims to post CAPS for their sims on the CAPS server.
                m_server.AddStreamHandler(new CAPSHandler(Password, m_capsService));
            }
        }

        public void AddNewRegistry(IConfigSource config, IRegistryCore registry)
        {
        }
    }

    public class CAPSHandler : BaseStreamHandler
    {
        private static readonly ILog m_log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected string CAPSPass;
        protected ICapsService m_capsService;

        public CAPSHandler(string pass, ICapsService service) :
            base("POST", "/CAPS/REGISTER")
        {
            CAPSPass = pass;
            m_capsService = service;
        }

        public override byte[] Handle(string path, Stream requestData,
                OSHttpRequest httpRequest, OSHttpResponse httpResponse)
        {
            StreamReader sr = new StreamReader(requestData);
            string body = sr.ReadToEnd();
            sr.Close();
            body = body.Trim();

            //m_log.DebugFormat("[XXX]: query String: {0}", body);
            string method = string.Empty;
            try
            {
                Dictionary<string, object> request = new Dictionary<string, object>();
                request = WebUtils.ParseQueryString(body);
                if (request.Count == 1)
                    request = WebUtils.ParseXmlResponse(body);
                object value = null;
                request.TryGetValue("<?xml version", out value);
                if (value != null)
                    request = WebUtils.ParseXmlResponse(body);

                return ProcessAddCAP(request);
            }
            catch (Exception)
            {
            }

            return null;

        }

        private byte[] ProcessAddCAP(Dictionary<string, object> m_dhttpMethod)
        {
            //This is called by the user server
            if ((string)m_dhttpMethod["PASS"] != CAPSPass)
            {
                Dictionary<string, object> result = new Dictionary<string, object>();
                result.Add("result", "false");
                string xmlString = WebUtils.BuildXmlResponse(result);
                UTF8Encoding encoding = new UTF8Encoding();
                return encoding.GetBytes(xmlString);
            }
            else
            {
                string CAPS = (string)m_dhttpMethod["CAPSSEEDPATH"];
                string simCAPS = m_dhttpMethod["SIMCAPS"].ToString();
                UUID AgentID = UUID.Parse((string)m_dhttpMethod["AGENTID"]);
                ulong regionHandle = ulong.Parse((string)m_dhttpMethod["REGIONHANDLE"]);

                m_capsService.RemoveCAPS(AgentID);
                m_capsService.CreateCAPS(AgentID, simCAPS, CAPS, regionHandle);

                Dictionary<string, object> result = new Dictionary<string, object>();
                result.Add("result", "true");
                string xmlString = WebUtils.BuildXmlResponse(result);
                UTF8Encoding encoding = new UTF8Encoding();
                return encoding.GetBytes(xmlString);
            }
        }
    }
}
