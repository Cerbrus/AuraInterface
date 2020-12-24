namespace Aura.SDK.Helpers {
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// The SDK's utility helper class
    /// </summary>
    internal static class Util {
        /// <summary>
        /// Creates an array from a memory pointer populated by an external function.
        /// </summary>
        /// <param name="size">Size of the array to be retrieved</param>
        /// <param name="handler">Function that populates the memory space at the pointer</param>
        /// <returns></returns>
        internal static IntPtr[] ArrayFromPointer(int size, Action<IntPtr> handler) {
            var array = new IntPtr[size];
            var pointer = Marshal.AllocHGlobal(size);

            handler(pointer);

            Marshal.Copy(pointer, array, 0, size);
            Marshal.FreeHGlobal(pointer);

            return array;
        }

        /// <summary>
        /// Get the named method from the DLL
        /// </summary>
        /// <typeparam name="TMethod">The method's type</typeparam>
        /// <param name="dllHandle">The DLL handle</param>
        /// <param name="name">The method's name</param>
        /// <returns cref="Delegate">The found method</returns>
        internal static TMethod GetMethod<TMethod>(IntPtr dllHandle, string name)
            where TMethod : Delegate =>
            (TMethod)Marshal.GetDelegateForFunctionPointer(NativeMethods.GetProcAddress(dllHandle, name), typeof(TMethod));
    }
}
