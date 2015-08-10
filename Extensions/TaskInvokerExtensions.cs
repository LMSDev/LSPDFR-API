using System;
using System.Reflection;

using Rage;

public static class TaskInvokerExtensions
{
    /// <summary>
    /// Returns the internal <see cref="Rage.Ped"/> of the task invoker.
    /// </summary>
    /// <param name="taskInvoker">The task invoker.</param>
    /// <returns>A <see cref="Rage.Ped"/> instance.</returns>
    public static Ped GetInstancePed(this TaskInvoker taskInvoker)
    {
        PropertyInfo p = taskInvoker.GetType().GetProperty("Ped", BindingFlags.NonPublic | BindingFlags.Instance);
        if (p != null)
        {
            Ped instancePed = (Ped)p.GetMethod.Invoke(taskInvoker, null);
            return instancePed;
        }

        return null;
    }
}