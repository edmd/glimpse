using glimpse.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace glimpse.Models.Repository
{
    public static class ExtensionMethods
    {
        public static void DetachLocal<T>(this DbContext context, T t, Guid entryId)
            where T : class, IIdentifier
        {
            var local = context.Set<T>()
                .Local
                .FirstOrDefault(entry => entry.Id.Equals(entryId));
            if (local != null)
            {
                context.Entry(local).State = EntityState.Detached;
            }
            context.Entry(t).State = EntityState.Modified;
        }
    }
}