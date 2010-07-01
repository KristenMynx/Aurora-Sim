﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aurora.Framework
{
	public interface IAssetConnector
	{
		ObjectMediaURL GetObjectMediaInfo(string objectID, int side);

        ObjectMediaURL GetObjectMediaInfoByInventoryID(string inventoryID, int side);

        void UpdateObjectMediaInfo(ObjectMediaURL media, int side, OpenMetaverse.UUID ObjectID);

        void UpdateLSLData(string token, string key, string value);

        List<string> FindLSLData(string token, string key);
    }
}