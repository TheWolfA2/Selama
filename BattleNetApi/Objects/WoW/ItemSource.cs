﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleNetApi.Objects.WoW
{
    public class ItemSource
    {
        #region Properties
        public int SourceId { get; private set; }

        public string SourceType { get; private set; }
        #endregion

        internal static ItemSource ParseItemSource(JObject itemSourceJson)
        {
            return new ItemSource
            {
                SourceId = itemSourceJson["sourceId"].Value<int>(),
                SourceType = itemSourceJson["sourceType"].Value<string>()
            };
        }
    }
}
