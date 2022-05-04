using System;
using System.Collections.Generic;
using System.Text;

namespace Dislinkt.Connections
{
    public class ConnectionData
    {
        /// <summary>
        /// Unique identifier of source user
        /// </summary>
        public string SourceId { get; set; }
        /// <summary>
        /// Unique identifier of target user
        /// </summary>
        public string TargetId { get; set; }

        /// <summary>
        /// Connection label
        /// </summary>
        public string ConnectionName { get; set; }
    }
}
