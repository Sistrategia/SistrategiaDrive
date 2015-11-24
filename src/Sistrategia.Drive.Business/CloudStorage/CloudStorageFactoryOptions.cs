using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sistrategia.Drive.Business.CloudStorage
{
    public class CloudStorageFactoryOptions<T> where T : IDisposable
    {
        public ICloudStorageFactoryProvider<T> Provider { get; set; }
    }

    public interface ICloudStorageFactoryProvider<T> where T : IDisposable
    {
        /// <summary>
        ///     Called once per request to create an object
        /// </summary>
        /// <param name="options"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        T Create(CloudStorageFactoryOptions<T> options, IOwinContext context);

        /// <summary>
        ///     Called at the end of the request to dispose the object created
        /// </summary>
        /// <param name="options"></param>
        /// <param name="instance"></param>
        void Dispose(CloudStorageFactoryOptions<T> options, T instance);
    }

    public class CloudStorageFactoryProvider<T> : ICloudStorageFactoryProvider<T> where T : class, IDisposable
    {
        public CloudStorageFactoryProvider() {
            OnDispose = (options, instance) => { };
            OnCreate = (options, context) => null;
        }

        public Func<CloudStorageFactoryOptions<T>, IOwinContext, T> OnCreate { get; set; }

        public Action<CloudStorageFactoryOptions<T>, T> OnDispose { get; set; }

        public virtual T Create(CloudStorageFactoryOptions<T> options, IOwinContext context) {
            return OnCreate(options, context);
        }

        public virtual void Dispose(CloudStorageFactoryOptions<T> options, T instance) {
            OnDispose(options, instance);
        }
    }
}
