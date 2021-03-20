using System;
using System.Runtime.Serialization;

namespace LeagueAPI_Classes
{
    [Serializable]
    internal class APIKeyIsInvalidException : Exception
    {
        public APIKeyIsInvalidException(string message) : base(message)
        {
        }
    }
}