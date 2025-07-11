﻿using System.Security.Claims;

namespace Brainwave.API.Extensions
{
    public class AppUserService //: IAppUserService //TODO: implementar
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? GetId()
        {
            if (IsAuthenticated() == false)
                return null;

            var id = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (Guid.TryParse(id, out var idValue) == true)
                return idValue;

            return null;
        }

        public bool IsAdmin()
        {
            if (IsAuthenticated() == false)
                return false;

            return _httpContextAccessor.HttpContext?.User?.IsInRole("ADMIN") ?? false;
        }

        public bool IsStudent()
        {
            if (IsAuthenticated() == false)
                return false;

            return _httpContextAccessor.HttpContext?.User?.IsInRole("STUDENT") ?? false;
        }

        public bool IsAuthenticated()
        {
            return _httpContextAccessor.HttpContext?.User.Identity is { IsAuthenticated: true };
        }

        public bool IsLoggedUser(string userId)
        {
            if (Guid.TryParse(userId, out var idValue) == false)
                return false;

            return idValue == GetId();
        }
    }
}
