using Novell.Directory.Ldap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    public class LdapAuthService : IAuthenticationService
    {

        private const string MemberOfAttribute = "memberOf";
        private const string DisplayNameAttribute = "displayName";
        private const string SAMAccountNameAttribute = "sAMAccountName";

        //private readonly LdapConfig _config;
        private readonly LdapConnection _connection;

        public LdapAuthenticationService(IOptions<LdapConfig> config)
        {
            _config = config.Value;
            _connection = new LdapConnection
            {
                SecureSocketLayer = true
            };
        }

        public LdapUser Login(string username, string password)
        {
            _connection.Connect("ldap.dcs.aber.ac.uk", LdapConnection.DEFAULT_SSL_PORT);
            _connection.Bind("ou=People,dc=dcs,dc=aber,dc=ac,dc=uk", password);

            var searchFilter = string.Format("(objectClass=*)", username);
            var result = _connection.Search(
                _config.SearchBase,
                LdapConnection.SCOPE_SUB,
                searchFilter,
                new[] { MemberOfAttribute, DisplayNameAttribute, SAMAccountNameAttribute },
                false
            );

            try
            {
                var user = result.next();
                if (user != null)
                {
                    _connection.Bind(user.DN, password);
                    if (_connection.Bound)
                    {
                        //return new LdapUser
                        //{
                        //    DisplayName = user.getAttribute(DisplayNameAttribute).StringValue,
                        //    Uid = user.getAttribute(SAMAccountNameAttribute).StringValue,
                        //    IsAdmin = user.getAttribute(MemberOfAttribute).StringValueArray.Contains(_config.AdminCn)
                        //};
                    }
                }
            }
            catch
            {
                throw new Exception("Login failed.");
            }
            _connection.Disconnect();
            return null;
        }
    }
}
