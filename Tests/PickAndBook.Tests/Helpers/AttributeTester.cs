﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PickAndBook.Data.Common;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace PickAndBook.Tests.Helpers
{
    public static class AttributeTester
    {
        // Via http://stackoverflow.com/questions/8817031/how-to-check-if-method-has-an-attribute
        public static bool MethodHasAttribute(Expression<Action> expression, Type attributeType)
        {
            var body = (MethodCallExpression)expression.Body;
            var method = body.Method;

            return method.GetCustomAttributes(attributeType, false).Any();
        }

        public static bool ClassHasAttribute(Type classType, Type attributeType)
        {
            var attr = Attribute.GetCustomAttribute(classType, attributeType);
            return attr != null;
        }

        public static void EnsureClassHasAdminAuthorizationAttribute(Type classType)
        {
            var attr = Attribute.GetCustomAttribute(classType, typeof(AuthorizeAttribute)) as AuthorizeAttribute;

            Assert.IsNotNull(attr);
            Assert.AreEqual(attr.Roles, DataConstants.AdministratorRoleName);
        }

        public static void EnsureClassHasClientAuthorizationAttribute(Type classType)
        {
            var attr = Attribute.GetCustomAttribute(classType, typeof(AuthorizeAttribute)) as AuthorizeAttribute;

            Assert.IsNotNull(attr);
            Assert.AreEqual(attr.Roles, DataConstants.ClientRoleName);
        }
    }
}
