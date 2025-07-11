﻿using System.Net.Http.Headers;

namespace  Brainwave.API.Tests.Config;

public static class TestsExtensions
{
    public static void AssignToken(this HttpClient client, string token)
    {
        client.AssignJsonMediaType();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public static void AssignJsonMediaType(this HttpClient client)
    {
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}
