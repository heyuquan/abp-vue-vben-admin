
using System;
using System.Runtime.ConstrainedExecution;

namespace Leopard.Utils
{
    /// <summary>
    /// 可以被释放类的基类
    /// </summary>
    public abstract class DisposableObject : CriticalFinalizerObject, IDisposable
    {
        // 释放过就不用再释放了
        protected bool isDisposed = false;

        #region Finalization Constructs
        /// <summary>
        /// Finalizes the object.
        /// </summary>
        ~DisposableObject()
        {
            this.Dispose(false);
        }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing">A <see cref="System.Boolean"/> value which indicates whether
        /// the object should be disposed explicitly.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    // 释放托管资源
                    this.DisposeManageResource();
                }

                // 释放非托管资源
                this.DisposeUnManageResource();
            }
            isDisposed = true;
        }

        /// <summary>
        /// 释放托管资源
        /// </summary>
        protected virtual void DisposeManageResource()
        {
 
        }

        /// <summary>
        /// 释放非托管资源
        /// </summary>
        protected virtual void DisposeUnManageResource()
        {

        }

        /// <summary>
        /// Provides the facility that disposes the object in an explicit manner,
        /// preventing the Finalizer from being called after the object has been
        /// disposed explicitly.
        /// </summary>
        protected void ExplicitDispose()
        {
            this.Dispose(true);

            // // 由于用户是显示调用，所以资源释放不再由GC来完成
            GC.SuppressFinalize(this);
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.ExplicitDispose();
        }
        #endregion
    }
}
