﻿namespace PM.Contracts.Auth;
public record RegisterRequest(
    string Email,
    string Password,
    Guid ProvinceId);

