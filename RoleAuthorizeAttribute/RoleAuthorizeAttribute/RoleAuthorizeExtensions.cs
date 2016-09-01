﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;

namespace RoleAuthorize.Extensions
{
    public static class RoleAuthorizeExtensions
    {
        private static readonly char[] Delimiter = new char[] { ',' };

        /// <summary>
        /// Checks if a user is in any of a set of named roles.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="names">A comma separated list of role names.</param>
        /// <returns>True if the user is in at least one named role, otherwise false.</returns>
        public static bool IsInRole(this IPrincipal user, string names)
        {
            var roleNamesSplit = names.Split(Delimiter).Select(_ => _.Trim()).Where(_ => !String.IsNullOrEmpty(_));
            return user.IsInRole(roleNamesSplit);
        }

        /// <summary>
        /// Checks if a user is in any of a set of named roles.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="names">An enumerable of role names.</param>
        /// <returns>True if the user is in at least one named role, otherwise false.</returns>
        public static bool IsInRole(this IPrincipal user, IEnumerable<string> names)
        {
            var users = names.SelectMany(_ => Config.RoleConfig.GetUsers(_)).ToList();
            if (users.Count > 0 && users.Contains(user.Identity.Name, StringComparer.OrdinalIgnoreCase))
                return true;

            var roles = names.SelectMany(_ => Config.RoleConfig.GetRoles(_)).ToList();
            if (roles.Count > 0 && roles.Any(user.IsInRole))
                return true;

            return false;
        }
    }
}