// <copyright file="FacadeBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace GameStore.BusinessLayer.Interfaces.BaseClass
{
    using GameStore.BusinessLayer.Interfaces.Exceptions;

    public abstract class FacadeBase
    {
        protected static async Task<T?> GetResponseOrNull<T>(Func<Task<T>> serviceCall)
            where T : class
        {
            try
            {
                return await serviceCall();
            }
            catch (NotFoundException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected static async Task<Task> ExecuteActionOrNull(Func<Task> serviceCall)
        {
            try
            {
                await serviceCall();
                return Task.CompletedTask;
            }
            catch (Exception)
            {
                return Task.CompletedTask;
            }
        }
    }
}
