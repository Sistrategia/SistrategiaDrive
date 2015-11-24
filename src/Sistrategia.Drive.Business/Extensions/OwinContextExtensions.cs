using System;
using Microsoft.Owin;

namespace Sistrategia.Drive.Business.CloudStorage.Owin
{
    /// <summary>
    ///     Extension methods for OwinContext/>
    /// </summary>
    public static class OwinContextExtensions
    {
        private static readonly string CloudStorageKeyPrefix = "Sistrategia.Drive.CloudStorage.Owin:";

        private static string GetKey(Type t) {
            return CloudStorageKeyPrefix + t.AssemblyQualifiedName;
        }
            
        public static IOwinContext Set<T>(this IOwinContext context, T value) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }
            return context.Set(GetKey(typeof(T)), value);
        }

        public static T Get<T>(this IOwinContext context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }
            return context.Get<T>(GetKey(typeof(T)));
        }

        public static TManager GetCloudStorageManager<TManager>(this IOwinContext context) {
            if (context == null) {
                throw new ArgumentNullException("context");
            }
            return context.Get<TManager>();
        }
    }
}
