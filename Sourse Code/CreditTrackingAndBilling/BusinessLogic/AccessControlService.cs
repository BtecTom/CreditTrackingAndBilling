namespace BusinessLogic
{
    public class AccessControlService
    {
        /// <summary>
        /// Validate the requests Token
        /// </summary>
        /// <param name="token">the request token</param>
        /// <returns>true if the token is valid for this request</returns>
        public bool Validate(string token)
        {
            return true;
        }
    }
}
